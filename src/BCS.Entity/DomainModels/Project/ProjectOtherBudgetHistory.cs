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
    [Entity(TableCnName = "项目其它成本费用预算历史", TableName = "ProjectOtherBudgetHistory")]
    public partial class ProjectOtherBudgetHistory : BaseEntity
    {
        /// <summary>
        ///
        /// </summary>
        [Key]
        [Display(Name = "Id")]
        [Column(TypeName = "int")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public int Id { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Display(Name = "Project_Id")]
        [Column(TypeName = "int")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public int Project_Id { get; set; }

        /// <summary>
        /// 结算币种 CNY|USD|INR|JPY|SGD|KRW
        /// </summary>
        [Display(Name = "结算币种")]
        [MaxLength(50)]
        [Column(TypeName = "nvarchar(50)")]
        public string Settlement_Currency { get; set; }

        /// <summary>
        /// 月份 如：2023-09
        /// </summary>
        [Display(Name = "YearMonth")]
        [MaxLength(200)]
        [Column(TypeName = "nvarchar(10)")]
        [Editable(true)]
        public string YearMonth { get; set; }

        /// <summary>
        /// 津贴奖金
        /// </summary>
        [Display(Name = "Bonus_Cost")]
        [DisplayFormat(DataFormatString = "20,2")]
        [Column(TypeName = "decimal")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public decimal Bonus_Cost { get; set; }

        /// <summary>
        /// 项目差旅费用
        /// </summary>
        [Display(Name = "Travel_Cost")]
        [DisplayFormat(DataFormatString = "20,2")]
        [Column(TypeName = "decimal")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public decimal Travel_Cost { get; set; }

        /// <summary>
        /// 项目报销费用
        /// </summary>
        [Display(Name = "Reimbursement_Cost")]
        [DisplayFormat(DataFormatString = "20,2")]
        [Column(TypeName = "decimal")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public decimal Reimbursement_Cost { get; set; }

        /// <summary>
        /// 其它费用
        /// </summary>
        [Display(Name = "Other_Cost")]
        [DisplayFormat(DataFormatString = "20,2")]
        [Column(TypeName = "decimal")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public decimal Other_Cost { get; set; }

        /// <summary>
        /// 分包收入（含税）
        /// </summary>
        [Display(Name = "Subcontracting_Income")]
        [DisplayFormat(DataFormatString = "20,2")]
        [Column(TypeName = "decimal")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public decimal Subcontracting_Income { get; set; }

        /// <summary>
        /// 分包成本（含税）
        /// </summary>
        [Display(Name = "Subcontracting_Cost")]
        [DisplayFormat(DataFormatString = "20,2")]
        [Column(TypeName = "decimal")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public decimal Subcontracting_Cost { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Display(Name = "Remark")]
        [MaxLength(500)]
        [Column(TypeName = "nvarchar(500)")]
        [Editable(true)]
        public string Remark { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Display(Name = "CreateID")]
        [Column(TypeName = "int")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public int CreateID { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Display(Name = "Creator")]
        [MaxLength(200)]
        [Column(TypeName = "nvarchar(200)")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public string Creator { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Display(Name = "CreateDate")]
        [Column(TypeName = "datetime")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public DateTime CreateDate { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Display(Name = "ModifyID")]
        [Column(TypeName = "int")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public int ModifyID { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Display(Name = "Modifier")]
        [MaxLength(200)]
        [Column(TypeName = "nvarchar(200)")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public string Modifier { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Display(Name = "ModifyDate")]
        [Column(TypeName = "datetime")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public DateTime ModifyDate { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Display(Name = "CreateTime")]
        [Column(TypeName = "datetime")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public DateTime CreateTime { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Display(Name = "Version")]
        [Column(TypeName = "int")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public int Version { get; set; }


    }
}