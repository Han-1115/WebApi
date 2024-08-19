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
    [Entity(TableCnName = "系统日历",TableName = "Sys_Calendar")]
    public partial class Sys_Calendar:BaseEntity
    {
        /// <summary>
       ///ID
       /// </summary>
       [Key]
       [Display(Name ="ID")]
       [Column(TypeName="int")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public int Id { get; set; }

       /// <summary>
       ///节假日体系 1:中国,2:美国,3:日本,4:韩国,5:印度,6:马来西亚,7:新加坡,8:香港,9:菲律宾
       /// </summary>
       [Display(Name ="节假日体系 1:中国,2:美国,3:日本,4:韩国,5:印度,6:马来西亚,7:新加坡,8:香港,9:菲律宾")]
       [Column(TypeName="int")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public int Holiday_SystemId { get; set; }

       /// <summary>
       ///年
       /// </summary>
       [Display(Name ="年")]
       [Column(TypeName="int")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public int Year { get; set; }

       /// <summary>
       ///月
       /// </summary>
       [Display(Name ="月")]
       [Column(TypeName="int")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public int Month { get; set; }

       /// <summary>
       ///日
       /// </summary>
       [Display(Name ="日")]
       [Column(TypeName="int")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public int Day { get; set; }

       /// <summary>
       ///日期
       /// </summary>
       [Display(Name ="日期")]
       [Column(TypeName="date")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public DateTime Date { get; set; }

       /// <summary>
       ///星期几(0:Sunday,1:Monday,2:Tuesday,3:Wednesday,4:Thursday,5:Friday,6:Saturday)
       /// </summary>
       [Display(Name ="星期几(0:Sunday,1:Monday,2:Tuesday,3:Wednesday,4:Thursday,5:Friday,6:Saturday)")]
       [Column(TypeName="int")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public int DayOfWeek { get; set; }

       /// <summary>
       ///是否工作日 (0:否,1:是)
       /// </summary>
       [Display(Name ="是否工作日 (0:否,1:是)")]
       [Column(TypeName="tinyint")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public byte IsWorkingDay { get; set; }

       /// <summary>
       ///是否节假日 (0:否,1:是)
       /// </summary>
       [Display(Name ="是否节假日 (0:否,1:是)")]
       [Column(TypeName="tinyint")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public byte IsHoliday { get; set; }

       /// <summary>
       ///节假日名称
       /// </summary>
       [Display(Name ="节假日名称")]
       [MaxLength(500)]
       [Column(TypeName="nvarchar(500)")]
       [Editable(true)]
       public string HolidayName { get; set; }

       /// <summary>
       ///是否周末 (0:否,1:是)
       /// </summary>
       [Display(Name ="是否周末 (0:否,1:是)")]
       [Column(TypeName="tinyint")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public byte IsWeekend { get; set; }

       /// <summary>
       ///是否补班 (0:否,1:是)
       /// </summary>
       [Display(Name ="是否补班 (0:否,1:是)")]
       [Column(TypeName="tinyint")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public byte IsShiftDay { get; set; }

       /// <summary>
       ///备注
       /// </summary>
       [Display(Name ="备注")]
       [MaxLength(500)]
       [Column(TypeName="nvarchar(500)")]
       [Editable(true)]
       public string Remark { get; set; }

       /// <summary>
       ///创建人ID
       /// </summary>
       [Display(Name ="创建人ID")]
       [Column(TypeName="int")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public int CreateID { get; set; }

       /// <summary>
       ///创建人
       /// </summary>
       [Display(Name ="创建人")]
       [MaxLength(200)]
       [Column(TypeName="nvarchar(200)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string Creator { get; set; }

       /// <summary>
       ///创建时间
       /// </summary>
       [Display(Name ="创建时间")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public DateTime CreateDate { get; set; }

       /// <summary>
       ///修改人ID
       /// </summary>
       [Display(Name ="修改人ID")]
       [Column(TypeName="int")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public int ModifyID { get; set; }

       /// <summary>
       ///修改人
       /// </summary>
       [Display(Name ="修改人")]
       [MaxLength(200)]
       [Column(TypeName="nvarchar(200)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string Modifier { get; set; }

       /// <summary>
       ///修改时间
       /// </summary>
       [Display(Name ="修改时间")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public DateTime ModifyDate { get; set; }

       
    }
}