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
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;

namespace BCS.Entity.DomainModels
{
    [Entity(TableCnName = "人员项目关系表",TableName = "StaffProject")]
    public partial class StaffProject:BaseEntity
    {
        /// <summary>
       ///主键ID
       /// </summary>
       [Key]
       [Display(Name ="主键ID")]
       [Column(TypeName="int")]
       [Required(AllowEmptyStrings=false)]
       public int Id { get; set; }

       /// <summary>
       ///项目ID
       /// </summary>
       [Display(Name ="项目ID")]
       [Column(TypeName="int")]
       [Required(AllowEmptyStrings=false)]
       public int ProjectId { get; set; }

       /// <summary>
       ///是否分包人员
       /// </summary>
       [Display(Name ="是否分包人员")]
       [Column(TypeName="bit")]
       [Required(AllowEmptyStrings=false)]
       public bool IsSubcontract { get; set; }

       /// <summary>
       ///Charge Rate金额
       /// </summary>
       [Display(Name ="Charge Rate金额")]
       [DisplayFormat(DataFormatString="20,2")]
       [Column(TypeName="decimal")]
       [Required(AllowEmptyStrings=false)]
       public decimal ChargeRate { get; set; }

       /// <summary>
       ///投入开始日期
       /// </summary>
       [Display(Name ="投入开始日期")]
       [Column(TypeName="datetime")]
       public DateTime? InputStartDate { get; set; }

       /// <summary>
       ///投入结束日期
       /// </summary>
       [Display(Name ="投入结束日期")]
       [Column(TypeName="datetime")]
       public DateTime? InputEndDate { get; set; }

       /// <summary>
       ///投入百分比
       /// </summary>
       [Display(Name ="投入百分比")]
       [DisplayFormat(DataFormatString="20,2")]
       [Column(TypeName="decimal")]
       [Required(AllowEmptyStrings=false)]
       public decimal InputPercentage { get; set; }

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
       ///员工ID
       /// </summary>
       [Display(Name ="员工ID")]
       [Column(TypeName="int")]
       [Required(AllowEmptyStrings=false)]
       public int StaffId { get; set; }

        /// <summary>
        ///是否删除（0:否，1:预删除，2:已删除）
        /// </summary>
        [Display(Name ="是否删除")]
       [Column(TypeName="tinyint")]
       [Required(AllowEmptyStrings=false)]
       public byte IsDelete { get; set; }

        /// <summary>
        ///变更类型
        /// </summary>
        [Display(Name = "变更类型")]
        [Column(TypeName = "int")]
        [Required(AllowEmptyStrings = false)]
        public int ChangeType { get; set; }

        /// <summary>
        ///变更类型名称
        /// </summary>
        [Display(Name = "变更类型名称")]
        [MaxLength(200)]
        [Column(TypeName = "nvarchar(200)")]
        public string ChangeTypeName { get; set; }

        /// <summary>
        ///变更原因
        /// </summary>
        [Display(Name = "变更原因")]
        [MaxLength(200)]
        [Column(TypeName = "nvarchar(200)")]
        public string ChangeReason { get; set; }


    }
}