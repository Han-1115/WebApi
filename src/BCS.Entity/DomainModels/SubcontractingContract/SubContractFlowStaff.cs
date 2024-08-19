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
    [Entity(TableCnName = "分包合同人员备案流程表",TableName = "SubContractFlowStaff")]
    public partial class SubContractFlowStaff:BaseEntity
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
       [Display(Name ="SubContractFlowId")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? SubContractFlowId { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="SubcontractingStaffName")]
       [MaxLength(300)]
       [Column(TypeName="nvarchar(300)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string SubcontractingStaffName { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="SubcontractingStaffNo")]
       [MaxLength(200)]
       [Column(TypeName="nvarchar(200)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string SubcontractingStaffNo { get; set; }

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
       [Display(Name ="Country")]
       [MaxLength(200)]
       [Column(TypeName="nvarchar(200)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string Country { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="Age")]
       [Column(TypeName="int")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public int Age { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="Sex")]
       [Column(TypeName="tinyint")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public byte Sex { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="Skill")]
       [MaxLength(200)]
       [Column(TypeName="nvarchar(200)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string Skill { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="Cost_Rate")]
       [DisplayFormat(DataFormatString="20,2")]
       [Column(TypeName="decimal")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public decimal Cost_Rate { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="Cost_Rate_Unit")]
       [Column(TypeName="tinyint")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public byte Cost_Rate_Unit { get; set; }

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
       [Display(Name ="Expiration_Date")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public DateTime Expiration_Date { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="IsDelete")]
       [Column(TypeName="tinyint")]
       [Editable(true)]
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
       [Display(Name ="SubContractStaffId")]
       [Column(TypeName="int")]
       [Required(AllowEmptyStrings=false)]
       public int SubContractStaffId { get; set; }

       
    }
}