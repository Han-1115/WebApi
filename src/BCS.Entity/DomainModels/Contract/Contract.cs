using Newtonsoft.Json;
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
using System.ComponentModel;

namespace BCS.Entity.DomainModels
{
    [Table("Contract")]
    [EntityAttribute(TableCnName = "合同基本信息")]
    public partial class Contract : BaseEntity
    {
        /// <summary>
        ///Id
        /// </summary>
        [Key]
        [Display(Name = "Id")]
        [Column(TypeName = "int")]
        [Required(AllowEmptyStrings = false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        ///合同注册编码（Contract Registration ID）
        /// </summary>
        [Display(Name = "合同注册编码")]
        [Column(TypeName = "nvarchar(50)")]
        [Editable(false)]
        public string Code { get; set; }

        /// <summary>
        ///客户合同编码
        /// </summary>
        [Display(Name = "客户合同编码")]
        [Column(TypeName = "nvarchar(50)")]
        [Editable(true)]
        public string Client_Contract_Code { get; set; }

        /// <summary>
        //是否拿到PO
        /// </summary>
        [Display(Name = "是否拿到PO")]
        [Column(TypeName = "tinyint")]
        [Editable(true)]
        public byte IsPO { get; set; }

        /// <summary>
        ///合同属性大类
        /// </summary>
        [Display(Name = "合同属性大类")]
        [Column(TypeName = "int")]
        [Editable(true)]
        public int Category { get; set; }

        /// <summary>
        //客户合同号（PO #）
        /// </summary>
        [Display(Name = "客户合同号（PO #")]
        [MaxLength(50)]
        [Column(TypeName = "nvarchar(100)")]
        [Editable(true)]
        public string Customer_Contract_Number { get; set; }

        /// <summary>
        ///合同名称
        /// </summary>
        [Display(Name = "合同名称")]
        [MaxLength(500)]
        [Column(TypeName = "nvarchar(500)")]
        [Editable(true)]
        public string Name { get; set; }

        /// <summary>
        ///签单部门ID
        /// </summary>
        [Display(Name = "签单部门ID")]
        [MaxLength(50)]
        [Column(TypeName = "nvarchar(50)")]
        [Editable(true)]
        public string Signing_Department_Id { get; set; }

        /// <summary>
        ///签单部门Id
        /// </summary>
        [Display(Name = "签单部门Id")]
        [MaxLength(50)]
        [Column(TypeName = "nvarchar(50)")]
        [Editable(true)]
        public string Signing_Department { get; set; }

        /// <summary>
        ///框架合同Id
        /// </summary>
        [Display(Name = "框架合同Id")]
        [Column(TypeName = "int")]
        public int Frame_Contract_Id { get; set; }

        /// <summary>
        ///客户实体Id
        /// </summary>
        [Display(Name = "客户实体Id")]
        [Column(TypeName = "int")]
        public int Client_Id { get; set; }

        /// <summary>
        ///签单法人实体
        /// </summary>
        [Display(Name = "签单法人实体")]
        [MaxLength(500)]
        [Column(TypeName = "nvarchar(500)")]
        [Editable(true)]
        public string Signing_Legal_Entity { get; set; }

        /// <summary>
        ///合同采购类型
        /// </summary>
        [Display(Name = "合同采购类型")]
        [MaxLength(50)]
        [Column(TypeName = "nvarchar(50)")]
        [Editable(true)]
        public string Procurement_Type { get; set; }

        /// <summary>
        ///计费类型
        /// </summary>
        [Display(Name = "计费类型")]
        [MaxLength(50)]
        [Column(TypeName = "nvarchar(50)")]
        [Editable(true)]
        public string Billing_Type { get; set; }

        /// <summary>
        ///销售类型
        /// </summary>
        [Display(Name = "销售类型")]
        [MaxLength(50)]
        [Column(TypeName = "nvarchar(50)")]
        [Editable(true)]
        public string Sales_Type { get; set; }

        /// <summary>
        ///客户合同类型
        /// </summary>
        [Display(Name = "客户合同类型")]
        [MaxLength(50)]
        [Column(TypeName = "nvarchar(50)")]
        [Editable(true)]
        public string Client_Contract_Type { get; set; }

        /// <summary>
        ///客户组织名称
        /// </summary>
        [Display(Name = "客户组织名称")]
        [MaxLength(50)]
        [Column(TypeName = "nvarchar(50)")]
        [Editable(true)]
        public string Client_Organization_Name { get; set; }

        /// <summary>
        ///销售经理
        /// </summary>
        [Display(Name = "销售经理")]
        [MaxLength(50)]
        [Column(TypeName = "nvarchar(50)")]
        [Editable(true)]
        public string Sales_Manager { get; set; }

        /// <summary>
        ///销售经理ID
        /// </summary>
        [Display(Name = "销售经理ID")]
        [Column(TypeName = "int")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public int Sales_Manager_Id { get; set; }

        /// <summary>
        ///PO Owner
        /// </summary>
        [Display(Name = "PO Owner")]
        [MaxLength(50)]
        [Column(TypeName = "nvarchar(50)")]
        [Editable(true)]
        public string PO_Owner { get; set; }

        /// <summary>
        ///合同创建人
        /// </summary>
        [Display(Name = "合同创建人")]
        [MaxLength(50)]
        [Column(TypeName = "nvarchar(50)")]
        public string Creator { get; set; }

        /// <summary>
        ///合同创建人ID
        /// </summary>
        [Display(Name = "合同创建人ID")]
        [Column(TypeName = "int")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public int CreatorID { get; set; }

        /// <summary>
        ///合同生效日期
        /// </summary>
        [Display(Name = "合同生效日期")]
        [Column(TypeName = "datetime")]
        [Editable(true)]
        public DateTime? Effective_Date { get; set; }

        /// <summary>
        ///合同结束日期
        /// </summary>
        [Display(Name = "合同结束日期")]
        [Column(TypeName = "datetime")]
        [Editable(true)]
        public DateTime? End_Date { get; set; }

        /// <summary>
        ///结算币种
        /// </summary>
        [Display(Name = "结算币种")]
        [MaxLength(50)]
        [JsonIgnore]
        [Column(TypeName = "nvarchar(50)")]
        public string Settlement_Currency { get; set; }

        /// <summary>
        ///关联合同编码
        /// </summary>
        [Display(Name = "关联合同编码")]
        [MaxLength(50)]
        [JsonIgnore]
        [Column(TypeName = "nvarchar(50)")]
        public string Associated_Contract_Code { get; set; }

        /// <summary>
        ///合同金额
        /// </summary>
        [Display(Name = "合同金额")]
        [MaxLength(50)]
        [JsonIgnore]
        [Column(TypeName = "decimal(20,2)")]
        public decimal PO_Amount { get; set; }

        /// <summary>
        ///采购税率
        /// </summary>
        [Display(Name = "采购税率")]
        [MaxLength(50)]
        [JsonIgnore]
        [Column(TypeName = "nvarchar(50)")]
        public string Tax_Rate { get; set; }


        /// <summary>
        ///非采购税率
        /// </summary>
        [Display(Name = "非采购税率")]
        [MaxLength(50)]
        [JsonIgnore]
        [Column(TypeName = "nvarchar(50)")]
        public string Tax_Rate_No_Purchase { get; set; }

        /// <summary>
        ///预算汇率
        /// </summary>
        [Display(Name = "预算汇率")]
        [MaxLength(50)]
        [JsonIgnore]
        [Column(TypeName = "nvarchar(50)")]
        public string Exchange_Rate { get; set; }

        /// <summary>
        ///结算周期
        /// </summary>
        [Display(Name = "结算周期")]
        [MaxLength(50)]
        [JsonIgnore]
        [Column(TypeName = "nvarchar(50)")]
        public string Billing_Cycle { get; set; }

        /// <summary>
        ///预算开票周期
        /// </summary>
        [Display(Name = "预算开票周期")]
        [MaxLength(50)]
        [JsonIgnore]
        [Column(TypeName = "nvarchar(50)")]
        public string Estimated_Billing_Cycle { get; set; }

        /// <summary>
        ///预算开票周期
        /// </summary>
        [Display(Name = "回款周期")]
        [MaxLength(50)]
        [JsonIgnore]
        [Column(TypeName = "nvarchar(50)")]
        public string Collection_Period { get; set; }


        /// <summary>
        //是否为标准charge rate（0：否，1：是）
        /// </summary>
        [Display(Name = "是否为标准charge rate")]
        [Column(TypeName = "tinyint")]
        [Editable(true)]
        public byte Is_Charge_Rate_Type { get; set; }

        /// <summary>
        ///charge rate体系单位
        /// </summary>
        [Display(Name = "charge rate体系单位")]
        [MaxLength(50)]
        [JsonIgnore]
        [Column(TypeName = "nvarchar(50)")]
        public string Charge_Rate_Unit { get; set; }

        /// <summary>
        ///合同拿回日期
        /// </summary>
        [Display(Name = "合同拿回日期")]
        [JsonIgnore]
        [Column(TypeName = "DateTime")]
        public DateTime? Contract_Takenback_Date { get; set; }

        /// <summary>
        ///预计合同拿回日期
        /// </summary>
        [Display(Name = "预计合同拿回日期")]
        [JsonIgnore]
        [Column(TypeName = "DateTime")]
        public DateTime? Estimated_Contract_Takenback_Date { get; set; }

        /// <summary>
        ///备注
        /// </summary>
        [Display(Name = "备注")]
        [MaxLength(500)]
        [JsonIgnore]
        [Column(TypeName = "nvarchar(500)")]
        public string Remark { get; set; }

        /// <summary>
        ///操作状态
        /// </summary>
        [Display(Name = "操作状态")]
        [Column(TypeName = "tinyint")]
        public byte Operating_Status { get; set; }

        /// <summary>
        ///操作状态
        /// </summary>
        [Display(Name = "审批状态")]
        [Column(TypeName = "tinyint")]
        public byte Approval_Status { get; set; }

        /// <summary>
        /// 工作流表Id
        /// </summary>
        [Display(Name = "WorkFlowTable_Id")]
        [Column(TypeName = "uniqueidentifier")]
        public Guid? WorkFlowTable_Id { get; set; }

        /// <summary>
        ///是否删除
        /// </summary>
        [Display(Name = "是否删除")]
        [Column(TypeName = "tinyint")]
        public byte IsDelete { get; set; }

        /// <summary>
        // 是否触发项目变更，1是，0否
        /// </summary>
        [Display(Name = "是否触发项目变更")]
        [Column(TypeName = "tinyint")]
        [DefaultValue(0)]
        [Editable(true)]
        public byte? Is_Handle_Change { get; set; }

        /// <summary>
        ///变更原因
        /// </summary>
        [Display(Name = "变更原因")]
        [Column(TypeName = "nvarchar(Max)")]
        public string Reason_change { get; set; }

        /// <summary>
        ///合同创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        [Column(TypeName = "datetime")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 更新合同状态
        /// </summary>
        /// <param name="operating_Status"></param>
        /// <param name="approval_Status"></param>
        public void UpdateStatus(byte operating_Status, byte approval_Status)
        {
            Operating_Status = operating_Status;
            Approval_Status = approval_Status;
        }

        /// <summary>
        /// 更新是否项目变更
        /// </summary>
        /// <param name="changeStatus"></param>
        public void UpdateChangeStatus(byte? changeStatus)
        {
            Is_Handle_Change = changeStatus;
        }

        /// <summary>
        /// 更新合同删除状态
        /// </summary>
        /// <param name="isDelete"></param>
        public void UpdateIsDelete(byte isDelete)
        {
            IsDelete = isDelete;
        }
    }
}
