using BCS.Core.BaseProvider;
using BCS.Entity.DomainModels;
using BCS.Core.Utilities;
using System.Linq.Expressions;

namespace BCS.Business.IServices
{
    public partial interface IContractProjectHistoryService
    {
        /// <summary>
        /// 添加合同项目关联表历史
        /// </summary>
        /// <param name="contract">合同项目关联表</param>
        /// <param name="version">变更版本号</param>
        /// <returns>添加成返回true，否则false</returns>
        bool AddHistory(ContractProject contractProject, int version);
    }
}
