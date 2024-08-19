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
    [Entity(TableCnName = "分包合同流程付款计划表",TableName = "SubContractFlowPaymentPlan")]
    public partial class SubContractFlowPaymentPlan:BaseEntity
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
       [Required(AllowEmptyStrings=false)]
       public int SubContractFlowId { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="Payment_Point_Description")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string Payment_Point_Description { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="Payment_Point_Amount")]
       [DisplayFormat(DataFormatString="20,2")]
       [Column(TypeName="decimal")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public decimal Payment_Point_Amount { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="Payment_Point_Ratio")]
       [DisplayFormat(DataFormatString="20,2")]
       [Column(TypeName="decimal")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public decimal Payment_Point_Ratio { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="Expected_Payment_Date")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public DateTime Expected_Payment_Date { get; set; }

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
       [Display(Name ="PaymentPlanId")]
       [Column(TypeName="int")]
       [Required(AllowEmptyStrings=false)]
       public int PaymentPlanId { get; set; }

       
    }
}