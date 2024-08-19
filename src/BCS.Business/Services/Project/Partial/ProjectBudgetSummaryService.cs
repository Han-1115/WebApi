/*
 *所有关于ProjectBudgetSummary类的业务代码应在此处编写
*可使用repository.调用常用方法，获取EF/Dapper等信息
*如果需要事务请使用repository.DbContextBeginTransaction
*也可使用DBServerProvider.手动获取数据库相关信息
*用户信息、权限、角色等使用UserContext.Current操作
*ProjectBudgetSummaryService对增、删、改查、导入、导出、审核业务代码扩展参照ServiceFunFilter
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
using BCS.Business.Repositories;
using BCS.Core.EFDbContext;
using BCS.Core.DBManager;
using BCS.Entity.DTO.Contract;
using BCS.Entity.DTO.Project;
using Confluent.Kafka;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Numeric;
using BCS.Core.ManageUser;
using AutoMapper;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using SkiaSharp;
using System.Collections.Generic;
using StackExchange.Redis;
using System.DrawingCore.Printing;
using BCS.Core.Configuration;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using BCS.Core.Infrastructure;
using BCS.Core.Enums;

namespace BCS.Business.Services
{
    public partial class ProjectBudgetSummaryService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IProjectBudgetSummaryRepository _repository;//访问数据库
        private readonly IProjectBudgetKeyItemRepository _projectBudgetKeyItemRepository;//访问数据库
        private readonly IProjectResourceBudgetRepository _projectResourceBudgetRepository;
        private readonly IProjectPlanInfoRepository _projectPlanInfoRepository;
        private readonly IProjectOtherBudgetRepository _projectOtherBudgetRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly ISys_DepartmentSettingRepository _sys_DepartmentSettingRepository;
        private readonly ISys_CalendarRepository _sys_CalendarRepository;//访问数据库
        private readonly IMapper _mapper;

        /// <summary>
        /// 月标准天数
        /// </summary>
        public const string Standard_Number_of_Days_Per_Month = "Standard_Number_of_Days_Per_Month";

        [ActivatorUtilitiesConstructor]
        public ProjectBudgetSummaryService(
            IProjectBudgetSummaryRepository dbRepository,
            IProjectBudgetKeyItemRepository projectBudgetKeyItemRepository,
            IProjectResourceBudgetRepository projectResourceBudgetRepository,
            IProjectPlanInfoRepository projectPlanInfoRepository,
            IProjectOtherBudgetRepository projectOtherBudgetRepository,
            IProjectRepository projectRepository,
            ISys_CalendarRepository sys_CalendarRepository,
            ISys_DepartmentSettingRepository sys_DepartmentSettingRepository,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper
            )
        : base(dbRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = dbRepository;
            _projectBudgetKeyItemRepository = projectBudgetKeyItemRepository;
            _projectResourceBudgetRepository = projectResourceBudgetRepository;
            _projectPlanInfoRepository = projectPlanInfoRepository;
            _projectOtherBudgetRepository = projectOtherBudgetRepository;
            _projectRepository = projectRepository;
            _sys_CalendarRepository = sys_CalendarRepository;
            _sys_DepartmentSettingRepository = sys_DepartmentSettingRepository;
            _mapper = mapper;
            //多租户会用到这init代码，其他情况可以不用
            //base.Init(dbRepository);
        }

        /// <summary>
        /// 获取项目预算汇总
        /// <para>1001	Income of Own Delivery	自有交付收入</para>
        /// <para>1002	Income of Subcontract (Exclude Tax)	分包收入(不含税)</para>
        /// <para>1003	Labor Cost of Own Delivery	自有交付人力成本</para>
        /// <para>1004	Cost of Subcontract (Exclude Tax)	分包成本(不含税)</para>
        /// <para>1005	Other Project Cost	其它成本费用</para>
        /// <para>1006	GP of Own Delivery	自有交付毛利</para>
        /// <para>1007	GPM of Own Delivery (%)	自有交付毛利率</para>
        /// <para>1008	Project GP	项目毛利</para>
        /// <para>1009	Project GPM (%)	项目毛利率 </para>
        /// </summary>
        /// <param name="projectId">项目Id</param>
        /// <returns></returns>
        public async Task<WebResponseContent> GetProjectBudgetSummary(int projectId)
        {
            UserInfo userInfo = UserContext.Current.UserInfo;
            DateTime currentTime = DateTime.Now;
            userInfo.UserName = string.IsNullOrEmpty(userInfo.UserName) ? string.Empty : userInfo.UserName;
            var project = await _projectRepository.FindAsyncFirst(x => x.Id == projectId);
            // 1、获取项目预算汇总信息
            var budgetSummaries = new List<ProjectBudgetSummary>();
            // 分情况获取预算汇总信息
            if (!_repository.Exists(x => x.Project_Id == projectId))
            {
                #region 1、首次查询时，添加默认的预算汇总信息
                var projectBudgetKeyItems = await _projectBudgetKeyItemRepository.FindAsync(o => o.KeyItemID > 0);
                if (!await _sys_DepartmentSettingRepository.ExistsAsync(o => o.Year == currentTime.Year && o.DepartmentId == Guid.Parse(project.Delivery_Department_Id)))
                {
                    return WebResponseContent.Instance.Error("请维护部门指标配置信息");
                }
                var departmentSetting = await _sys_DepartmentSettingRepository.FindFirstAsync(o => o.Year == currentTime.Year && o.DepartmentId == Guid.Parse(project.Delivery_Department_Id));
                foreach (var item in projectBudgetKeyItems)
                {
                    var insertItem = new ProjectBudgetSummary
                    {
                        Project_Id = projectId,
                        KeyItemID = item.KeyItemID,
                        PlanAmount = 0,
                        PlanAmountScroll = 0,
                        EnableProportionOfProjectAmount = item.EnableProportionOfProjectAmount,
                        ProjectAmountRate = 0,
                        ProjectAmountRateScroll = 0,
                        EnableDepartmentMetric = item.EnableDepartmentMetric,
                        DepartmentMetric = 0,
                        DeviationExplanation = string.Empty,
                        Remark = string.Empty,
                        CreateID = userInfo.User_Id,
                        Creator = userInfo.UserName,
                        CreateDate = currentTime,
                        ModifyID = userInfo.User_Id,
                        Modifier = userInfo.UserName,
                        ModifyDate = currentTime,
                    };
                    //KeyItemOrder	KeyItemEn	KeyItemCn
                    //1001	Income of Own Delivery	自有交付收入
                    //1002	Income of Subcontract (Exclude Tax)	分包收入(不含税)
                    //1003	Labor Cost of Own Delivery	自有交付人力成本
                    //1004	Cost of Subcontract (Exclude Tax)	分包成本(不含税)
                    //1005	Other Project Cost	其它成本费用
                    //1006	GP of Own Delivery	自有交付毛利
                    //1007	GPM of Own Delivery (%)	自有交付毛利率
                    //1008	Project GP	项目毛利
                    //1009	Project GPM (%)	项目毛利率 
                    switch (insertItem.KeyItemID)
                    {
                        case 1003:
                            {
                                #region 自有交付人力成本
                                insertItem.DepartmentMetric = departmentSetting.LaborCostofOwnDelivery ?? 0;
                                #endregion
                            }
                            break;
                        case 1009:
                            {
                                #region 项目毛利率
                                insertItem.DepartmentMetric = departmentSetting.ProjectGPM ?? 0;
                                #endregion
                            }
                            break;
                        default:
                            break;
                    }

                    budgetSummaries.Add(insertItem);
                }

                // 2、TODO: 需要计算预算汇总信息
                await this.CalculateBudgetSummary(projectId, budgetSummaries);

                try
                {
                    repository.AddRange(budgetSummaries, true);
                }
                catch (Exception ex)
                {
                    return WebResponseContent.Instance.Error($"保存项目预算汇总信息异常:[{ex.Message}]");
                }

                #endregion
            }
            else
            {
                //重要：根据项目状态变化，只有项目状态为：【Approval_Status==未发起】时，才更新预算汇总信息
                if (project.Approval_Status == 4)
                {
                    try
                    {
                        budgetSummaries = _repository.Find(x => x.Project_Id == projectId).ToList();

                        #region 项目重新提交时，获取系统维护的最新部门指标
                        var projectBudgetKeyItems = await _projectBudgetKeyItemRepository.FindAsync(o => o.KeyItemID > 0);
                        if (!await _sys_DepartmentSettingRepository.ExistsAsync(o => o.Year == currentTime.Year && o.DepartmentId == Guid.Parse(project.Delivery_Department_Id)))
                        {
                            return WebResponseContent.Instance.Error("请维护部门指标配置信息");
                        }
                        var departmentSetting = await _sys_DepartmentSettingRepository.FindFirstAsync(o => o.Year == currentTime.Year && o.DepartmentId == Guid.Parse(project.Delivery_Department_Id));
                        foreach (var item in projectBudgetKeyItems)
                        {
                            foreach (var existsItem in budgetSummaries)
                            {
                                if (existsItem.KeyItemID == item.KeyItemID)
                                {
                                    //KeyItemOrder	KeyItemEn	KeyItemCn
                                    //1001	Income of Own Delivery	自有交付收入
                                    //1002	Income of Subcontract (Exclude Tax)	分包收入(不含税)
                                    //1003	Labor Cost of Own Delivery	自有交付人力成本
                                    //1004	Cost of Subcontract (Exclude Tax)	分包成本(不含税)
                                    //1005	Other Project Cost	其它成本费用
                                    //1006	GP of Own Delivery	自有交付毛利
                                    //1007	GPM of Own Delivery (%)	自有交付毛利率
                                    //1008	Project GP	项目毛利
                                    //1009	Project GPM (%)	项目毛利率 
                                    switch (existsItem.KeyItemID)
                                    {
                                        case 1003:
                                            {
                                                #region 自有交付人力成本
                                                existsItem.DepartmentMetric = departmentSetting.LaborCostofOwnDelivery ?? 0;
                                                #endregion
                                            }
                                            break;
                                        case 1009:
                                            {
                                                #region 项目毛利率
                                                existsItem.DepartmentMetric = departmentSetting.ProjectGPM ?? 0;
                                                #endregion
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                }
                            }
                        }

                        #endregion
                        //已存在时，获取后更新预算汇总信息  
                        await this.CalculateBudgetSummary(projectId, budgetSummaries);
                        int result = repository.UpdateRange(budgetSummaries, true);
                    }
                    catch (Exception ex)
                    {
                        return WebResponseContent.Instance.Error($"更新项目预算汇总信息异常:[{ex.Message}]");
                    }
                }
            }

            BCSContext dbContext = DBServerProvider.GetEFDbContext();

            // 每次获取此接口时，根据项目状态确定是否需要计算预算汇总信息
            // 3、向前端返回预算汇总信息
            var projectBudgetSummary = (from budgetSummary in dbContext.Set<ProjectBudgetSummary>()
                                        where budgetSummary.Project_Id == projectId
                                        join keyItem in dbContext.Set<ProjectBudgetKeyItem>()
                                        on new { budgetSummary.KeyItemID } equals new { keyItem.KeyItemID }
                                        select new ProjectBudgetSummaryDTO
                                        {
                                            KeyItemEn = keyItem.KeyItemEn,
                                            KeyItemCn = keyItem.KeyItemCn,
                                            Id = budgetSummary.Id,
                                            Project_Id = budgetSummary.Project_Id,
                                            KeyItemID = budgetSummary.KeyItemID,
                                            PlanAmount = budgetSummary.PlanAmount,
                                            PlanAmountScroll = budgetSummary.PlanAmountScroll,
                                            EnableProportionOfProjectAmount = budgetSummary.EnableProportionOfProjectAmount,
                                            ProjectAmountRate = budgetSummary.ProjectAmountRate,
                                            ProjectAmountRateScroll = budgetSummary.ProjectAmountRateScroll,
                                            EnableDepartmentMetric = budgetSummary.EnableDepartmentMetric,
                                            DepartmentMetric = budgetSummary.DepartmentMetric,
                                            DeviationExplanation = budgetSummary.DeviationExplanation,
                                            Remark = budgetSummary.Remark,
                                            CreateID = budgetSummary.CreateID,
                                            Creator = budgetSummary.Creator,
                                            CreateDate = budgetSummary.CreateDate,
                                            ModifyID = budgetSummary.ModifyID,
                                            Modifier = budgetSummary.Modifier,
                                            ModifyDate = budgetSummary.ModifyDate
                                        }).ToList();

            return await Task.FromResult(WebResponseContent.Instance.OK("获取项目预算汇总成功", projectBudgetSummary));
        }

        /// <summary>
        /// 保存-项目其它成本费用预算
        /// </summary>
        /// <param name="projectId">项目id</param>
        /// <param name="projectBudgetSumary">项目其它成本费用预算</param>
        /// <returns></returns>
        public async Task<WebResponseContent> SaveProjectBudgetSummary(int projectId, ICollection<ProjectBudgetSummaryDTO> projectBudgetSumary)
        {
            UserInfo userInfo = UserContext.Current.UserInfo;
            DateTime currentTime = DateTime.Now;
            userInfo.UserName = string.IsNullOrEmpty(userInfo.UserName) ? string.Empty : userInfo.UserName;
            //项目资源预算

            var existsList = await _repository.FindAsync(x => x.Project_Id == projectId);
            var insertList = new List<ProjectBudgetSummary>();
            var updateList = new List<ProjectBudgetSummary>();
            var removeList = new List<ProjectBudgetSummary>();

            foreach (var item in projectBudgetSumary)
            {
                var projectBudgetSummary = _mapper.Map<ProjectBudgetSummary>(item);
                if (projectBudgetSummary.Id <= 0)
                {
                    //新增业务
                    projectBudgetSummary.Project_Id = projectId;
                    projectBudgetSummary.CreateID = userInfo.User_Id;
                    projectBudgetSummary.Creator = userInfo.UserName;
                    projectBudgetSummary.CreateDate = currentTime;
                    projectBudgetSummary.ModifyID = userInfo.User_Id;
                    projectBudgetSummary.Modifier = userInfo.UserName;
                    projectBudgetSummary.ModifyDate = currentTime;
                    insertList.Add(projectBudgetSummary);
                }
                else
                {
                    //更新业务
                    projectBudgetSummary.ModifyID = userInfo.User_Id;
                    projectBudgetSummary.Modifier = userInfo.UserName;
                    projectBudgetSummary.ModifyDate = currentTime;
                    updateList.Add(projectBudgetSummary);
                }
            }
            //删除业务
            foreach (var item in existsList)
            {
                if (projectBudgetSumary.Any(o => o.Id == item.Id))
                {
                    continue;
                }
                var projectBudgetSummary = _mapper.Map<ProjectBudgetSummary>(item);
                removeList.Add(projectBudgetSummary);
            }
            BCSContext dbContext = DBServerProvider.GetEFDbContext();
            using (var transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    repository.AddRange(insertList);
                    repository.UpdateRange(updateList);
                    if (removeList.Count > 0)
                    {
                        repository.DeleteWithKeys(removeList.Select(o => (object)o.Id).ToArray());
                    }

                    dbContext.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception($"更新项目预算汇总异常:[{ex.Message}]");
                }
            }
            return WebResponseContent.Instance.OK("更新项目预算汇总成功");
        }

        /// <summary>
        /// 计算项目预算汇总信息
        /// <para>重要：根据项目状态变化，只有项目状态为：【Approval_Status==未发起】时，才需要计算预算汇总信息</para>
        /// <para> Cost_Rate 为人民币</para>
        /// <para> Charge_Rate 为合同原币种</para>
        /// <para>项目预算汇总信息：统一用原币种计算</para>
        /// <para>1001	Income of Own Delivery	自有交付收入</para>
        /// <para>1002	Income of Subcontract (Exclude Tax)	分包收入(不含税)</para>
        /// <para>1003	Labor Cost of Own Delivery	自有交付人力成本</para>
        /// <para>1004	Cost of Subcontract (Exclude Tax)	分包成本(不含税)</para>
        /// <para>1005	Other Project Cost	其它成本费用</para>
        /// <para>1006	GP of Own Delivery	自有交付毛利</para>
        /// <para>1007	GPM of Own Delivery (%)	自有交付毛利率</para>
        /// <para>1008	Project GP	项目毛利</para>
        /// <para>1009	Project GPM (%)	项目毛利率 </para>
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        private async Task CalculateBudgetSummary(int projectId, List<ProjectBudgetSummary> budgetSummaries)
        {
            // 0、获取项目信息
            var project = await _projectRepository.FindAsyncFirst(x => x.Id == projectId);
            // 重要：根据项目状态变化，只有项目状态为：【Approval_Status==未发起】时，才需要计算预算汇总信息
            if (project.Approval_Status != 4)
            {
                return;
            }
            if (project.Project_Amount <= 0)
            {
                //throw new Exception($"项目金额为:[{project.Project_Amount}]，无法进行计算。");
                return;
            }
            //查询项目合同 无合同时，直接返回
            BCSContext context = DBServerProvider.GetEFDbContext();
            var contractContainer = (from contractProject in context.Set<ContractProject>().Where(s => s.Status == (int)Status.Active)
                                     where contractProject.Project_Id == projectId
                                     join contract in context.Set<Contract>()
                                     on new { contractProject.Contract_Id } equals new { Contract_Id = contract.Id }
                                     select contract).ToList();
            if (contractContainer.Count <= 0)
            {
                //throw new Exception($"无合同，无法计算。");
                return;
            }
            var projectContract = contractContainer.First();
            // 3、获取项目资源预算信息
            var projectResourceBudgetS = await _projectResourceBudgetRepository.FindAsync(x => x.Project_Id == projectId);
            if (projectResourceBudgetS.Count <= 0)
            {
                return;
            }
            UserInfo userInfo = UserContext.Current.UserInfo;
            DateTime currentTime = DateTime.Now;
            userInfo.UserName = string.IsNullOrEmpty(userInfo.UserName) ? string.Empty : userInfo.UserName;
            //// 2、获取项目节段信息
            //var projectPlanInfoS = await _projectPlanInfoRepository.FindAsync(x => x.Project_Id == projectId);
            // 3、获取项目其它预算信息
            var projectOtherBudgetS = await _projectOtherBudgetRepository.FindAsync(x => x.Project_Id == projectId);
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
            // 按资源预算的记录计算
            //1001	Income of Own Delivery	自有交付收入
            //1002	Income of Subcontract (Exclude Tax)	分包收入(不含税)
            //1003	Labor Cost of Own Delivery	自有交付人力成本
            //1004	Cost of Subcontract (Exclude Tax)	分包成本(不含税)
            //1005	Other Project Cost	其它成本费用
            //1006	GP of Own Delivery	自有交付毛利
            //1007	GPM of Own Delivery (%)	自有交付毛利率 
            //1008	Project GP	项目毛利
            //1009	Project GPM (%)	项目毛利率 
            decimal incomeofOwnDelivery = 0; //1001    Income of Own Delivery	自有交付收入
            decimal incomeofSubcontract_Exclude_Tax = 0; //1002    Income of Subcontract (Exclude Tax)	分包收入(不含税)
            decimal laborCostofOwnDelivery = 0;//1003    Labor Cost of Own Delivery	自有交付人力成本
            decimal costofSubcontract_Exclude_Tax = 0;//1004    Cost of Subcontract (Exclude Tax)	分包成本(不含税)
            decimal otherProjectCost = 0;//1005    Other Project Cost	其它成本费用
            decimal gPofOwnDelivery = 0;//1006    GP of Own Delivery	自有交付毛利
            decimal gPMofOwnDelivery = 0;//1007    GPM of Own Delivery (%)	自有交付毛利率
            decimal projectGP = 0;//1008    Project GP	项目毛利
            decimal projectGPM = 0;//1009    Project GPM (%)	项目毛利率
            // 计算项目预算汇总信息
            var budgetSummary = this.CalculateProjectBudgetSummary(projectResourceBudgetS, sys_CalendarRecord, project, projectContract);
            // 更新实体
            //KeyItemOrder	KeyItemEn	KeyItemCn
            //项目原币金额
            var projectAmount_OriginalCurrency = project.Project_Amount;
            //1001	Income of Own Delivery	自有交付收入(不含税)
            incomeofOwnDelivery = budgetSummary.IncomeofOwnDelivery;
            incomeofOwnDelivery = incomeofOwnDelivery > projectAmount_OriginalCurrency ? projectAmount_OriginalCurrency : incomeofOwnDelivery;
            incomeofOwnDelivery = CalculateByTaxRate(incomeofOwnDelivery, project, projectContract);
            //1002	Income of Subcontract (Exclude Tax)	分包收入(不含税)
            incomeofSubcontract_Exclude_Tax = projectOtherBudgetS.Sum(o => o.Subcontracting_Income);
            incomeofSubcontract_Exclude_Tax = CalculateByTaxRate(incomeofSubcontract_Exclude_Tax, project, projectContract);
            //1003	Labor Cost of Own Delivery	自有交付人力成本
            laborCostofOwnDelivery = budgetSummary.LaborCostofOwnDelivery;
            laborCostofOwnDelivery = this.CalculateByExchangeRate(laborCostofOwnDelivery, project, projectContract);
            //1004	Cost of Subcontract (Exclude Tax)	分包成本(不含税)
            costofSubcontract_Exclude_Tax = projectOtherBudgetS.Sum(o => o.Subcontracting_Cost);
            costofSubcontract_Exclude_Tax = CalculateByTaxRate(costofSubcontract_Exclude_Tax, project, projectContract);
            //1005	Other Project Cost	其它成本费用
            otherProjectCost += projectOtherBudgetS.Sum(o => o.Bonus_Cost + o.Travel_Cost + o.Reimbursement_Cost + o.Other_Cost);
            //1006	GP of Own Delivery	自有交付毛利
            gPofOwnDelivery = incomeofOwnDelivery - laborCostofOwnDelivery;
            //1007	GPM of Own Delivery (%)	自有交付毛利率
            gPMofOwnDelivery = (gPofOwnDelivery / incomeofOwnDelivery) * 100;
            //1008	Project GP	项目毛利
            projectGP = incomeofOwnDelivery - laborCostofOwnDelivery - otherProjectCost;
            //1009	Project GPM (%)	项目毛利率 
            projectGPM = (projectGP / incomeofOwnDelivery) * 100;

            foreach (var item in budgetSummaries)
            {
                item.ModifyID = userInfo.User_Id;
                item.Modifier = userInfo.UserName;
                item.ModifyDate = currentTime;
                switch (item.KeyItemID)
                {
                    case 1001:
                        {
                            #region 自有交付收入
                            item.PlanAmount = Math.Round(incomeofOwnDelivery, 2);
                            item.ProjectAmountRate = Math.Round((incomeofOwnDelivery / projectAmount_OriginalCurrency) * 100, 2);
                            #endregion
                        }
                        break;
                    case 1002:
                        {
                            #region 分包收入(不含税)
                            item.PlanAmount = Math.Round(incomeofSubcontract_Exclude_Tax, 2);
                            #endregion
                        }
                        break;
                    case 1003:
                        {
                            #region 自有交付人力成本
                            item.PlanAmount = Math.Round(laborCostofOwnDelivery, 2);
                            item.ProjectAmountRate = Math.Round((laborCostofOwnDelivery / projectAmount_OriginalCurrency) * 100, 2);
                            #endregion
                        }
                        break;
                    case 1004:
                        {
                            #region 分包成本(不含税)
                            item.PlanAmount = Math.Round(costofSubcontract_Exclude_Tax, 2);
                            item.ProjectAmountRate = Math.Round((costofSubcontract_Exclude_Tax / projectAmount_OriginalCurrency) * 100, 2);
                            #endregion
                        }
                        break;
                    case 1005:
                        {
                            #region 其它成本费用
                            item.PlanAmount = otherProjectCost;
                            item.ProjectAmountRate = Math.Round((otherProjectCost / projectAmount_OriginalCurrency) * 100, 2);
                            #endregion
                        }
                        break;
                    case 1006:
                        {
                            #region 自有交付毛利
                            item.PlanAmount = Math.Round(gPofOwnDelivery, 2);
                            #endregion
                        }
                        break;
                    case 1007:
                        {
                            #region 自有交付毛利率
                            item.PlanAmount = Math.Round(gPMofOwnDelivery, 2);
                            #endregion
                        }
                        break;
                    case 1008:
                        {
                            #region 项目毛利
                            item.PlanAmount = Math.Round(projectGP, 2);
                            #endregion
                        }
                        break;
                    case 1009:
                        {
                            #region 项目毛利率
                            item.PlanAmount = Math.Round(projectGPM, 2);
                            #endregion
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// 根据税率计算金额
        /// </summary>
        /// <param name="inputvalue"></param>
        /// <param name="project"></param>
        /// <param name="contract"></param>
        /// <returns></returns>
        private decimal CalculateByTaxRate(decimal inputvalue, Project project, Contract contract)
        {
            //Holiday_SystemId: 
            //China:1
            //US: 2
            //India: 3
            //South Korea:4
            //Japan: 5
            //Phillipines: 6
            decimal Tax_Rate = 0;
            string temp_Tax_Rate = string.Empty;
            // 项目类型Id
            // <para>1:Delivery Project</para>
            // <para>2:Procurement Project</para>
            switch (project.Project_TypeId)
            {
                case 1:
                    temp_Tax_Rate = string.IsNullOrEmpty(contract.Tax_Rate_No_Purchase) ? string.Empty : contract.Tax_Rate_No_Purchase.Replace("%", "");
                    break;
                case 2:
                    temp_Tax_Rate = string.IsNullOrEmpty(contract.Tax_Rate) ? string.Empty : contract.Tax_Rate.Replace("%", "");
                    break;
            }
            Tax_Rate = string.IsNullOrEmpty(temp_Tax_Rate) ? 0 : decimal.Parse(temp_Tax_Rate);
            Tax_Rate = Tax_Rate / 100;
            inputvalue = contract.Settlement_Currency.Trim() switch
            {
                "CNY" => inputvalue / (1 + Tax_Rate),
                "USD" => inputvalue * (1 - Tax_Rate),
                "INR" => inputvalue * (1 - Tax_Rate),
                "JPY" => inputvalue * (1 - Tax_Rate),
                //"SGD" => inputvalue * (1 - Tax_Rate),
                //"KRW" => inputvalue * (1 - Tax_Rate),
                _ => inputvalue,
            };
            return inputvalue;
        }

        /// <summary>
        /// 根据汇率计算金额
        /// </summary>
        /// <param name="inputvalue"></param>
        /// <param name="project"></param>
        /// <param name="contract"></param>
        /// <returns></returns>
        private decimal CalculateByExchangeRate(decimal inputvalue, Project project, Contract contract)
        {
            //汇率
            decimal Exchange_Rate = string.IsNullOrEmpty(contract.Exchange_Rate) ? 1 : decimal.Parse(contract.Exchange_Rate);
            inputvalue = inputvalue / Exchange_Rate;
            return inputvalue;
        }

        /// <summary>
        /// 计算 
        /// <para>1001:Income of Own Delivery	自有交付收入</para>
        /// <para>1003:Labor Cost of Own Delivery	自有交付人力成本</para>
        /// </summary>
        /// <param name="projectResourceBudgets"></param>
        /// <param name="sys_CalendarRecord"></param>
        /// <param name="project"></param>
        /// <param name="contract"></param>
        /// <returns></returns>
        private (decimal IncomeofOwnDelivery, decimal LaborCostofOwnDelivery) CalculateProjectBudgetSummary(List<ProjectResourceBudget> projectResourceBudgets, List<Sys_Calendar> sys_CalendarRecord, Project project, Contract contract)
        {
            decimal incomeofOwnDelivery = 0;
            decimal laborCostofOwnDelivery = 0;
            // 根据项目节段计算出 项目横跨的年月信息
            var planIds = projectResourceBudgets.Select(o => o.ProjectPlanInfo_Id).Distinct().ToList();
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
            //自有交付收入-计算逻辑
            decimal CalculateIncomeofOwnDelivery(bool isFullMonth, bool isFixedWorkingDay, decimal charge_Rate, decimal fixedWorkingDay, decimal workingDay, decimal actualWorkingDay, decimal headCount)
            {
                //分情况：人时、人天，人月
                decimal returnValue = 0;
                if (isFullMonth)
                {
                    switch (contract.Charge_Rate_Unit.ToLower())
                    {
                        case "manhour":
                            {
                                //完整月
                                //value=Charge_Rate*月工作日天数*日标准小时数*HeadCount
                                returnValue += charge_Rate * (isFixedWorkingDay ? fixedWorkingDay : workingDay) * project.Standard_Daily_Hours * headCount;
                            }
                            break;
                        case "manday":
                            {
                                //完整月
                                //value=Charge_Rate*月工作日天数*日标准小时数*HeadCount
                                returnValue += charge_Rate * (isFixedWorkingDay ? fixedWorkingDay : workingDay) * headCount;
                            }
                            break;
                        case "manmonth":
                            {
                                //value=Charge_Rate*HeadCount               
                                returnValue += charge_Rate * headCount;
                            }
                            break;
                    }
                }
                else
                {
                    switch (contract.Charge_Rate_Unit.ToLower())
                    {
                        case "manhour":
                            {
                                //固定工作与与非固定工作日天数计算逻辑一样
                                //value=Charge_Rate*实际工作日天数*日标准小时数*HeadCount
                                returnValue += charge_Rate * actualWorkingDay * project.Standard_Daily_Hours * headCount;
                            }
                            break;
                        case "manday":
                            {
                                //value=Charge_Rate*实际工作日天数*HeadCount               
                                returnValue += charge_Rate * actualWorkingDay * headCount;
                            }
                            break;
                        case "manmonth":
                            {
                                if (isFixedWorkingDay)
                                {
                                    ////月固定工作日天数计算逻辑
                                    //value=Charge_Rate*(实际工作日天数/当月工作日天数)*固定工作日天数*HeadCount
                                    returnValue += charge_Rate * (actualWorkingDay / workingDay) * fixedWorkingDay * headCount / fixedWorkingDay;
                                }
                                else
                                {
                                    //value=Charge_Rate/月工作日天数*实际工作日天数*HeadCount               
                                    returnValue += charge_Rate / workingDay * actualWorkingDay * headCount;
                                }
                            }
                            break;
                    }
                }
                return returnValue;
            }

            //自有交付人力成本-计算逻辑
            decimal CalculateLaborCostofOwnDelivery(bool isFullMonth, bool isFixedWorkingDay, decimal cost_Rate, decimal fixedWorkingDay, decimal workingDay, decimal actualWorkingDay, decimal headCount)
            {
                decimal returnValue = 0;
                if (isFullMonth)
                {
                    //完整月
                    //value=Cost_Rate*HeadCount
                    returnValue += cost_Rate * headCount;
                }
                else
                {
                    //不完整月 
                    if (isFixedWorkingDay)
                    {
                        ////月固定工作日天数计算逻辑
                        //value=Cost_Rate*(实际工作日天数/当月工作日天数)*固定工作日天数*HeadCount/固定工作日天数
                        returnValue += cost_Rate * (actualWorkingDay / workingDay) * fixedWorkingDay * headCount / fixedWorkingDay;
                    }
                    else
                    {
                        //value=Cost_Rate/工作日天数*实际工作天数*HeadCount
                        returnValue += cost_Rate / workingDay * actualWorkingDay * headCount;
                    }
                }
                return returnValue;
            }

            // 获取每月标准天数
            //(bool fromSysCalendar, decimal standardWorkingDays) = GetStandard_Number_of_Days_Per_Month(project.Standard_Number_of_Days_Per_MonthId);
            var workingDaySetting = GetStandard_Number_of_Days_Per_Month(project.Standard_Number_of_Days_Per_MonthId);
            var resourceFormUi = projectResourceBudgets.ToList();
            foreach (var item in listYearMonth)
            {
                //当前月的最大开始日期
                DateTime currentYearMonthStartMax = new DateTime(item.Year, item.Month, DateTime.DaysInMonth(item.Year, item.Month));
                //当前月的最小结束日期
                DateTime currentYearMonthEndMin = new DateTime(item.Year, item.Month, 1);
                Predicate<ProjectResourceBudget> predicate = o =>
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
                        #region 同年同月

                        //每月工作天数
                        decimal workingDayCount = sys_CalendarRecord.Where(o =>
                            o.Year == budgetDTO.Start_Date.Year
                            && o.Month == budgetDTO.Start_Date.Month
                            && o.IsWorkingDay == 1).Count();
                        bool isFullMonth = budgetDTO.Start_Date.Day == 1 && budgetDTO.End_Date.Day == DateTime.DaysInMonth(budgetDTO.Start_Date.Year, budgetDTO.Start_Date.Month);
                        if (isFullMonth)
                        {
                            //完整月
                            incomeofOwnDelivery += CalculateIncomeofOwnDelivery(isFullMonth, workingDaySetting.IsFixedWorkingDay, budgetDTO.Charge_Rate, workingDaySetting.FixedWorkingDay, workingDayCount, workingDayCount, budgetDTO.HeadCount);
                            laborCostofOwnDelivery += CalculateLaborCostofOwnDelivery(isFullMonth, workingDaySetting.IsFixedWorkingDay, budgetDTO.Cost_Rate, workingDaySetting.FixedWorkingDay, workingDayCount, workingDayCount, budgetDTO.HeadCount);
                        }
                        else
                        {
                            decimal actualWorkingCount = sys_CalendarRecord.Where(o =>
                                o.Year == budgetDTO.Start_Date.Year
                                && o.Month == budgetDTO.Start_Date.Month
                                && o.IsWorkingDay == 1
                                && o.Day >= budgetDTO.Start_Date.Day
                                && o.Day <= budgetDTO.End_Date.Day).Count();

                            incomeofOwnDelivery += CalculateIncomeofOwnDelivery(isFullMonth, workingDaySetting.IsFixedWorkingDay, budgetDTO.Charge_Rate, workingDaySetting.FixedWorkingDay, workingDayCount, actualWorkingCount, budgetDTO.HeadCount);
                            laborCostofOwnDelivery += CalculateLaborCostofOwnDelivery(isFullMonth, workingDaySetting.IsFixedWorkingDay, budgetDTO.Cost_Rate, workingDaySetting.FixedWorkingDay, workingDayCount, actualWorkingCount, budgetDTO.HeadCount);
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
                                incomeofOwnDelivery += CalculateIncomeofOwnDelivery(isFullMonth, workingDaySetting.IsFixedWorkingDay, budgetDTO.Charge_Rate, workingDaySetting.FixedWorkingDay, workingDayCount, workingDayCount, budgetDTO.HeadCount);
                                laborCostofOwnDelivery += CalculateLaborCostofOwnDelivery(isFullMonth, workingDaySetting.IsFixedWorkingDay, budgetDTO.Cost_Rate, workingDaySetting.FixedWorkingDay, workingDayCount, workingDayCount, budgetDTO.HeadCount);
                            }
                            else
                            {
                                //当前待计算的人月年月是当前预算的开始月
                                decimal actualWorkingCount = sys_CalendarRecord.Where(o =>
                                                           o.Year == budgetDTO.Start_Date.Year
                                                           && o.Month == budgetDTO.Start_Date.Month
                                                           && o.IsWorkingDay == 1
                                                           && o.Day >= budgetDTO.Start_Date.Day).Count();
                                incomeofOwnDelivery += CalculateIncomeofOwnDelivery(isFullMonth, workingDaySetting.IsFixedWorkingDay, budgetDTO.Charge_Rate, workingDaySetting.FixedWorkingDay, workingDayCount, actualWorkingCount, budgetDTO.HeadCount);
                                laborCostofOwnDelivery += CalculateLaborCostofOwnDelivery(isFullMonth, workingDaySetting.IsFixedWorkingDay, budgetDTO.Cost_Rate, workingDaySetting.FixedWorkingDay, workingDayCount, actualWorkingCount, budgetDTO.HeadCount);
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
                                incomeofOwnDelivery += CalculateIncomeofOwnDelivery(isFullMonth, workingDaySetting.IsFixedWorkingDay, budgetDTO.Charge_Rate, workingDaySetting.FixedWorkingDay, workingDayCount, workingDayCount, budgetDTO.HeadCount);
                                laborCostofOwnDelivery += CalculateLaborCostofOwnDelivery(isFullMonth, workingDaySetting.IsFixedWorkingDay, budgetDTO.Cost_Rate, workingDaySetting.FixedWorkingDay, workingDayCount, workingDayCount, budgetDTO.HeadCount);
                            }
                            else
                            {
                                //当前待计算的人月年月是当前预算的结束月
                                decimal actualWorkingCount = sys_CalendarRecord.Where(o =>
                                                           o.Year == budgetDTO.End_Date.Year
                                                           && o.Month == budgetDTO.End_Date.Month
                                                           && o.IsWorkingDay == 1
                                                           && o.Day <= budgetDTO.End_Date.Day).Count();
                                incomeofOwnDelivery += CalculateIncomeofOwnDelivery(isFullMonth, workingDaySetting.IsFixedWorkingDay, budgetDTO.Charge_Rate, workingDaySetting.FixedWorkingDay, workingDayCount, actualWorkingCount, budgetDTO.HeadCount);
                                laborCostofOwnDelivery += CalculateLaborCostofOwnDelivery(isFullMonth, workingDaySetting.IsFixedWorkingDay, budgetDTO.Cost_Rate, workingDaySetting.FixedWorkingDay, workingDayCount, actualWorkingCount, budgetDTO.HeadCount);
                            }
                        }
                        else
                        {
                            //全月
                            decimal workingDayCount = sys_CalendarRecord.Where(o =>
                                                   o.Year == item.Year
                                                   && o.Month == item.Month
                                                   && o.IsWorkingDay == 1).Count();
                            incomeofOwnDelivery += CalculateIncomeofOwnDelivery(true, workingDaySetting.IsFixedWorkingDay, budgetDTO.Charge_Rate, workingDaySetting.FixedWorkingDay, workingDayCount, workingDayCount, budgetDTO.HeadCount);
                            laborCostofOwnDelivery += CalculateLaborCostofOwnDelivery(true, workingDaySetting.IsFixedWorkingDay, budgetDTO.Cost_Rate, workingDaySetting.FixedWorkingDay, workingDayCount, workingDayCount, budgetDTO.HeadCount);
                        }

                        #endregion
                    }
                }
                Console.WriteLine($"{item.YearMonth}:[incomeofOwnDelivery:{incomeofOwnDelivery}][laborCostofOwnDelivery:{laborCostofOwnDelivery}]");
            }
            return (incomeofOwnDelivery, laborCostofOwnDelivery);
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