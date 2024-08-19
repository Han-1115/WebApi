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
    [Entity(TableCnName = "项目资源预算历史", TableName = "ProjectResourceBudgetHistory")]
    public partial class ProjectResourceBudgetHistory : BaseEntity
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
        /// 项目Id
        /// </summary>
        [Display(Name = "Project_Id")]
        [Column(TypeName = "int")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public int Project_Id { get; set; }

        /// <summary>
        /// 项目计划Id
        /// </summary>
        [Display(Name = "ProjectPlanInfo_Id")]
        [Column(TypeName = "int")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public int ProjectPlanInfo_Id { get; set; }

        /// <summary>
        /// 岗位（工种、技能)
        /// </summary>
        [Display(Name = "PositionId")]
        [Column(TypeName = "int")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public int PositionId { get; set; }

        /// <summary>
        /// 级别（职级、等级)
        /// </summary>
        [Display(Name = "LevelId")]
        [Column(TypeName = "int")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public int LevelId { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        [Display(Name = "CityId")]
        [Column(TypeName = "int")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public int CityId { get; set; }

        /// <summary>
        /// Cost rate金额'
        /// </summary>
        [Display(Name = "Cost_Rate")]
        [DisplayFormat(DataFormatString = "20,2")]
        [Column(TypeName = "decimal")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public decimal Cost_Rate { get; set; }

        /// <summary>
        /// Site_Type 0 :offsite, 1: onsite
        /// </summary>
        [Display(Name = "Site_TypeId")]
        [Column(TypeName = "tinyint")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public byte Site_TypeId { get; set; }

        /// <summary>
        /// 人数
        /// </summary>
        [Display(Name = "HeadCount")]
        [Column(TypeName = "int")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public int HeadCount { get; set; }

        /// <summary>
        /// Charge Rate金额
        /// </summary>
        [Display(Name = "Charge_Rate")]
        [DisplayFormat(DataFormatString = "20,2")]
        [Column(TypeName = "decimal")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public decimal Charge_Rate { get; set; }

        /// <summary>
        /// 投入开始日期
        /// </summary>
        [Display(Name = "Start_Date")]
        [Column(TypeName = "date")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public DateTime Start_Date { get; set; }

        /// <summary>
        /// 投入结束日期
        /// </summary>
        [Display(Name = "End_Date")]
        [Column(TypeName = "date")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public DateTime End_Date { get; set; }

        /// <summary>
        /// 预计工时合计（人天）
        /// </summary>
        [Display(Name = "TotalManHourCapacity")]
        [DisplayFormat(DataFormatString = "20,2")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public decimal TotalManHourCapacity { get; set; }

        /// <summary>
        /// 备注
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