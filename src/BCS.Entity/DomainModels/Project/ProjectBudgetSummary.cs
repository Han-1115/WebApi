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
    [Entity(TableCnName = "项目预算汇总",TableName = "ProjectBudgetSummary")]
    public partial class ProjectBudgetSummary:BaseEntity
    {
        /// <summary>
       ///
       /// </summary>
       [Key]
       [Display(Name ="Id")]
       [Column(TypeName="int")]
       [Required(AllowEmptyStrings=false)]
       public int Id { get; set; }

       /// <summary>
       ///项目Id
       /// </summary>
       [Display(Name ="项目Id")]
       [Column(TypeName="int")]
       [Required(AllowEmptyStrings=false)]
       public int Project_Id { get; set; }

       /// <summary>
       ///项目预算关键项Id
       /// </summary>
       [Display(Name ="项目预算关键项Id")]
       [Column(TypeName="int")]
       [Required(AllowEmptyStrings=false)]
       public int KeyItemID { get; set; }

       /// <summary>
       ///计划金额
       /// </summary>
       [Display(Name ="计划金额")]
       [DisplayFormat(DataFormatString="20,2")]
       [Column(TypeName="decimal")]
       [Required(AllowEmptyStrings=false)]
       public decimal PlanAmount { get; set; }

       /// <summary>
       ///计划金额滚动(已发生损益+未来预算)
       /// </summary>
       [Display(Name ="计划金额滚动(已发生损益+未来预算)")]
       [DisplayFormat(DataFormatString="20,2")]
       [Column(TypeName="decimal")]
       [Required(AllowEmptyStrings=false)]
       public decimal PlanAmountScroll { get; set; }

       /// <summary>
       ///显示占项目金额比重  0:否,1:是
       /// </summary>
       [Display(Name ="显示占项目金额比重  0:否,1:是")]
       [Column(TypeName="int")]
       [Required(AllowEmptyStrings=false)]
       public int EnableProportionOfProjectAmount { get; set; }

       /// <summary>
       ///占项目金额比重(%)
       /// </summary>
       [Display(Name ="占项目金额比重(%)")]
       [DisplayFormat(DataFormatString="10,2")]
       [Column(TypeName="decimal")]
       [Required(AllowEmptyStrings=false)]
       public decimal ProjectAmountRate { get; set; }

       /// <summary>
       ///占项目金额比重(%)滚动
       /// </summary>
       [Display(Name ="占项目金额比重(%)滚动")]
       [DisplayFormat(DataFormatString="10,2")]
       [Column(TypeName="decimal")]
       [Required(AllowEmptyStrings=false)]
       public decimal ProjectAmountRateScroll { get; set; }

       /// <summary>
       ///显示部门指标 0:否,1:是
       /// </summary>
       [Display(Name ="显示部门指标 0:否,1:是")]
       [Column(TypeName="int")]
       [Required(AllowEmptyStrings=false)]
       public int EnableDepartmentMetric { get; set; }

       /// <summary>
       ///部门指标(%)
       /// </summary>
       [Display(Name ="部门指标(%)")]
       [DisplayFormat(DataFormatString="10,2")]
       [Column(TypeName="decimal")]
       [Required(AllowEmptyStrings=false)]
       public decimal DepartmentMetric { get; set; }

       /// <summary>
       ///偏差说明
       /// </summary>
       [Display(Name ="偏差说明")]
       [MaxLength(500)]
       [Column(TypeName="nvarchar(500)")]
       public string DeviationExplanation { get; set; }

       /// <summary>
       ///备注
       /// </summary>
       [Display(Name ="备注")]
       [MaxLength(500)]
       [Column(TypeName="nvarchar(500)")]
       public string Remark { get; set; }

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

       
    }
}