using BCS.Entity.DTO.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCS.Entity.DomainModels;
using Microsoft.AspNetCore.Mvc;

namespace BCS.Entity.DTO.Project
{

    public class ProjectUpdateDTO
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 变更类型
        /// <para>1:变更项目经理</para>
        /// <para>2:变更项目总监</para>
        /// </summary>
        public int UpdateType { get; set; }

        /// <summary>
        /// 项目经理Id
        /// </summary>
        public int Project_Manager_Id { get; set; }

        /// <summary>
        /// 项目经理
        /// </summary>
        public string Project_Manager { get; set; }

        /// <summary>
        /// 项目总监ID
        /// </summary>
        public int Project_Director_Id { get; set; }

        /// <summary>
        /// 项目总监
        /// </summary>
        public string Project_Director { get; set; }


    }
    public class ProjectDTO : BaseDTO
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 客户合同对应关系
        /// </summary>
        public string Contract_Project_Relationship { get; set; }

        /// <summary>
        /// 项目编码
        /// </summary>
        public string Project_Code { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string Project_Name { get; set; }

        /// <summary>
        /// 项目金额
        /// </summary>
        public decimal Project_Amount { get; set; }

        /// <summary>
        /// 项目类型Id
        /// </summary>
        public int Project_TypeId { get; set; }

        /// <summary>
        /// 项目类型
        /// </summary>
        public string Project_Type { get; set; }

        /// <summary>
        /// 执行部门Id
        /// </summary>
        public string Delivery_Department_Id { get; set; }

        /// <summary>
        /// 执行部门
        /// </summary>
        public string Delivery_Department { get; set; }

        /// <summary>
        /// 项目经理Id
        /// </summary>
        public int? Project_Manager_Id { get; set; }

        public string Project_Manager_Employee_Number { get; set; }

        /// <summary>
        /// 项目经理
        /// </summary>
        public string Project_Manager { get; set; }

        /// <summary>
        ///客户组织名称
        /// </summary>
        public string Client_Organization_Name { get; set; }

        /// <summary>
        /// 合作类型下拉框选择 1:资源,2:任务,3:项目,4:解决方案,5:劳务派遣
        /// </summary>
        public byte Cooperation_TypeId { get; set; }

        /// <summary>
        /// 合作类型下拉框选择 1:资源,2:任务,3:项目,4:解决方案,5:劳务派遣
        /// </summary>
        public string Cooperation_Type { get; set; }

        /// <summary>
        ///结算模式 1:TM-计时,2:TM-计件
        /// </summary>
        public byte Billing_ModeId { get; set; }

        /// <summary>
        ///结算模式 1:TM-计时,2:TM-计件
        /// </summary>
        public string Billing_Mode { get; set; }

        /// <summary>
        /// 项目所在城市
        /// </summary>
        public string Project_LocationCity { get; set; }

        /// <summary>
        /// 项目开始日期
        /// </summary>
        public DateTime Start_Date { get; set; }

        /// <summary>
        /// 项目结束日期
        /// </summary>
        public DateTime End_Date { get; set; }

        /// <summary>
        /// 是否纯分包项目  0否 1是
        /// </summary>
        public byte IsPurely_Subcontracted_Project { get; set; }

        /// <summary>
        /// 服务类型 1:IT交付,2:非IT交付,3:产品研发,4:咨询,5:运维,6:集成
        /// </summary>
        public byte Service_TypeId { get; set; }

        /// <summary>
        /// 服务类型 1:IT交付,2:非IT交付,3:产品研发,4:咨询,5:运维,6:集成
        /// </summary>
        public string Service_Type { get; set; }

        /// <summary>
        /// 结算周期 1:单月,2:双月,3:季度,4:半年度,5:年度,6:里程碑
        /// </summary>
        public byte Billing_CycleId { get; set; }

        /// <summary>
        /// 结算周期 1:单月,2:双月,3:季度,4:半年度,5:年度,6:里程碑
        /// </summary>
        public string Billing_Cycle { get; set; }

        /// <summary>
        /// 预计结算周期 15,30,45,60,90,120,150,180,360
        /// </summary>
        public int Estimated_Billing_Cycle { get; set; }

        /// <summary>
        /// 是offshore还是onshore  0 :offshore, 1: onshore
        /// </summary>
        public byte Shore_TypeId { get; set; }

        /// <summary>
        /// 是offshore还是onshore  0 :offshore, 1: onshore
        /// </summary>
        public string Shore_Type { get; set; }

        /// <summary>
        /// 是offsite还是onsite  0 :offsite, 1: onsite
        /// </summary>
        public byte Site_TypeId { get; set; }

        /// <summary>
        /// 是offsite还是onsite  0 :offsite, 1: onsite
        /// </summary>
        public string Site_Type { get; set; }

        /// <summary>
        /// 节假日体系 1:中国,2:美国,3:日本,4:韩国,5:印度,6:马来西亚,7:新加坡,8:香港,9:菲律宾
        /// </summary>
        public int Holiday_SystemId { get; set; }

        /// <summary>
        /// 节假日体系 1:中国,2:美国,3:日本,4:韩国,5:印度,6:马来西亚,7:新加坡,8:香港,9:菲律宾
        /// </summary>
        public string Holiday_System { get; set; }

        /// <summary>
        /// 月标准天数
        /// </summary>
        public int Standard_Number_of_Days_Per_MonthId { get; set; }

        /// <summary>
        /// 月标准天数
        /// </summary>
        public string Standard_Number_of_Days_Per_Month { get; set; }

        /// <summary>
        /// 日标准小时数 
        /// </summary>
        public decimal Standard_Daily_Hours { get; set; }

        /// <summary>
        /// 项目总监ID
        /// </summary>
        public int Project_Director_Id { get; set; }

        /// <summary>
        /// 项目总监
        /// </summary>
        public string Project_Director { get; set; }

        public string Project_Director_Employee_Number { get; set; }

        /// <summary>
        /// 立项说明
        /// </summary>
        public string Project_Description { get; set; }

        /// <summary>
        /// 变更类型 0:默认,1:项目主动变更，2：合同变更引发项目变更
        /// </summary>
        public byte Change_From { get; set; }

        /// <summary>
        /// 变更类型 0:默认,1:项目主动变更，2：合同变更引发项目变更
        /// </summary>
        public byte Change_TypeId { get; set; }

        /// <summary>
        /// 变更类型 0:默认,1:项目主动变更，2：合同变更引发项目变更
        /// </summary>
        public string Change_Type { get; set; }

        /// <summary>
        /// 变更原因
        /// </summary>
        public string Change_Reason { get; set; }

        /// <summary>
        /// <para>0:默认状态</para>
        /// <para>1:已提交</para>
        /// <para>2:草稿待提交</para>
        /// <para>3:变更待提交</para>
        /// </summary>
        public byte Operating_Status { get; set; }

        /// <summary>
        /// <para>0:默认状态</para>
        /// <para>1:审核通过</para>
        /// <para>2:审批驳回</para>
        /// <para>3:审批中</para>
        /// <para>4:未发起</para>
        /// <para>5:已撤回(发起人主动发起)</para>
        /// </summary>
        public byte Approval_Status { get; set; }

        /// <summary>
        /// <para>0:默认状态</para>
        /// <para>1:进行中</para>
        /// <para>2:正常结项</para>
        /// <para>3:未开始</para>
        /// </summary>
        public byte Project_Status { get; set; }

        /// <summary>
        /// 审批开始时间
        /// </summary>
        public DateTime Approval_StartTime { get; set; }

        /// <summary>
        /// 审批通过时间
        /// </summary>
        public DateTime Approval_EndTime { get; set; }

        /// <summary>
        /// 工作流表Id
        /// </summary>
        public Guid? WorkFlowTable_Id { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 版本号
        /// </summary>
        public new int Version { get; set; }

        /// <summary>
        /// 是否删除（0:否，1:是）
        /// </summary>
        public byte IsDelete { get; set; }

        /// <summary>
        /// 删除时间
        /// </summary>
        public DateTime DeleteTime { get; set; }

        /// <summary>
        /// 是否触发项目变更（0:否，1:是）
        /// </summary>
        public byte Is_Handle_Change { get; set; }
    }

    public class ApprovedProjectListOutPutDTO: ProjectListOutPutDTO
    {
        public string Exchange_Rate { get; set; }
        public string Client_Entity { get; set; }
        public decimal PO_Amount { get; set; }
        public Decimal? SubcontractCostBudgetBalance { get; set; }
    }

    public class ProjectListOutPutDTO : ProjectDTO
    {

        #region Project Extension
        public int Project_Id { get; set; }
        public string Signing_Legal_Entity { get; set; }
        public decimal Income_From_Own_Delivery { get; set; }
        public decimal Subcontracting_Income { get; set; }
        //Own Delivery HR Cost
        public decimal Own_Delivery_HR_Cost { get; set; }
        //Subcontracting Cost
        public decimal Subcontracting_Cost { get; set; }
        //Other Project Costs
        public decimal Other_Project_Costs { get; set; }
        //Gross Profit from Own Delivery
        public decimal Gross_Profit_From_Own_Delivery { get; set; }
        //Gross Profit Margin from Own Delivery (%)
        public decimal Gross_Profit_Margin_From_Own_Delivery { get; set; }
        //Project Gross Profit
        public decimal Project_Gross_Profit { get; set; }
        //Project Gross Profit Margin (%)
        public decimal Project_Gross_Profit_Margin { get; set; }
        public bool IsAllowEdit { get; set; }
        #endregion

        #region Contract Extension
        public int Contract_Id { get; set; }
        public string Procurement_Type { get; set; }
        public string Contract_Code { get; set; }
        public string Customer_Contract_Number { get; set; }
        public string Contract_Name { get; set; }
        public string Settlement_Currency { get; set; }
        public string Conrtact_Settlement_Currency { get; set; }
        public string Client_Contract_Code { get; set; }
        public string Tax_Rate { get; set; }
        public string Client_Contract_Type { get; set; }
        public byte IsPo { get; set; }
        public string Signing_Department { get; set; }
        public int Category { get; set; }
        /// <summary>
        /// 项目计费类型
        /// </summary>
        public string Billing_Type { get; set; }
        /// <summary>
        /// 合同计费类型
        /// </summary>
        public string Contract_Billing_Type { get; set; }
        public decimal PO_Amount { get; set; }
        public string Customer_Contract_Name { get; set; }
        public string Client_line_Group { get; set; }
        public string Sales_Manager { get; set; }
        public int Sales_Manager_Id { get; set; }
        public string Sales_Manager_Employee_Number { get; set; }
        public string Sales_Type { get; set; }
        public DateTime? Contract_Takenback_Date { get; set; }
        public string Contract_Creator { get; set; }
        public string Contract_Creator_Employee_Number { get; set; }

        #endregion
    }

    public class ProjectOutPutDTO : ProjectDTO
    {
        /// <summary>
        /// 项目关联合同
        /// </summary>
        public ICollection<ContractSaveModel> Contract { get; set; }

        //项目关联客户
        public Entity.DTO.Contract.Client Client { get; set; }
    }

    /// <summary>
    /// 保存资源预算+人月信息-input
    /// </summary>
    /// <param name="Project_Id">项目ID</param>
    /// <param name="ProjectPlanInfoDTOs">项目节段</param>
    /// <param name="ProjectResourceBudgetDTOs">项目资源预算</param>
    /// <param name="ProjectOtherBudgetDTOs">项目其它预算</param>
    public record SaveResourceBudgeInputDTO(int Project_Id, ICollection<ProjectPlanInfoDTO> ProjectPlanInfoDTOs, ICollection<ProjectResourceBudgetDTO> ProjectResourceBudgetDTOs, ICollection<ProjectOtherBudgetDTO> ProjectOtherBudgetDTOs);

    /// <summary>
    /// 保存资源预算+人月信息-output
    /// </summary>
    /// <param name="Project_Id">项目ID</param>
    /// <param name="ProjectPlanInfoDTOs">项目节段</param>
    /// <param name="ProjectResourceBudgetDTOs">项目资源预算</param>
    /// <param name="ProjectResourceBudgetHCDTOs">项目资源预算HC</param>
    /// <param name="ProjectOtherBudgetDTOs">项目其它预算</param>
    public record SaveResourceBudgeOutPutDTO(int Project_Id, ICollection<ProjectPlanInfoDTO> ProjectPlanInfoDTOs, ICollection<ProjectResourceBudgetDTO> ProjectResourceBudgetDTOs, ICollection<ProjectResourceBudgetHCDTO> ProjectResourceBudgetHCDTOs, ICollection<ProjectOtherBudgetDTO> ProjectOtherBudgetDTOs);

    /// <summary>
    /// 资源预算人月信息-input
    /// </summary>
    /// <param name="Project_Id">项目ID</param>
    /// <param name="ProjectResourceBudgetDTOs">项目资源预算</param>
    /// <param name="ProjectOtherBudgetDTOs">项目其它预算</param>
    public record CalculateResourceBudgetHCInputDTO(int Project_Id, ICollection<ProjectResourceBudgetDTO> ProjectResourceBudgetDTOs, ICollection<ProjectOtherBudgetDTO> ProjectOtherBudgetDTOs);

    /// <summary>
    /// 资源预算人月信息-output
    /// </summary>
    /// <param name="Project_Id">项目ID</param>
    /// <param name="ProjectResourceBudget">项目资源预算</param>
    /// <param name="ProjectResourceBudgetHCDTOs">项目资源预算HC</param>
    /// <param name="ProjectOtherBudgetDTOs">项目其它预算</param>
    public record CalculateResourceBudgetHCOutPutDTO(int Project_Id, ICollection<ProjectResourceBudgetDTO> ProjectResourceBudget, ICollection<ProjectResourceBudgetHCDTO> ProjectResourceBudgetDTOs, ICollection<ProjectOtherBudgetDTO> ProjectOtherBudgetDTOs);

    /// <summary>
    /// 提交审批-input
    /// </summary>
    /// <param name="Project_Id">项目ID</param>
    /// <param name="ProjectBudgetSummary">项目预算汇总</param>
    public record ProjectSubmitInputDTO(int Project_Id, ICollection<ProjectBudgetSummaryDTO> ProjectBudgetSummary);
}
