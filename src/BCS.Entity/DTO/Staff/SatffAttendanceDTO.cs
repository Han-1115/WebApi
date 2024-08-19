using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCS.Entity.DTO.Staff
{
    public class SatffAttendanceDTO
    {

        /// <summary>
        ///工号
        /// </summary>
        public string StaffNo { get; set; }

        /// <summary>
        ///日期
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        ///旷工次数
        /// </summary>
        public int Absenteeism { get; set; }

        /// <summary>
        ///旷工小时数
        /// </summary>
        public int AbsenteeismHours { get; set; }

        /// <summary>
        ///婚假（天）
        /// </summary>
        public int MarriageLeave { get; set; }

        /// <summary>
        ///产假（天）
        /// </summary>
        public int MaternityLeave { get; set; }

        /// <summary>
        ///产检假（天）
        public int MaternalExaminationLeave { get; set; }

        /// <summary>
        ///陪产假（天）
        /// </summary>
        public int PaternityLeave { get; set; }

        /// <summary>
        ///医疗期（天）
        /// </summary>
        public int MedicalPeriod { get; set; }

        /// <summary>
        ///丧假（天）
        /// </summary>
        public int BereavementLeave { get; set; }

        /// <summary>
        ///应出勤工时
        /// </summary>
        public int RequiredAttendanceHours { get; set; }

        /// <summary>
        ///实际出勤工时
        /// </summary>
        public int ActualAttendanceHours { get; set; }

        /// <summary>
        ///应出勤天数
        /// </summary>
        public int RequiredAttendanceDays { get; set; }

        /// <summary>
        ///实际出勤天数
        /// </summary>
        public int ActualAttendanceDays { get; set; }

        /// <summary>
        ///请假次数
        /// </summary>
        public int LeaveNumbers { get; set; }

        /// <summary>
        ///请假时长（小时）
        /// </summary>
        public int LeaveHours { get; set; }

        /// <summary>
        ///请假时长（天）
        /// </summary>
        public int LeaveDays { get; set; }

        /// <summary>
        ///病假次数（次）
        /// </summary>
        public int SickLeaveNumbers { get; set; }

        /// <summary>
        ///病假小时数（小时）
        /// </summary>
        public int SickLeaveHours { get; set; }

        /// <summary>
        ///病假天数（天）
        /// </summary>
        public int SickLeaveDays { get; set; }

        /// <summary>
        ///年假次数（次）
        /// </summary>
        public int AnnualLeaveNumbers { get; set; }

        /// <summary>
        ///年假小时数（小时）
        /// </summary>
        public int AnnualLeaveHours { get; set; }

        /// <summary>
        ///年假天数（天）
        /// </summary>
        public int AnnualLeaveDays { get; set; }

        /// <summary>
        ///事假次数（次）
        /// </summary>
        public int PersonalLeaveNumbers { get; set; }

        /// <summary>
        ///事假小时数（小时）
        /// </summary>
        public int PersonalLeaveHours { get; set; }

        /// <summary>
        ///事假天数（天）
        /// </summary>
        public int PersonalLeaveDays { get; set; }

        /// <summary>
        ///保胎假（天）
        /// </summary>
        public int MaternityProtectionLeave { get; set; }

        /// <summary>
        ///产前假（天）
        /// </summary>
        public int PrenatalLeave { get; set; }

        /// <summary>
        ///流产假（天）
        /// </summary>
        public int AbortionLeave { get; set; }

        /// <summary>
        ///独生子女护理假（天）
        /// </summary>
        public int OnlyChildCareLeave { get; set; }

        /// <summary>
        ///哺乳假（小时）
        /// </summary>
        public int BreastfeedingLeaveHours { get; set; }

        /// <summary>
        ///哺乳假（天）
        /// </summary>
        public int BreastfeedingLeaveDays { get; set; }

        /// <summary>
        ///是否全勤（0:否，1:是）
        /// </summary>
        public byte IsFullAttendance { get; set; }

        /// <summary>
        ///育儿假（天）
        /// </summary>
        public int ParentalLeave { get; set; }

        /// <summary>
        ///外出（天）
        /// </summary>
        public int Outgoing { get; set; }
    }
}
