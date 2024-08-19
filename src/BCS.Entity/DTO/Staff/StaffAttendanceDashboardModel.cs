using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCS.Entity.DTO.Staff
{
    public class StaffAttendanceDashboardModel
    {
        /// <summary>
        /// 交付部门
        /// </summary>
        public string Delivery_Department { get; set; }
        /// <summary>
        /// 考勤月份
        /// </summary>
        public string AttendanceMonth { get; set; }
        /// <summary>
        /// 项目编码
        /// </summary>
        public string ProjectCode { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// 项目经理
        /// </summary>
        public string Project_Manager { get; set; }
        /// <summary>
        /// 时长汇总
        /// </summary>
        public decimal AbsenceSummaryHours { get { return PersonalLeaveHours + LateHours + LeaveEarlyHours + AbsenteeismHours; } }
        /// <summary>
        /// 事假时长（小时）
        /// </summary>
        public decimal PersonalLeaveHours { get; set; }
        /// <summary>
        /// 迟到次数
        /// </summary>
        public decimal LateNumbers { get; set; }
        /// <summary>
        /// 迟到时长（小时）
        /// </summary>
        public decimal LateHours { get; set; }
        /// <summary>
        /// 早退次数
        /// </summary>
        public decimal LeaveEarlyNumbers { get; set; }

        /// <summary>
        /// 早退时长（小时）
        /// </summary>
        public decimal LeaveEarlyHours { get; set; }
        /// <summary>
        /// 缺卡次数（旷工次数）
        /// </summary>
        public decimal Absenteeism { get; set; }
        /// <summary>
        /// 旷工小时数
        /// </summary>
        public decimal AbsenteeismHours { get; set; }
        /// <summary>
        /// 病假小时数（小时）
        /// </summary>
        public decimal SickLeaveHours { get; set; }
        /// <summary>
        /// 医疗期小时数（小时）
        /// </summary>
        public decimal MedicalPeriodHours { get; set; }
    }
}
