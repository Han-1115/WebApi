/*
 *所有关于ProjectResourceBudgetHC类的业务代码应在此处编写
*可使用repository.调用常用方法，获取EF/Dapper等信息
*如果需要事务请使用repository.DbContextBeginTransaction
*也可使用DBServerProvider.手动获取数据库相关信息
*用户信息、权限、角色等使用UserContext.Current操作
*ProjectResourceBudgetHCService对增、删、改查、导入、导出、审核业务代码扩展参照ServiceFunFilter
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
using AutoMapper;
using BCS.Entity.DTO.Project;
using BCS.Core.ManageUser;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
using System.Net.Mail;
using BCS.Business.Repositories;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Database;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using System.Diagnostics.Eventing.Reader;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using BCS.Core.ConverterContainer;
using BCS.Business.IServices;
using BCS.Core.EFDbContext;
using BCS.Core.DBManager;
using BCS.Core.Infrastructure;
using static Quartz.Logging.OperationName;
using Microsoft.AspNetCore.Http.Metadata;
using System.Diagnostics.Contracts;
using BCS.Core.Enums;

namespace BCS.Business.Services
{
    public partial class ProjectResourceBudgetHCService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IProjectResourceBudgetHCRepository _repository;//访问数据库
        private readonly IProjectRepository _projectRepository;//访问数据库
        private readonly IProjectResourceBudgetRepository _projectResourceBudgetRepository;//访问数据库
        private readonly IProjectOtherBudgetRepository _projectOtherBudgetRepository;//访问数据库
        private readonly ISys_CalendarRepository _sys_CalendarRepository;//访问数据库
        private readonly ISys_SalaryMapRepository _sys_SalaryMapRepository;//访问数据库
        private readonly IProjectPlanInfoService _projectPlanInfoService;
        private readonly IProjectPlanInfoRepository _projectPlanInfoRepository;//访问数据库
        private readonly IProjectResourceBudgetHCRepository _projectResourceBudgetHCRepository;//访问数据库
        private readonly IProjectOtherBudgetService _projectOtherBudgetService;
        private readonly IMapper _mapper;

        /// <summary>
        /// 月标准天数
        /// </summary>
        public const string Standard_Number_of_Days_Per_Month = "Standard_Number_of_Days_Per_Month";

        [ActivatorUtilitiesConstructor]
        public ProjectResourceBudgetHCService(
            IProjectResourceBudgetHCRepository dbRepository,
            IProjectRepository projectRepository,
            IProjectResourceBudgetRepository projectResourceBudgetRepository,
            IProjectOtherBudgetRepository projectOtherBudgetRepository,
            ISys_CalendarRepository sys_CalendarRepository,
            ISys_SalaryMapRepository sys_SalaryMapRepository,
            IProjectPlanInfoService projectPlanInfoService,
            IProjectPlanInfoRepository projectPlanInfoRepository,
            IProjectResourceBudgetHCRepository projectResourceBudgetHCRepository,
            IProjectOtherBudgetService projectOtherBudgetService,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper
            )
        : base(dbRepository)
        {
            _repository = dbRepository;
            _projectRepository = projectRepository;
            _projectResourceBudgetRepository = projectResourceBudgetRepository;
            _projectOtherBudgetRepository = projectOtherBudgetRepository;
            _sys_CalendarRepository = sys_CalendarRepository;
            _sys_SalaryMapRepository = sys_SalaryMapRepository;
            _projectPlanInfoService = projectPlanInfoService;
            _projectPlanInfoRepository = projectPlanInfoRepository;
            _projectResourceBudgetHCRepository = projectResourceBudgetHCRepository;
            _projectOtherBudgetService = projectOtherBudgetService;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            //多租户会用到这init代码，其他情况可以不用
            //base.Init(dbRepository);
        }

        /// <summary>
        /// 计算资源预算人月信息
        /// </summary>
        /// <param name="calculateResourceBudgetHCInputDTO">项目ID,资源计划</param>
        /// <returns></returns>
        public async Task<WebResponseContent> CalculateResourceBudgetHC(CalculateResourceBudgetHCInputDTO calculateResourceBudgetHCInputDTO)
        {
            if (!await _projectRepository.ExistsAsync(x => x.Id == calculateResourceBudgetHCInputDTO.Project_Id))
            {
                return WebResponseContent.Instance.Error("项目不存在");
            }
            //资源预算根据薪资地图进行验证
            List<string> checkResults = new List<string>();
            foreach (var item in calculateResourceBudgetHCInputDTO.ProjectResourceBudgetDTOs)
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
            //project info.
            var project = await _projectRepository.FindAsyncFirst(x => x.Id == calculateResourceBudgetHCInputDTO.Project_Id);
            var project_Id = calculateResourceBudgetHCInputDTO.Project_Id;
            var projectResourceBudgetDTOs = calculateResourceBudgetHCInputDTO.ProjectResourceBudgetDTOs;
            var projectOtherBudgetDTOs = calculateResourceBudgetHCInputDTO.ProjectOtherBudgetDTOs;
            if (projectOtherBudgetDTOs.Count <= 0)
            {
                // 项目其它成本费用预算
                var projectOtherBudgetsOld = await _projectOtherBudgetRepository.FindAsync(x => x.Project_Id == project_Id);
                projectOtherBudgetDTOs = _mapper.Map<ICollection<ProjectOtherBudgetDTO>>(projectOtherBudgetsOld);
            }
            if (projectResourceBudgetDTOs == null || projectResourceBudgetDTOs.Count <= 0)
            {
                return WebResponseContent.Instance.Error("请传入资源预算信息");
            }
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
            // 已设置的工作日
            var queryYearMonth = sys_CalendarRecord.Select(o => $"{o.Year}-{o.Month.ToString().PadLeft(2, '0')}").Distinct().ToList();
            UserInfo userInfo = UserContext.Current.UserInfo;
            DateTime currentTime = DateTime.Now;
            userInfo.UserName = string.IsNullOrEmpty(userInfo.UserName) ? string.Empty : userInfo.UserName;
            // 根据项目节段计算出 项目横跨的年月信息
            var planIds = projectResourceBudgetDTOs.Select(o => o.ProjectPlanInfo_Id).Distinct().ToList();
            List<YearMonthItem> listYearMonth = new List<YearMonthItem>();
            foreach (var planInfoId in planIds)
            {
                var beginDate = project.Start_Date;
                var endDate = project.End_Date;
                while (beginDate <= endDate)
                {
                    Predicate<YearMonthItem> predicate = o =>
                    {
                        return o.ProjectPlanInfo_Id == planInfoId && o.YearMonth == beginDate.ToString("yyyy-MM");
                    };
                    if (!listYearMonth.Exists(predicate))
                    {
                        listYearMonth.Add(new YearMonthItem(planInfoId, beginDate.ToString("yyyy-MM"), beginDate.Year, beginDate.Month));
                    }

                    beginDate = beginDate.AddMonths(1);
                }
            }

            // 资源预算的最大年月
            var checkResourceYearMonth = calculateResourceBudgetHCInputDTO.ProjectResourceBudgetDTOs.Select(o => o.End_Date.ToString("yyyy-MM")).Distinct();
            var checkResult = checkResourceYearMonth.Except(queryYearMonth).ToList();
            if (checkResult.Count > 0)
            {
                return WebResponseContent.Instance.Error($"系统还未设置[{string.Join(',', checkResult)}]的工作日历。请先联系管理员维护系统日历，或者调整资源预算周期。");
            }

            BCSContext context = DBServerProvider.GetEFDbContext();
            // 获取合同结算币种
            var queryResult = (from ProjectItem in context.Set<BCS.Entity.DomainModels.Project>()
                               where project.Id == ProjectItem.Id
                               join ContractProjectItem in context.Set<ContractProject>().Where(s => s.Status == (int)Status.Active) on ProjectItem.Id equals ContractProjectItem.Project_Id
                               join ContractItem in context.Set<BCS.Entity.DomainModels.Contract>() on ContractProjectItem.Contract_Id equals ContractItem.Id
                               select ContractItem).ToList();
            if (queryResult.Count <= 0)
            {
                return WebResponseContent.Instance.Error("未找到项目合同信息。");
            }
            var projectContract = queryResult.First();

            List<ProjectResourceBudgetHCDTO> list = BuildProjectResourceBudgetHC(projectResourceBudgetDTOs.ToList(), sys_CalendarRecord, listYearMonth, project, projectContract);

            //计算人力资源预算总工时
            foreach (var item in projectResourceBudgetDTOs)
            {
                //获取工作日天数
                item.TotalManHourCapacity = this.CalculateManHourCapacity(listYearMonth, new List<ProjectResourceBudgetDTO> { item }, sys_CalendarRecord, project, projectContract);
            }

            #region 组织项目费用预算
            string settlement_Currency = projectContract.Settlement_Currency;

            List<ProjectOtherBudgetDTO> listProjectOtherBudget = new List<ProjectOtherBudgetDTO>();
            var resourceProjectOtherBudgetFormUi = projectOtherBudgetDTOs.ToList();
            var listProjectOtherBudgetYearMonth = listYearMonth.Select(o => o.YearMonth).Distinct().ToList();
            foreach (var yearMonth in listProjectOtherBudgetYearMonth)
            {
                Predicate<ProjectOtherBudgetDTO> predicate = o =>
                {
                    return o.YearMonth == yearMonth;
                };
                if (resourceProjectOtherBudgetFormUi.Exists(predicate))
                {
                    var existsProjectOtherBudget = resourceProjectOtherBudgetFormUi.Find(predicate);
                    ArgumentNullException.ThrowIfNull(existsProjectOtherBudget, nameof(existsProjectOtherBudget));
                    listProjectOtherBudget.Add(existsProjectOtherBudget);
                }
                else
                {
                    ProjectOtherBudgetDTO projectOtherBudget = new ProjectOtherBudgetDTO();
                    projectOtherBudget.Project_Id = project_Id;
                    projectOtherBudget.Settlement_Currency = settlement_Currency;
                    projectOtherBudget.YearMonth = yearMonth;
                    projectOtherBudget.Remark = string.Empty;
                    projectOtherBudget.CreateID = userInfo.User_Id;
                    projectOtherBudget.Creator = userInfo.UserName;
                    projectOtherBudget.CreateDate = currentTime;
                    projectOtherBudget.ModifyID = userInfo.User_Id;
                    projectOtherBudget.Modifier = userInfo.UserName;
                    projectOtherBudget.ModifyDate = currentTime;
                    listProjectOtherBudget.Add(projectOtherBudget);
                }
            }

            #endregion

            CalculateResourceBudgetHCOutPutDTO calculateResourceBudgetHCOutPutDTO = new CalculateResourceBudgetHCOutPutDTO(project_Id, projectResourceBudgetDTOs, list, listProjectOtherBudget);
            return WebResponseContent.Instance.OK("生成项目人月信息和项目成本预算成功", calculateResourceBudgetHCOutPutDTO);
        }

        /// <summary>
        /// 保存资源预算+人月信息
        /// </summary>
        /// <param name="saveResourceBudgeInputDTO"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> SaveProjectResourceBudget(SaveResourceBudgeInputDTO saveResourceBudgeInputDTO)
        {
            if (!await _projectRepository.ExistsAsync(x => x.Id == saveResourceBudgeInputDTO.Project_Id))
            {
                return WebResponseContent.Instance.Error("项目不存在");
            }
            //资源预算根据薪资地图进行验证
            List<string> checkResults = new List<string>();
            foreach (var item in saveResourceBudgeInputDTO.ProjectResourceBudgetDTOs)
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
            //project info.
            var project = await _projectRepository.FindAsyncFirst(x => x.Id == saveResourceBudgeInputDTO.Project_Id);
            var project_Id = saveResourceBudgeInputDTO.Project_Id;
            var projectPlanInfoDTOs = saveResourceBudgeInputDTO.ProjectPlanInfoDTOs;
            var projectResourceBudgetDTOs = saveResourceBudgeInputDTO.ProjectResourceBudgetDTOs;
            var projectOtherBudgetDTOs = saveResourceBudgeInputDTO.ProjectOtherBudgetDTOs;
            if (projectOtherBudgetDTOs.Count <= 0)
            {
                // 项目其它成本费用预算
                var projectOtherBudgetsOld = await _projectOtherBudgetRepository.FindAsync(x => x.Project_Id == project_Id);
                projectOtherBudgetDTOs = _mapper.Map<ICollection<ProjectOtherBudgetDTO>>(projectOtherBudgetsOld);
            }

            if (projectResourceBudgetDTOs == null || projectResourceBudgetDTOs.Count <= 0)
            {
                return WebResponseContent.Instance.Error("请传入资源预算信息");
            }
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
            // 已设置的工作日
            var queryYearMonth = sys_CalendarRecord.Select(o => $"{o.Year}-{o.Month.ToString().PadLeft(2, '0')}").Distinct().ToList();
            UserInfo userInfo = UserContext.Current.UserInfo;
            DateTime currentTime = DateTime.Now;
            userInfo.UserName = string.IsNullOrEmpty(userInfo.UserName) ? string.Empty : userInfo.UserName;
            // 根据项目节段计算出 项目横跨的年月信息
            var planIds = projectResourceBudgetDTOs.Select(o => o.ProjectPlanInfo_Id).Distinct().ToList();
            List<YearMonthItem> listYearMonth = new List<YearMonthItem>();
            foreach (var planInfoId in planIds)
            {
                var beginDate = project.Start_Date;
                var endDate = project.End_Date;
                while (beginDate <= endDate)
                {
                    Predicate<YearMonthItem> predicate = o =>
                    {
                        return o.ProjectPlanInfo_Id == planInfoId && o.YearMonth == beginDate.ToString("yyyy-MM");
                    };
                    if (!listYearMonth.Exists(predicate))
                    {
                        listYearMonth.Add(new YearMonthItem(planInfoId, beginDate.ToString("yyyy-MM"), beginDate.Year, beginDate.Month));
                    }

                    beginDate = beginDate.AddMonths(1);
                }
            }

            // 资源预算的最大年月
            var checkResourceYearMonth = saveResourceBudgeInputDTO.ProjectResourceBudgetDTOs.Select(o => o.End_Date.ToString("yyyy-MM")).Distinct();
            var checkResult = checkResourceYearMonth.Except(queryYearMonth).ToList();
            if (checkResult.Count > 0)
            {
                return WebResponseContent.Instance.Error($"系统还未设置[{string.Join(',', checkResult)}]的工作日历。请先联系管理员维护系统日历，或者调整资源预算周期。");
            }

            BCSContext context = DBServerProvider.GetEFDbContext();
            // 获取合同结算币种
            var queryResult = (from ProjectItem in context.Set<BCS.Entity.DomainModels.Project>()
                               where project.Id == ProjectItem.Id
                               join ContractProjectItem in context.Set<ContractProject>().Where(s => s.Status == (int)Status.Active) on ProjectItem.Id equals ContractProjectItem.Project_Id
                               join ContractItem in context.Set<BCS.Entity.DomainModels.Contract>() on ContractProjectItem.Contract_Id equals ContractItem.Id
                               select ContractItem).ToList();
            if (queryResult.Count <= 0)
            {
                return WebResponseContent.Instance.Error("未找到项目合同信息。");
            }
            var projectContract = queryResult.First();

            List<ProjectResourceBudgetHCDTO> list = BuildProjectResourceBudgetHC(projectResourceBudgetDTOs.ToList(), sys_CalendarRecord, listYearMonth, project, projectContract);


            //计算人力资源预算总工时
            foreach (var item in projectResourceBudgetDTOs)
            {
                //获取工作日天数
                item.TotalManHourCapacity = this.CalculateManHourCapacity(listYearMonth, new List<ProjectResourceBudgetDTO> { item }, sys_CalendarRecord, project, projectContract);
            }

            #region 组织项目费用预算

            string settlement_Currency = projectContract.Settlement_Currency;

            List<ProjectOtherBudgetDTO> listProjectOtherBudget = new List<ProjectOtherBudgetDTO>();
            var resourceProjectOtherBudgetFormUi = projectOtherBudgetDTOs.ToList();
            var listProjectOtherBudgetYearMonth = listYearMonth.Select(o => o.YearMonth).Distinct().ToList();
            foreach (var yearMonth in listProjectOtherBudgetYearMonth)
            {
                Predicate<ProjectOtherBudgetDTO> predicate = o =>
                {
                    return o.YearMonth == yearMonth;
                };
                if (resourceProjectOtherBudgetFormUi.Exists(predicate))
                {
                    var existsProjectOtherBudget = resourceProjectOtherBudgetFormUi.Find(predicate);
                    ArgumentNullException.ThrowIfNull(existsProjectOtherBudget, nameof(existsProjectOtherBudget));
                    listProjectOtherBudget.Add(existsProjectOtherBudget);
                }
                else
                {
                    ProjectOtherBudgetDTO projectOtherBudget = new ProjectOtherBudgetDTO();
                    projectOtherBudget.Project_Id = project_Id;
                    projectOtherBudget.Settlement_Currency = settlement_Currency;
                    projectOtherBudget.YearMonth = yearMonth;
                    projectOtherBudget.Remark = string.Empty;
                    projectOtherBudget.CreateID = userInfo.User_Id;
                    projectOtherBudget.Creator = userInfo.UserName;
                    projectOtherBudget.CreateDate = currentTime;
                    projectOtherBudget.ModifyID = userInfo.User_Id;
                    projectOtherBudget.Modifier = userInfo.UserName;
                    projectOtherBudget.ModifyDate = currentTime;
                    listProjectOtherBudget.Add(projectOtherBudget);
                }
            }

            #endregion
            // 保存项目计划信息
            var result = await this._projectPlanInfoService.SaveProjectPlanInfo(project_Id, projectPlanInfoDTOs, projectResourceBudgetDTOs, list);
            if (!result.Status)
            {
                return WebResponseContent.Instance.Error(result.Message);
            }
            // 保存项目其它预算
            var saveOtherBudgetResult = await _projectOtherBudgetService.SaveOtherBudget(project_Id, listProjectOtherBudget);
            if (!saveOtherBudgetResult.Status)
            {
                return WebResponseContent.Instance.Error(saveOtherBudgetResult.Message);
            }
            // 项目计划信息
            var projectPlanInfo = await _projectPlanInfoRepository.FindAsync(x => x.Project_Id == project_Id);
            var projectPlanInfoDTO = _mapper.Map<ICollection<ProjectPlanInfoDTO>>(projectPlanInfo);
            // 项目资源预算   
            var projectResourceBudget = await _projectResourceBudgetRepository.FindAsync(x => x.Project_Id == project_Id);
            var projectResourceBudgetDTO = _mapper.Map<ICollection<ProjectResourceBudgetDTO>>(projectResourceBudget);
            foreach (var item in projectResourceBudgetDTO)
            {
                //通过字典获取展示信息
                item.Position = ConverterContainer.PositionConverter(item.PositionId);
                item.Level = ConverterContainer.LevelConverter(item.LevelId);
                item.City = ConverterContainer.CityConverter(item.CityId);
            }
            // 项目其它成本费用预算
            var projectOtherBudgets = await _projectOtherBudgetRepository.FindAsync(x => x.Project_Id == project_Id);
            var projectOtherBudgetsDTO = _mapper.Map<ICollection<ProjectOtherBudgetDTO>>(projectOtherBudgets);

            // 项目资源预算HC 
            var projectResourceBudgetHC = await _projectResourceBudgetHCRepository.FindAsync(x => x.Project_Id == project_Id);
            var projectResourceBudgetHCDTO = _mapper.Map<ICollection<ProjectResourceBudgetHCDTO>>(projectResourceBudgetHC);

            SaveResourceBudgeOutPutDTO saveResourceBudgeOutPutDTO = new SaveResourceBudgeOutPutDTO(project_Id, projectPlanInfoDTO, projectResourceBudgetDTO, projectResourceBudgetHCDTO, projectOtherBudgetsDTO);
            return WebResponseContent.Instance.OK("保存项目资源预算+人月信息成功", saveResourceBudgeOutPutDTO);
        }

        /// <summary>
        /// 生成项目人月信息
        /// </summary>
        /// <param name="projectResourceBudgetDTOs"></param>
        /// <param name="sys_CalendarRecord"></param>
        /// <param name="listYearMonth"></param>
        /// <param name="project"></param>
        /// <param name="contract"></param>
        /// <returns></returns>
        private List<ProjectResourceBudgetHCDTO> BuildProjectResourceBudgetHC(List<ProjectResourceBudgetDTO> projectResourceBudgetDTOs, List<Sys_Calendar> sys_CalendarRecord, List<YearMonthItem> listYearMonth, Project project, Entity.DomainModels.Contract contract)
        {
            UserInfo userInfo = UserContext.Current.UserInfo;
            DateTime currentTime = DateTime.Now;
            userInfo.UserName = string.IsNullOrEmpty(userInfo.UserName) ? string.Empty : userInfo.UserName;
            // TODO:需要考虑各国每月工作天数不同
            // 组织项目人月信息
            // 获取每月标准天数
            //资源计划人月-HCCount-计算逻辑
            decimal CalculateYearMonthHCCount(bool isFullMonth, bool isFixedWorkingDay, decimal fixedWorkingDay, decimal workingDay, decimal actualWorkingDay, decimal headCount)
            {
                decimal returnValue = 0;
                if (isFullMonth)
                {
                    //完整月
                    //value=月工作日天数*HeadCount
                    returnValue += headCount;
                }
                else
                {
                    //不完整月 
                    if (isFixedWorkingDay)
                    {
                        ////月固定工作日天数计算逻辑(下面的这个算法有点奇怪，但是符合业务逻辑)
                        //value=(实际工作日天数/当月工作日天数)*固定工作日天数*HeadCount*固定工作日天数
                        returnValue += (actualWorkingDay / workingDay) * fixedWorkingDay * headCount / fixedWorkingDay;
                    }
                    else
                    {
                        //value=实际工作天数/工作日天数*HeadCount
                        returnValue += actualWorkingDay / workingDay * headCount;
                    }
                }
                return returnValue;
            }

            List<ProjectResourceBudgetHCDTO> list = new List<ProjectResourceBudgetHCDTO>();
            var resourceFormUi = projectResourceBudgetDTOs.ToList();
            foreach (var item in listYearMonth)
            {
                //当前月的最大开始日期
                DateTime currentYearMonthStartMax = new DateTime(item.Year, item.Month, DateTime.DaysInMonth(item.Year, item.Month));
                //当前月的最小结束日期
                DateTime currentYearMonthEndMin = new DateTime(item.Year, item.Month, 1);
                Predicate<ProjectResourceBudgetDTO> predicate = o =>
                {
                    return o.ProjectPlanInfo_Id == item.ProjectPlanInfo_Id && currentYearMonthStartMax >= o.Start_Date && currentYearMonthEndMin <= o.End_Date;
                };

                var resourceList = resourceFormUi.FindAll(predicate);
                decimal tempHCCountPlan = 0;
                foreach (var budgetDTO in resourceList)
                {
                    // TODO :需要考虑各国每月工作天数不同，以及并不是每个资源预算都是整月的情况
                    //项目资源预算人月
                    //需要考虑以下情况 
                    //同年同月，同年不同月，不同年同月，不同年不同月
                    //同年同月: 完整月，不完整月
                    //非同年同月: 完整月，不完整月
                    bool isSameYearMonth = budgetDTO.Start_Date.Month == budgetDTO.End_Date.Month && budgetDTO.Start_Date.Year == budgetDTO.End_Date.Year;
                    var workingDaySetting = GetStandard_Number_of_Days_Per_Month(project.Standard_Number_of_Days_Per_MonthId);
                    if (isSameYearMonth)
                    {
                        //不完整月
                        decimal workingDayCount = sys_CalendarRecord.Where(o =>
                            o.Year == budgetDTO.Start_Date.Year
                            && o.Month == budgetDTO.Start_Date.Month
                            && o.IsWorkingDay == 1).Count();
                        #region 同年同月

                        bool isFullMonth = budgetDTO.Start_Date.Day == 1 && budgetDTO.End_Date.Day == DateTime.DaysInMonth(budgetDTO.Start_Date.Year, budgetDTO.Start_Date.Month);
                        if (isFullMonth)
                        {
                            //完整月
                            tempHCCountPlan += CalculateYearMonthHCCount(isFullMonth, workingDaySetting.IsFixedWorkingDay, workingDaySetting.FixedWorkingDay, workingDayCount, workingDayCount, budgetDTO.HeadCount);
                        }
                        else
                        {
                            decimal actualWorkingCount = sys_CalendarRecord.Where(o =>
                                o.Year == budgetDTO.Start_Date.Year
                                && o.Month == budgetDTO.Start_Date.Month
                                && o.IsWorkingDay == 1
                                && o.Day >= budgetDTO.Start_Date.Day
                                && o.Day <= budgetDTO.End_Date.Day).Count();
                            tempHCCountPlan += CalculateYearMonthHCCount(isFullMonth, workingDaySetting.IsFixedWorkingDay, workingDaySetting.FixedWorkingDay, workingDayCount, actualWorkingCount, budgetDTO.HeadCount);
                        }

                        #endregion
                    }
                    else
                    {
                        #region 同年不同月|同月不同年|不同年不同月

                        //考虑 循环年月，是否在当前预算的年月范围内，即当前待计算的人月年月是否在 当前属于当前预算的开始月或者结束月 如果不是，则按整月计算
                        if (item.YearMonth == budgetDTO.Start_Date.ToString("yyyy-MM"))
                        {
                            decimal workingDayCount = sys_CalendarRecord.Where(o =>
                                                   o.Year == budgetDTO.Start_Date.Year
                                                   && o.Month == budgetDTO.Start_Date.Month
                                                   && o.IsWorkingDay == 1).Count();
                            //全月判断条件 1.开始日期是1号 2.结束日期是当月最后一天
                            bool isFullMonth = budgetDTO.Start_Date.Day == 1;
                            if (isFullMonth)
                            {
                                tempHCCountPlan += CalculateYearMonthHCCount(isFullMonth, workingDaySetting.IsFixedWorkingDay, workingDaySetting.FixedWorkingDay, workingDayCount, workingDayCount, budgetDTO.HeadCount);
                            }
                            else
                            {
                                //当前待计算的人月年月是当前预算的开始月
                                decimal actualWorkingCount = sys_CalendarRecord.Where(o =>
                                                           o.Year == budgetDTO.Start_Date.Year
                                                           && o.Month == budgetDTO.Start_Date.Month
                                                           && o.IsWorkingDay == 1
                                                           && o.Day >= budgetDTO.Start_Date.Day).Count();
                                tempHCCountPlan += CalculateYearMonthHCCount(isFullMonth, workingDaySetting.IsFixedWorkingDay, workingDaySetting.FixedWorkingDay, workingDayCount, actualWorkingCount, budgetDTO.HeadCount);
                            }
                        }
                        else if (item.YearMonth == budgetDTO.End_Date.ToString("yyyy-MM"))
                        {
                            decimal workingDayCount = sys_CalendarRecord.Where(o =>
                                                   o.Year == budgetDTO.End_Date.Year
                                                   && o.Month == budgetDTO.End_Date.Month
                                                   && o.IsWorkingDay == 1).Count();
                            //全月判断条件 1.开始日期是1号 2.结束日期是当月最后一天
                            bool isFullMonth = budgetDTO.End_Date.Day == DateTime.DaysInMonth(budgetDTO.End_Date.Year, budgetDTO.End_Date.Month);
                            if (isFullMonth)
                            {
                                tempHCCountPlan += CalculateYearMonthHCCount(isFullMonth, workingDaySetting.IsFixedWorkingDay, workingDaySetting.FixedWorkingDay, workingDayCount, workingDayCount, budgetDTO.HeadCount);
                            }
                            else
                            {
                                //当前待计算的人月年月是当前预算的结束月
                                decimal actualWorkingCount = sys_CalendarRecord.Where(o =>
                                                           o.Year == budgetDTO.End_Date.Year
                                                           && o.Month == budgetDTO.End_Date.Month
                                                           && o.IsWorkingDay == 1
                                                           && o.Day <= budgetDTO.End_Date.Day).Count();
                                tempHCCountPlan += CalculateYearMonthHCCount(isFullMonth, workingDaySetting.IsFixedWorkingDay, workingDaySetting.FixedWorkingDay, workingDayCount, actualWorkingCount, budgetDTO.HeadCount);
                                //temoHCCountPlan += Math.Round(budgetDTO.HeadCount * actualWorkingCount / workingDayCount, 2);
                            }
                        }
                        else
                        {
                            decimal workingDayCount = sys_CalendarRecord.Where(o =>
                                                   o.Year == item.Year
                                                   && o.Month == item.Month
                                                   && o.IsWorkingDay == 1).Count();
                            tempHCCountPlan += CalculateYearMonthHCCount(true, workingDaySetting.IsFixedWorkingDay, workingDaySetting.FixedWorkingDay, workingDayCount, workingDayCount, budgetDTO.HeadCount);
                        }

                        #endregion
                    }
                }

                var singleItem = new ProjectResourceBudgetHCDTO();
                singleItem.Project_Id = project.Id;
                singleItem.YearMonth = item.YearMonth;
                singleItem.ProjectPlanInfo_Id = item.ProjectPlanInfo_Id;
                singleItem.HCCountPlan = Math.Round(tempHCCountPlan, 2);
                singleItem.HCCountActual = 0;
                singleItem.Remark = string.Empty;
                singleItem.CreateID = userInfo.User_Id;
                singleItem.Creator = userInfo.UserName;
                singleItem.CreateDate = currentTime;
                singleItem.ModifyID = userInfo.User_Id;
                singleItem.Modifier = userInfo.UserName;
                singleItem.ModifyDate = currentTime;
                list.Add(singleItem);
            }

            return list;
        }

        /// <summary>
        /// 计算每个人月的工时(人天)
        /// </summary>
        /// <param name="listYearMonth"></param>
        /// <param name="resourceList"></param>
        /// <param name="sys_CalendarRecord"></param>
        /// <param name="project"></param>
        /// <returns></returns>
        private decimal CalculateManHourCapacity(List<YearMonthItem> listYearMonth, List<ProjectResourceBudgetDTO> resourceFormUi, List<Sys_Calendar> sys_CalendarRecord, Project project, Entity.DomainModels.Contract contract)
        {
            decimal totalManHourCapacity = 0;
            //资源计划人天-计算逻辑
            decimal CalculateManHourCapacity(bool isFullMonth, bool isFixedWorkingDay, decimal fixedWorkingDay, decimal workingDay, decimal actualWorkingDay, decimal headCount)
            {
                decimal returnValue = 0;
                if (isFullMonth)
                {
                    //完整月
                    //value=月工作日天数*HeadCount
                    returnValue += (isFixedWorkingDay ? fixedWorkingDay : workingDay) * headCount;
                }
                else
                {
                    //不完整月 
                    if (isFixedWorkingDay)
                    {
                        ////月固定工作日天数计算逻辑
                        //value=(实际工作日天数/当月工作日天数)*固定工作日天数*HeadCount
                        returnValue += (actualWorkingDay / workingDay) * fixedWorkingDay * headCount;
                    }
                    else
                    {
                        //value=实际工作天数/工作日天数*工作日天数*HeadCount
                        returnValue += (actualWorkingDay / workingDay) * workingDay * headCount;
                    }
                }
                return returnValue;
            }

            // 获取每月标准天数
            var workingDaySetting = GetStandard_Number_of_Days_Per_Month(project.Standard_Number_of_Days_Per_MonthId);

            foreach (var item in listYearMonth)
            {
                //当前月的最大开始日期
                DateTime currentYearMonthStartMax = new DateTime(item.Year, item.Month, DateTime.DaysInMonth(item.Year, item.Month));
                //当前月的最小结束日期
                DateTime currentYearMonthEndMin = new DateTime(item.Year, item.Month, 1);
                Predicate<ProjectResourceBudgetDTO> predicate = o =>
                {
                    return o.ProjectPlanInfo_Id == item.ProjectPlanInfo_Id && currentYearMonthStartMax >= o.Start_Date && currentYearMonthEndMin <= o.End_Date;
                };
                var resourceList = resourceFormUi.FindAll(predicate);
                foreach (var budgetDTO in resourceList)
                {
                    // TODO :需要考虑各国每月工作天数不同，以及并不是每个资源预算都是整月的情况
                    //项目资源预算人月
                    //需要考虑以下情况 
                    //同年同月，同年不同月，不同年同月，不同年不同月
                    //同年同月: 完整月，不完整月
                    //非同年同月: 完整月，不完整月
                    bool isSameYearMonth = budgetDTO.Start_Date.Month == budgetDTO.End_Date.Month && budgetDTO.Start_Date.Year == budgetDTO.End_Date.Year;
                    if (isSameYearMonth)
                    {
                        //每月工作日天数
                        decimal workingDayCount = sys_CalendarRecord.Where(o =>
                            o.Year == budgetDTO.Start_Date.Year
                            && o.Month == budgetDTO.Start_Date.Month
                            && o.IsWorkingDay == 1).Count();
                        #region 同年同月

                        bool isFullMonth = budgetDTO.Start_Date.Day == 1 && budgetDTO.End_Date.Day == DateTime.DaysInMonth(budgetDTO.Start_Date.Year, budgetDTO.Start_Date.Month);
                        if (isFullMonth)
                        {
                            //完整月
                            totalManHourCapacity += CalculateManHourCapacity(isFullMonth, workingDaySetting.IsFixedWorkingDay, workingDaySetting.FixedWorkingDay, workingDayCount, workingDayCount, budgetDTO.HeadCount);
                        }
                        else
                        {
                            //不完整月
                            decimal actualWorkingCount = sys_CalendarRecord.Where(o =>
                                o.Year == budgetDTO.Start_Date.Year
                                && o.Month == budgetDTO.Start_Date.Month
                                && o.IsWorkingDay == 1
                                && o.Day >= budgetDTO.Start_Date.Day
                                && o.Day <= budgetDTO.End_Date.Day).Count();

                            totalManHourCapacity += CalculateManHourCapacity(isFullMonth, workingDaySetting.IsFixedWorkingDay, workingDaySetting.FixedWorkingDay, workingDayCount, actualWorkingCount, budgetDTO.HeadCount);
                        }

                        #endregion
                    }
                    else
                    {
                        #region 同年不同月|同月不同年|不同年不同月

                        //考虑 循环年月，是否在当前预算的年月范围内，即当前待计算的人月年月是否在 当前属于当前预算的开始月或者结束月 如果不是，则按整月计算
                        if (item.YearMonth == budgetDTO.Start_Date.ToString("yyyy-MM"))
                        {
                            //开始月工作日天数
                            decimal workingDayCount = sys_CalendarRecord.Where(o =>
                                                   o.Year == budgetDTO.Start_Date.Year
                                                   && o.Month == budgetDTO.Start_Date.Month
                                                   && o.IsWorkingDay == 1).Count();
                            //全月判断条件 1.开始日期是1号 2.结束日期是当月最后一天
                            bool isFullMonth = budgetDTO.Start_Date.Day == 1;
                            if (isFullMonth)
                            {
                                totalManHourCapacity += CalculateManHourCapacity(isFullMonth, workingDaySetting.IsFixedWorkingDay, workingDaySetting.FixedWorkingDay, workingDayCount, workingDayCount, budgetDTO.HeadCount);
                            }
                            else
                            {
                                //当前待计算的人月年月是当前预算的开始月
                                decimal actualWorkingCount = sys_CalendarRecord.Where(o =>
                                                           o.Year == budgetDTO.Start_Date.Year
                                                           && o.Month == budgetDTO.Start_Date.Month
                                                           && o.IsWorkingDay == 1
                                                           && o.Day >= budgetDTO.Start_Date.Day).Count();

                                totalManHourCapacity += CalculateManHourCapacity(isFullMonth, workingDaySetting.IsFixedWorkingDay, workingDaySetting.FixedWorkingDay, workingDayCount, actualWorkingCount, budgetDTO.HeadCount);
                            }
                        }
                        else if (item.YearMonth == budgetDTO.End_Date.ToString("yyyy-MM"))
                        {
                            //结束月工作日天数
                            decimal workingDayCount = sys_CalendarRecord.Where(o =>
                                                   o.Year == budgetDTO.End_Date.Year
                                                   && o.Month == budgetDTO.End_Date.Month
                                                   && o.IsWorkingDay == 1).Count();
                            //全月判断条件 1.开始日期是1号 2.结束日期是当月最后一天
                            bool isFullMonth = budgetDTO.End_Date.Day == DateTime.DaysInMonth(budgetDTO.End_Date.Year, budgetDTO.End_Date.Month);
                            if (isFullMonth)
                            {
                                totalManHourCapacity += CalculateManHourCapacity(isFullMonth, workingDaySetting.IsFixedWorkingDay, workingDaySetting.FixedWorkingDay, workingDayCount, workingDayCount, budgetDTO.HeadCount);
                            }
                            else
                            {
                                //当前待计算的人月年月是当前预算的结束月
                                decimal actualWorkingCount = sys_CalendarRecord.Where(o =>
                                                           o.Year == budgetDTO.End_Date.Year
                                                           && o.Month == budgetDTO.End_Date.Month
                                                           && o.IsWorkingDay == 1
                                                           && o.Day <= budgetDTO.End_Date.Day).Count();

                                totalManHourCapacity += CalculateManHourCapacity(isFullMonth, workingDaySetting.IsFixedWorkingDay, workingDaySetting.FixedWorkingDay, workingDayCount, actualWorkingCount, budgetDTO.HeadCount);
                            }
                        }
                        else
                        {
                            //全月
                            decimal workingDayCount = sys_CalendarRecord.Where(o =>
                                                   o.Year == item.Year
                                                   && o.Month == item.Month
                                                   && o.IsWorkingDay == 1).Count();

                            totalManHourCapacity += CalculateManHourCapacity(true, workingDaySetting.IsFixedWorkingDay, workingDaySetting.FixedWorkingDay, workingDayCount, workingDayCount, budgetDTO.HeadCount);
                        }

                        #endregion
                    }
                }
            }
            return Math.Round(totalManHourCapacity, 2);
        }

        /// <summary>
        /// 获取项目 Standard_Number_of_Days_Per_Month 是否固定天数
        /// <para>返回值</para>
        /// <para>true,Standard_Number_of_Days_Per_Month 为固定天数</para>
        /// <para>false,从系统日历取当月工作日天数</para>
        /// </summary>
        /// <param name="keyValue">月标准天数数据字典keyValue</param>
        /// <returns></returns>
        private (bool IsFixedWorkingDay, decimal FixedWorkingDay) GetStandard_Number_of_Days_Per_Month(int keyValue)
        {
            bool isFixedWorkingDay = false;
            decimal fixedWorkingDay = 0;
            //获取月标准天数
            Sys_Dictionary sys_Dictionary = DictionaryManager.GetDictionary(Standard_Number_of_Days_Per_Month);

            foreach (Sys_DictionaryList item in sys_Dictionary.Sys_DictionaryList)
            {
                if (string.Equals(item.DicValue.ToString(), keyValue.ToString().Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    string dicName = item.DicName;
                    isFixedWorkingDay = decimal.TryParse(dicName, out fixedWorkingDay);
                    return (isFixedWorkingDay, fixedWorkingDay);
                }
            }
            return (isFixedWorkingDay, fixedWorkingDay);
        }
    }
}
