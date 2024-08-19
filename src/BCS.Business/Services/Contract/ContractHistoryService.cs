using BCS.Business.IRepositories;
using BCS.Business.IServices;
using BCS.Core.BaseProvider;
using BCS.Core.Extensions.AutofacManager;
using BCS.Core.Utilities;
using BCS.Entity.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BCS.Business.Services
{
    public partial class ContractHistoryService : ServiceBase<ContractHistory, IContractHistoryRepository>
    , IContractHistoryService, IDependency
    {
        private WebResponseContent Response { get; set; }

        public ContractHistoryService(IContractHistoryRepository repository)
        : base(repository)
        {
            Init(repository);
            Response = new WebResponseContent(true);
        }
        public static IContractHistoryRepository Instance
        {
            get { return AutofacContainerModule.GetService<IContractHistoryRepository>(); }
        }

        public bool AddHistory(Contract contract, int version)
        {
            if (contract == null || contract.Id == 0) return false;

            repository.DbContextBeginTransaction(() =>
            {
                var history = new ContractHistory
                {
                    Contract_Id = contract.Id,
                    Code = contract.Code,
                    Client_Contract_Code = contract.Client_Contract_Code,
                    IsPO = contract.IsPO,
                    Category = contract.Category,
                    Customer_Contract_Number = contract.Customer_Contract_Number,
                    Name = contract.Name,
                    Signing_Department_Id = contract.Signing_Department_Id,
                    Signing_Department = contract.Signing_Department,
                    Frame_Contract_Id = contract.Frame_Contract_Id,
                    Client_Id = contract.Client_Id,
                    Signing_Legal_Entity = contract.Signing_Legal_Entity,
                    Procurement_Type = contract.Procurement_Type,
                    Billing_Type = contract.Billing_Type,
                    Sales_Type = contract.Sales_Type,
                    Client_Contract_Type = contract.Client_Contract_Type,
                    Client_Organization_Name = contract.Client_Organization_Name,
                    Sales_Manager = contract.Sales_Manager,
                    Sales_Manager_Id = contract.Sales_Manager_Id,
                    PO_Owner = contract.PO_Owner,
                    Creator = contract.Creator,
                    CreatorID = contract.CreatorID,
                    Effective_Date = contract.Effective_Date,
                    End_Date = contract.End_Date,
                    Settlement_Currency = contract.Settlement_Currency,
                    Associated_Contract_Code = contract.Associated_Contract_Code,
                    PO_Amount = contract.PO_Amount,
                    Exchange_Rate = contract.Exchange_Rate,
                    Tax_Rate = contract.Tax_Rate,
                    Tax_Rate_No_Purchase = contract.Tax_Rate_No_Purchase,
                    Billing_Cycle = contract.Billing_Cycle,
                    Estimated_Billing_Cycle = contract.Estimated_Billing_Cycle,
                    Collection_Period = contract.Collection_Period,
                    Is_Charge_Rate_Type = contract.Is_Charge_Rate_Type,
                    Charge_Rate_Unit = contract.Charge_Rate_Unit,
                    Contract_Takenback_Date = contract.Contract_Takenback_Date,
                    Estimated_Contract_Takenback_Date = contract.Estimated_Contract_Takenback_Date,
                    Remark = contract.Remark,
                    Reason_change = contract.Reason_change,
                    IsDelete = contract.IsDelete,
                    Operating_Status = contract.Operating_Status,
                    Approval_Status = contract.Approval_Status,
                    Version = version,
                    CreateTime = DateTime.Now
                };
                repository.Add(history);                
                repository.DbContext.SaveChanges();
                return Response.OK();
            });
            return true;
        }

        public int NextVersion(int contract_id)
        {
            var histories = GetContractHistories(contract_id);
            if (histories == null || histories.Count == 0) return 0;

            return histories.OrderByDescending(x => x.Version).FirstOrDefault().Version + 1;
        }
    }
}
