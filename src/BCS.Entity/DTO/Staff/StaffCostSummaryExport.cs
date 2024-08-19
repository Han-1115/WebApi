using Magicodes.ExporterAndImporter.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCS.Entity.DTO.Staff
{
    public class StaffCostSummaryExport
    {
        /// <summary>
        /// 年月
        /// </summary>
        [ExporterHeader(DisplayName = "Attendance Month")]
        public string YearMonth { get; set; }
        /// <summary>
        /// 人员工号
        /// </summary>
        [ExporterHeader(DisplayName = "EID")]
        public string StaffNo { get; set; }
        /// <summary>
        /// 人员姓名
        /// </summary>
        [ExporterHeader(DisplayName = "Employee Name")]
        public string StaffName { get; set; }
        /// <summary>
        /// 人员部门
        /// </summary>
        [ExporterHeader(DisplayName = "Staff Department")]
        public string StaffDepartment { get; set; }
        /// <summary>
        /// 办公地点
        /// </summary>
        [ExporterHeader(DisplayName = "Location")]
        public string OfficeLocation { get; set; }
        /// <summary>
        /// 入职日期
        /// </summary>
        [ExporterHeader(DisplayName = "Date of Enrollment")]
        public string EnterDate { get; set; }
        /// <summary>
        /// 离职日期
        /// </summary>
        [ExporterHeader(DisplayName = "Date of Departure")]
        public string LeaveDate { get; set; }
        /// <summary>
        /// 项目编码
        /// </summary>
        [ExporterHeader(DisplayName = "Project #")]
        public string ProjectCode { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        [ExporterHeader(DisplayName = "Project Name")]
        public string ProjectName { get; set; }
        /// <summary>
        /// 工作日体系
        /// </summary>
        [ExporterHeader(DisplayName = "Holiday System")]
        public string Holiday_System { get; set; }
        /// <summary>
        /// 签单法人实体
        /// </summary>
        [ExporterHeader(DisplayName = "Signing Legal Entity")]
        public string Signing_Legal_Entity { get; set; }
        /// <summary>
        /// 执行部门
        /// </summary>
        [ExporterHeader(DisplayName = "Project Department")]
        public string Delivery_Department { get; set; }
        /// <summary>
        /// 项目类型
        /// </summary>
        [ExporterHeader(DisplayName = "Project Type")]
        public string Project_Type { get; set; }
        /// <summary>
        /// 项目计费类型
        /// </summary>
        [ExporterHeader(DisplayName = "Billing Type")]
        public string Billing_Type { get; set; }
        /// <summary>
        /// 项目开始时间
        /// </summary>
        [ExporterHeader(DisplayName = "Project Start Date")]
        public DateTime StartDate { get; set; }
        /// <summary>
        /// 项目结束时间
        /// </summary>
        [ExporterHeader(DisplayName = "Project End Date")]
        public DateTime EndDate { get; set; }
        /// <summary>
        /// 本月进入项目日期
        /// </summary>
        [ExporterHeader(DisplayName = "Onboarding Date")]
        public DateTime? EnteringProjectDate { get; set; }
        /// <summary>
        /// 本月离开项目日期
        /// </summary>
        [ExporterHeader(DisplayName = "Offboarding Date")]
        public DateTime? LeavingProjectDate { get; set; }
        /// <summary>
        /// 人力投入项目人月（财务）
        /// </summary>
        [ExporterHeader(DisplayName = "Man-Months Invested in Projects (Financial)")]
        public decimal? NumberOfManpowerFinancial { get; set; }
        /// <summary>
        /// 人力投入项目人月（实际）
        /// </summary>
        [ExporterHeader(DisplayName = "Man-Months Invested in Projects (Actual)")]
        public decimal? NumberOfManpowerActual { get; set; }
    }
}
