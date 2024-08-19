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
    [Entity(TableCnName = "员工信息表",TableName = "Staff")]
    public partial class Staff:BaseEntity
    {
        /// <summary>
       ///主键ID
       /// </summary>
       [Key]
       [Display(Name ="主键ID")]
       [Column(TypeName="int")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public int Id { get; set; }

       /// <summary>
       ///员工工号
       /// </summary>
       [Display(Name ="员工工号")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       [Editable(true)]
       public string StaffNo { get; set; }

       /// <summary>
       ///员工姓名
       /// </summary>
       [Display(Name ="员工姓名")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       [Editable(true)]
       public string StaffName { get; set; }

       /// <summary>
       ///创建时间
       /// </summary>
       [Display(Name ="创建时间")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public DateTime CreateTime { get; set; }

       /// <summary>
       ///部门Id
       /// </summary>
       [Display(Name ="部门Id")]
       [Column(TypeName="uniqueidentifier")]
       public Guid? DepartmentId { get; set; }

       /// <summary>
       ///办公地点
       /// </summary>
       [Display(Name ="办公地点")]
       [MaxLength(200)]
       [Column(TypeName="nvarchar(200)")]
       public string OfficeLocation { get; set; }

       /// <summary>
       ///入职日期
       /// </summary>
       [Display(Name ="入职日期")]
       [Column(TypeName="datetime")]
       public DateTime? EnterDate { get; set; }

       /// <summary>
       ///离职日期
       /// </summary>
       [Display(Name ="离职日期")]
       [Column(TypeName="datetime")]
       public DateTime? LeaveDate { get; set; }

       /// <summary>
       ///岗位
       /// </summary>
       [Display(Name ="岗位")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       public string Position { get; set; }

        /// <summary>
        ///创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        [Column(TypeName = "datetime")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public DateTime CreatedTime { get; set; }

        /// <summary>
        ///创建时间
        /// </summary>
        [Display(Name = "修改时间")]
        [Column(TypeName = "datetime")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public DateTime ModifiedTime { get; set; }

    }
}