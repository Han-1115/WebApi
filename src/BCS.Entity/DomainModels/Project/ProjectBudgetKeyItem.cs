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
    [Entity(TableCnName = "项目预算关键项", TableName = "ProjectBudgetKeyItem")]
    public partial class ProjectBudgetKeyItem : BaseEntity
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        [Display(Name = "主键ID 业务中会根据此ID进行业务操作")]
        [Column(TypeName = "int")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public int KeyItemID { get; set; }

        /// <summary>
        ///关键项目序号
        /// </summary>
        [Display(Name = "关键项目序号")]
        [Column(TypeName = "int")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public int KeyItemOrder { get; set; }

        /// <summary>
        ///关键项目英文名
        /// </summary>
        [Display(Name = "关键项目英文名")]
        [MaxLength(500)]
        [Column(TypeName = "nvarchar(500)")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public string KeyItemEn { get; set; }

        /// <summary>
        ///关键项目中文名
        /// </summary>
        [Display(Name = "关键项目中文名")]
        [MaxLength(500)]
        [Column(TypeName = "nvarchar(500)")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public string KeyItemCn { get; set; }

        /// <summary>
        ///显示占项目金额比重  0:否,1:是
        /// </summary>
        [Display(Name = "显示占项目金额比重  0:否,1:是")]
        [Column(TypeName = "int")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public int EnableProportionOfProjectAmount { get; set; }

        /// <summary>
        ///显示部门指标 0:否,1:是
        /// </summary>
        [Display(Name = "显示部门指标 0:否,1:是")]
        [Column(TypeName = "int")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public int EnableDepartmentMetric { get; set; }

        /// <summary>
        ///备注
        /// </summary>
        [Display(Name = "备注")]
        [MaxLength(500)]
        [Column(TypeName = "nvarchar(500)")]
        [Editable(true)]
        public string Remark { get; set; }

        /// <summary>
        ///创建人ID
        /// </summary>
        [Display(Name = "创建人ID")]
        [Column(TypeName = "int")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public int CreateID { get; set; }

        /// <summary>
        ///创建人
        /// </summary>
        [Display(Name = "创建人")]
        [MaxLength(200)]
        [Column(TypeName = "nvarchar(200)")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public string Creator { get; set; }

        /// <summary>
        ///创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        [Column(TypeName = "datetime")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public DateTime CreateDate { get; set; }

        /// <summary>
        ///修改人ID
        /// </summary>
        [Display(Name = "修改人ID")]
        [Column(TypeName = "int")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public int ModifyID { get; set; }

        /// <summary>
        ///修改人
        /// </summary>
        [Display(Name = "修改人")]
        [MaxLength(200)]
        [Column(TypeName = "nvarchar(200)")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public string Modifier { get; set; }

        /// <summary>
        ///修改时间
        /// </summary>
        [Display(Name = "修改时间")]
        [Column(TypeName = "datetime")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public DateTime ModifyDate { get; set; }


    }
}