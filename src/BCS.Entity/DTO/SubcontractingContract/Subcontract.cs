using BCS.Entity.DomainModels;
using BCS.Entity.DTO.Contract;
using Magicodes.ExporterAndImporter.Core;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCS.Entity.DTO.SubcontractingContract
{
    public class SubcontractListDetailsEqualityComparer : IEqualityComparer<SubcontractListDetails>
    {
        public bool Equals(SubcontractListDetails x, SubcontractListDetails y)
        {
            if (ReferenceEquals(x, y))
                return true;

            if (x is null || y is null)
                return false;

            return x.SubContractId == y.SubContractId;
        }

        public int GetHashCode([DisallowNull] SubcontractListDetails obj)
        {
            if (obj.SubContractId ==0)
            {
                return obj.GetHashCode();
            }
            else
            {
                return obj.SubContractId.GetHashCode();
            }
            
        }
    }
    public class SubcontractListDetails 
    {
        [ExporterHeader(IsIgnore = true)]
        public int Id { get; set; } = 0;
        [ExporterHeader(IsIgnore = true)]
        public int Index { get; set; }
        [ExporterHeader(IsIgnore = true)]
        public int SubContractId { get; set; }
        [ExporterHeader(DisplayName = "Sales Contract ID")]
        public string ContractCode { get; set; }
        [ExporterHeader(DisplayName = "Sales Contract Name")]
        public string ContractName { get; set; }
        [ExporterHeader(DisplayName = "Project#")]
        public int ProjectId { get; set; }
        public string ProjectCode { get; set; }
        [ExporterHeader(DisplayName = "Project Name")]
        public string ProjectName { get; set; }
        [ExporterHeader(DisplayName = "Subcontract ID")]
        public string SubcontractCode { get; set; }
        [ExporterHeader(DisplayName = "Subcontract Name")]
        public string SubcontractName { get; set; }
        [ExporterHeader(DisplayName = "Subcontract Submition Date",Format = "yyyy-MM-dd")]
        public DateTime? SubcontractCreateTime { get; set; }
        [ExporterHeader(IsIgnore = true)]
        public string SubcontractDeliveryDepartmentId { get; set; }
        [ExporterHeader(DisplayName = "Department")]
        public string SubcontractDeliveryDepartment { get; set; }
        public string Supplier { get; set; }
        [ExporterHeader(IsIgnore = true)]
        public int SubcontractDirectorId { get; set; }
        [ExporterHeader(IsIgnore = true)]
        public string SubcontractDirector { get; set; }
        [ExporterHeader(IsIgnore = true)]
        public int SubcontractManagerId { get; set; }
        [ExporterHeader(DisplayName = "Project Manager")]
        public string SubcontractManager { get; set; }
        [ExporterHeader(DisplayName = "Subcontract Type")]
        public string SubcontractProcurementType { get; set; }
        [ExporterHeader(IsIgnore = true)]
        public int SubcontractChargeTypeId { get; set; }
        [ExporterHeader(DisplayName = "Billing Mode")]
        public string SubcontractChargeType { get; set; }
        [ExporterHeader(IsIgnore = true)]
        public int SubcontractBillingModelId { get; set; }
        [ExporterHeader(DisplayName = "Billing Type")]
        public string SubcontractBillingModel { get; set; }
        [ExporterHeader(DisplayName = "Subcontract Tax Rate(%)")]
        public string SubcontractTaxRate { get; set; }
        [ExporterHeader(DisplayName = "Subcontract Amount(PO Currency)")]
        public Decimal? SubcontractAmount { get; set; }
        [ExporterHeader(DisplayName = "Subcontract Effective Date", Format = "yyyy-MM-dd")]
        public DateTime? SubcontractEffectiveDate { get; set; }
        [ExporterHeader(DisplayName = "Subcontract End Date", Format = "yyyy-MM-dd")]
        public DateTime? SubcontractEndDate { get; set; }
        [ExporterHeader(IsIgnore = true)]
        public int SubcontractOperatingStatusId { get; set; }
        [ExporterHeader(DisplayName = "Status")]
        public string SubcontractOperatingStatus { get; set; }
        [ExporterHeader(IsIgnore = true)]
        public int SubcontractApprovalStatusId { get; set; }
        [ExporterHeader(DisplayName = "Approval Status")]
        public string SubcontractApprovalStatus { get; set; }
        [ExporterHeader(IsIgnore = true)]
        public bool Is_Handle_Change { get; set; }
    }

    public class SubcontractHistoryList
    {
        public int SubContractFlowId { get; set; }
        public int Version { get; set; }
        public string ChangeReason { get; set; }
        public DateTime? ApproveEndDate { get; set; }
    }

    public class SubcontractHistoryContractAndProjectInfo
    {
        public string Project_Code { get; set; }
        public string Project_Name { get; set; }
        public string Project_Type { get; set; }
        public string Project_Billing_Mode { get; set; }
        public decimal Subcontracting_Cost { get; set; }
        public decimal Subcontracting_Balance { get; set; }
        public string Contract_Code { get; set; }
        public string Contract_Name { get; set; }
        public string Client_Organization_Name { get; set; }
        public decimal? PO_Amount { get; set; }
    }

    public class SubcontractHistoryBasicInfo
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Delivery_Department { get; set; }
        public string Procurement_Type { get; set; }
        public string Supplier {  get; set; }
        public decimal Subcontracting_Contract_Amount {  get; set; }
        public string Settlement_Currency { get; set; }
        public string Exchange_Rate { get; set; }
        public decimal Subcontracting_Contract_Amount_CNY { get; set; }
        public string Tax_Rate { get; set; }
        public int Payment_CycleId { get; set; }
        public string Payment_Cycle { get; set; }
        public int Charge_TypeId {  get; set; }
        public string Charge_Type { get; set; }
        public int Billing_ModeId { get; set; }
        public string Billing_Mode { get; set; }
        public int Billing_CycleId { get; set; }
        public string Billing_Cycle { get;set; }
        public DateTime Effective_Date { get; set; }
        public DateTime End_Date { get; set; }
        public int Cost_Rate_UnitId { get; set; }
        public string Cost_Rate { get; set; }
        public int Is_Assigned_Supplier { get; set; }
        public string Is_Assigned { get; set; }
        public string Contract_Director { get; set; }
        public string Subcontracting_Reason { get; set; }
        public string Contract_Manager { get; set; }
        public string Change_Reason { get; set; }
    }


    public class SubcontractHistoryDetails
    {
        public CompareInfo<SubcontractHistoryContractAndProjectInfo> subcontractHistoryContractAndProjectInfo { get; set; }
        public CompareInfo<SubcontractHistoryBasicInfo> subcontractHostoryBasicInfo { get; set; }
        public CompareInfo<List<SubContractFlowStaff>> subContractFlowStaffLists { get; set; }
        public CompareInfo<List<SubContractFlowPaymentPlan>> flowPaymentPlanLists { get; set; }
        public CompareInfo<List<SubContractFlowAttachment>> flowAttachMentLists { get; set; }
        public CompareInfo<List<Sys_WorkFlowTableStep>> sys_WorkFlowTableStep { get; set; } 
    }

}
