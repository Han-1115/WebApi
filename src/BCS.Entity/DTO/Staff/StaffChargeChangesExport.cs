using Magicodes.ExporterAndImporter.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCS.Entity.DTO.Staff
{
    public class StaffChargeChangesExport
    {
        //年月
        [ExporterHeader(DisplayName = "Month Year", AutoCenterColumn = true)]
        public int YearMonth { get; set; }
        ////员工id
        //[ExporterHeader(DisplayName = "员工id")]
        //public int StaffId { get; set; }
        //员工工号
        [ExporterHeader(DisplayName = "EID", AutoCenterColumn = true)]
        public string StaffNo { get; set; }
        //员工姓名
        [ExporterHeader(DisplayName = "Employee Name", AutoCenterColumn = true)]
        public string StaffName { get; set; }
        //员工部门编号
        //[ExporterHeader(DisplayName = "部门编号")]
        //public string DepartmentId { get; set; }
        //员工部门
        [ExporterHeader(DisplayName = "Staff Department", AutoCenterColumn = true)]
        public string DepartmentName { get; set; }
        //是否为分包人员
        [ExporterHeader(DisplayName = "Subcontractor", AutoCenterColumn = true)]
        public string IsSubcontract { get; set; }
        ////项目部门
        //[ExporterHeader(DisplayName = "项目部门id")]
        //public string Delivery_Department_Id { get; set; }
        //项目部门
        [ExporterHeader(DisplayName = "Project Department", AutoCenterColumn = true)]
        public string Delivery_Department { get; set; }
        //项目编号
        [ExporterHeader(DisplayName = "Project#", AutoCenterColumn = true)]
        public string Project_Code { get; set; }
        //项目名称
        [ExporterHeader(DisplayName = "Project Name", AutoCenterColumn = true)]
        public string Project_Name { get; set; }
        //项目开始日期
        [ExporterHeader(DisplayName = "Project Start Date", Format = "MM/dd/yyyy", AutoCenterColumn = true)]
        public DateTime? ProjectStartDate { get; set; }

        //项目结束日期
        [ExporterHeader(DisplayName = "Project End Date", Format = "MM/dd/yyyy", AutoCenterColumn = true)]
        public DateTime? ProjectEndDate { get; set; }
        //项目经理编号
        //[ExporterHeader(DisplayName = "项目经理ID")]
        //public int Project_Manager_Id { get; set; }
        //项目经理名称
        [ExporterHeader(DisplayName = "Program Manager", AutoCenterColumn = true)]
        public string Project_Manager { get; set; }
        //结算类型 
        [ExporterHeader(DisplayName = "Billing Type", AutoCenterColumn = true)]
        public string Billing_Type { get; set; }
        //结算币种
        [ExporterHeader(DisplayName = "Settlement Currency", AutoCenterColumn = true)]
        public string Settlement_Currency { get; set; }
        //ChargeRate单位
        [ExporterHeader(DisplayName = "Charge Rate Unit", AutoCenterColumn = true)]
        public string Charge_Rate_Unit { get; set; }
        //投入开始日期
        [ExporterHeader(DisplayName = "Onboarding Date", Format = "MM/dd/yyyy", AutoCenterColumn = true)]
        public DateTime? OnboardingDate { get; set; }
        //投入结束日期
        [ExporterHeader(DisplayName = "Offboarding Date", Format = "MM/dd/yyyy", AutoCenterColumn = true)]
        public DateTime? OffboardingDate { get; set; }


        [ExporterHeader(DisplayName = "Before Change Charge Rate", AutoCenterColumn = true)]
        public decimal ChargeRateBefore { get; set; }
        /// <summary>
        /// 月初日期
        /// </summary>
        [ExporterHeader(DisplayName = "Date", Format = "MM/dd/yyyy", AutoCenterColumn = true)]
        public DateTime? OriginDate { get; set; }
        /// <summary>
        /// 变动后ChargeRate   
        /// </summary>
        [ExporterHeader(DisplayName = "After Change Charge Rate", AutoCenterColumn = true)]
        public decimal ChargeRate { get; set; }
        /// <summary>
        /// 最终变动日期
        /// </summary>
        [ExporterHeader(DisplayName = "Change Date", Format = "MM/dd/yyyy", AutoCenterColumn = true)]
        public DateTime? ChangeDate { get; set; }
        /// 最终变动类型
        [ExporterHeader(DisplayName = "Change Type", AutoCenterColumn = true)]
        public string ChangeTypeName { get; set; }
        /// <summary>
        /// 变动原因
        /// </summary>
        [ExporterHeader(DisplayName = "Change Reason", AutoCenterColumn = true)]
        public string ChangeReason { get; set; }
        /// <summary>
        /// 变动次数
        /// </summary>
        [ExporterHeader(DisplayName = "Change Count", AutoCenterColumn = true)]
        public int ChangeTimes { get; set; }

    }
}
