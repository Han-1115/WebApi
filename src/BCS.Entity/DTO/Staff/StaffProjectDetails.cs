using BCS.Entity.DomainModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Magicodes.ExporterAndImporter.Core;

namespace BCS.Entity.DTO.Staff
{
    public class StaffProjectDetailsModel
    {
        public ContractBasicInfo contractBasicInfo { get; set; }
        public PageGridData<StaffProjectDetails> staffProjectDetails { get; set; }
        public ProjectBasicInfo projectBasicInfo { get; set; }
        public ProjectBudgetInfo projectBudgetInfo { get; set; }
        public List<ManMonthDetails> manMonthDetails { get; set; }
    }

    public class StaffProjectDetailsModelV2
    {
        public ContractBasicInfo contractBasicInfo { get; set; }
        public List<StaffProjectDetailsV2> staffProjectDetails { get; set; }
        public ProjectBasicInfo projectBasicInfo { get; set; }
        public ProjectBudgetInfo projectBudgetInfo { get; set; }
        public List<ManMonthDetails> manMonthDetails { get; set; }
    }

    public class ManMonthDetails
    {
        public string YearMonth { get; set; }
        public decimal HCCountPlan { get; set; }
    }

    public class StaffProjectDetailsV2 : StaffProjectDetails
    {
        public int StaffProjectId { get; set; }

        public Decimal ChargeRateBefore { get; set; }

        public byte IsChargeRateChange { get; set; }

        public byte IsSubmitted { get; set; }
    }
    public class StaffProjectDetails
    {
        [ExporterHeader(IsIgnore = true)]
        public int Id { get; set; }
        [ExporterHeader(IsIgnore = true)]
        public int ProjectId { get; set; }
        [ExporterHeader(DisplayName = "EID")]
        public string StaffNo { get; set; }
        [ExporterHeader(DisplayName = "Employee Name")]
        public string StaffName { get; set; }
        [ExporterHeader(DisplayName = "Staff Department")]
        public string StaffDepartment { get; set; }
        [ExporterHeader(IsIgnore = true)]
        public DateTime? StaffEnterDate { get; set; }
        [ExporterHeader(IsIgnore = true)]
        public DateTime? StaffLeaveDate { get; set; }
        [ExporterHeader(DisplayName = "Subcontractor")]
        public bool IsSubcontract { get; set; }
        [ExporterHeader(DisplayName = "Charge Rate", Format = "###.00")]
        public Decimal ChargeRate { get; set; }
        [ExporterHeader(DisplayName = "Onboarding Date", Format = "yyyy-MM-dd")]
        public DateTime? InputStartDate { get; set; }
        [ExporterHeader(DisplayName = "Offboarding Date", Format = "yyyy-MM-dd")]
        public DateTime? InputEndDate { get; set; }
        [ExporterHeader(DisplayName = "Capacity (%)", Format = "###.00")]
        public Decimal InputPercentage { get; set; }
        [ExporterHeader(IsIgnore = true)]
        public int StaffId { get; set; }
        [ExporterHeader(IsIgnore = true)]
        public int CreateID { get; set; }
        [ExporterHeader(IsIgnore = true)]
        public string Creator { get; set; }
        [ExporterHeader(IsIgnore = true)]
        public DateTime CreateDate { get; set; }
        [ExporterHeader(IsIgnore = true)]
        public byte IsDelete { get; set; }
        [ExporterHeader(IsIgnore = true)]
        public int ChangeType { get; set; }
        [ExporterHeader(DisplayName = "Change Type")]
        public string ChangeTypeName { get; set; }
        [ExporterHeader(DisplayName = "Change Reason")]
        public string ChangeReason { get; set; }
    }

    public class ContractBasicInfo
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Signing_Department { get; set; }
        public int Category { get; set; }
        public string Settlement_Currency { get; set; }
        public string Charge_Rate_Unit { get; set; }
        public string Client_Entity { get; set; }
    }

    public class ProjectBasicInfo
    {
        public string Project_Code { get; set; }
        public string Project_Name { get; set; }
        public string Signing_Legal_Entity { get; set; }
        public string Project_Type { get; set; }
        public int Project_TypeId { get; set; }
        public string Billing_Type { get; set; }
        public byte Billing_ModeId { get; set; }
        public string Billing_Model { get; set; }
        public decimal Project_Amount { get; set; }
        public DateTime Start_Date { get; set; }
        public DateTime End_Date { get; set; }
        public string Delivery_Department { get; set; }
        public string Delivery_Subcontract_Budget { get; set; } = "0";
        public string Tax_Rate { get; set; }
    }

    public class ProjectBudgetInfo
    {
        public int HeadCount { get; set; }
        public decimal TotalManHourCapacity { get; set; }
        public decimal Labor_Cost { get; set; } = 0;
        public decimal Project_Budget_Own_Delivery { get; set; } = 0;
        public decimal Rolling_Budget_Own_Delivery { get; set; } = 0;
        public decimal Threshold { get; set; } = 15;
    }
}
