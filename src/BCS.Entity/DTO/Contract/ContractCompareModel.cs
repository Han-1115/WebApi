using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCS.Entity.DomainModels;

namespace BCS.Entity.DTO.Contract
{
    public class ContractCompareModel
    {
        public CompareInfo<Frame_Contract> FrameContract { get; set; }
        public CompareInfo<ContractCompareDTO> Contract { get; set; }
        public CompareInfo<List<ContractAttachments>> Attachment { get; set; }
        public CompareInfo<List<Contract_Project>> Project { get; set; }
        public CompareInfo<List<Sys_WorkFlowTableStep>> SysWorkFlowTableStep { get; set; }

        public string Reason_change { get; set; }
    }

    public class ContractCompareDTO
    {
        public int Id { get; set; }

        public int Contract_Id { get; set; }

        public byte IsPO { get; set; }

        public int Category { get; set; }

        public string Customer_Contract_Number { get; set; }

        public string Name { get; set; }

        public string Signing_Department_Id { get; set; }

        public string Signing_Department { get; set; }

        public string Code { get; set; }

        public string Client_Contract_Code { get; set; }

        public string Signing_Legal_Entity { get; set; }

        public string Procurement_Type { get; set; }

        public string Billing_Type { get; set; }

        public string Sales_Type { get; set; }

        public string Client_Contract_Type { get; set; }

        public string Client_Organization_Name { get; set; }

        public string Sales_Manager { get; set; }

        public string PO_Owner { get; set; }

        public string Creator { get; set; }

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

        public string Reason_change { get; set; }

        public string CreateTime { get; set; }

        public int Version { get; set; }

        #region 客户信息

        /// <summary>
        ///客户实体
        /// </summary>
        public string Client_Entity { get; set; }

        /// <summary>
        ///客户编码
        /// </summary>
        public string Client_Code { get; set; }

        /// <summary>
        ///客户系/群
        /// </summary>
        public string Client_line_Group { get; set; }

        /// <summary>
        ///客户所属行业
        /// </summary>
        public string Client_Industry { get; set; }

        /// <summary>
        ///客户所属城市
        /// </summary>
        public string Client_Location_City { get; set; }

        #endregion
    }


    public class CompareInfo<T>
    {

        public T Current { get; set; }

        public T Previous { get; set; }
    }
}
