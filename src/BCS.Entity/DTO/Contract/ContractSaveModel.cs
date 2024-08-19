using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCS.Entity.DomainModels;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace BCS.Entity.DTO.Contract
{
    public class ContractSaveModel
    {
        public int Id { get; set; }

        public byte IsPO { get; set; }

        public int Category { get; set; }

        public string Customer_Contract_Number { get; set; }

        public string Name { get; set; }

        public string Signing_Department_Id { get; set; }

        public string Signing_Department { get; set; }

        public Frame_Contract Frame_Contract { get; set; }

        public Client Client { get; set; }

        public List<ContractAttachments> Files { get; set; } = new();

        public List<Contract_Project> Contract_Projects { get; set; } = new();

        public string Code { get; set; }

        public string Client_Contract_Code { get; set; }

        public string Signing_Legal_Entity { get; set; }

        public string Procurement_Type { get; set; }

        public string Billing_Type { get; set; }

        public string Sales_Type { get; set; }

        public string Client_Contract_Type { get; set; }

        public string Client_Organization_Name { get; set; }

        public string Sales_Manager { get; set; }

        public int Sales_Manager_Id { get; set; }

        public string PO_Owner { get; set; }

        public string Creator { get; set; }

        public string Creator_Employee_Number { get; set; }

        public string Effective_Date { get; set; }

        public string End_Date { get; set; }

        public string Settlement_Currency { get; set; }

        public string Associated_Contract_Code { get; set; }

        public string PO_Amount { get; set; }

        public string Exchange_Rate { get; set; }

        public string Tax_Rate { get; set; }

        public string Tax_Rate_No_Purchase { get; set; }

        public string Billing_Cycle { get; set; }

        public string Estimated_Billing_Cycle { get; set; }

        public string Collection_Period { get; set; }

        public string Is_Charge_Rate_Type { get; set; }

        public string Charge_Rate_Unit { get; set; }

        public string Contract_Takenback_Date { get; set; }

        public string Estimated_Contract_Takenback_Date { get; set; }

        public string Remark { get; set; }

        public byte? Is_Handle_Change { get; set; }

        public string Reason_change { get; set; }

        public string CreateTime { get; set; }

    }


    public class Client
    {
        public int Id { get; set; }

        public string Client_Entity { get; set; }

        public string Client_Code { get; set; }

        public string Client_line_Group { get; set; }

        public string Client_Industry { get; set; }

        public string Client_Location_City { get; set; }
    }

    public class Contract_Project
    {
        public int Id { get; set; }

        public string Contract_Project_Relationship { get; set; }

        public string Project_Code { get; set; }

        public string Project_Name { get; set; }

        public decimal Project_Amount { get; set; }

        public int Project_TypeId { get; set; }
        public string Project_Type { get; set; }

        public string Delivery_Department_Id { get; set; }

        public string Delivery_Department { get; set; }

        public int Project_Manager_Id { get; set; }

        public string Project_Manager { get; set; }

        public string Remark { get; set; }
    }

    public class ContractAttachments
    {
        public IFormFile File { get; set; }

        public int Id { get; set; }

        public string FileName { get; set; }

        public string FilePath { get; set; }

        public DateTime UploadTime { get; set; }

        public DateTime CreateTime { get; set; }
    }


    /// <summary>
    /// 框架合同信息
    /// </summary>
    public class Frame_Contract
    {
        /// <summary>
        /// 框架合同Id（父级Id）
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// 框架合同编码（父级Code）
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        ///框架客户合同编码（父级Client_Contract_Code）
        /// </summary>
        public string Client_Contract_Code { get; set; }

        /// <summary>
        /// 框架合同名称（父级Name）
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 框架合同类型(父级Procurement_Type)
        /// </summary>
        public string Procurement_Type { get; set; }
    }
}
