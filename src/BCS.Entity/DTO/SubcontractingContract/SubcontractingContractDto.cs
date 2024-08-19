/*
 *代码由框架生成,任何更改都可能导致被代码生成器覆盖
 *如果数据库字段发生变化，请在代码生器重新生成此Model
 */
using BCS.Entity.DomainModels;
using System;
using System.Collections.Generic;

namespace BCS.Entity.DTO.SubcontractingContract
{
    public class SubcontractingContractDto : DomainModels.SubcontractingContract
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

        public Decimal? SubcontractCostBudgetBalance { get; set; }

        public int SubContract_Id { get; set; }

        public List<SubcontractingContractPaymentPlan> SubContractFlowPaymentPlanList { get; set; }

        public List<SubcontractingContractAttachment> SubContractFlowAttachmentList { get; set; }

        public List<DomainModels.SubcontractingStaff> SubContractFlowStaffList { get; set; }

    }
}