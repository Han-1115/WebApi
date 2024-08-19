using BCS.Core.BaseProvider;
using BCS.Entity.DomainModels;
using BCS.Core.Utilities;
using System.Linq.Expressions;

namespace BCS.Business.IServices
{
    public partial interface IContractHistoryService
    {
        /// <summary>
        /// 添加合同历史
        /// </summary>
        /// <param name="contract">合同</param>
        /// <param name="version">变更版本号</param>
        /// <returns>添加成返回true，否则false</returns>
        bool AddHistory(Contract contract, int version);

        /// <summary>
        /// 通过合同id查询变更记录
        /// </summary>
        /// <param name="contract_id">合同id</param>
        /// <returns></returns>
        List<ContractHistory> GetContractHistories(int contract_id);

        /// <summary>
        /// 通过id获取合同历史
        /// </summary>
        /// <param name="contractHistoryId">合同历史Id</param>
        /// <returns>合同历史Entity</returns>
        ContractHistory GetContractHistory(int contractHistoryId);

        /// <summary>
        /// 通过contract_id获取下一次的历史版本号
        /// </summary>
        /// <param name="contract_id">合同Id</param>
        /// <returns>合同历史Entity</returns>
        int NextVersion(int contract_id);
    }
}
