using Magicodes.ExporterAndImporter.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCS.Entity.DTO.Staff
{
    public class StaffPagerExport
    {
        /// <summary>
        /// 工号
        /// </summary>
        [ExporterHeader(DisplayName = "EID")]
        public string StaffNo { get; set; }
        /// <summary>
        /// 员工姓名
        /// </summary>
        [ExporterHeader(DisplayName = "Employee Name")]
        public string StaffName { get; set; }
        /// <summary>
        ///人员所在部门
        /// </summary>
        [ExporterHeader(DisplayName = "Staff Department")]
        public string StaffDepartment { get; set; }
        /// <summary>
        /// 是否分包人员
        /// </summary>
        [ExporterHeader(DisplayName = "Subcontractor")]
        public string IsSubcontract { get; set; }
        /// <summary>
        /// 交付部门
        /// </summary>
        [ExporterHeader(DisplayName = "Project Department")]
        public string Delivery_Department { get; set; }
        /// <summary>
        /// 项目编码
        /// </summary>
        [ExporterHeader(DisplayName = "Project #")]
        public string Project_Code { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        [ExporterHeader(DisplayName = "Project Name")]
        public string Project_Name { get; set; }
        /// <summary>
        /// 项目开始日期
        /// </summary>
        [ExporterHeader(DisplayName = "Project Start Date")]
        public string Project_Start_Date { get; set; }
        /// <summary>
        /// 项目结束日期
        /// </summary>
        [ExporterHeader(DisplayName = "Project End Date")]
        public string Project_End_Date { get; set; }
        /// <summary>
        /// 项目类型
        /// </summary>
        [ExporterHeader(DisplayName = "Project Type")]
        public string Project_Type { get; set; }
        /// <summary>
        /// 计费类型
        /// </summary>
        [ExporterHeader(DisplayName = "Billing Type")]
        public string Billing_Type { get; set; }
        /// <summary>
        /// 项目经理
        /// </summary>
        [ExporterHeader(DisplayName = "Program Manager")]
        public string Project_Manager { get; set; }
        /// <summary>
        ///Charge Rate金额
        /// </summary>
        [ExporterHeader(DisplayName = "Charge Rate")]
        public decimal ChargeRate { get; set; }
        /// <summary>
        /// 投入开始日期
        /// </summary>
        [ExporterHeader(DisplayName = "Onboarding Date")]
        public string InputStartDate { get; set; }
        /// <summary>
        /// 投入结束日期
        /// </summary>
        [ExporterHeader(DisplayName = "Offboarding Date")]
        public string InputEndDate { get; set; }
        /// <summary>
        /// 投入百分比
        /// </summary>
        [ExporterHeader(DisplayName = "Capacity (%)")]
        public decimal InputPercentage { get; set; }
        
    }
}
