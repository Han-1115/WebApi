/*
 *代码由框架生成,任何更改都可能导致被代码生成器覆盖
 *如果数据库字段发生变化，请在代码生器重新生成此Model
 */
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCS.Entity.SystemModels;

namespace BCS.Entity.DomainModels
{
    [Entity(TableCnName = "项目", TableName = "Project")]
    public partial class Project : BaseEntity
    {
        /// <summary>
        /// ID
        /// </summary>
        [Key]
        [Display(Name = "Id")]
        [Column(TypeName = "int")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public int Id { get; set; }

        /// <summary>
        /// 客户合同对应关系
        /// </summary>
        [Display(Name = "Contract_Project_Relationship")]
        [MaxLength(50)]
        [Column(TypeName = "nvarchar(50)")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public string Contract_Project_Relationship { get; set; }

        /// <summary>
        /// 项目编码
        /// </summary>
        [Display(Name = "Project_Code")]
        [MaxLength(50)]
        [Column(TypeName = "nvarchar(50)")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public string Project_Code { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        [Display(Name = "Project_Name")]
        [MaxLength(500)]
        [Column(TypeName = "nvarchar(500)")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public string Project_Name { get; set; }

        /// <summary>
        /// 项目金额
        /// </summary>
        [Display(Name = "Project_Amount")]
        [DisplayFormat(DataFormatString = "20,2")]
        [Column(TypeName = "decimal")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public decimal Project_Amount { get; set; }

        /// <summary>
        /// 项目类型Id
        /// <para>1:Delivery Project</para>
        /// <para>2:Procurement Project</para>
        /// </summary>
        [Display(Name = "Project_TypeId")]
        [Column(TypeName = "int")]
        [Editable(true)]
        public int Project_TypeId { get; set; }

        /// <summary>
        /// 项目类型
        /// <para>1:Delivery Project</para>
        /// <para>2:Procurement Project</para>
        /// </summary>
        [Display(Name = "Project_Type")]
        [MaxLength(50)]
        [Column(TypeName = "nvarchar(50)")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public string Project_Type { get; set; }

        /// <summary>
        /// 执行部门Id
        /// </summary>
        [Display(Name = "Delivery_Department_Id")]
        [MaxLength(200)]
        [Column(TypeName = "nvarchar(200)")]
        [Editable(true)]
        public string Delivery_Department_Id { get; set; }

        /// <summary>
        /// 执行部门
        /// </summary>
        [Display(Name = "Delivery_Department")]
        [MaxLength(200)]
        [Column(TypeName = "nvarchar(200)")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public string Delivery_Department { get; set; }

        /// <summary>
        /// 项目经理Id
        /// </summary>
        [Display(Name = "Project_Manager_Id")]
        [Column(TypeName = "int")]
        [Editable(true)]
        public int Project_Manager_Id { get; set; }

        /// <summary>
        /// 项目经理
        /// </summary>
        [Display(Name = "Project_Manager")]
        [MaxLength(50)]
        [Column(TypeName = "nvarchar(50)")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public string Project_Manager { get; set; }

        /// <summary>
        ///客户组织名称
        /// </summary>
        [Display(Name = "客户组织名称")]
        [MaxLength(50)]
        [Column(TypeName = "nvarchar(50)")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public string Client_Organization_Name { get; set; }

        /// <summary>
        /// 合作类型下拉框选择 1:资源,2:任务,3:项目,4:解决方案,5:劳务派遣
        /// </summary>
        [Display(Name = "Cooperation_TypeId")]
        [Column(TypeName = "tinyint")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public byte Cooperation_TypeId { get; set; }

        /// <summary>
        /// 结算模式 1:TM-计时,2:TM-计件
        /// </summary>
        [Display(Name = "Billing_ModeId")]
        [Column(TypeName = "tinyint")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public byte Billing_ModeId { get; set; }

        /// <summary>
        /// 项目所在城市
        /// </summary>
        [Display(Name = "Project_LocationCity")]
        [MaxLength(100)]
        [Column(TypeName = "nvarchar(100)")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public string Project_LocationCity { get; set; }

        /// <summary>
        /// 项目开始日期
        /// </summary>
        [Display(Name = "Start_Date")]
        [Column(TypeName = "datetime")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public DateTime Start_Date { get; set; }

        /// <summary>
        /// 项目结束日期
        /// </summary>
        [Display(Name = "End_Date")]
        [Column(TypeName = "datetime")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public DateTime End_Date { get; set; }

        /// <summary>
        /// 是否纯分包项目  0否 1是
        /// </summary>
        [Display(Name = "IsPurely_Subcontracted_Project")]
        [Column(TypeName = "tinyint")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public byte IsPurely_Subcontracted_Project { get; set; }

        /// <summary>
        /// 服务类型 1:IT交付,2:非IT交付,3:产品研发,4:咨询,5:运维,6:集成
        /// </summary>
        [Display(Name = "Service_TypeId")]
        [Column(TypeName = "tinyint")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public byte Service_TypeId { get; set; }

        /// <summary>
        /// 结算周期 1:单月,2:双月,3:季度,4:半年度,5:年度,6:里程碑
        /// </summary>
        [Display(Name = "Billing_CycleId")]
        [Column(TypeName = "tinyint")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public byte Billing_CycleId { get; set; }

        /// <summary>
        /// 预计结算周期 15,30,45,60,90,120,150,180,360
        /// </summary>
        [Display(Name = "Estimated_Billing_Cycle")]
        [Column(TypeName = "int")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public int Estimated_Billing_Cycle { get; set; }

        /// <summary>
        /// 是offshore还是onshore  0 :offshore, 1: onshore
        /// </summary>
        [Display(Name = "Shore_TypeId")]
        [Column(TypeName = "tinyint")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public byte Shore_TypeId { get; set; }

        /// <summary>
        /// 是offsite还是onsite  0 :offsite, 1: onsite
        /// </summary>
        [Display(Name = "Site_TypeId")]
        [Column(TypeName = "tinyint")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public byte Site_TypeId { get; set; }

        /// <summary>
        /// 节假日体系 1:China,2:US,3:India,4:South Korea,5:Japan,6:Phillipines
        /// </summary>
        [Display(Name = "Holiday_SystemId")]
        [Column(TypeName = "int")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public int Holiday_SystemId { get; set; }

        /// <summary>
        /// 月标准天数
        /// </summary>
        [Display(Name = "Standard_Number_of_Days_Per_MonthId")]
        [Column(TypeName = "int")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public int Standard_Number_of_Days_Per_MonthId { get; set; }

        /// <summary>
        /// 日标准小时数 
        /// </summary>
        [Display(Name = "Standard_Daily_Hours")]
        [DisplayFormat(DataFormatString = "20,2")]
        [Column(TypeName = "decimal")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public decimal Standard_Daily_Hours { get; set; }

        /// <summary>
        /// 项目总监ID
        /// </summary>
        [Display(Name = "Project_Director_Id")]
        [Column(TypeName = "int")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public int Project_Director_Id { get; set; }

        /// <summary>
        /// 项目总监
        /// </summary>
        [Display(Name = "Project_Director")]
        [MaxLength(100)]
        [Column(TypeName = "nvarchar(100)")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public string Project_Director { get; set; }

        /// <summary>
        /// 立项说明
        /// </summary>
        [Display(Name = "Project_Description")]
        [Column(TypeName = "nvarchar(max)")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public string Project_Description { get; set; }

        /// <summary>
        /// 变更类型 0:默认,1:项目主动变更，2：合同变更引发项目变更
        /// </summary>
        [Display(Name = "Change_From")]
        [Column(TypeName = "tinyint")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public byte Change_From { get; set; }

        /// <summary>
        /// 变更类型
        /// <para>1:需求变更</para>
        /// <para>2:缺陷修改</para>
        /// <para>3:计划调整</para>
        /// </summary>
        [Display(Name = "Change_TypeId")]
        [Column(TypeName = "tinyint")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public byte Change_TypeId { get; set; }

        /// <summary>
        /// 变更原因
        /// </summary>
        [Display(Name = "Change_Reason")]
        [Column(TypeName = "nvarchar(1000)")]
        [Editable(true)]
        public string Change_Reason { get; set; }

        /// <summary>
        /// 操作状态
        /// <para>0:默认状态</para>
        /// <para>1:已提交</para>
        /// <para>2:草稿待提交</para>
        /// <para>3:变更待提交</para>
        /// </summary>
        [Display(Name = "Operating_Status")]
        [Column(TypeName = "tinyint")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public byte Operating_Status { get; set; }

        /// <summary>
        /// 审批状态
        /// <para>0:默认状态</para>
        /// <para>1:审核通过</para>
        /// <para>2:审批驳回</para>
        /// <para>3:审批中</para>
        /// <para>4:未发起</para>
        /// <para>5:已撤回(发起人主动发起)</para>
        /// </summary>
        [Display(Name = "Approval_Status")]
        [Column(TypeName = "tinyint")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public byte Approval_Status { get; set; }

        /// <summary>
        /// 项目状态
        /// <para>0:默认状态</para>
        /// <para>1:进行中</para>
        /// <para>2:正常结项</para>
        /// <para>3:未开始</para>
        /// </summary>
        [Display(Name = "Project_Status")]
        [Column(TypeName = "tinyint")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public byte Project_Status { get; set; }

        /// <summary>
        /// 审批开始时间
        /// </summary>
        [Display(Name = "Approval_StartTime")]
        [Column(TypeName = "datetime")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public DateTime Approval_StartTime { get; set; }

        /// <summary>
        /// 审批完成时间
        /// </summary>
        [Display(Name = "Approval_EndTime")]
        [Column(TypeName = "datetime")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public DateTime Approval_EndTime { get; set; }

        /// <summary>
        /// 工作流表Id
        /// </summary>
        [Display(Name = "WorkFlowTable_Id")]
        [Column(TypeName = "uniqueidentifier")]
        public Guid? WorkFlowTable_Id { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "Remark")]
        [MaxLength(500)]
        [Column(TypeName = "nvarchar(500)")]
        [Editable(true)]
        public string Remark { get; set; }

        /// <summary>
        /// 版本号
        /// </summary>
        [Display(Name = "Version")]
        [Column(TypeName = "int")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public int Version { get; set; }

        /// <summary>
        /// 创建人ID
        /// </summary>
        [Display(Name = "CreateID")]
        [Column(TypeName = "int")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public int CreateID { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [Display(Name = "Creator")]
        [MaxLength(200)]
        [Column(TypeName = "nvarchar(200)")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public string Creator { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "CreateDate")]
        [Column(TypeName = "datetime")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 修改人ID
        /// </summary>
        [Display(Name = "ModifyID")]
        [Column(TypeName = "int")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public int ModifyID { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        [Display(Name = "Modifier")]
        [MaxLength(200)]
        [Column(TypeName = "nvarchar(200)")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public string Modifier { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        [Display(Name = "ModifyDate")]
        [Column(TypeName = "datetime")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public DateTime ModifyDate { get; set; }

        /// <summary>
        /// 是否删除（0:否，1:是）
        /// </summary>
        [Display(Name = "IsDelete")]
        [Column(TypeName = "tinyint")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public byte IsDelete { get; set; }

        /// <summary>
        /// 删除时间
        /// </summary>
        [Display(Name = "DeleteTime")]
        [Column(TypeName = "datetime")]
        [Editable(true)]
        public DateTime DeleteTime { get; set; }

        /// <summary>
        /// 出入项状态（0：未规划，1：已规划未提交，2：已提交）
        /// </summary>
        [Display(Name = "EntryExitProjectStatus")]
        [Column(TypeName = "tinyint")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public byte EntryExitProjectStatus { get; set; }
    }
}