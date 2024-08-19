using BCS.Core.BaseProvider;
using BCS.Entity.DomainModels;
using BCS.Core.Utilities;
using System.Linq.Expressions;

namespace BCS.Business.IServices
{
    public partial interface IContractAttachmentsHistoryService
    {
        /// <summary>
        /// 添加合同附件历史
        /// </summary>
        /// <param name="contractAttachments">合同附件</param>
        /// <param name="version">变更版本号</param>
        /// <returns>添加成返回true，否则false</returns>
        bool AddHistory(BCS.Entity.DomainModels.ContractAttachments contractAttachments, int version);
    }
}
