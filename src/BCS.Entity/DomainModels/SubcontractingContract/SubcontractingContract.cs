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
    [Entity(TableCnName = "分包合同表",TableName = "SubcontractingContract")]
    public partial class SubcontractingContract:BaseEntity
    {
        /// <summary>
       ///ID
       /// </summary>
       [Key]
       [Display(Name ="ID")]
       [Column(TypeName="int")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public int Id { get; set; }

       /// <summary>
       ///项目ID
       /// </summary>
       [Display(Name ="项目ID")]
       [Column(TypeName="int")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public int Project_Id { get; set; }

       /// <summary>
       ///分包合同编码
       /// </summary>
       [Display(Name ="分包合同编码")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string Code { get; set; }

       /// <summary>
       ///分包合同名称
       /// </summary>
       [Display(Name ="分包合同名称")]
       [MaxLength(500)]
       [Column(TypeName="nvarchar(500)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string Name { get; set; }

       /// <summary>
       ///交付部门ID
       /// </summary>
       [Display(Name ="交付部门ID")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string Delivery_Department_Id { get; set; }

       /// <summary>
       ///交付部门
       /// </summary>
       [Display(Name ="交付部门")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string Delivery_Department { get; set; }

       /// <summary>
       ///分包合同类型
       /// </summary>
       [Display(Name ="分包合同类型")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string Procurement_Type { get; set; }

       /// <summary>
       ///供应商
       /// </summary>
       [Display(Name ="供应商")]
       [MaxLength(200)]
       [Column(TypeName="nvarchar(200)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string Supplier { get; set; }

       /// <summary>
       ///分包合同金额（原币)
       /// </summary>
       [Display(Name ="分包合同金额（原币)")]
       [DisplayFormat(DataFormatString="20,2")]
       [Column(TypeName="decimal")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public decimal Subcontracting_Contract_Amount { get; set; }

       /// <summary>
       ///结算币种
       /// </summary>
       [Display(Name ="结算币种")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string Settlement_Currency { get; set; }

       /// <summary>
       ///预算汇率
       /// </summary>
       [Display(Name ="预算汇率")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string Exchange_Rate { get; set; }

       /// <summary>
       ///分包合同金额CNY 等于分包合同金额（原币）*预算汇率；纯数字，保留2位小数；
       /// </summary>
       [Display(Name ="分包合同金额CNY 等于分包合同金额（原币）*预算汇率；纯数字，保留2位小数；")]
       [DisplayFormat(DataFormatString="20,2")]
       [Column(TypeName="decimal")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public decimal Subcontracting_Contract_Amount_CNY { get; set; }

       /// <summary>
       ///项目税率
       /// </summary>
       [Display(Name ="项目税率")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string Tax_Rate { get; set; }

       /// <summary>
       ///付款周期 1:单月,2:双月,3:季度,4:半年度,5:年度,6:里程碑,7:其他
       /// </summary>
       [Display(Name ="付款周期 1:单月,2:双月,3:季度,4:半年度,5:年度,6:里程碑,7:其他")]
       [Column(TypeName="tinyint")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public byte Payment_CycleId { get; set; }

       /// <summary>
       ///计费类型 1：TM,2：FP
       /// </summary>
       [Display(Name ="计费类型 1：TM,2：FP")]
       [Column(TypeName="tinyint")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public byte Charge_TypeId { get; set; }

       /// <summary>
       ///结算模式 1:TM-计时,2:TM-计件
       /// </summary>
       [Display(Name ="结算模式 1:TM-计时,2:TM-计件")]
       [Column(TypeName="tinyint")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public byte Billing_ModeId { get; set; }

       /// <summary>
       ///结算周期 1:单月,2:双月,3:季度,4:半年度,5:年度,6:里程碑,7:其他
       /// </summary>
       [Display(Name ="结算周期 1:单月,2:双月,3:季度,4:半年度,5:年度,6:里程碑,7:其他")]
       [Column(TypeName="tinyint")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public byte Billing_CycleId { get; set; }

       /// <summary>
       ///分包合同开始日期 分包合同开始和结束日期要在项目的周期内
       /// </summary>
       [Display(Name ="分包合同开始日期 分包合同开始和结束日期要在项目的周期内")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public DateTime Effective_Date { get; set; }

       /// <summary>
       ///分包合同结束日期
       /// </summary>
       [Display(Name ="分包合同结束日期")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public DateTime End_Date { get; set; }

       /// <summary>
       ///Cost rate unit 1：人时，2：人天，3：人月
       /// </summary>
       [Display(Name ="Cost rate unit 1：人时，2：人天，3：人月")]
       [Column(TypeName="tinyint")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public byte Cost_Rate_UnitId { get; set; }

       /// <summary>
       ///是否指定供应商 1：是，2：否
       /// </summary>
       [Display(Name ="是否指定供应商 1：是，2：否")]
       [Column(TypeName="tinyint")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public byte Is_Assigned_Supplier { get; set; }

       /// <summary>
       ///分包合同总监ID
       /// </summary>
       [Display(Name ="分包合同总监ID")]
       [Column(TypeName="int")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public int Contract_Director_Id { get; set; }

       /// <summary>
       ///分包合同总监
       /// </summary>
       [Display(Name ="分包合同总监")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string Contract_Director { get; set; }

       /// <summary>
       ///分包原因
       /// </summary>
       [Display(Name ="分包原因")]
       [MaxLength(2000)]
       [Column(TypeName="nvarchar(2000)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string Subcontracting_Reason { get; set; }

       /// <summary>
       ///备注
       /// </summary>
       [Display(Name ="备注")]
       [MaxLength(500)]
       [Column(TypeName="nvarchar(500)")]
       [Editable(true)]
       public string Remark { get; set; }

       /// <summary>
       ///版本号
       /// </summary>
       [Display(Name ="版本号")]
       [Column(TypeName="int")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public int Version { get; set; }

       /// <summary>
       ///变更原因
       /// </summary>
       [Display(Name ="变更原因")]
       [MaxLength(1000)]
       [Column(TypeName="nvarchar(1000)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string Change_Reason { get; set; }

       /// <summary>
       ///操作状态 (1：已提交,2：草稿待提交,3:变更待提交)
       /// </summary>
       [Display(Name ="操作状态 (1：已提交,2：草稿待提交,3:变更待提交)")]
       [Column(TypeName="tinyint")]
       [Editable(true)]
       public byte? Operating_Status { get; set; }

       /// <summary>
       ///审批状态 (1:审批通过,2:审批驳回,3:审批中 ,4:未发起 ,5:已撤回 )
       /// </summary>
       [Display(Name ="审批状态 (1:审批通过,2:审批驳回,3:审批中 ,4:未发起 ,5:已撤回 )")]
       [Column(TypeName="tinyint")]
       [Editable(true)]
       public byte? Approval_Status { get; set; }

       /// <summary>
       ///工作流表Id
       /// </summary>
       [Display(Name ="工作流表Id")]
       [Column(TypeName="uniqueidentifier")]
       [Editable(true)]
       public Guid? WorkFlowTable_Id { get; set; }

       /// <summary>
       ///删除原因
       /// </summary>
       [Display(Name ="删除原因")]
       [Column(TypeName="nvarchar(max)")]
       [Editable(true)]
       public string Reason_change { get; set; }

       /// <summary>
       ///是否删除（0:否，1:是）
       /// </summary>
       [Display(Name ="是否删除（0:否，1:是）")]
       [Column(TypeName="tinyint")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public byte IsDelete { get; set; }

       /// <summary>
       ///创建人ID
       /// </summary>
       [Display(Name ="创建人ID")]
       [Column(TypeName="int")]
       [Required(AllowEmptyStrings=false)]
       public int CreateID { get; set; }

       /// <summary>
       ///创建人
       /// </summary>
       [Display(Name ="创建人")]
       [MaxLength(200)]
       [Column(TypeName="nvarchar(200)")]
       [Required(AllowEmptyStrings=false)]
       public string Creator { get; set; }

       /// <summary>
       ///创建时间
       /// </summary>
       [Display(Name ="创建时间")]
       [Column(TypeName="datetime")]
       [Required(AllowEmptyStrings=false)]
       public DateTime CreateDate { get; set; }

       /// <summary>
       ///修改人ID
       /// </summary>
       [Display(Name ="修改人ID")]
       [Column(TypeName="int")]
       [Required(AllowEmptyStrings=false)]
       public int ModifyID { get; set; }

       /// <summary>
       ///修改人
       /// </summary>
       [Display(Name ="修改人")]
       [MaxLength(200)]
       [Column(TypeName="nvarchar(200)")]
       [Required(AllowEmptyStrings=false)]
       public string Modifier { get; set; }

       /// <summary>
       ///修改时间
       /// </summary>
       [Display(Name ="修改时间")]
       [Column(TypeName="datetime")]
       [Required(AllowEmptyStrings=false)]
       public DateTime ModifyDate { get; set; }

       /// <summary>
       ///删除时间
       /// </summary>
       [Display(Name ="删除时间")]
       [Column(TypeName="datetime")]
       public DateTime? DeleteTime { get; set; }

       /// <summary>
       ///分包合同经理ID
       /// </summary>
       [Display(Name ="分包合同经理ID")]
       [Column(TypeName="int")]
       [Required(AllowEmptyStrings=false)]
       public int Contract_Manager_Id { get; set; }

       /// <summary>
       ///分包合同经理人
       /// </summary>
       [Display(Name ="分包合同经理人")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       [Required(AllowEmptyStrings=false)]
       public string Contract_Manager { get; set; }

       
    }
}