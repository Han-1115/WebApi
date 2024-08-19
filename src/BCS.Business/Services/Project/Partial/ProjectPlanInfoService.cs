/*
 *所有关于ProjectPlanInfo类的业务代码应在此处编写
*可使用repository.调用常用方法，获取EF/Dapper等信息
*如果需要事务请使用repository.DbContextBeginTransaction
*也可使用DBServerProvider.手动获取数据库相关信息
*用户信息、权限、角色等使用UserContext.Current操作
*ProjectPlanInfoService对增、删、改查、导入、导出、审核业务代码扩展参照ServiceFunFilter
*/
using BCS.Core.BaseProvider;
using BCS.Core.Extensions.AutofacManager;
using BCS.Entity.DomainModels;
using System.Linq;
using BCS.Core.Utilities;
using System.Linq.Expressions;
using BCS.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using BCS.Business.IRepositories;
using BCS.Entity.DTO.Project;
using BCS.Business.IServices;
using BCS.Core.ManageUser;
using AutoMapper;
using System.Net.Mail;
using BCS.Core.EFDbContext;
using BCS.Core.DBManager;
using BCS.Business.Repositories;
using static Dapper.SqlMapper;
using BCS.Core.ConverterContainer;

namespace BCS.Business.Services
{
    public partial class ProjectPlanInfoService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IProjectPlanInfoRepository _repository;//访问数据库
        private readonly IProjectResourceBudgetRepository _projectResourceBudgetRepository;//访问数据库
        private readonly IProjectResourceBudgetHCRepository _projectResourceBudgetHCRepository;//访问数据库
        private readonly IProjectRepository _projectRepository;
        private readonly ISys_CalendarRepository _sys_CalendarRepository;//访问数据库
        private readonly ISys_SalaryMapRepository _sys_SalaryMapRepository;//访问数据库
        private readonly IMapper _mapper;

        [ActivatorUtilitiesConstructor]
        public ProjectPlanInfoService(
            IProjectPlanInfoRepository dbRepository,
            IProjectResourceBudgetRepository projectResourceBudgetRepository,
            IProjectResourceBudgetHCRepository projectResourceBudgetHCRepository,
            IProjectRepository projectRepository,
            ISys_CalendarRepository sys_CalendarRepository,
            ISys_SalaryMapRepository sys_SalaryMapRepository,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper
            )
        : base(dbRepository)
        {
            _repository = dbRepository;
            _projectResourceBudgetRepository = projectResourceBudgetRepository;
            _projectResourceBudgetHCRepository = projectResourceBudgetHCRepository;
            _projectRepository = projectRepository;
            _sys_CalendarRepository = sys_CalendarRepository;
            _sys_SalaryMapRepository = sys_SalaryMapRepository;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            //多租户会用到这init代码，其他情况可以不用
            //base.Init(dbRepository);
        }


        /// <summary>
        /// 保存项目计划信息，项目资源预算
        /// </summary>
        /// <param name="projectId">项目id</param>
        /// <param name="projectExtraInfos">项目计划信息</param>
        /// <param name="projectResourceBudgets">项目预算信息</param>
        /// <param name="projectResourceBudgetHCs">项目预算信息HC</param>
        /// <returns></returns>
        public async Task<WebResponseContent> SaveProjectPlanInfo(int projectId, ICollection<ProjectPlanInfoDTO> projectPlanInfos, ICollection<ProjectResourceBudgetDTO> projectResourceBudgets, ICollection<ProjectResourceBudgetHCDTO> projectResourceBudgetHCs)
        {
            UserInfo userInfo = UserContext.Current.UserInfo;
            DateTime currentTime = DateTime.Now;
            userInfo.UserName = string.IsNullOrEmpty(userInfo.UserName) ? string.Empty : userInfo.UserName;
            //资源预算根据薪资地图进行验证
            List<string> checkResults = new List<string>();
            foreach (var item in projectResourceBudgets)
            {
                if (!await _sys_SalaryMapRepository.ExistsAsync(o => o.CityId == item.CityId && o.PositionId == item.PositionId && o.LevelId == item.LevelId))
                {
                    continue;
                }
                var salaryLimit = await _sys_SalaryMapRepository.FindAsyncFirst(o => o.CityId == item.CityId && o.PositionId == item.PositionId && o.LevelId == item.LevelId);
                ////最低薪资
                //if (salaryLimit.MinCost_Rate != null)
                //{
                //    if (item.Cost_Rate < salaryLimit.MinCost_Rate)
                //    {
                //        checkResults.Add($"[{ConverterContainer.CityConverter(item.CityId)}][{ConverterContainer.PositionConverter(item.PositionId)}][{ConverterContainer.LevelConverter(item.LevelId)}]低于最低薪资{Environment.NewLine}");
                //    }
                //}
                //最高薪资
                if (salaryLimit.MaxCost_Rate != null)
                {
                    if (item.Cost_Rate > salaryLimit.MaxCost_Rate)
                    {
                        checkResults.Add($"[{ConverterContainer.CityConverter(item.CityId)}][{ConverterContainer.PositionConverter(item.PositionId)}][{ConverterContainer.LevelConverter(item.LevelId)}]高于最高薪资{Environment.NewLine}");
                    }
                }
            }
            if (checkResults.Count > 0)
            {
                return WebResponseContent.Instance.Error($"薪资预算验证失败。请参考以下信息{Environment.NewLine}{string.Join(Environment.NewLine, checkResults)}");
            }
            if (projectPlanInfos.Count <= 0)
            {
                return WebResponseContent.Instance.Error($"请上传项目节段参数");
            }
            if (projectPlanInfos.Count > 1)
            {
                return WebResponseContent.Instance.Error($"目前不支持多个项目节段");
            }
            // 0、获取项目信息
            var project = await _projectRepository.FindAsyncFirst(x => x.Id == projectId);
            var defaultProjectPlanInfo = projectPlanInfos.First();
            var updatePlanInfoList = new List<ProjectPlanInfo>();
            //项目计划信息
            foreach (var item in projectPlanInfos)
            {
                var projectPlanInfo = _mapper.Map<ProjectPlanInfo>(item);
                if (projectPlanInfo.Id <= 0)
                {
                    // TODO: 需要完善新增业务
                    throw new Exception("当前系统不支持 项目计划和资源预算同时新增，请先添加项目计划，再添加资源预算");
                }
                else
                {
                    projectPlanInfo.ModifyID = userInfo.User_Id;
                    projectPlanInfo.Modifier = userInfo.UserName;
                    projectPlanInfo.ModifyDate = currentTime;
                    updatePlanInfoList.Add(projectPlanInfo);
                }
            }

            #region 项目资源预算
            // 系统日历 
            List<int> yearContainer = new List<int>();
            int minYear = project.Start_Date.Year;
            int maxYear = project.End_Date.Year;
            do
            {
                yearContainer.Add(minYear);
                minYear = minYear + 1;
            } while (minYear <= maxYear);
            // Sys_Calendar data record.
            var sys_CalendarRecord = await _sys_CalendarRepository.FindAsync(x => x.Holiday_SystemId == project.Holiday_SystemId && yearContainer.Contains(x.Year));

            //项目资源预算
            var existsProjectResourceBudgets = await _projectResourceBudgetRepository.FindAsync(x => x.Project_Id == projectId);
            var insertList = new List<ProjectResourceBudget>();
            var updateList = new List<ProjectResourceBudget>();
            var removeList = new List<ProjectResourceBudget>();

            foreach (var item in projectResourceBudgets)
            {
                //获取工作日天数
                decimal workingDayCount = sys_CalendarRecord.Where(o => o.Date >= item.Start_Date && o.Date <= item.End_Date && o.IsWorkingDay == 1).Count();
                item.TotalManHourCapacity = Math.Round(item.HeadCount * workingDayCount, 2);
            }

            foreach (var item in projectResourceBudgets)
            {
                var projectResourceBudget = _mapper.Map<ProjectResourceBudget>(item);
                projectResourceBudget.ProjectPlanInfo_Id = defaultProjectPlanInfo.Id;
                if (projectResourceBudget.Id <= 0)
                {
                    //新增业务
                    projectResourceBudget.Project_Id = projectId;
                    projectResourceBudget.CreateID = userInfo.User_Id;
                    projectResourceBudget.Creator = userInfo.UserName;
                    projectResourceBudget.CreateDate = currentTime;
                    projectResourceBudget.ModifyID = userInfo.User_Id;
                    projectResourceBudget.Modifier = userInfo.UserName;
                    projectResourceBudget.ModifyDate = currentTime;
                    insertList.Add(projectResourceBudget);
                }
                else
                {
                    //更新业务
                    projectResourceBudget.ModifyID = userInfo.User_Id;
                    projectResourceBudget.Modifier = userInfo.UserName;
                    projectResourceBudget.ModifyDate = currentTime;
                    updateList.Add(projectResourceBudget);
                }
            }
            //删除业务
            foreach (var item in existsProjectResourceBudgets)
            {
                if (projectResourceBudgets.Any(o => o.Id == item.Id))
                {
                    continue;
                }
                var projectResourceBudget = _mapper.Map<ProjectResourceBudget>(item);
                removeList.Add(projectResourceBudget);
            }

            #endregion

            #region 项目资源预算HC| 此业务特殊，暂时做成先删除再添加

            //项目资源预算
            var existsProjectResourceBudgetsHC = await _projectResourceBudgetHCRepository.FindAsync(x => x.Project_Id == projectId);
            var insertListHC = new List<ProjectResourceBudgetHC>();

            foreach (var item in projectResourceBudgetHCs)
            {
                var projectResourceBudgetHC = _mapper.Map<ProjectResourceBudgetHC>(item);
                //新增业务
                projectResourceBudgetHC.Project_Id = projectId;
                projectResourceBudgetHC.CreateID = userInfo.User_Id;
                projectResourceBudgetHC.Creator = userInfo.UserName;
                projectResourceBudgetHC.CreateDate = currentTime;
                projectResourceBudgetHC.ModifyID = userInfo.User_Id;
                projectResourceBudgetHC.Modifier = userInfo.UserName;
                projectResourceBudgetHC.ModifyDate = currentTime;
                insertListHC.Add(projectResourceBudgetHC);
            }

            #endregion

            BCSContext dbContext = DBServerProvider.GetEFDbContext();
            using (var transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    repository.UpdateRange(updatePlanInfoList);
                    // 项目资源预算
                    _projectResourceBudgetRepository.AddRange(insertList);
                    _projectResourceBudgetRepository.UpdateRange(updateList);
                    if (removeList.Count > 0)
                    {
                        _projectResourceBudgetRepository.DeleteWithKeys(removeList.Select(o => (object)o.Id).ToArray());
                    }
                    // 项目资源预算HC
                    _projectResourceBudgetHCRepository.AddRange(insertListHC);
                    if (existsProjectResourceBudgetsHC.Count > 0)
                    {
                        _projectResourceBudgetHCRepository.DeleteWithKeys(existsProjectResourceBudgetsHC.Select(o => (object)o.Id).ToArray());
                    }
                    dbContext.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception($"保存项目和资源预算异常:[{ex.Message}]");
                }
            }
            return WebResponseContent.Instance.OK("保存项目计划和资源预算成功");
        }
    }
}
