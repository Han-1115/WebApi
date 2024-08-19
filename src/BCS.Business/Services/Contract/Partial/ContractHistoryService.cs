using BCS.Entity.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCS.Business.Services
{
    public partial class ContractHistoryService
    {
        public List<ContractHistory> GetContractHistories(int contract_id) => repository.Find(x => x.Contract_Id == contract_id);

        /// <summary>
        /// 通过id获取合同历史
        /// </summary>
        /// <param name="contractHistoryId">合同历史Id</param>
        /// <returns>合同历史Entity</returns>
        public BCS.Entity.DomainModels.ContractHistory GetContractHistory(int contractHistoryId)=> repository.FindFirst(x => x.Id == contractHistoryId);
    }
}
