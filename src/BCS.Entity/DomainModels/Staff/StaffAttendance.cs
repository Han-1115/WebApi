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
    [Entity(TableCnName = "员工考勤表",TableName = "StaffAttendance")]
    public partial class StaffAttendance:BaseEntity
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
       ///员工工号
       /// </summary>
       [Display(Name ="员工工号")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       public string StaffNo { get; set; }

       /// <summary>
       ///日期
       /// </summary>
       [Display(Name ="日期")]
       [Column(TypeName="datetime")]
       public DateTime? Date { get; set; }

       /// <summary>
       ///应出勤时数
       /// </summary>
       [Display(Name ="应出勤时数")]
       [DisplayFormat(DataFormatString="5,2")]
       [Column(TypeName="decimal")]
       [Required(AllowEmptyStrings=false)]
       public decimal RequiredAttendanceHours { get; set; }

       /// <summary>
       ///实际出勤时数
       /// </summary>
       [Display(Name ="实际出勤时数")]
       [DisplayFormat(DataFormatString="5,2")]
       [Column(TypeName="decimal")]
       [Required(AllowEmptyStrings=false)]
       public decimal ActualAttendanceHours { get; set; }

       /// <summary>
       ///应出勤天数
       /// </summary>
       [Display(Name ="应出勤天数")]
       [DisplayFormat(DataFormatString="5,2")]
       [Column(TypeName="decimal")]
       [Required(AllowEmptyStrings=false)]
       public decimal RequiredAttendanceDays { get; set; }

       /// <summary>
       ///实际出勤天数
       /// </summary>
       [Display(Name ="实际出勤天数")]
       [DisplayFormat(DataFormatString="5,2")]
       [Column(TypeName="decimal")]
       [Required(AllowEmptyStrings=false)]
       public decimal ActualAttendanceDays { get; set; }

       /// <summary>
       ///缺卡次数（旷工次数）
       /// </summary>
       [Display(Name ="缺卡次数（旷工次数）")]
       [DisplayFormat(DataFormatString="5,2")]
       [Column(TypeName="decimal")]
       [Required(AllowEmptyStrings=false)]
       public decimal Absenteeism { get; set; }

       /// <summary>
       ///旷工小时数
       /// </summary>
       [Display(Name ="旷工小时数")]
       [DisplayFormat(DataFormatString="5,2")]
       [Column(TypeName="decimal")]
       [Required(AllowEmptyStrings=false)]
       public decimal AbsenteeismHours { get; set; }

       /// <summary>
       ///请假次数
       /// </summary>
       [Display(Name ="请假次数")]
       [DisplayFormat(DataFormatString="5,2")]
       [Column(TypeName="decimal")]
       [Required(AllowEmptyStrings=false)]
       public decimal LeaveNumbers { get; set; }

       /// <summary>
       ///请假时长（天）
       /// </summary>
       [Display(Name ="请假时长（天）")]
       [DisplayFormat(DataFormatString="5,2")]
       [Column(TypeName="decimal")]
       [Required(AllowEmptyStrings=false)]
       public decimal LeaveDays { get; set; }

       /// <summary>
       ///病假小时数（小时）
       /// </summary>
       [Display(Name ="病假小时数（小时）")]
       [DisplayFormat(DataFormatString="5,2")]
       [Column(TypeName="decimal")]
       [Required(AllowEmptyStrings=false)]
       public decimal SickLeaveHours { get; set; }

       /// <summary>
       ///病假天数（天）
       /// </summary>
       [Display(Name ="病假天数（天）")]
       [DisplayFormat(DataFormatString="5,2")]
       [Column(TypeName="decimal")]
       [Required(AllowEmptyStrings=false)]
       public decimal SickLeaveDays { get; set; }

       /// <summary>
       ///年假小时数（小时）
       /// </summary>
       [Display(Name ="年假小时数（小时）")]
       [DisplayFormat(DataFormatString="5,2")]
       [Column(TypeName="decimal")]
       [Required(AllowEmptyStrings=false)]
       public decimal AnnualLeaveHours { get; set; }

       /// <summary>
       ///年假天数（天）
       /// </summary>
       [Display(Name ="年假天数（天）")]
       [DisplayFormat(DataFormatString="5,2")]
       [Column(TypeName="decimal")]
       [Required(AllowEmptyStrings=false)]
       public decimal AnnualLeaveDays { get; set; }

       /// <summary>
       ///事假小时数（小时）
       /// </summary>
       [Display(Name ="事假小时数（小时）")]
       [DisplayFormat(DataFormatString="5,2")]
       [Column(TypeName="decimal")]
       [Required(AllowEmptyStrings=false)]
       public decimal PersonalLeaveHours { get; set; }

       /// <summary>
       ///事假天数（天）
       /// </summary>
       [Display(Name ="事假天数（天）")]
       [DisplayFormat(DataFormatString="5,2")]
       [Column(TypeName="decimal")]
       [Required(AllowEmptyStrings=false)]
       public decimal PersonalLeaveDays { get; set; }

       /// <summary>
       ///调休假时长（小时）
       /// </summary>
       [Display(Name ="调休假时长（小时）")]
       [DisplayFormat(DataFormatString="5,2")]
       [Column(TypeName="decimal")]
       [Required(AllowEmptyStrings=false)]
       public decimal CompensatoryLeaveHours { get; set; }

       /// <summary>
       ///调休假时长（天）
       /// </summary>
       [Display(Name ="调休假时长（天）")]
       [DisplayFormat(DataFormatString="5,2")]
       [Column(TypeName="decimal")]
       [Required(AllowEmptyStrings=false)]
       public decimal CompensatoryLeaveDays { get; set; }

       /// <summary>
       ///婚假（天）
       /// </summary>
       [Display(Name ="婚假（天）")]
       [DisplayFormat(DataFormatString="5,2")]
       [Column(TypeName="decimal")]
       [Required(AllowEmptyStrings=false)]
       public decimal MarriageLeave { get; set; }

       /// <summary>
       ///产假（天）
       /// </summary>
       [Display(Name ="产假（天）")]
       [DisplayFormat(DataFormatString="5,2")]
       [Column(TypeName="decimal")]
       [Required(AllowEmptyStrings=false)]
       public decimal MaternityLeave { get; set; }

       /// <summary>
       ///陪产假（天）
       /// </summary>
       [Display(Name ="陪产假（天）")]
       [DisplayFormat(DataFormatString="5,2")]
       [Column(TypeName="decimal")]
       [Required(AllowEmptyStrings=false)]
       public decimal PaternityLeave { get; set; }

       /// <summary>
       ///保胎假（天）
       /// </summary>
       [Display(Name ="保胎假（天）")]
       [DisplayFormat(DataFormatString="5,2")]
       [Column(TypeName="decimal")]
       [Required(AllowEmptyStrings=false)]
       public decimal MaternityProtectionLeave { get; set; }

       /// <summary>
       ///产前假（天）
       /// </summary>
       [Display(Name ="产前假（天）")]
       [DisplayFormat(DataFormatString="5,2")]
       [Column(TypeName="decimal")]
       [Required(AllowEmptyStrings=false)]
       public decimal PrenatalLeave { get; set; }

       /// <summary>
       ///流产假（天）
       /// </summary>
       [Display(Name ="流产假（天）")]
       [DisplayFormat(DataFormatString="5,2")]
       [Column(TypeName="decimal")]
       [Required(AllowEmptyStrings=false)]
       public decimal AbortionLeave { get; set; }

       /// <summary>
       ///丧假（天）
       /// </summary>
       [Display(Name ="丧假（天）")]
       [DisplayFormat(DataFormatString="5,2")]
       [Column(TypeName="decimal")]
       [Required(AllowEmptyStrings=false)]
       public decimal BereavementLeave { get; set; }

       /// <summary>
       ///独生子女护理假（天）
       /// </summary>
       [Display(Name ="独生子女护理假（天）")]
       [DisplayFormat(DataFormatString="5,2")]
       [Column(TypeName="decimal")]
       [Required(AllowEmptyStrings=false)]
       public decimal OnlyChildCareLeave { get; set; }

       /// <summary>
       ///医疗期（天）
       /// </summary>
       [Display(Name ="医疗期（天）")]
       [DisplayFormat(DataFormatString="5,2")]
       [Column(TypeName="decimal")]
       [Required(AllowEmptyStrings=false)]
       public decimal MedicalPeriod { get; set; }

       /// <summary>
       ///产检假时长（天）
       /// </summary>
       [Display(Name ="产检假时长（天）")]
       [DisplayFormat(DataFormatString="5,2")]
       [Column(TypeName="decimal")]
       [Required(AllowEmptyStrings=false)]
       public decimal MaternalExaminationLeave { get; set; }

       /// <summary>
       ///哺乳假（天）
       /// </summary>
       [Display(Name ="哺乳假（天）")]
       [DisplayFormat(DataFormatString="5,2")]
       [Column(TypeName="decimal")]
       [Required(AllowEmptyStrings=false)]
       public decimal BreastfeedingLeaveDays { get; set; }

       /// <summary>
       ///育儿假（天）
       /// </summary>
       [Display(Name ="育儿假（天）")]
       [DisplayFormat(DataFormatString="5,2")]
       [Column(TypeName="decimal")]
       [Required(AllowEmptyStrings=false)]
       public decimal ParentalLeave { get; set; }

       /// <summary>
       ///外出（天）
       /// </summary>
       [Display(Name ="外出（天）")]
       [DisplayFormat(DataFormatString="5,2")]
       [Column(TypeName="decimal")]
       [Required(AllowEmptyStrings=false)]
       public decimal Outgoing { get; set; }

       /// <summary>
       ///迟到次数
       /// </summary>
       [Display(Name ="迟到次数")]
       [DisplayFormat(DataFormatString="5,2")]
       [Column(TypeName="decimal")]
       [Required(AllowEmptyStrings=false)]
       public decimal LateNumbers { get; set; }

       /// <summary>
       ///早退次数
       /// </summary>
       [Display(Name ="早退次数")]
       [DisplayFormat(DataFormatString="5,2")]
       [Column(TypeName="decimal")]
       [Required(AllowEmptyStrings=false)]
       public decimal LeaveEarlyNumbers { get; set; }

       /// <summary>
       ///迟到分钟数（分钟)）
       /// </summary>
       [Display(Name ="迟到分钟数（分钟)）")]
       [DisplayFormat(DataFormatString="5,2")]
       [Column(TypeName="decimal")]
       [Required(AllowEmptyStrings=false)]
       public decimal LateMinutes { get; set; }

       /// <summary>
       ///早退分钟数（分钟）
       /// </summary>
       [Display(Name ="早退分钟数（分钟）")]
       [DisplayFormat(DataFormatString="5,2")]
       [Column(TypeName="decimal")]
       [Required(AllowEmptyStrings=false)]
       public decimal LeaveEarlyMinutes { get; set; }

       
    }
}