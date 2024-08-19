using Confluent.Kafka;
using Esprima.Ast;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCS.Core.Const
{
    /// <summary>
    /// 系统常量
    /// </summary>
    public class CommonConst
    {
        /// <summary>
        /// CEO
        /// </summary>
        public static string CEO = "CEO";
        /// <summary>
        /// 交付经理
        /// </summary>
        public static string DeliveryManager = "Delivery Manager";
        /// <summary>
        /// 项目总监
        /// </summary>
        public static string SeniorProgramManager = "Senior Program Manager";
        /// <summary>
        /// 项目经理
        /// </summary>
        public static string ProgramManager = "Program Manager";
        /// <summary>
        /// 销售
        /// </summary>
        public static string Sales = "Sales";
        /// <summary>
        /// GM
        /// </summary>
        public static string GM = "GM";
        /// <summary>
        /// OPS Director
        /// </summary>
        public static string OPSDirector = "OPS Director";

        /// <summary>
        /// 考勤项目
        /// </summary>
        public static string[] AttendanceItems = new string[] {
            "RequiredAttendanceHours",
            "ActualAttendanceHours",
            "RequiredAttendanceDays",
            "ActualAttendanceDays",
            "Absenteeism",
            "AbsenteeismHours",
            "LeaveNumbers",
            "LeaveDays",
            "SickLeaveHours",
            "SickLeaveDays",
            "AnnualLeaveHours",
            "AnnualLeaveDays",
            "PersonalLeaveHours",
            "PersonalLeaveDays",
            "CompensatoryLeaveHours",
            "CompensatoryLeaveDays",
            "MarriageLeave",
            "MaternityLeave",
            "PaternityLeave",
            "MaternityProtectionLeave",
            "PrenatalLeave",
            "AbortionLeave",
            "BereavementLeave",
            "OnlyChildCareLeave",
            "MedicalPeriod",
            "MaternalExaminationLeave",
            "BreastfeedingLeaveDays",
            "ParentalLeave",
            "Outgoing"
        };
    }
}
