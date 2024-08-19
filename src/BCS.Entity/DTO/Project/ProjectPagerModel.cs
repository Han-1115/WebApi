using BCS.Entity.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BCS.Entity.DTO.Project
{
    [Obsolete("暂时弃用")]
    public class ProjectPagerModel
    {
        public int Project_Id { get; set; }
        public string Project_Code { get; set; }
        public string Project_Name { get; set; }
        public string Signing_Legal_Entity { get; set; }
        public string Service_Type { get; set; }
        public string Billing_Type { get; set; }
        public string Billing_Mode { get; set; }
        public string Project_Type { get; set; }
        public string Cooperation_Type { get; set; }
        public string Delivery_Department { get; set; }
        public string Settlement_Currency { get; set; }
        public string Tax_Rate { get; set; }
        public byte Purely_Subcontracted_Project { get; set; }
        public decimal Project_Amount { get; set; }
        public DateTime Start_Date { get; set; } = DateTime.Now;
        public DateTime End_Date { get; set; } = DateTime.Now;
        public string Project_Manager { get; set; }
        public int? Project_ManagerId { get; set; }
        public byte Operating_Status { get; set; }
        public byte Approval_Status { get; set; }
        public byte Project_Status { get; set; }
        public string Project_LocationCity { get; set; }
        public string Client_Organization { get; set; }
        public string Client_Contract_Type { get; set; }
        public decimal Income_From_Own_Delivery { get; set; }
        public decimal Subcontracting_Income { get; set; }
        //Own Delivery HR Cost
        public decimal Own_Delivery_HR_Cost { get; set; }
        //Subcontracting Cost
        public decimal Subcontracting_Cost { get; set; }
        //Other Project Costs
        public decimal Other_Project_Costs { get; set; }
        //Gross Profit from Own Delivery
        public decimal Gross_Profit_From_Own_Delivery { get; set; }
        //Gross Profit Margin from Own Delivery (%)
        public decimal Gross_Profit_Margin_From_Own_Delivery { get; set; }
        //Project Gross Profit
        public decimal Project_Gross_Profit { get; set; }
        //Project Gross Profit Margin (%)
        public decimal Project_Gross_Profit_Margin { get; set; }
        //Project Gross Profit Margin (%)
        public decimal Holiday_System { get; set; }
        public decimal Standard_NumberOf_Days_Per_Month { get; set; }
        public decimal Standard_Daily_Hours { get; set; }

        public string Project_Director { get; set; }
        public String  Contract_Code { get; set; }
        public String Customer_Contract_Number { get; set; }
        public byte IsPo { get; set; }
        public string Signing_Department { get; set; }
        public int Category { get; set; }
        public string Contract_Billing_Type { get; set; }
        public decimal PO_Amount { get; set; }
        public string Customer_Contract_Name { get; set; }
        public string Client_line_Group { get; set; }
        public string Client_Organization_Name { get; set; }
        public string Sales_Manager { get; set; }
        public string Sales_Type { get; set; }
        public DateTime? Contract_Takenback_Date { get; set; }
        public string Reason_change { get; set; }
        public string Contract_Project_Relationship { get; set; }
        public string Contract_Creator { get; set; }
        public byte Site_TypeId { get; set; }
        public string Conrtact_Settlement_Currency { get; set; }
        public int Contract_Id { get; set; }
        public string Contract_Name { get; set; }
        public string Client_Contract_Code { get; set; }

        /// <summary>
        /// 工作流表Id
        /// </summary>
        public Guid WorkFlowTable_Id { get; set; }

    }
}
