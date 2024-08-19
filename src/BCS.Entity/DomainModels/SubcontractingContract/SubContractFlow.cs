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
    [Entity(TableCnName = "分包合同流程表",TableName = "SubContractFlow")]
    public partial class SubContractFlow:BaseEntity
    {
        /// <summary>
       ///
       /// </summary>
       [Key]
       [Display(Name ="Id")]
       [Column(TypeName="int")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public int Id { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="SubContract_Id")]
       [Column(TypeName="int")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public int SubContract_Id { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="Project_Id")]
       [Column(TypeName="int")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public int Project_Id { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="Project_Code")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string Project_Code { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="Project_Name")]
       [MaxLength(500)]
       [Column(TypeName="nvarchar(500)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string Project_Name { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="Project_Type")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string Project_Type { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="Subcontracting_Cost")]
       [DisplayFormat(DataFormatString="20,2")]
       [Column(TypeName="decimal")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public decimal Subcontracting_Cost { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="Subcontracting_Balance")]
       [DisplayFormat(DataFormatString="20,2")]
       [Column(TypeName="decimal")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public decimal Subcontracting_Balance { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="Contract_Code")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       [Editable(true)]
       public string Contract_Code { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="Contract_Name")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       [Editable(true)]
       public string Contract_Name { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="Client_Organization_Name")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       [Editable(true)]
       public string Client_Organization_Name { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="PO_Amount")]
       [DisplayFormat(DataFormatString="20,2")]
       [Column(TypeName="decimal")]
       [Editable(true)]
       public decimal? PO_Amount { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="Code")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string Code { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="Name")]
       [MaxLength(500)]
       [Column(TypeName="nvarchar(500)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string Name { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="Delivery_Department_Id")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string Delivery_Department_Id { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="Delivery_Department")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string Delivery_Department { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="Procurement_Type")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string Procurement_Type { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="Supplier")]
       [MaxLength(200)]
       [Column(TypeName="nvarchar(200)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string Supplier { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="Subcontracting_Contract_Amount")]
       [DisplayFormat(DataFormatString="20,2")]
       [Column(TypeName="decimal")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public decimal Subcontracting_Contract_Amount { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="Settlement_Currency")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string Settlement_Currency { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="Exchange_Rate")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string Exchange_Rate { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="Subcontracting_Contract_Amount_CNY")]
       [DisplayFormat(DataFormatString="20,2")]
       [Column(TypeName="decimal")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public decimal Subcontracting_Contract_Amount_CNY { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="Tax_Rate")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string Tax_Rate { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="Payment_CycleId")]
       [Column(TypeName="tinyint")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public byte Payment_CycleId { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="Charge_TypeId")]
       [Column(TypeName="tinyint")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public byte Charge_TypeId { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="Billing_ModeId")]
       [Column(TypeName="tinyint")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public byte Billing_ModeId { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="Billing_CycleId")]
       [Column(TypeName="tinyint")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public byte Billing_CycleId { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="Effective_Date")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public DateTime Effective_Date { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="End_Date")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public DateTime End_Date { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="Cost_Rate_UnitId")]
       [Column(TypeName="tinyint")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public byte Cost_Rate_UnitId { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="Is_Assigned_Supplier")]
       [Column(TypeName="tinyint")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public byte Is_Assigned_Supplier { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="Contract_Director_Id")]
       [Column(TypeName="int")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public int Contract_Director_Id { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="Contract_Director")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string Contract_Director { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="Subcontracting_Reason")]
       [MaxLength(500)]
       [Column(TypeName="nvarchar(500)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string Subcontracting_Reason { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="Remark")]
       [MaxLength(500)]
       [Column(TypeName="nvarchar(500)")]
       [Editable(true)]
       public string Remark { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="Version")]
       [Column(TypeName="int")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public int Version { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="Change_Reason")]
       [MaxLength(1000)]
       [Column(TypeName="nvarchar(1000)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string Change_Reason { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="Operating_Status")]
       [Column(TypeName="tinyint")]
       [Editable(true)]
       public byte? Operating_Status { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="Approval_Status")]
       [Column(TypeName="tinyint")]
       [Editable(true)]
       public byte? Approval_Status { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="WorkFlowTable_Id")]
       [Column(TypeName="uniqueidentifier")]
       [Editable(true)]
       public Guid? WorkFlowTable_Id { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="CreateID")]
       [Column(TypeName="int")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public int CreateID { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="Creator")]
       [MaxLength(200)]
       [Column(TypeName="nvarchar(200)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string Creator { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="CreateDate")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public DateTime CreateDate { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="Type")]
       [Column(TypeName="int")]
       public int? Type { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="Project_Billing_Mode")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       [Required(AllowEmptyStrings=false)]
       public string Project_Billing_Mode { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="IsDelete")]
       [Column(TypeName="tinyint")]
       [Required(AllowEmptyStrings=false)]
       public byte IsDelete { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="DeleteTime")]
       [Column(TypeName="datetime")]
       public DateTime? DeleteTime { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="Contract_Manager_Id")]
       [Column(TypeName="int")]
       [Required(AllowEmptyStrings=false)]
       public int Contract_Manager_Id { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="Contract_Manager")]
       [MaxLength(100)]
       [Column(TypeName="nvarchar(100)")]
       [Required(AllowEmptyStrings=false)]
       public string Contract_Manager { get; set; }

       
    }
}