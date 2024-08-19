using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCS.Entity.DTO.Contract
{
    public class ContractPagerModel
    {
        public int ContractHistoryId { get; set; }
        public int Version { get; set; }

        public int Id { get; set; }
        public string Name { get; set; }

        public string Code { get; set; }

        public string Customer_Contract_Number { get; set; }

        public string Settlement_Currency { get; set; }

        public decimal PO_Amount { get; set; }

        public string Billing_Type { get; set; }

        public int Category { get; set; }

        public string Client_Code { get; set; }

        public string Client_Entity { get; set; }

        public int ChangeCount { get; set; }

        public string Signing_Legal_Entity { get; set; }

        public string Sales_Manager { get; set; }

        public int Sales_Manager_Id { get; set; }

        public string Sales_Manager_Employee_Number { get;set; }

        public string Signing_Department_Id { get; set; }

        public string Signing_Department { get; set; }

        public byte Operating_Status { get; set; }

        public byte Approval_Status { get; set; }

        public DateTime? Contract_Takenback_Date { get; set; }

        public byte IsPO { get; set; }

        public string Sales_Type { get; set; }

        public string Client_Organization_Name { get; set; }

        public string Billing_Cycle { get; set; }

        public string Collection_Period { get; set; }

        public string Client_line_Group { get; set; }

        public string Creator { get; set; }

        public string Creator_Employee_Number { get; set; }

        public DateTime CreateTime { get; set; }
        public DateTime? End_Date { get; set; }
        public DateTime? Effective_Date { get; set; }
        public string Reason_change { get; set; }
        public string Client_Contract_Type { get; set; }
        public string Procurement_Type { get; set; }
        public int Frame_Contract_Id { get; set; }
        public string Frame_Contract_Code { get; set; }

        public int Client_Id { get; set; }

        public byte? Is_Handle_Change { get; set; }

        public string WorkFlowTable_Id { get; set; }
    }
}
