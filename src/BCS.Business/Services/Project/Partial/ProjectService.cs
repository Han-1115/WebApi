/*
 *所有关于Project类的业务代码应在此处编写
*可使用repository.调用常用方法，获取EF/Dapper等信息
*如果需要事务请使用repository.DbContextBeginTransaction
*也可使用DBServerProvider.手动获取数据库相关信息
*用户信息、权限、角色等使用UserContext.Current操作
*ProjectService对增、删、改查、导入、导出、审核业务代码扩展参照ServiceFunFilter
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
using BCS.Business.Repositories;
using BCS.Entity.DTO.Contract;
using AutoMapper;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using BCS.Core.ManageUser;
using BCS.Core.EFDbContext;
using BCS.Core.Enums;
using System.Diagnostics.Contracts;
using BCS.Core.DBManager;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Numeric;
using System;
using BCS.Core.ConverterContainer;
using BCS.Business.IServices;
using BCS.Core.WorkFlow;
using Microsoft.Data.SqlClient;
using static Dapper.SqlMapper;
using BCS.Core.Configuration;
using System.Security.Policy;
using System.IO;
using SkiaSharp;
using BCS.Core.Const;
using BCS.Business.IRepositories.System;

namespace BCS.Business.Services
{
    public partial class ProjectService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IProjectRepository _repository;//访问数据库
        private readonly IContractRepository _contractRepository;//访问数据库
        private readonly IContractProjectRepository _contractProjectRepository;//访问数据库
        private readonly IClientRepository _clientRepository;//访问数据库
        private readonly IProjectAttachmentListRepository _projectAttachmentListRepository;//访问数据库
        private readonly IProjectAttachmentListHistoryRepository _projectAttachmentListHistoryRepository;//访问数据库
        private readonly IProjectAttachmentListService _projectAttachmentListService;//访问业务代码
        private readonly IProjectPlanInfoRepository _projectPlanInfoRepository;//访问数据库
        private readonly IProjectPlanInfoHistoryRepository _projectPlanInfoHistoryRepository;//访问数据库
        private readonly IProjectPlanInfoService _projectPlanInfoService;//访问业务代码
        private readonly IProjectResourceBudgetRepository _projectResourceBudgetRepository;//访问数据库
        private readonly IProjectResourceBudgetHistoryRepository _projectResourceBudgetHistoryRepository;//访问数据库
        private readonly IProjectResourceBudgetHCRepository _projectResourceBudgetHCRepository;//访问数据库
        private readonly IProjectResourceBudgetHCHistoryRepository _projectResourceBudgetHCHistoryRepository;//访问数据库
        private readonly IProjectResourceBudgetService _projectResourceBudgetService;//访问业务代码
        private readonly IProjectOtherBudgetRepository _projectOtherBudgetRepository;//访问数据库
        private readonly IProjectOtherBudgetHistoryRepository _projectOtherBudgetHistoryRepository;//访问数据库
        private readonly IProjectOtherBudgetService _projectOtherBudgetService;//访问业务代码
        private readonly IProjectBudgetSummaryRepository _projectBudgetSummaryRepository;//访问数据库
        private readonly IProjectBudgetSummaryService _projectBudgetSummaryService;//访问业务代码
        private readonly IProjectHistoryRepository _projectHistoryRepository;//访问数据库
        private readonly IContractProjectHistoryRepository _contractProjectHistoryRepository;//访问数据库
        private readonly IContractHistoryRepository _contractHistoryRepository;//访问数据库
        private readonly ISys_CalendarRepository _sys_CalendarRepository;//访问数据库
        private readonly IEmailTemplateRepository _emailTemplateRepository;//访问数据库
        private readonly IEmailSendLogRepository _emailSendLogRepository;//访问数据库
        private readonly ISys_DepartmentSettingRepository _sys_DepartmentSettingRepository;
        private readonly IMapper _mapper;

        [ActivatorUtilitiesConstructor]
        public ProjectService(
            IProjectRepository dbRepository,
            IContractRepository contractRepository,
            IContractProjectRepository contractProjectRepository,
            IClientRepository clientRepository,
            IProjectAttachmentListRepository projectAttachmentListRepository,
            IProjectAttachmentListService projectAttachmentListService,
            IProjectPlanInfoRepository projectPlanInfoRepository,
            IProjectPlanInfoService projectPlanInfoService,
            IProjectResourceBudgetRepository projectResourceBudgetRepository,
            IProjectResourceBudgetHCRepository projectResourceBudgetHCRepository,
            IProjectResourceBudgetService projectResourceBudgetService,
            IProjectOtherBudgetRepository projectOtherBudgetRepository,
            IProjectOtherBudgetService projectOtherBudgetService,
            IProjectBudgetSummaryRepository projectBudgetSummaryRepository,
            IProjectBudgetSummaryService projectBudgetSummaryService,
            IProjectHistoryRepository projectHistoryRepository,
            ISys_CalendarRepository sys_CalendarRepository,
            IEmailTemplateRepository emailTemplateRepository,
            IEmailSendLogRepository emailSendLogRepository,
            ISys_DepartmentSettingRepository sys_DepartmentSettingRepository,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper
,
            Magicodes.ExporterAndImporter.Excel.IExcelExporter exporter,
            IContractProjectHistoryRepository contractProjectHistoryRepository,
            IContractHistoryRepository contractHistoryRepository,
            IProjectAttachmentListHistoryRepository projectAttachmentListHistoryRepository,
            IProjectPlanInfoHistoryRepository projectPlanInfoHistoryRepository,
            IProjectResourceBudgetHistoryRepository projectResourceBudgetHistoryRepository,
            IProjectResourceBudgetHCHistoryRepository projectResourceBudgetHCHistoryRepository,
            IProjectOtherBudgetHistoryRepository projectOtherBudgetHistoryRepository)
        : base(dbRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = dbRepository;
            _contractRepository = contractRepository;
            _contractProjectRepository = contractProjectRepository;
            _clientRepository = clientRepository;
            _projectAttachmentListRepository = projectAttachmentListRepository;
            _projectAttachmentListService = projectAttachmentListService;
            _projectPlanInfoRepository = projectPlanInfoRepository;
            _projectPlanInfoService = projectPlanInfoService;
            _projectResourceBudgetRepository = projectResourceBudgetRepository;
            _projectResourceBudgetHCRepository = projectResourceBudgetHCRepository;
            _projectResourceBudgetService = projectResourceBudgetService;
            _projectOtherBudgetRepository = projectOtherBudgetRepository;
            _projectOtherBudgetService = projectOtherBudgetService;
            _projectBudgetSummaryRepository = projectBudgetSummaryRepository;
            _projectBudgetSummaryService = projectBudgetSummaryService;
            _projectHistoryRepository = projectHistoryRepository;
            _sys_CalendarRepository = sys_CalendarRepository;
            _emailTemplateRepository = emailTemplateRepository;
            _emailSendLogRepository = emailSendLogRepository;
            _sys_DepartmentSettingRepository = sys_DepartmentSettingRepository;
            _mapper = mapper;
            _exporter = exporter;
            _contractProjectHistoryRepository = contractProjectHistoryRepository;
            _contractHistoryRepository = contractHistoryRepository;
            _contractProjectHistoryRepository = contractProjectHistoryRepository;
            _contractHistoryRepository = contractHistoryRepository;
            _projectAttachmentListHistoryRepository = projectAttachmentListHistoryRepository;
            _projectPlanInfoHistoryRepository = projectPlanInfoHistoryRepository;
            _projectResourceBudgetHistoryRepository = projectResourceBudgetHistoryRepository;
            _projectResourceBudgetHCHistoryRepository = projectResourceBudgetHCHistoryRepository;
            _projectOtherBudgetHistoryRepository = projectOtherBudgetHistoryRepository;
            //多租户会用到这init代码，其他情况可以不用
            //base.Init(dbRepository);
        }

        /// <summary>
        /// 根据项目ID获取项目详情
        /// </summary>
        /// <param name="projectId">项目Id</param>
        /// <returns></returns>
        public async Task<WebResponseContent> GetProjectDetail(int projectId)
        {
            ContractProject contractProject = await _contractProjectRepository.FindAsyncFirst(x => x.Project_Id == projectId && x.Status == (int)Status.Active);
            if (contractProject == null)
            {
                return WebResponseContent.Instance.Error("获取项目合同关联信息失败");
            }
            Entity.DomainModels.Contract contract = await _contractRepository.FindAsyncFirst(x => x.Id == contractProject.Contract_Id);
            if (contract == null)
            {
                return WebResponseContent.Instance.Error("获取项目合同信息失败");
            }
            Project project = await _repository.FindAsyncFirst(x => x.Id == projectId);
            if (project == null)
            {
                return WebResponseContent.Instance.Error("获取项目信息失败");
            }
            Entity.DomainModels.Client client = await _clientRepository.FindAsyncFirst(x => x.Id == contract.Client_Id);
            if (client == null)
            {
                return WebResponseContent.Instance.Error("获取客户信息失败");
            }

            ProjectOutPutDTO projectDTO = _mapper.Map<ProjectOutPutDTO>(project);

            //通过字典获取展示信息
            projectDTO.Project_Type = ConverterContainer.ProjectTypeConverter(project.Project_TypeId);
            projectDTO.Change_Type = ConverterContainer.ChangeTypeConverter(project.Change_TypeId);
            projectDTO.Cooperation_Type = ConverterContainer.CooperationTypeConverter(project.Cooperation_TypeId);
            projectDTO.Billing_Mode = ConverterContainer.BillingModeConverter(project.Billing_ModeId);
            projectDTO.Service_Type = ConverterContainer.ServiceTypeConverter(project.Service_TypeId);
            projectDTO.Billing_Cycle = ConverterContainer.BillingCycleConverter(project.Billing_CycleId);
            projectDTO.Shore_Type = ConverterContainer.ShoreConverter(project.Shore_TypeId);
            projectDTO.Site_Type = ConverterContainer.SiteConverter(project.Site_TypeId);
            projectDTO.Holiday_System = ConverterContainer.HolidaySystemConverter(project.Holiday_SystemId);
            projectDTO.Standard_Number_of_Days_Per_Month = ConverterContainer.StandardNumberofDaysperMonthConverter(project.Standard_Number_of_Days_Per_MonthId);
            //验证当前项目是否存在审批同意的历史记录。
            bool existsHistoryInfo = _projectHistoryRepository.Exists(o => o.Project_Id == projectDTO.Id && o.ChangeSource == 2);
            projectDTO.Is_Handle_Change = existsHistoryInfo ? Convert.ToByte(1) : Convert.ToByte(0);
            projectDTO.Contract = new List<ContractSaveModel> { _mapper.Map<ContractSaveModel>(contract) };
            projectDTO.Client = _mapper.Map<Entity.DTO.Contract.Client>(client);

            return WebResponseContent.Instance.OK("获取项目成功", projectDTO);
        }

        /// <summary>
        /// 根据项目历史ID获取项目历史详情
        /// </summary>
        /// <param name="projectId">项目Id</param>
        /// <returns></returns>
        public async Task<WebResponseContent> GetProjectHistoryDetail(int projectHistoryId)
        {
            var projectHistory = await _projectHistoryRepository.FindAsyncFirst(x => x.Id == projectHistoryId);
            if (projectHistory == null)
            {
                return WebResponseContent.Instance.Error("获取项目历史信息失败");
            }

            var contractProjectHistory = await _contractProjectHistoryRepository.FindAsyncFirst(x => x.Project_Id == projectHistory.Project_Id && x.Version == projectHistory.ContractVersion);
            if (contractProjectHistory == null)
            {
                return WebResponseContent.Instance.Error("获取项目合同关联历史信息失败");
            }
            var contractHistory = await _contractHistoryRepository.FindAsyncFirst(x => x.Contract_Id == contractProjectHistory.Contract_Id && x.Version == contractProjectHistory.Version);
            if (contractHistory == null)
            {
                return WebResponseContent.Instance.Error("获取项目合同历史信息失败");
            }

            Entity.DomainModels.Client client = await _clientRepository.FindAsyncFirst(x => x.Id == contractHistory.Client_Id);
            if (client == null)
            {
                return WebResponseContent.Instance.Error("获取客户信息失败");
            }

            ProjectOutPutDTO projectDTO = _mapper.Map<ProjectOutPutDTO>(projectHistory);

            //通过字典获取展示信息
            projectDTO.Project_Type = ConverterContainer.ProjectTypeConverter(projectHistory.Project_TypeId);
            projectDTO.Change_Type = ConverterContainer.ChangeTypeConverter(projectHistory.Change_TypeId);
            projectDTO.Cooperation_Type = ConverterContainer.CooperationTypeConverter(projectHistory.Cooperation_TypeId);
            projectDTO.Billing_Mode = ConverterContainer.BillingModeConverter(projectHistory.Billing_ModeId);
            projectDTO.Service_Type = ConverterContainer.ServiceTypeConverter(projectHistory.Service_TypeId);
            projectDTO.Billing_Cycle = ConverterContainer.BillingCycleConverter(projectHistory.Billing_CycleId);
            projectDTO.Shore_Type = ConverterContainer.ShoreConverter(projectHistory.Shore_TypeId);
            projectDTO.Site_Type = ConverterContainer.SiteConverter(projectHistory.Site_TypeId);
            projectDTO.Holiday_System = ConverterContainer.HolidaySystemConverter(projectHistory.Holiday_SystemId);
            projectDTO.Standard_Number_of_Days_Per_Month = ConverterContainer.StandardNumberofDaysperMonthConverter(projectHistory.Standard_Number_of_Days_Per_MonthId);
            //验证当前历史项目是否存在审批同意的历史记录。
            bool existsHistoryInfo = _projectHistoryRepository.Exists(o => o.Project_Id == projectDTO.Id && o.ChangeSource == 2);
            projectDTO.Is_Handle_Change = existsHistoryInfo ? Convert.ToByte(1) : Convert.ToByte(0);
            projectDTO.Contract = new List<ContractSaveModel> { _mapper.Map<ContractSaveModel>(contractHistory) };
            projectDTO.Client = _mapper.Map<Entity.DTO.Contract.Client>(client);

            return WebResponseContent.Instance.OK("获取项目历史成功", projectDTO);
        }

        /// <summary>
        /// 根据项目ID获取项目额外信息详情
        /// <para>1:项目计划信息|ProjectPlanInfo</para>
        /// <para>2:项目资源预算|ProjectResourceBudget</para>
        /// <para>3:项目附件列表|ProjectAttachmentList</para>
        /// <para>4:项目其它成本费用预算|ProjectOtherBudget</para>
        /// <para>5:项目预算汇总|ProjectBudgetSummary</para>
        /// <para>6:项目资源预算|ProjectResourceBudgetHC</para>
        /// </summary>
        /// <param name="projectId">项目Id</param>
        /// <returns></returns>
        public async Task<WebResponseContent> GetProjectExtraInfo(int projectId)
        {
            ContractProject contractProject = await _contractProjectRepository.FindAsyncFirst(x => x.Project_Id == projectId && x.Status == (int)Status.Active);
            if (contractProject == null)
            {
                return WebResponseContent.Instance.Error("获取项目合同关联信息失败");
            }
            Entity.DomainModels.Contract contract = await _contractRepository.FindAsyncFirst(x => x.Id == contractProject.Contract_Id);
            if (contract == null)
            {
                return WebResponseContent.Instance.Error("获取项目合同信息失败");
            }
            Project project = await _repository.FindAsyncFirst(x => x.Id == projectId);
            if (project == null)
            {
                return WebResponseContent.Instance.Error("获取项目信息失败");
            }
            Entity.DomainModels.Client client = await _clientRepository.FindAsyncFirst(x => x.Id == contract.Client_Id);
            if (client == null)
            {
                return WebResponseContent.Instance.Error("获取客户信息失败");
            }
            ProjectExtraInfoDTO projectDTO = new ProjectExtraInfoDTO();
            //projectDTO.Contract = new List<ContractSaveModel> { _mapper.Map<ContractSaveModel>(contract) };
            //projectDTO.Client = _mapper.Map<Entity.DTO.Contract.Client>(client);
            // 项目附件信息
            var projectAttachment = await _projectAttachmentListRepository.FindAsync(x => x.Project_Id == projectId && x.IsDelete == 0);
            var projectAttachmentDTO = _mapper.Map<ICollection<ProjectAttachmentListDTO>>(projectAttachment);
            projectDTO.ProjectAttachmentList = projectAttachmentDTO;
            // 项目计划信息
            var projectPlanInfo = await _projectPlanInfoRepository.FindAsync(x => x.Project_Id == projectId);
            var projectPlanInfoDTO = _mapper.Map<ICollection<ProjectPlanInfoDTO>>(projectPlanInfo);
            projectDTO.ProjectPlanInfo = projectPlanInfoDTO;
            // 项目资源预算   
            var projectResourceBudget = await _projectResourceBudgetRepository.FindAsync(x => x.Project_Id == projectId);
            var projectResourceBudgetDTO = _mapper.Map<ICollection<ProjectResourceBudgetDTO>>(projectResourceBudget);
            // 项目资源预算HC 
            var projectResourceBudgetHC = await _projectResourceBudgetHCRepository.FindAsync(x => x.Project_Id == projectId);
            var projectResourceBudgetHCDTO = _mapper.Map<ICollection<ProjectResourceBudgetHCDTO>>(projectResourceBudgetHC);
            foreach (var item in projectResourceBudgetDTO)
            {
                //通过字典获取展示信息
                item.Position = ConverterContainer.PositionConverter(item.PositionId);
                item.Level = ConverterContainer.LevelConverter(item.LevelId);
                item.City = ConverterContainer.CityConverter(item.CityId);
            }
            projectDTO.ProjectResourceBudget = projectResourceBudgetDTO;
            projectDTO.ProjectResourceBudgetHC = projectResourceBudgetHCDTO;
            // 项目其它成本费用预算
            var projectOtherBudget = await _projectOtherBudgetRepository.FindAsync(x => x.Project_Id == projectId);
            var projectOtherBudgetDTO = _mapper.Map<ICollection<ProjectOtherBudgetDTO>>(projectOtherBudget);
            projectDTO.ProjectOtherBudget = projectOtherBudgetDTO;
            // 项目预算汇总
            BCSContext dbContext = DBServerProvider.GetEFDbContext();
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
            projectDTO.ProjectBudgetSummary = projectBudgetSummary;

            return WebResponseContent.Instance.OK("获取项目成功", projectDTO);
        }

        /// <summary>
        /// 根据项目ID获取项目历史额外信息详情
        /// <para>1:项目计划信息|ProjectPlanInfo</para>
        /// <para>2:项目资源预算|ProjectResourceBudget</para>
        /// <para>3:项目附件列表|ProjectAttachmentList</para>
        /// <para>4:项目其它成本费用预算|ProjectOtherBudget</para>
        /// <para>5:项目预算汇总|ProjectBudgetSummary</para>
        /// <para>6:项目资源预算|ProjectResourceBudgetHC</para>
        /// </summary>
        /// <param name="projectHistoryId">项目历史Id</param>
        /// <returns></returns>
        public async Task<WebResponseContent> GetProjectHistoryExtraInfo(int projectHistoryId)
        {
            var projectHistory = await _projectHistoryRepository.FindAsyncFirst(x => x.Id == projectHistoryId);
            if (projectHistory == null)
            {
                return WebResponseContent.Instance.Error("获取项目历史信息失败");
            }

            var contractProjectHistory = await _contractProjectHistoryRepository.FindAsyncFirst(x => x.Project_Id == projectHistory.Project_Id && x.Version == projectHistory.ContractVersion);
            if (contractProjectHistory == null)
            {
                return WebResponseContent.Instance.Error("获取项目合同关联历史信息失败");
            }
            var contractHistory = await _contractHistoryRepository.FindAsyncFirst(x => x.Contract_Id == contractProjectHistory.Contract_Id && x.Version == contractProjectHistory.Version);
            if (contractHistory == null)
            {
                return WebResponseContent.Instance.Error("获取项目合同历史信息失败");
            }

            Entity.DomainModels.Client client = await _clientRepository.FindAsyncFirst(x => x.Id == contractHistory.Client_Id);
            if (client == null)
            {
                return WebResponseContent.Instance.Error("获取客户信息失败");
            }
            ProjectExtraInfoDTO projectDTO = new ProjectExtraInfoDTO();
            //projectDTO.Contract = new List<ContractSaveModel> { _mapper.Map<ContractSaveModel>(contract) };
            //projectDTO.Client = _mapper.Map<Entity.DTO.Contract.Client>(client);
            // 项目附件信息
            var projectAttachment = await _projectAttachmentListHistoryRepository.FindAsync(x => x.Project_Id == projectHistory.Project_Id && x.Version == projectHistory.Version);
            var projectAttachmentDTO = _mapper.Map<ICollection<ProjectAttachmentListDTO>>(projectAttachment);
            projectDTO.ProjectAttachmentList = projectAttachmentDTO;
            // 项目计划信息
            var projectPlanInfo = await _projectPlanInfoHistoryRepository.FindAsync(x => x.Project_Id == projectHistory.Project_Id && x.Version == projectHistory.Version);
            var projectPlanInfoDTO = _mapper.Map<ICollection<ProjectPlanInfoDTO>>(projectPlanInfo);
            projectDTO.ProjectPlanInfo = projectPlanInfoDTO;
            // 项目资源预算   
            var projectResourceBudget = await _projectResourceBudgetHistoryRepository.FindAsync(x => x.Project_Id == projectHistory.Project_Id && x.Version == projectHistory.Version);
            var projectResourceBudgetDTO = _mapper.Map<ICollection<ProjectResourceBudgetDTO>>(projectResourceBudget);
            // 项目资源预算HC 
            var projectResourceBudgetHC = await _projectResourceBudgetHCHistoryRepository.FindAsync(x => x.Project_Id == projectHistory.Project_Id && x.Version == projectHistory.Version);
            var projectResourceBudgetHCDTO = _mapper.Map<ICollection<ProjectResourceBudgetHCDTO>>(projectResourceBudgetHC);
            foreach (var item in projectResourceBudgetDTO)
            {
                //通过字典获取展示信息
                item.Position = ConverterContainer.PositionConverter(item.PositionId);
                item.Level = ConverterContainer.LevelConverter(item.LevelId);
                item.City = ConverterContainer.CityConverter(item.CityId);
            }
            projectDTO.ProjectResourceBudget = projectResourceBudgetDTO;
            projectDTO.ProjectResourceBudgetHC = projectResourceBudgetHCDTO;
            // 项目其它成本费用预算
            var projectOtherBudget = await _projectOtherBudgetHistoryRepository.FindAsync(x => x.Project_Id == projectHistory.Project_Id && x.Version == projectHistory.Version);
            var projectOtherBudgetDTO = _mapper.Map<ICollection<ProjectOtherBudgetDTO>>(projectOtherBudget);
            projectDTO.ProjectOtherBudget = projectOtherBudgetDTO;
            // 项目预算汇总
            BCSContext dbContext = DBServerProvider.GetEFDbContext();
            var projectBudgetSummary = (from budgetSummary in dbContext.Set<ProjectBudgetSummaryHistory>()
                                        where budgetSummary.Project_Id == projectHistory.Project_Id && budgetSummary.Version == projectHistory.Version
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
            projectDTO.ProjectBudgetSummary = projectBudgetSummary;

            return WebResponseContent.Instance.OK("获取项目历史额外信息成功", projectDTO);
        }

        /// <summary>
        /// 保存项目信息
        /// </summary>
        /// <param name="projectId">项目Id</param>
        /// <returns></returns>
        public async Task<WebResponseContent> SaveProject(ProjectDTO projectDTO)
        {
            if (!await repository.ExistsAsync(x => x.Id == projectDTO.Id))
            {
                return WebResponseContent.Instance.Error("项目不存在");
            }
            UserInfo userInfo = UserContext.Current.UserInfo;
            DateTime currentTime = DateTime.Now;
            userInfo.UserName = string.IsNullOrEmpty(userInfo.UserName) ? string.Empty : userInfo.UserName;
            //界面参数转换为实体
            Project projectFromUI = _mapper.Map<Project>(projectDTO);
            //数据库中的实体
            var projectExists = await repository.FindAsyncFirst(x => x.Id == projectDTO.Id);
            if (projectExists.Approval_Status == (byte)ApprovalStatus.PendingApprove)
            {
                return WebResponseContent.Instance.Error("当前项目正在审批中，不能修改。");
            }
            //验证当前项目是否存在审批同意的历史记录。当有审批同意的历史记录时，需要同时保存变更信息
            bool existsHistoryInfo = _projectHistoryRepository.Exists(o => o.Project_Id == projectExists.Id && o.ChangeSource == 2);
            if (existsHistoryInfo)
            {
                if (projectFromUI.Change_TypeId <= 0)
                {
                    return WebResponseContent.Instance.Error("please upload Change_Type parameter.");
                }
                if (string.IsNullOrEmpty(projectFromUI.Change_Reason))
                {
                    return WebResponseContent.Instance.Error("please upload Change_Reason parameter.");
                }
                projectExists.Change_TypeId = projectFromUI.Change_TypeId;
                projectExists.Change_Reason = projectFromUI.Change_Reason;
                projectExists.Change_From = 1;
            }
            projectExists.Project_Name = projectFromUI.Project_Name;
            projectExists.Client_Organization_Name = projectFromUI.Client_Organization_Name;
            projectExists.Cooperation_TypeId = projectFromUI.Cooperation_TypeId;
            projectExists.Billing_ModeId = projectFromUI.Billing_ModeId;
            projectExists.Start_Date = projectFromUI.Start_Date;
            projectExists.End_Date = projectFromUI.End_Date;
            projectExists.IsPurely_Subcontracted_Project = projectFromUI.IsPurely_Subcontracted_Project;
            projectExists.Project_Amount = projectFromUI.Project_Amount;
            projectExists.Service_TypeId = projectFromUI.Service_TypeId;
            projectExists.Billing_CycleId = projectFromUI.Billing_CycleId;
            projectExists.Estimated_Billing_Cycle = projectFromUI.Estimated_Billing_Cycle;
            projectExists.Shore_TypeId = projectFromUI.Shore_TypeId;
            projectExists.Site_TypeId = projectFromUI.Site_TypeId;
            projectExists.Project_LocationCity = projectFromUI.Project_LocationCity;
            projectExists.Holiday_SystemId = projectFromUI.Holiday_SystemId;
            projectExists.Standard_Number_of_Days_Per_MonthId = projectFromUI.Standard_Number_of_Days_Per_MonthId;
            projectExists.Standard_Daily_Hours = projectFromUI.Standard_Daily_Hours;
            projectExists.Project_Director = projectFromUI.Project_Director;
            projectExists.Project_Director_Id = projectFromUI.Project_Director_Id;
            projectExists.Project_Description = projectFromUI.Project_Description;
            projectExists.Remark = projectFromUI.Remark;
            //项目状态
            projectExists.Operating_Status = existsHistoryInfo ? (byte)3 : (byte)2;
            projectExists.Approval_Status = 4;
            //项目版本号
            projectExists.Version = await FindNextVersion(projectExists.Id);
            projectExists.ModifyID = userInfo.User_Id;
            projectExists.Modifier = userInfo.UserName;
            projectExists.ModifyDate = currentTime;
            //目前项目只有一个项目计划。
            bool isExists = await _projectPlanInfoRepository.ExistsAsync(x => x.Project_Id == projectExists.Id);
            ProjectPlanInfo projectPlanInfo = new ProjectPlanInfo();
            if (!isExists)
            {
                //projectPlanInfo.Id;
                projectPlanInfo.Project_Id = projectExists.Id;//, --项目Id
                projectPlanInfo.PlanOrderNo = 1;//, --序号
                projectPlanInfo.PlanName = "Default Plan";//, --序号
                projectPlanInfo.Start_Date = projectExists.Start_Date;//,---项目开始日期
                projectPlanInfo.End_Date = projectExists.End_Date;//,---项目结束日期
                projectPlanInfo.Remark = string.Empty;//---备注
                projectPlanInfo.CreateID = userInfo.User_Id;//, --创建人ID
                projectPlanInfo.Creator = userInfo.UserName;//, --创建人
                projectPlanInfo.CreateDate = currentTime;//, --创建时间
                projectPlanInfo.ModifyID = userInfo.User_Id;//, --修改人ID
                projectPlanInfo.Modifier = userInfo.UserName;//, --修改人
                projectPlanInfo.ModifyDate = currentTime;//
            }
            WebResponseContent webResponseContent;

            BCSContext dbContext = DBServerProvider.GetEFDbContext();
            using (var transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    if (!isExists)
                    {
                        _projectPlanInfoRepository.Add(projectPlanInfo);
                    }
                    repository.Update(projectExists);
                    dbContext.SaveChanges();
                    transaction.Commit();
                    webResponseContent = WebResponseContent.Instance.OK("保存项目成功", projectDTO.Id);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    webResponseContent = WebResponseContent.Instance.Error($"保存项目异常[{ex.Message}]");
                }
            }
            return webResponseContent;
        }

        /// <summary>
        /// 项目变更
        /// <para>UpdateType</para>
        /// <para>1:变更项目经理</para>
        /// <para>2:变更项目总监</para>
        /// </summary>
        /// <param name="projectUpdateDTO"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> UpdateProject(ProjectUpdateDTO projectUpdateDTO)
        {
            if (!await repository.ExistsAsync(x => x.Id == projectUpdateDTO.Id))
            {
                return WebResponseContent.Instance.Error("项目不存在");
            }
            UserInfo userInfo = UserContext.Current.UserInfo;
            DateTime currentTime = DateTime.Now;
            userInfo.UserName = string.IsNullOrEmpty(userInfo.UserName) ? string.Empty : userInfo.UserName;
            if (!string.Equals(userInfo.RoleName, CommonConst.DeliveryManager, StringComparison.OrdinalIgnoreCase))
            {
                return WebResponseContent.Instance.Error("只有DM才能变更项目经理或项目总监");
            }
            List<string> updateFields = new List<string>() { "ModifyID", "Modifier", "ModifyDate" };
            //数据库中的实体
            var projectExists = await repository.FindAsyncFirst(x => x.Id == projectUpdateDTO.Id);
            switch (projectUpdateDTO.UpdateType)
            {
                case 1:
                    {
                        projectExists.Project_Manager_Id = projectUpdateDTO.Project_Manager_Id;
                        projectExists.Project_Manager = projectUpdateDTO.Project_Manager;
                        updateFields.AddRange(new string[] { "Project_Manager_Id", "Project_Manager" });
                    }
                    break;
                case 2:
                    {
                        projectExists.Project_Director_Id = projectUpdateDTO.Project_Director_Id;
                        projectExists.Project_Director = projectUpdateDTO.Project_Director;
                        updateFields.AddRange(new string[] { "Project_Director_Id", "Project_Director" });
                    }
                    break;
                default:
                    break;
            }
            projectExists.ModifyID = userInfo.User_Id;
            projectExists.Modifier = userInfo.UserName;
            projectExists.ModifyDate = currentTime;

            WebResponseContent webResponseContent;

            BCSContext dbContext = DBServerProvider.GetEFDbContext();
            using (var transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    repository.Update(projectExists, updateFields.ToArray());
                    dbContext.SaveChanges();
                    transaction.Commit();
                    webResponseContent = WebResponseContent.Instance.OK("更新成功", projectUpdateDTO.Id);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    webResponseContent = WebResponseContent.Instance.Error($"更新异常[{ex.Message}]");
                }
            }
            return webResponseContent;
        }

        /// <summary>
        /// 保存项目额外信息
        /// <para>1:项目计划信息|ProjectPlanInfo</para>
        /// <para>2:项目资源预算|ProjectResourceBudget</para>
        /// <para>3:项目附件列表|ProjectAttachmentList</para>
        /// <para>4:项目其它成本费用预算|ProjectOtherBudget</para>
        /// <para>5:项目资源预算|ProjectResourceBudgetHC</para>
        /// </summary>
        /// <param name="projectId">项目Id</param>
        /// <param name="projectExtraInfoDTO">项目额外信息</param>
        /// <returns></returns>
        public async Task<WebResponseContent> SaveProjectExtraInfo(int projectId, ProjectExtraInfoDTO projectExtraInfoDTO)
        {
            if (!await repository.ExistsAsync(x => x.Id == projectId))
            {
                return WebResponseContent.Instance.Error("项目不存在");
            }
            //数据库中的实体
            var projectExists = await repository.FindAsyncFirst(x => x.Id == projectId);
            if (projectExists.Approval_Status == (byte)ApprovalStatus.PendingApprove)
            {
                return WebResponseContent.Instance.Error("当前项目正在审批中，不能修改。");
            }
            // 上传附件
            var uploadAttachmentResult = await _projectAttachmentListService.UploadAttachment(projectId, projectExtraInfoDTO.ProjectAttachmentList);
            if (!uploadAttachmentResult.Status)
            {
                return uploadAttachmentResult;
            }
            // 保存项目计划信息
            var saveProjectPlanInfoResult = await _projectPlanInfoService.SaveProjectPlanInfo(projectId, projectExtraInfoDTO.ProjectPlanInfo, projectExtraInfoDTO.ProjectResourceBudget, projectExtraInfoDTO.ProjectResourceBudgetHC);
            if (!saveProjectPlanInfoResult.Status)
            {
                return saveProjectPlanInfoResult;
            }
            // 项目其它成本费用预算
            var saveOtherBudgetResult = await _projectOtherBudgetService.SaveOtherBudget(projectId, projectExtraInfoDTO.ProjectOtherBudget);
            if (!saveOtherBudgetResult.Status)
            {
                return saveOtherBudgetResult;
            }

            return WebResponseContent.Instance.OK("项目信息保存成功", projectId);
        }

        /// <summary>
        /// 项目提交审批[包括项目ID,项目预算汇总信息]
        /// <para>1:项目预算汇总|ProjectBudgetSummary</para>
        /// </summary>
        /// <param name="projectSubmitInputDTO"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> Submit(ProjectSubmitInputDTO projectSubmitInputDTO)
        {
            if (!await repository.ExistsAsync(x => x.Id == projectSubmitInputDTO.Project_Id))
            {
                return WebResponseContent.Instance.Error("项目不存在");
            }
            //数据库中的实体
            var projectExists = await repository.FindAsyncFirst(x => x.Id == projectSubmitInputDTO.Project_Id);
            if (projectExists.Approval_Status == (byte)ApprovalStatus.PendingApprove)
            {
                return WebResponseContent.Instance.Error("当前项目正在审批中，不能重复提交。");
            }
            // 保存项目预算汇总信息
            var updateProjectBudgetSummary = await _projectBudgetSummaryService.SaveProjectBudgetSummary(projectSubmitInputDTO.Project_Id, projectSubmitInputDTO.ProjectBudgetSummary);
            if (!updateProjectBudgetSummary.Status)
            {
                return updateProjectBudgetSummary;
            }
            UserInfo userInfo = UserContext.Current.UserInfo;
            DateTime currentTime = DateTime.Now;
            userInfo.UserName = string.IsNullOrEmpty(userInfo.UserName) ? string.Empty : userInfo.UserName;
            WebResponseContent webResponseContent = WebResponseContent.Instance.Error("提交审批失败");
            //是否有工作流配置
            WorkFlowTableOptions workFlow = WorkFlowContainer.GetFlowOptions(projectExists);
            //没有对应的流程信息
            if (workFlow == null || workFlow.FilterList.Count == 0)
            {
                if (projectExists.Approval_Status == (byte)ApprovalStatus.PendingApprove)
                {
                    return WebResponseContent.Instance.Error("审批中，不可重复提交审批");
                }

                #region 无工作流配置
                List<string> updateFileds = new List<string> {
                    "ModifyID",
                    "Modifier",
                    "ModifyDate",
                    "Operating_Status",
                    "Approval_Status",
                    "Approval_EndTime",
                    "Approval_EndTime",
                };
                projectExists.ModifyID = userInfo.User_Id;
                projectExists.Modifier = userInfo.UserName;
                projectExists.ModifyDate = currentTime;
                projectExists.Operating_Status = 1;
                projectExists.Approval_Status = (byte)ApprovalStatus.PendingApprove;
                projectExists.Approval_StartTime = currentTime;
                projectExists.Approval_EndTime = new DateTime(1900, 01, 01);

                int result = repository.Update(projectExists, updateFileds.ToArray(), true);
                if (result > 0)
                {
                    webResponseContent = WebResponseContent.Instance.OK("提交审批成功。", projectSubmitInputDTO.Project_Id);
                }
                else
                {
                    webResponseContent = WebResponseContent.Instance.Error("提交审批失败。");
                }
                #endregion
            }
            else
            {
                #region 有工作流配置
                //新建的数据进入审批流程前处理，
                AddWorkFlowExecuting = (Project project) =>
                {
                    #region 检查当前业务表单是否已存在待审批数据

                    string workTable = typeof(Project).GetEntityTableName();

                    Sys_WorkFlowTable? workFlowCheck = DBServerProvider.DbContext.Set<Sys_WorkFlowTable>()
                               .Where(x => x.WorkTable == workTable && x.WorkTableKey == project.Id.ToString() && x.AuditStatus == (int)ApprovalStatus.PendingApprove)
                                .OrderByDescending(x => x.CreateDate)
                               .FirstOrDefault();

                    if (workFlowCheck != null)
                    {
                        webResponseContent = WebResponseContent.Instance.Error("提交审批失败，当前业务数据存在未走完的流程");
                        return false;
                    }

                    #endregion

                    //返回false，当前数据不会进入审批流程
                    return true;
                };
                //
                AddWorkFlowTableExecuting = (Project project, Sys_WorkFlowTable workFlowTable) =>
                {
                    projectExists.WorkFlowTable_Id = workFlowTable.WorkFlowTable_Id;
                    //验证当前项目是否存在审批同意的历史记录。当有审批同意的历史记录时，需要同时保存变更信息
                    bool existsHistoryInfo = _projectHistoryRepository.Exists(o => o.Project_Id == projectSubmitInputDTO.Project_Id && o.ChangeSource == 2);
                    if (existsHistoryInfo)
                    {
                        workFlowTable.BusinessName = project.Project_Name;
                        workFlowTable.BusinessType = 4;
                    }
                    else
                    {
                        workFlowTable.BusinessName = project.Project_Name;
                        workFlowTable.BusinessType = 3;
                    }
                };

                //新建的数据写入审批流程后,第二个参数为审批人的用户id
                AddWorkFlowExecuted = (Project project, List<int> userIds) =>
                {
                    projectExists.ModifyID = userInfo.User_Id;
                    projectExists.Modifier = userInfo.UserName;
                    projectExists.ModifyDate = currentTime;
                    projectExists.Operating_Status = 1;
                    projectExists.Approval_Status = (byte)ApprovalStatus.PendingApprove;
                    projectExists.Approval_StartTime = currentTime;
                    projectExists.Approval_EndTime = new DateTime(1900, 01, 01);

                    int result = repository.Update(projectExists, true);
                    if (result > 0)
                    {
                        webResponseContent = WebResponseContent.Instance.OK("工作流提交成功，业务表单数据更新成功。", projectSubmitInputDTO.Project_Id);
                    }
                    else
                    {
                        webResponseContent = WebResponseContent.Instance.Error("工作流提交成功，更新业务表单数据失败。");
                    }
                    // TODO: 发送邮件通知
                    //这里可以做发邮件通知
                    //var userInfo = repository.DbContext.Set<Sys_User>()
                    //                .Where(x => userIds.Contains(x.User_Id))
                    //                .Select(s => new { s.User_Id, s.UserTrueName, s.Email, s.PhoneNo }).ToList();
                    //发送邮件方法
                    //MailHelper.Send()
                };
                this.DecorateProjectExtraInfo(projectExists);
                AddProcese(projectExists);
                #endregion
            }

            return webResponseContent;
        }

        /// <summary>
        /// 审批业务
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="auditStatus"></param>
        /// <param name="auditReason"></param>
        /// <returns></returns>
        public override WebResponseContent Audit(object[] keys, int? auditStatus, string auditReason)
        {
            if (keys.Length > 1)
            {
                return WebResponseContent.Instance.Error("目前只支持单个审批，不支持批量审批。");
            }
            UserInfo userInfo = UserContext.Current.UserInfo;
            DateTime currentTime = DateTime.Now;
            userInfo.UserName = string.IsNullOrEmpty(userInfo.UserName) ? string.Empty : userInfo.UserName;

            bool writeHistoryFlag = false;
            //status当前审批状态,lastAudit是否最后一个审批节点
            AuditWorkFlowExecuting = (Project project, ApprovalStatus status, bool lastAudit) =>
            {
                this.DecorateProjectExtraInfo(project);
                return WebResponseContent.Instance.OK();
            };
            //status当前审批状态,nextUserIds下一个节点审批人的帐号id(可以从sys_user表中查询用户具体信息),lastAudit是否最后一个审批节点
            AuditWorkFlowExecuted = (Project project, ApprovalStatus status, FilterOptions filterOptions, List<int> nextUserIds, bool lastAudit) =>
            {
                List<string> updateFileds = new List<string> {
                    "ModifyID",
                    "Modifier",
                    "ModifyDate",
                    "Operating_Status",
                    "Project_Status",
                    "Approval_EndTime",
                };
                //lastAudit=true时，流程已经结束 
                if (lastAudit && status == ApprovalStatus.Approved)
                {
                    this.SendMail(project, nextUserIds);
                }

                //审批流程回退功能，回到第一个审批人重新审批(重新生成审批流程)
                //if (status == ApprovalStatus.Rejected)
                //{
                //    base.RewriteFlow(project);
                //}
                ////审批流程回退功能，回到第一个审批人重新审批(重新生成审批流程)
                //if (status == ApprovalStatus.Recalled)
                //{
                //    base.RewriteFlow(project);
                //}
                project.ModifyID = userInfo.User_Id;
                project.Modifier = userInfo.UserName;
                project.ModifyDate = currentTime;
                //审批流程结束后处理
                if (lastAudit)
                {
                    if (status == ApprovalStatus.Approved)
                    {
                        if (project.End_Date < currentTime)
                        {
                            project.Project_Status = 2;
                        }
                        if (project.Start_Date > currentTime)
                        {
                            project.Project_Status = 3;
                        }
                        if (currentTime >= project.Start_Date && currentTime <= project.End_Date)
                        {
                            project.Project_Status = 1;
                        }
                    }
                    project.Approval_EndTime = currentTime;
                    // 最后审批同意后，备份项目信息
                    writeHistoryFlag = status == ApprovalStatus.Approved;
                }

                //更新项目信息
                repository.Update(project, updateFileds.ToArray(), true);

                return WebResponseContent.Instance.OK();
            };

            #region 当无工作流配置时，使用以下委托完成相关审批业务逻辑

            //审核保存前处理(不是审批流程)
            AuditOnExecuting = (List<Project> projects) =>
            {
                return WebResponseContent.Instance.OK();
            };
            //审核后处理(不是审批流程)
            AuditOnExecuted = (List<Project> projects) =>
            {
                List<string> updateFileds = new List<string> {
                    "ModifyID",
                    "Modifier",
                    "ModifyDate",
                    "Operating_Status",
                    "Project_Status",
                    "Approval_EndTime",
                };
                foreach (var project in projects)
                {
                    project.ModifyID = userInfo.User_Id;
                    project.Modifier = userInfo.UserName;
                    project.ModifyDate = currentTime;
                    project.Operating_Status = 1;
                    ApprovalStatus status = (ApprovalStatus)Enum.Parse(typeof(ApprovalStatus), auditStatus!.Value.ToString());
                    if (status == ApprovalStatus.Approved)
                    {
                        if (project.End_Date < currentTime)
                        {
                            project.Project_Status = 2;
                        }
                        if (project.Start_Date > currentTime)
                        {
                            project.Project_Status = 3;
                        }
                        if (currentTime >= project.Start_Date && currentTime <= project.End_Date)
                        {
                            project.Project_Status = 1;
                        }
                        //审批通过后，备份项目信息
                        writeHistoryFlag = true;
                    }
                    project.Approval_EndTime = currentTime;
                }
                //更新项目信息
                repository.UpdateRange(projects, updateFileds.ToArray(), true);
                return WebResponseContent.Instance.OK();
            };

            #endregion

            var webResponseContent = base.Audit(keys, auditStatus, auditReason);
            //审批成功后，且最后审批状态为审批同意时，备份项目信息
            if (writeHistoryFlag && webResponseContent.Status)
            {
                try
                {
                    //备份 项目历史信息以及和项目相关的信息
                    var result = repository.ExecuteSqlCommand($"exec dbo.usp_BackupLatestProject @User_Id,@Project_Id,@SystemTime,@ChangeSource", new SqlParameter[] {
                        new SqlParameter("@User_Id",userInfo.User_Id),
                        new SqlParameter("@Project_Id",keys[0]),
                        new SqlParameter("@SystemTime",currentTime),
                        new SqlParameter("@ChangeSource",2),
                    });
                    if (result <= 0)
                    {
                        webResponseContent = WebResponseContent.Instance.Error($"审批成功，备份数据异常失败");
                    }
                    else
                    {
                        webResponseContent = WebResponseContent.Instance.OK($"审批成功，备份数据成功");
                    }
                }
                catch (Exception ex)
                {
                    webResponseContent = WebResponseContent.Instance.Error($"审批成功，备份数据异常[{ex.Message}]");
                }
            }
            return webResponseContent;
        }

        /// <summary>
        /// 撤回
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> ReCall(int projectId)
        {
            if (!await repository.ExistsAsync(x => x.Id == projectId))
            {
                return WebResponseContent.Instance.Error("项目不存在");
            }
            UserInfo userInfo = UserContext.Current.UserInfo;
            DateTime currentTime = DateTime.Now;
            userInfo.UserName = string.IsNullOrEmpty(userInfo.UserName) ? string.Empty : userInfo.UserName;

            //status当前审批状态,lastAudit是否最后一个审批节点
            AuditWorkFlowExecuting = (Project project, ApprovalStatus status, bool lastAudit) =>
            {
                return WebResponseContent.Instance.OK();
            };

            //status当前审批状态,nextUserIds下一个节点审批人的帐号id(可以从sys_user表中查询用户具体信息),lastAudit是否最后一个审批节点
            AuditWorkFlowExecuted = (Project project, ApprovalStatus status, FilterOptions filterOptions, List<int> nextUserIds, bool lastAudit) =>
            {
                //验证当前项目是否存在审批同意的历史记录。
                bool existsHistoryInfo = _projectHistoryRepository.Exists(o => o.Project_Id == projectId && o.ChangeSource == 2);
                project.ModifyID = userInfo.User_Id;
                project.Modifier = userInfo.UserName;
                project.ModifyDate = currentTime;
                project.Operating_Status = existsHistoryInfo ? (byte)OperatingStatus.Changed : (byte)OperatingStatus.Draft;
                //更新项目信息
                repository.Update(project, true);

                return WebResponseContent.Instance.OK();
            };

            return base.Audit(new object[] { projectId }, (int)ApprovalStatus.Recalled, "发起人主动撤回");
        }

        /// <summary>
        /// 撤回变更
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> ReCallChange(int projectId)
        {
            if (!await repository.ExistsAsync(x => x.Id == projectId))
            {
                return WebResponseContent.Instance.Error("项目不存在");
            }
            UserInfo userInfo = UserContext.Current.UserInfo;
            DateTime currentTime = DateTime.Now;
            // TODO: 用户信息|Project-Workflow-提交的工作流数据
            var result = repository.ExecuteSqlCommand($"exec dbo.usp_RevertProjectToPreviousVersion @User_Id,@Project_Id,@SystemTime,@ChangeSource", new SqlParameter[] {
                new SqlParameter("@User_Id",userInfo.User_Id),
                new SqlParameter("@Project_Id",projectId),
                new SqlParameter("@SystemTime",currentTime),
                new SqlParameter("@ChangeSource",2),
            });
            return result > 0 ? WebResponseContent.Instance.OK("撤回变更成功", projectId) : WebResponseContent.Instance.Error("撤回变更失败");
        }

        /// <summary>
        /// 获取下一版本号
        /// </summary>
        /// <param name="projectId">项目ID</param>
        /// <returns></returns>
        private async Task<int> FindNextVersion(int projectId)
        {
            if (!await _projectHistoryRepository.ExistsAsync(o => o.Project_Id == projectId && o.ChangeSource == 2))
            {
                return 0;
            }
            var project = _projectHistoryRepository.Find(o => o.Project_Id == projectId && o.ChangeSource == 2).OrderByDescending(o => o.Version).First();
            return project.Version + 1;
        }

        /// <summary>
        /// 查询项目变更前后对比信息
        /// </summary>
        /// <param name="projectHistoryId">项目历史Id</param>
        /// <returns></returns>
        public WebResponseContent GetProjectCompareInfo(int projectHistoryId)
        {
            BCSContext context = DBServerProvider.GetEFDbContext();

            ProjectCompareModel contractCompareModel = new ProjectCompareModel();

            #region 1.项目信息历史
            CompareInfo<ProjectOutPutDTO> projectCompareInfo = new CompareInfo<ProjectOutPutDTO>();
            var currentProject = ProjectHistoryRepository.Instance.FindFirst(x => x.Id == projectHistoryId);
            if (currentProject == null)
            {
                return WebResponseContent.Instance.Error("项目历史信息不存在");
            }
            projectCompareInfo.Current = _mapper.Map<ProjectOutPutDTO>(currentProject);

            #region 与当前项目历史信息对应的合同历史信息和客户信息
            var currentContracts = (from contractProject in context.Set<ContractProjectHistory>()
                                    where contractProject.Project_Id == currentProject.Project_Id && contractProject.Version == currentProject.ContractVersion
                                    join contract in context.Set<ContractHistory>()
                                    on new { contractProject.Contract_Id, contractProject.Version } equals new { contract.Contract_Id, contract.Version }
                                    select contract).ToList();
            projectCompareInfo.Current.Contract = _mapper.Map<List<ContractSaveModel>>(currentContracts);

            if (currentContracts.Count > 0)
            {
                var contract = currentContracts[0];
                var currentClient = (from client in context.Set<BCS.Entity.DomainModels.Client>()
                                     where client.Id == contract.Client_Id
                                     select client).FirstOrDefault();
                projectCompareInfo.Current.Client = _mapper.Map<Entity.DTO.Contract.Client>(currentClient);
            }
            #endregion

            // 获取前一次的historyID.
            var previousProject = context.Set<ProjectHistory>()
                .Where(o => o.Project_Id == currentProject.Project_Id && o.Version < currentProject.Version && o.ChangeSource == 2)
                .OrderByDescending(o => o.CreateTime)
                .FirstOrDefault();
            if (previousProject == null)
            {
                return WebResponseContent.Instance.Error("前一次的项目历史信息不存在");
            }
            projectCompareInfo.Previous = _mapper.Map<ProjectOutPutDTO>(previousProject);

            #region 与前一个项目历史信息对应的合同历史信息和客户信息
            var previousContracts = (from contractProject in context.Set<ContractProjectHistory>()
                                     where contractProject.Project_Id == previousProject.Project_Id && contractProject.Version == previousProject.ContractVersion
                                     join contract in context.Set<ContractHistory>()
                                     on new { contractProject.Contract_Id, contractProject.Version } equals new { contract.Contract_Id, contract.Version }
                                     select contract).ToList();
            projectCompareInfo.Previous.Contract = _mapper.Map<List<ContractSaveModel>>(previousContracts);

            if (previousContracts.Count > 0)
            {
                var contract = previousContracts[0];
                var previousClient = (from client in context.Set<BCS.Entity.DomainModels.Client>()
                                      where client.Id == contract.Client_Id
                                      select client).FirstOrDefault();
                projectCompareInfo.Previous.Client = _mapper.Map<Entity.DTO.Contract.Client>(previousClient);
            }
            #endregion

            contractCompareModel.Project = projectCompareInfo;
            #endregion

            #region 2.项目计划信息历史
            CompareInfo<List<ProjectPlanInfoDTO>> projectPlanCompareInfo = new CompareInfo<List<ProjectPlanInfoDTO>>();
            // current info
            var currentProjectPlans = (from projectPlan in context.Set<ProjectPlanInfoHistory>()
                                       where projectPlan.Project_Id == currentProject.Project_Id && projectPlan.Version == currentProject.Version
                                       select projectPlan).ToList();
            projectPlanCompareInfo.Current = _mapper.Map<List<ProjectPlanInfoDTO>>(currentProjectPlans);

            // previous info
            var previousProjectPlans = (from projectPlan in context.Set<ProjectPlanInfoHistory>()
                                        where projectPlan.Project_Id == previousProject.Project_Id && projectPlan.Version == previousProject.Version
                                        select projectPlan).ToList();
            projectPlanCompareInfo.Previous = _mapper.Map<List<ProjectPlanInfoDTO>>(previousProjectPlans);

            contractCompareModel.ProjectPlanInfo = projectPlanCompareInfo;
            #endregion

            #region 3.项目资源预算历史
            CompareInfo<List<ProjectResourceBudgetDTO>> projectResourceBudgetCompareInfo = new CompareInfo<List<ProjectResourceBudgetDTO>>();
            // current info
            var currentProjectResourceBudgets = (from projectResourceBudget in context.Set<ProjectResourceBudgetHistory>()
                                                 where projectResourceBudget.Project_Id == currentProject.Project_Id && projectResourceBudget.Version == currentProject.Version
                                                 select projectResourceBudget).ToList();
            projectResourceBudgetCompareInfo.Current = _mapper.Map<List<ProjectResourceBudgetDTO>>(currentProjectResourceBudgets);

            // previous info
            var previousProjectResourceBudgets = (from projectResourceBudget in context.Set<ProjectResourceBudgetHistory>()
                                                  where projectResourceBudget.Project_Id == previousProject.Project_Id && projectResourceBudget.Version == previousProject.Version
                                                  select projectResourceBudget).ToList();
            projectResourceBudgetCompareInfo.Previous = _mapper.Map<List<ProjectResourceBudgetDTO>>(previousProjectResourceBudgets);

            contractCompareModel.ProjectResourceBudget = projectResourceBudgetCompareInfo;
            #endregion

            #region 4.项目资源预算历史HC
            CompareInfo<List<ProjectResourceBudgetHCDTO>> projectResourceBudgetHCCompareInfo = new CompareInfo<List<ProjectResourceBudgetHCDTO>>();
            // current info
            var currentProjectResourceBudgetHCs = (from projectResourceBudgetHC in context.Set<ProjectResourceBudgetHCHistory>()
                                                   where projectResourceBudgetHC.Project_Id == currentProject.Project_Id && projectResourceBudgetHC.Version == currentProject.Version
                                                   select projectResourceBudgetHC).ToList();
            projectResourceBudgetHCCompareInfo.Current = _mapper.Map<List<ProjectResourceBudgetHCDTO>>(currentProjectResourceBudgetHCs);

            // previous info
            var previousProjectResourceBudgetHCs = (from projectResourceBudgetHC in context.Set<ProjectResourceBudgetHCHistory>()
                                                    where projectResourceBudgetHC.Project_Id == previousProject.Project_Id && projectResourceBudgetHC.Version == previousProject.Version
                                                    select projectResourceBudgetHC).ToList();
            projectResourceBudgetHCCompareInfo.Previous = _mapper.Map<List<ProjectResourceBudgetHCDTO>>(previousProjectResourceBudgetHCs);

            contractCompareModel.ProjectBudgetHC = projectResourceBudgetHCCompareInfo;
            #endregion

            #region 5.项目其它成本费用预算历史
            CompareInfo<List<ProjectOtherBudgetDTO>> projectOtherBudgetCompareInfo = new CompareInfo<List<ProjectOtherBudgetDTO>>();
            // current info
            var currentProjectOtherBudgets = (from projectOtherBudget in context.Set<ProjectOtherBudgetHistory>()
                                              where projectOtherBudget.Project_Id == currentProject.Project_Id && projectOtherBudget.Version == currentProject.Version
                                              select projectOtherBudget).ToList();
            projectOtherBudgetCompareInfo.Current = _mapper.Map<List<ProjectOtherBudgetDTO>>(currentProjectOtherBudgets);

            // previous info
            var previousProjectOtherBudgets = (from projectResourceBudget in context.Set<ProjectOtherBudgetHistory>()
                                               where projectResourceBudget.Project_Id == previousProject.Project_Id && projectResourceBudget.Version == previousProject.Version
                                               select projectResourceBudget).ToList();
            projectOtherBudgetCompareInfo.Previous = _mapper.Map<List<ProjectOtherBudgetDTO>>(previousProjectOtherBudgets);

            contractCompareModel.ProjectOtherBudget = projectOtherBudgetCompareInfo;
            #endregion

            #region 6.项目预算汇总历史
            var keyItemSource = context.Set<ProjectBudgetKeyItem>().ToList();
            CompareInfo<List<ProjectBudgetSummaryDTO>> projectBudgetSummaryCompareInfo = new CompareInfo<List<ProjectBudgetSummaryDTO>>();
            // current info
            var currentProjectBudgetSummarys = (from projectBudgetSummary in context.Set<ProjectBudgetSummaryHistory>()
                                                where projectBudgetSummary.Project_Id == currentProject.Project_Id && projectBudgetSummary.Version == currentProject.Version
                                                select projectBudgetSummary).ToList();
            projectBudgetSummaryCompareInfo.Current = _mapper.Map<List<ProjectBudgetSummaryDTO>>(currentProjectBudgetSummarys);
            projectBudgetSummaryCompareInfo.Current.ForEach(x =>
            {
                var query = keyItemSource.Where(y => y.KeyItemID == x.KeyItemID).ToList();
                if (query.Count > 0)
                {
                    var tempKeyItem = query.First();
                    x.KeyItemCn = tempKeyItem.KeyItemCn;
                    x.KeyItemEn = tempKeyItem.KeyItemEn;
                }
            });

            // previous info
            var previousProjectBudgetSummarys = (from projectBudgetSummary in context.Set<ProjectBudgetSummaryHistory>()
                                                 where projectBudgetSummary.Project_Id == previousProject.Project_Id && projectBudgetSummary.Version == previousProject.Version
                                                 select projectBudgetSummary).ToList();
            projectBudgetSummaryCompareInfo.Previous = _mapper.Map<List<ProjectBudgetSummaryDTO>>(previousProjectBudgetSummarys);
            projectBudgetSummaryCompareInfo.Previous.ForEach(x =>
            {
                var query = keyItemSource.Where(y => y.KeyItemID == x.KeyItemID).ToList();
                if (query.Count > 0)
                {
                    var tempKeyItem = query.First();
                    x.KeyItemCn = tempKeyItem.KeyItemCn;
                    x.KeyItemEn = tempKeyItem.KeyItemEn;
                }
            });

            contractCompareModel.ProjectBudgetSummary = projectBudgetSummaryCompareInfo;
            #endregion

            #region 7.项目附件列表历史
            CompareInfo<List<ProjectAttachmentListDTO>> projectAttachmentListCompareInfo = new CompareInfo<List<ProjectAttachmentListDTO>>();
            // current info
            var currentProjectAttachmentList = (from projectAttachmentList in context.Set<ProjectAttachmentListHistory>()
                                                where projectAttachmentList.Project_Id == currentProject.Project_Id && projectAttachmentList.Version == currentProject.Version
                                                select projectAttachmentList).ToList();
            projectAttachmentListCompareInfo.Current = _mapper.Map<List<ProjectAttachmentListDTO>>(currentProjectAttachmentList);

            // previous info
            var previousProjectAttachmentList = (from projectAttachmentList in context.Set<ProjectAttachmentListHistory>()
                                                 where projectAttachmentList.Project_Id == previousProject.Project_Id && projectAttachmentList.Version == previousProject.Version
                                                 select projectAttachmentList).ToList();
            projectAttachmentListCompareInfo.Previous = _mapper.Map<List<ProjectAttachmentListDTO>>(previousProjectAttachmentList);

            contractCompareModel.ProjectAttachment = projectAttachmentListCompareInfo;
            #endregion

            #region 8.根据Id字段填充Text字段
            FillProjectText(contractCompareModel.Project.Current);
            FillProjectText(contractCompareModel.Project.Previous);
            #endregion

            #region 9.工作流审批步骤

            CompareInfo<List<Sys_WorkFlowTableStep>> sysWorkFlowTableStepCompareInfo = new CompareInfo<List<Sys_WorkFlowTableStep>>();
            sysWorkFlowTableStepCompareInfo.Previous = LoadSys_WorkFlowTableStep(previousProject?.WorkFlowTable_Id);
            sysWorkFlowTableStepCompareInfo.Current = LoadSys_WorkFlowTableStep(currentProject?.WorkFlowTable_Id);
            contractCompareModel.SysWorkFlowTableStep = sysWorkFlowTableStepCompareInfo;

            #endregion

            return WebResponseContent.Instance.OK("获取项目对比信息成功", contractCompareModel);
        }

        /// <summary>
        /// 查询项目变更前后对比信息_审计
        /// </summary>
        /// <param name="projectId">项目历史Id</param>
        /// <returns></returns>
        public WebResponseContent GetProjectCompareInfoForAudit(int projectId)
        {
            BCSContext context = DBServerProvider.GetEFDbContext();

            ProjectCompareModel contractCompareModel = new ProjectCompareModel();

            #region 1.项目信息
            CompareInfo<ProjectOutPutDTO> projectCompareInfo = new CompareInfo<ProjectOutPutDTO>();
            var currentProject = (from project in context.Set<Project>() where project.Id == projectId select project).FirstOrDefault();
            if (currentProject == null)
            {
                return WebResponseContent.Instance.Error("项目信息不存在");
            }
            projectCompareInfo.Current = _mapper.Map<ProjectOutPutDTO>(currentProject);

            #region 与当前项目信息对应的合同信息和客户信息
            var currentContracts = (from contractProject in context.Set<ContractProject>()
                                    where contractProject.Project_Id == currentProject.Id && contractProject.Status == (int)Status.Active
                                    join contract in context.Set<Entity.DomainModels.Contract>()
                                    on contractProject.Contract_Id equals contract.Id
                                    select contract).ToList();
            projectCompareInfo.Current.Contract = _mapper.Map<List<ContractSaveModel>>(currentContracts);

            if (currentContracts.Count > 0)
            {
                var contract = currentContracts[0];
                var currentClient = (from client in context.Set<BCS.Entity.DomainModels.Client>()
                                     where client.Id == contract.Client_Id
                                     select client).FirstOrDefault();
                projectCompareInfo.Current.Client = _mapper.Map<Entity.DTO.Contract.Client>(currentClient);
            }
            #endregion

            // 获取最新的项目历史信息.
            var previousProject = context.Set<ProjectHistory>()
                .Where(o => o.Project_Id == currentProject.Id && o.ChangeSource == 2)
                .OrderByDescending(o => o.Version)
                .FirstOrDefault();
            if (previousProject == null)
            {
                return WebResponseContent.Instance.Error("最新的项目历史信息不存在");
            }
            projectCompareInfo.Previous = _mapper.Map<ProjectOutPutDTO>(previousProject);

            #region 与前一个项目历史信息对应的合同历史信息和客户信息
            var previousContracts = (from contractProject in context.Set<ContractProjectHistory>()
                                     where contractProject.Project_Id == previousProject.Project_Id && contractProject.Version == previousProject.ContractVersion
                                     join contract in context.Set<ContractHistory>()
                                     on new { contractProject.Contract_Id, contractProject.Version } equals new { contract.Contract_Id, contract.Version }
                                     select contract).ToList();
            projectCompareInfo.Previous.Contract = _mapper.Map<List<ContractSaveModel>>(previousContracts);

            if (previousContracts.Count > 0)
            {
                var contract = previousContracts[0];
                var previousClient = (from client in context.Set<BCS.Entity.DomainModels.Client>()
                                      where client.Id == contract.Client_Id
                                      select client).FirstOrDefault();
                projectCompareInfo.Previous.Client = _mapper.Map<Entity.DTO.Contract.Client>(previousClient);
            }
            #endregion

            contractCompareModel.Project = projectCompareInfo;
            #endregion

            #region 2.项目计划信息对比
            CompareInfo<List<ProjectPlanInfoDTO>> projectPlanCompareInfo = new CompareInfo<List<ProjectPlanInfoDTO>>();
            // current info
            var currentProjectPlans = (from projectPlan in context.Set<ProjectPlanInfo>()
                                       where projectPlan.Project_Id == currentProject.Id
                                       select projectPlan).ToList();
            projectPlanCompareInfo.Current = _mapper.Map<List<ProjectPlanInfoDTO>>(currentProjectPlans);

            // previous info
            var previousProjectPlans = (from projectPlan in context.Set<ProjectPlanInfoHistory>()
                                        where projectPlan.Project_Id == previousProject.Project_Id && projectPlan.Version == previousProject.Version
                                        select projectPlan).ToList();
            projectPlanCompareInfo.Previous = _mapper.Map<List<ProjectPlanInfoDTO>>(previousProjectPlans);

            contractCompareModel.ProjectPlanInfo = projectPlanCompareInfo;
            #endregion

            #region 3.项目资源预算对比
            CompareInfo<List<ProjectResourceBudgetDTO>> projectResourceBudgetCompareInfo = new CompareInfo<List<ProjectResourceBudgetDTO>>();
            // current info
            var currentProjectResourceBudgets = (from projectResourceBudget in context.Set<ProjectResourceBudget>()
                                                 where projectResourceBudget.Project_Id == currentProject.Id
                                                 select projectResourceBudget).ToList();
            projectResourceBudgetCompareInfo.Current = _mapper.Map<List<ProjectResourceBudgetDTO>>(currentProjectResourceBudgets);

            // previous info
            var previousProjectResourceBudgets = (from projectResourceBudget in context.Set<ProjectResourceBudgetHistory>()
                                                  where projectResourceBudget.Project_Id == previousProject.Project_Id && projectResourceBudget.Version == previousProject.Version
                                                  select projectResourceBudget).ToList();
            projectResourceBudgetCompareInfo.Previous = _mapper.Map<List<ProjectResourceBudgetDTO>>(previousProjectResourceBudgets);

            contractCompareModel.ProjectResourceBudget = projectResourceBudgetCompareInfo;
            #endregion

            #region 4.项目资源预算HC对比
            CompareInfo<List<ProjectResourceBudgetHCDTO>> projectResourceBudgetHCCompareInfo = new CompareInfo<List<ProjectResourceBudgetHCDTO>>();
            // current info
            var currentProjectResourceBudgetHCs = (from projectResourceBudgetHC in context.Set<ProjectResourceBudgetHC>()
                                                   where projectResourceBudgetHC.Project_Id == currentProject.Id
                                                   select projectResourceBudgetHC).ToList();
            projectResourceBudgetHCCompareInfo.Current = _mapper.Map<List<ProjectResourceBudgetHCDTO>>(currentProjectResourceBudgetHCs);

            // previous info
            var previousProjectResourceBudgetHCs = (from projectResourceBudgetHC in context.Set<ProjectResourceBudgetHCHistory>()
                                                    where projectResourceBudgetHC.Project_Id == previousProject.Project_Id && projectResourceBudgetHC.Version == previousProject.Version
                                                    select projectResourceBudgetHC).ToList();
            projectResourceBudgetHCCompareInfo.Previous = _mapper.Map<List<ProjectResourceBudgetHCDTO>>(previousProjectResourceBudgetHCs);

            contractCompareModel.ProjectBudgetHC = projectResourceBudgetHCCompareInfo;
            #endregion

            #region 5.项目其它成本费用预算对比
            CompareInfo<List<ProjectOtherBudgetDTO>> projectOtherBudgetCompareInfo = new CompareInfo<List<ProjectOtherBudgetDTO>>();
            // current info
            var currentProjectOtherBudgets = (from projectOtherBudget in context.Set<ProjectOtherBudget>()
                                              where projectOtherBudget.Project_Id == currentProject.Id
                                              select projectOtherBudget).ToList();
            projectOtherBudgetCompareInfo.Current = _mapper.Map<List<ProjectOtherBudgetDTO>>(currentProjectOtherBudgets);

            // previous info
            var previousProjectOtherBudgets = (from projectResourceBudget in context.Set<ProjectOtherBudgetHistory>()
                                               where projectResourceBudget.Project_Id == previousProject.Project_Id && projectResourceBudget.Version == previousProject.Version
                                               select projectResourceBudget).ToList();
            projectOtherBudgetCompareInfo.Previous = _mapper.Map<List<ProjectOtherBudgetDTO>>(previousProjectOtherBudgets);

            contractCompareModel.ProjectOtherBudget = projectOtherBudgetCompareInfo;
            #endregion

            #region 6.项目预算汇总对比

            var keyItemSource = context.Set<ProjectBudgetKeyItem>().ToList();
            CompareInfo<List<ProjectBudgetSummaryDTO>> projectBudgetSummaryCompareInfo = new CompareInfo<List<ProjectBudgetSummaryDTO>>();
            // current info
            var currentProjectBudgetSummarys = (from projectBudgetSummary in context.Set<ProjectBudgetSummary>()
                                                where projectBudgetSummary.Project_Id == currentProject.Id
                                                select projectBudgetSummary).ToList();
            projectBudgetSummaryCompareInfo.Current = _mapper.Map<List<ProjectBudgetSummaryDTO>>(currentProjectBudgetSummarys);
            projectBudgetSummaryCompareInfo.Current.ForEach(x =>
            {
                var query = keyItemSource.Where(y => y.KeyItemID == x.KeyItemID).ToList();
                if (query.Count > 0)
                {
                    var tempKeyItem = query.First();
                    x.KeyItemCn = tempKeyItem.KeyItemCn;
                    x.KeyItemEn = tempKeyItem.KeyItemEn;
                }
            });
            // previous info
            var previousProjectBudgetSummarys = (from projectBudgetSummary in context.Set<ProjectBudgetSummaryHistory>()
                                                 where projectBudgetSummary.Project_Id == previousProject.Project_Id && projectBudgetSummary.Version == previousProject.Version
                                                 select projectBudgetSummary).ToList();
            projectBudgetSummaryCompareInfo.Previous = _mapper.Map<List<ProjectBudgetSummaryDTO>>(previousProjectBudgetSummarys);
            projectBudgetSummaryCompareInfo.Previous.ForEach(x =>
             {
                 var query = keyItemSource.Where(y => y.KeyItemID == x.KeyItemID).ToList();
                 if (query.Count > 0)
                 {
                     var tempKeyItem = query.First();
                     x.KeyItemCn = tempKeyItem.KeyItemCn;
                     x.KeyItemEn = tempKeyItem.KeyItemEn;
                 }
             });

            contractCompareModel.ProjectBudgetSummary = projectBudgetSummaryCompareInfo;
            #endregion

            #region 7.项目附件列表对比
            CompareInfo<List<ProjectAttachmentListDTO>> projectAttachmentListCompareInfo = new CompareInfo<List<ProjectAttachmentListDTO>>();
            // current info
            var currentProjectAttachmentList = (from projectAttachmentList in context.Set<ProjectAttachmentList>()
                                                where projectAttachmentList.Project_Id == currentProject.Id
                                                select projectAttachmentList).ToList();
            projectAttachmentListCompareInfo.Current = _mapper.Map<List<ProjectAttachmentListDTO>>(currentProjectAttachmentList);

            // previous info
            var previousProjectAttachmentList = (from projectAttachmentList in context.Set<ProjectAttachmentListHistory>()
                                                 where projectAttachmentList.Project_Id == previousProject.Project_Id && projectAttachmentList.Version == previousProject.Version
                                                 select projectAttachmentList).ToList();
            projectAttachmentListCompareInfo.Previous = _mapper.Map<List<ProjectAttachmentListDTO>>(previousProjectAttachmentList);

            contractCompareModel.ProjectAttachment = projectAttachmentListCompareInfo;
            #endregion

            #region 8.根据Id字段填充Text字段
            FillProjectText(contractCompareModel.Project.Current);
            FillProjectText(contractCompareModel.Project.Previous);
            #endregion

            #region 9.工作流审批步骤

            CompareInfo<List<Sys_WorkFlowTableStep>> sysWorkFlowTableStepCompareInfo = new CompareInfo<List<Sys_WorkFlowTableStep>>();
            sysWorkFlowTableStepCompareInfo.Previous = LoadSys_WorkFlowTableStep(previousProject?.WorkFlowTable_Id);
            sysWorkFlowTableStepCompareInfo.Current = LoadSys_WorkFlowTableStep(currentProject?.WorkFlowTable_Id);
            contractCompareModel.SysWorkFlowTableStep = sysWorkFlowTableStepCompareInfo;

            #endregion

            return WebResponseContent.Instance.OK("获取项目对比信息成功", contractCompareModel);
        }

        /// <summary>
        /// 根据项目id查询项目历史信息
        /// </summary>
        /// <param name="projectId">项目Id</param>
        /// <returns></returns>
        public WebResponseContent GetProjectHistoryListByProjectId(int projectId)
        {
            BCSContext context = DBServerProvider.GetEFDbContext();
            var projectHistoryList = (from projectHistory in context.Set<ProjectHistory>()
                                      where projectHistory.Project_Id == projectId && projectHistory.ChangeSource == 2
                                      orderby projectHistory.Version descending
                                      select projectHistory).ToList();

            var datas = _mapper.Map<List<ProjectDTO>>(projectHistoryList);

            return WebResponseContent.Instance.OK("获取项目历史信息成功", datas);
        }

        private void SendMail(Project project, List<int> nextUserIds)
        {
            //验证当前项目是否存在审批同意的历史记录。
            bool existsHistoryInfo = _projectHistoryRepository.Exists(o => o.Project_Id == project.Id && o.ChangeSource == 2);
            int emailTemplateId = existsHistoryInfo ? 4 : 3;
            var emailTemplate = _emailTemplateRepository.FindAsyncFirst(x => x.Type == emailTemplateId && x.IsActive == 1).GetAwaiter().GetResult();
            if (emailTemplate == null)
            {
                return;
            }
            //这里可以给下一批审批发送邮件通知
            BCSContext context = DBServerProvider.GetEFDbContext();
            var userInfo = (from sysRole in context.Set<Sys_Role>()
                            where sysRole.RoleName == CommonConst.CEO
                            join sysUser in context.Set<Sys_User>()
                            on new { sysRole.Role_Id } equals new { sysUser.Role_Id }
                            select sysUser).ToList();

            List<string> emailList = userInfo.Where(x => !string.IsNullOrWhiteSpace(x.Email)).Select(x => x.Email).ToList();

            string subject = string.Format(emailTemplate.Subject, project.Project_Code, project.Project_Name);
            var projectViewUrl = string.Format("{0}/#/project-application-view?id={1}&workflowId={2}", AppSetting.PortalUrl, project.Id, project.WorkFlowTable_Id);
            string body = string.Format(emailTemplate.Body, project.Project_Code, project.Project_Name, projectViewUrl);
            EmailSendLog entity = new EmailSendLog()
            {
                Subject = subject,
                Body = body,
                Recipients = string.Join(",", emailList),
                EmailTemplateId = emailTemplateId,
                SendTime = DateTime.Now,
                SendStatus = (byte)SuccessStatus.Succeed
            };

            //发送邮件方法
            var result = MailHelper.Send(subject, body, emailList.ToArray());
            entity.SendStatus = (byte)(result.Result ? SuccessStatus.Succeed : SuccessStatus.Fail);
            _emailSendLogRepository.Add(entity);
            _emailSendLogRepository.SaveChanges();
        }

        private void FillProjectText(ProjectDTO item)
        {
            item.Project_Type = ConverterContainer.ProjectTypeConverter(item.Project_TypeId);
            item.Cooperation_Type = ConverterContainer.CooperationTypeConverter(item.Cooperation_TypeId);
            item.Billing_Mode = ConverterContainer.BillingModeConverter(item.Billing_ModeId);
            item.Service_Type = ConverterContainer.ServiceTypeConverter(item.Service_TypeId);
            item.Billing_Cycle = ConverterContainer.BillingCycleConverter(item.Billing_CycleId);
            item.Site_Type = ConverterContainer.SiteConverter(item.Site_TypeId);
            item.Shore_Type = ConverterContainer.ShoreConverter(item.Shore_TypeId);
            item.Holiday_System = ConverterContainer.HolidaySystemConverter(item.Holiday_SystemId);
            item.Standard_Number_of_Days_Per_Month = ConverterContainer.StandardNumberofDaysperMonthConverter(item.Standard_Number_of_Days_Per_MonthId);
            item.Change_Type = ConverterContainer.ChangeTypeConverter(item.Change_TypeId);
        }

        /// <summary>
        /// 加载项目额外信息
        /// </summary>
        /// <param name="project"></param>
        private void DecorateProjectExtraInfo(Project project)
        {
            if (project == null)
            {
                return;
            }
            BCSContext context = DBServerProvider.GetEFDbContext();

            //验证当前项目是否存在审批同意的历史记录。
            //1.是否变更
            bool existsHistoryInfo = _projectHistoryRepository.Exists(o => o.Project_Id == project.Id && o.ChangeSource == 2);
            project.Is_Handle_Change = existsHistoryInfo ? 1 : 0;
            //2.是否PO
            var query = from ProjectItem in context.Set<BCS.Entity.DomainModels.Project>()
                        where project.Id == ProjectItem.Id
                        join ContractProjectItem in context.Set<ContractProject>() on new { Project_Id = ProjectItem.Id, Status = (int)Status.Active } equals new { ContractProjectItem.Project_Id, ContractProjectItem.Status }
                        join ContractItem in context.Set<BCS.Entity.DomainModels.Contract>() on ContractProjectItem.Contract_Id equals ContractItem.Id
                        select ContractItem;
            if (query.ToList().Any(o => o.IsPO == 1))
            {
                project.IsPO = 1;
            }

            //3.预算汇总(毛利率≥基线目标)
            // 部门指标目标：
            if (_sys_DepartmentSettingRepository.Exists(o => o.Year == DateTime.Now.Year && o.DepartmentId.ToString() == project.Delivery_Department_Id.ToString()))
            {
                var departmentSetting = _sys_DepartmentSettingRepository.FindFirst(o => o.Year == DateTime.Now.Year && o.DepartmentId.ToString() == project.Delivery_Department_Id.ToString());
                project.ProjectGPMDepartment = departmentSetting.ProjectGPM ?? 0;
            }

            // 项目预设目标：
            if (_projectBudgetSummaryRepository.Exists(o => o.Project_Id == project.Id && o.KeyItemID == 1009))
            {
                // 更新实体
                //KeyItemId KeyItemEn   KeyItemCn
                //1001	Income of Own Delivery	自有交付收入
                //1002	Income of Subcontract (Exclude Tax)	分包收入(不含税)
                //1003	Labor Cost of Own Delivery	自有交付人力成本
                //1004	Cost of Subcontract (Exclude Tax)	分包成本(不含税)
                //1005	Other Project Cost	其它成本费用
                //1006	GP of Own Delivery	自有交付毛利
                //1007	GPM of Own Delivery (%)	自有交付毛利率
                //1008	Project GP	项目毛利
                //1009	Project GPM (%)	项目毛利率
                var singleProjectBudget = _projectBudgetSummaryRepository.FindFirst(o => o.Project_Id == project.Id && o.KeyItemID == 1009);
                project.ProjectGPM = singleProjectBudget.PlanAmount;
            }
        }

        /// <summary>
        /// 加载工作流审批步骤
        /// </summary>
        /// <param name="workFlowTable_Id"></param>
        private List<Sys_WorkFlowTableStep> LoadSys_WorkFlowTableStep(Guid? workFlowTable_Id)
        {
            BCSContext context = DBServerProvider.GetEFDbContext();
            return context.Set<Sys_WorkFlowTableStep>().Where(o => o.WorkFlowTable_Id == workFlowTable_Id).OrderByDescending(o => o.OrderId).ToList();
        }

        //生成项目编码
        public string GenerateProjectCode()
        {
            var currentMonthProjectCount = repository.Find(x => x.CreateDate.Year == DateTime.Now.Year && x.CreateDate.Month == DateTime.Now.Month && (x.Project_TypeId == (int)ProjectType.Deliver || x.Project_TypeId == (int)ProjectType.Purchase)).Count;
            return $"P{DateTime.Now.ToString("yyyyMM")}{(currentMonthProjectCount + 1).ToString("0000")}";
        }
    }
}
