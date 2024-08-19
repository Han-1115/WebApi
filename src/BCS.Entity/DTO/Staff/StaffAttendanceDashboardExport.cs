﻿using Magicodes.ExporterAndImporter.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCS.Entity.DTO.Staff
{
    public class StaffAttendanceDashboardExport
    {
        /// <summary>
        /// 考勤月份
        /// </summary>
        [ExporterHeader(DisplayName = "Attendance Month")]
        public string AttendanceMonth { get; set; }
        /// <summary>
        /// 项目编码
        /// </summary>
        [ExporterHeader(DisplayName = "Project #")]
        public string ProjectCode { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        [ExporterHeader(DisplayName = "Program Name")]
        public string ProjectName { get; set; }
        /// <summary>
        /// 交付部门
        /// </summary>
        [ExporterHeader(DisplayName = "Project Department")]
        public string Delivery_Department { get; set; }
        /// <summary>
        /// 项目经理
        /// </summary>
        [ExporterHeader(DisplayName = "Project Manager")]
        public string Project_Manager { get; set; }
        /// <summary>
        /// 时长汇总
        /// </summary>
        [ExporterHeader(DisplayName = "Total Hours of Personal Leave, Arriving Late, Early Leave, and Absence")]
        public decimal AbsenceSummaryHours { get; set; }
        /// <summary>
        /// 事假时长（小时）
        /// </summary>
        [ExporterHeader(DisplayName = "Total Hours of Personal Leave")]
        public decimal PersonalLeaveHours { get; set; }
        /// <summary>
        /// 迟到次数
        /// </summary>
        [ExporterHeader(DisplayName = "Number of Arriving Late")]
        public decimal LateNumbers { get; set; }
        /// <summary>
        /// 迟到时长（小时）
        /// </summary>
        [ExporterHeader(DisplayName = "Total Hours of Arriving Late")]
        public decimal LateHours { get; set; }
        /// <summary>
        /// 早退次数
        /// </summary>
        [ExporterHeader(DisplayName = "Number of Early Leave")]
        public decimal LeaveEarlyNumbers { get; set; }

        /// <summary>
        /// 早退时长（小时）
        /// </summary>
        [ExporterHeader(DisplayName = "Total Hours of Early Leave")]
        public decimal LeaveEarlyHours { get; set; }
        /// <summary>
        /// 缺卡次数（旷工次数）
        /// </summary>
        [ExporterHeader(DisplayName = "Number of Absence")]
        public decimal Absenteeism { get; set; }
        /// <summary>
        /// 旷工小时数
        /// </summary>
        [ExporterHeader(DisplayName = "Total Hours of Absence")]
        public decimal AbsenteeismHours { get; set; }
        /// <summary>
        /// 病假小时数（小时）
        /// </summary>
        [ExporterHeader(DisplayName = "Total Hours of Sick Leave")]
        public decimal SickLeaveHours { get; set; }
        /// <summary>
        /// 医疗期小时数（小时）
        /// </summary>
        [ExporterHeader(DisplayName = "Total Hours of Medical Leave")]
        public decimal MedicalPeriodHours { get; set; }
    }
}
