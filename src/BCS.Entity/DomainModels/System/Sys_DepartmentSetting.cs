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
    [Entity(TableCnName = "Sys_DepartmentSetting", TableName = "Sys_DepartmentSetting")]
    public partial class Sys_DepartmentSetting : BaseEntity
    {
        /// <summary>
        ///ID
        /// </summary>
        [Key]
        [Display(Name = "ID")]
        [Column(TypeName = "int")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public int Id { get; set; }

        /// <summary>
        ///部门ID
        /// </summary>
        [Display(Name = "部门ID")]
        [Column(TypeName = "uniqueidentifier")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public Guid DepartmentId { get; set; }

        /// <summary>
        ///年份
        /// </summary>
        [Display(Name = "年份")]
        [Column(TypeName = "int")]
        [Editable(true)]
        public int? Year { get; set; }

        /// <summary>
        ///自由交付人力成本
        /// </summary>
        [Display(Name = "自由交付人力成本")]
        [DisplayFormat(DataFormatString = "20,2")]
        [Column(TypeName = "decimal")]
        [Editable(true)]
        public decimal? LaborCostofOwnDelivery { get; set; }

        /// <summary>
        ///项目毛利率
        /// </summary>
        [Display(Name = "项目毛利率")]
        [DisplayFormat(DataFormatString = "20,2")]
        [Column(TypeName = "decimal")]
        [Editable(true)]
        public decimal? ProjectGPM { get; set; }

        /// <summary>
        ///备注
        /// </summary>
        [Display(Name = "备注")]
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
        public int? CreateID { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Display(Name = "Creator")]
        [MaxLength(30)]
        [Column(TypeName = "nvarchar(30)")]
        [Editable(true)]
        public string Creator { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Display(Name = "CreateDate")]
        [Column(TypeName = "datetime")]
        [Editable(true)]
        public DateTime? CreateDate { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Display(Name = "ModifyID")]
        [Column(TypeName = "int")]
        [Editable(true)]
        public int? ModifyID { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Display(Name = "Modifier")]
        [MaxLength(30)]
        [Column(TypeName = "nvarchar(30)")]
        [Editable(true)]
        public string Modifier { get; set; }

        /// <summary>
        ///
        /// </summary>
        [Display(Name = "ModifyDate")]
        [Column(TypeName = "datetime")]
        [Editable(true)]
        public DateTime? ModifyDate { get; set; }


    }
}