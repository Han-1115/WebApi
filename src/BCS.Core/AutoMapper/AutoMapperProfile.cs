using AutoMapper;
using BCS.Core.Enums;
using BCS.Entity.DomainModels;
using BCS.Entity.DTO.Contract;
using BCS.Entity.DTO.Flow;
using BCS.Entity.DTO.Project;
using BCS.Entity.DTO.Staff;
using BCS.Entity.DTO.SubcontractingContract;
using BCS.Entity.DTO.SubcontractingStaff;
using BCS.Entity.DTO.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCS.Core.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            #region 合同
            CreateMap<Contract, ContractHistory>().ReverseMap();

            CreateMap<ContractProject, ContractProjectHistory>().ReverseMap();

            CreateMap<Entity.DomainModels.ContractAttachments, ContractAttachmentsHistory>().ReverseMap();

            CreateMap<Contract, ContractSaveModel>();

            CreateMap<ContractSaveModel, Contract>();

            CreateMap<BCS.Entity.DomainModels.Client, Entity.DTO.Contract.Client>();

            CreateMap<Contract, Frame_Contract>();

            CreateMap<Project, Contract_Project>();

            CreateMap<ProjectHistory, Contract_Project>();

            CreateMap<ContractHistory, ContractCompareDTO>();

            CreateMap<Contract, ContractCompareDTO>();

            CreateMap<ContractHistory, Frame_Contract>();

            CreateMap<BCS.Entity.DomainModels.ContractAttachmentsHistory, Entity.DTO.Contract.ContractAttachments>();

            CreateMap<BCS.Entity.DomainModels.ContractAttachments, Entity.DTO.Contract.ContractAttachments>();

            CreateMap<ContractSaveModel, ContractHistory>().ReverseMap();
            #endregion

            #region 项目

            CreateMap<Project, ProjectDTO>();
            CreateMap<Project, ProjectOutPutDTO>();

            CreateMap<ProjectDTO, Project>();

            CreateMap<ProjectPlanInfo, ProjectPlanInfoDTO>();

            CreateMap<ProjectPlanInfoDTO, ProjectPlanInfo>();

            CreateMap<ProjectAttachmentList, ProjectAttachmentListDTO>();

            CreateMap<ProjectAttachmentListDTO, ProjectAttachmentList>();

            CreateMap<ProjectResourceBudget, ProjectResourceBudgetDTO>();

            CreateMap<ProjectResourceBudgetDTO, ProjectResourceBudget>();

            CreateMap<ProjectResourceBudgetHC, ProjectResourceBudgetHCDTO>();

            CreateMap<ProjectResourceBudgetHCDTO, ProjectResourceBudgetHC>();

            CreateMap<ProjectOtherBudget, ProjectOtherBudgetDTO>();

            CreateMap<ProjectOtherBudgetDTO, ProjectOtherBudget>();

            CreateMap<ProjectBudgetSummary, ProjectBudgetSummaryDTO>();

            CreateMap<ProjectBudgetSummaryDTO, ProjectBudgetSummary>();

            //History Map            
            CreateMap<ProjectHistory, ProjectDTO>().ReverseMap();

            CreateMap<ProjectHistory, ProjectOutPutDTO>().ReverseMap();

            CreateMap<ProjectPlanInfoHistory, ProjectPlanInfoDTO>().ReverseMap();

            CreateMap<ProjectAttachmentListHistory, ProjectAttachmentListDTO>().ReverseMap();

            CreateMap<ProjectResourceBudgetHistory, ProjectResourceBudgetDTO>().ReverseMap();

            CreateMap<ProjectOtherBudgetHistory, ProjectOtherBudgetDTO>().ReverseMap();

            CreateMap<ProjectBudgetSummaryHistory, ProjectBudgetSummaryDTO>().ReverseMap();

            CreateMap<ProjectResourceBudgetHCHistory, ProjectResourceBudgetHCDTO>().ReverseMap();

            CreateMap<ProjectListOutPutDTO, ProjectPagerExport>()
                .ForMember(s => s.Purely_Subcontracted_Project, d => d.MapFrom(s => s.IsPurely_Subcontracted_Project == 0 ? "No" : "Yes"))
                .ForMember(s => s.Start_Date, d => d.MapFrom(s => s.Start_Date.ToString("yyyy-MM-dd")))
                .ForMember(s => s.End_Date, d => d.MapFrom(s => s.End_Date.ToString("yyyy-MM-dd")))
                .ForMember(s => s.Approval_Status, d => d.MapFrom(s => Enum.IsDefined(typeof(ApprovalStatus), s.Approval_Status) ? ((ApprovalStatus)s.Approval_Status).ToString() : "UnKnow"))
                .ForMember(s => s.Project_Status, d => d.MapFrom(s => Enum.IsDefined(typeof(ProjectStatus), s.Project_Status) ? ((ProjectStatus)s.Project_Status).ToString() : "UnKnow"))
                .ForMember(s => s.Operating_Status, d => d.MapFrom(s => Enum.IsDefined(typeof(OperatingStatus), s.Operating_Status) ? ((OperatingStatus)s.Operating_Status).ToString() : "UnKnow"));

            CreateMap<ProjectListOutPutDTO, ContractProjectPagerExport>()
                .ForMember(s => s.Start_Date, d => d.MapFrom(s => s.Start_Date.ToString("yyyy-MM-dd")))
                .ForMember(s => s.End_Date, d => d.MapFrom(s => s.End_Date.ToString("yyyy-MM-dd")))
                .ForMember(s => s.Project_Status, d => d.MapFrom(s => Enum.IsDefined(typeof(ProjectStatus), s.Project_Status) ? ((ProjectStatus)s.Project_Status).ToString() : "UnKnow"))
                .ForMember(s => s.IsPo, d => d.MapFrom(s => s.IsPo == 0 ? "No" : "Yes"))
                .ForMember(s => s.Category, d => d.MapFrom(s => Enum.IsDefined(typeof(ContractCategory), s.Category) ? ((ContractCategory)s.Category).ToString() : "UnKnow"))
                .ForMember(s => s.Contract_Takenback_Date, d => d.MapFrom(s => s.Contract_Takenback_Date.HasValue ? s.Contract_Takenback_Date.Value.ToString("yyyy-MM-dd") : ""))
                .ForMember(s => s.Shore_TypeId, d => d.MapFrom(s => s.Shore_TypeId == 0 ? "offshore" : "onshore"))
                .ForMember(s => s.IsPurely_Subcontracted_Project, d => d.MapFrom(s => s.IsPurely_Subcontracted_Project == 0 ? "No" : "Yes"));

            CreateMap<ProjectResourceBudgetDTO, ProjectResourceBudgetPagerExport>()
                .ForMember(s => s.Start_Date, d => d.MapFrom(s => s.Start_Date.ToString("yyyy-MM-dd")))
                .ForMember(s => s.End_Date, d => d.MapFrom(s => s.End_Date.ToString("yyyy-MM-dd")))
                .ForMember(s => s.Is_Charge_Rate_Type, d => d.MapFrom(s => s.Is_Charge_Rate_Type == 0 ? "No" : "Yes"));
            #endregion

            #region 工作流
            CreateMap<Sys_WorkFlowTable, WorkFlowPagerModel>().ReverseMap();
            #endregion

            #region 系统设置

            CreateMap<Sys_Calendar, Sys_CalendarDTO>().ReverseMap();
            CreateMap<Sys_DepartmentSetting, Sys_DepartmentSettingDTO>().ReverseMap();
            CreateMap<Sys_SalaryMap, Sys_SalaryMapDTO>().ReverseMap();

            #endregion

            #region 人员管理

            CreateMap<StaffSaveModel, Staff>();
            CreateMap<StaffProjectDetails, StaffProject>().ReverseMap();
            CreateMap<StaffProjectDetailsV2, StaffProjectHistory>().ReverseMap();
            CreateMap<StaffProjectDetailsV2, StaffProject>()
                .ForMember(s => s.Id, d => d.MapFrom(s => s.StaffProjectId));
            CreateMap<StaffProject, StaffProjectDetailsV2>()
                .ForMember(s => s.StaffProjectId, d => d.MapFrom(s => s.Id));

            #endregion

            #region 员工及员工考勤管理
            CreateMap<StaffPagerModel, StaffPagerExport>()
               .ForMember(s => s.Project_Type, d => d.MapFrom(s => ConverterContainer.ConverterContainer.ProjectTypeConverter(s.Project_TypeId)))
               .ForMember(s => s.IsSubcontract, d => d.MapFrom(s => Enum.GetName(typeof(YesOrNoEnum), s.IsSubcontract)))
               .ForMember(s => s.InputStartDate, d => d.MapFrom(s => s.InputStartDate.HasValue ? s.InputStartDate.Value.ToString("yyyy-MM-dd") : ""))
               .ForMember(s => s.InputEndDate, d => d.MapFrom(s => s.InputEndDate.HasValue ? s.InputEndDate.Value.ToString("yyyy-MM-dd") : ""))
               .ForMember(s => s.Project_Start_Date, d => d.MapFrom(s => s.Project_Start_Date.HasValue ? s.Project_Start_Date.Value.ToString("yyyy-MM-dd") : ""))
               .ForMember(s => s.Project_End_Date, d => d.MapFrom(s => s.Project_End_Date.HasValue ? s.Project_End_Date.Value.ToString("yyyy-MM-dd") : ""));

            CreateMap<StaffProjectPagerModel, StaffProjectPagerExport>()
              .ForMember(s => s.Project_Type, d => d.MapFrom(s => ConverterContainer.ConverterContainer.ProjectTypeConverter(s.Project_TypeId)))
              .ForMember(s => s.Start_Date, d => d.MapFrom(s => s.Start_Date.HasValue ? s.Start_Date.Value.ToString("yyyy-MM-dd") : ""))
              .ForMember(s => s.End_Date, d => d.MapFrom(s => s.End_Date.HasValue ? s.End_Date.Value.ToString("yyyy-MM-dd") : ""))
              .ForMember(s => s.Entry_Exit_Project_Status, d => d.MapFrom(s => s.Entry_Exit_Project_Status.HasValue ? ConverterContainer.ConverterContainer.EntryExitProjectStatusConverter(s.Entry_Exit_Project_Status.Value) : ""));
            CreateMap<StaffAttendanceDashboardModel, StaffAttendanceDashboardExport>();
            CreateMap<StaffAttendanceSummaryModel, StaffAttendanceSummaryExport>();
            CreateMap<StaffCostSummaryModel, StaffCostSummaryExport>();

            CreateMap<StaffProjectChargeChangesModel, StaffChargeChangesExport>()
                .ForMember(s => s.IsSubcontract, d => d.MapFrom(s => Enum.GetName(typeof(YesOrNoEnum), s.IsSubcontract)));
            #endregion

            #region 分包合同
            CreateMap<SubContractFlow, SubContractFlowSaveModel>().ReverseMap();
            CreateMap<SubContractFlowAttachment, SubContractFlowAttachmentModel>().ReverseMap();
            CreateMap<SubcontractingContract, SubcontractingContractDto>().ReverseMap();
            CreateMap<SubContractFlow, SubcontractingContract>()
               .ForMember(s => s.ModifyID, d => d.MapFrom(s => s.CreateID))
               .ForMember(s => s.Modifier, d => d.MapFrom(s => s.Creator))
               .ForMember(s => s.ModifyDate, d => d.MapFrom(s => s.CreateDate))
               .ForMember(s => s.Id, d => d.MapFrom(s => s.SubContract_Id));
            CreateMap<SubContractFlowAttachment, SubcontractingContractAttachment>()
                .ForMember(s => s.Id, d => d.MapFrom(s => s.AttachmentId));
            CreateMap<SubContractFlowPaymentPlan, SubcontractingContractPaymentPlan>()
                .ForMember(s => s.Id, d => d.MapFrom(s => s.PaymentPlanId));
            #endregion

            #region 分包人员档案库管理
            CreateMap<SubcontractingStaffListDetails, SubcontractingStaffPagerExport>()
                .ForMember(s => s.IsInEffect, d => d.MapFrom(s => Enum.GetName(typeof(YesOrNoEnum), s.IsInEffect)));
            CreateMap<SubContractFlowStaff, SubcontractingStaff>()
                .ForMember(s => s.Id, d => d.MapFrom(s => s.SubContractStaffId));
            #endregion

            #region 分包历史信息
            CreateMap<SubContractFlow, SubcontractHistoryContractAndProjectInfo>().ReverseMap();
            CreateMap<SubContractFlow, SubcontractHistoryBasicInfo>().ReverseMap();
            #endregion
        }
    }
}
