using BCS.Core.BaseProvider;
using BCS.Entity.DomainModels;
using BCS.Core.Utilities;
using System.Linq.Expressions;

namespace BCS.Business.IServices
{
    public partial interface IContractAttachmentsService
    {
        /// <summary>
        /// 根据合同Id找到对应的所有附件
        /// </summary>
        /// <param name="id">合同Id</param>
        /// <returns>附件list</returns>
        List<ContractAttachments> GetContractAttachmentsByContactId(int contactId);

        /// <summary>
        /// 添加合同附件
        /// </summary>
        /// <param name = "id" > 合同Id </ param >
        /// <param name = "fileName" > 文件名 </ param >
        /// <param name = "filePath" > 文件路径 </ param >
        /// < returns > 合同附件Id </ returns >
        int Add(int contactId, string fileName, string filePath);

        /// <summary>
        /// 更新合同附件
        /// </summary>
        /// <param name = "id" > Id </ param >
        /// <param name = "contactId" > 合同Id </ param >
        /// <param name = "fileName" > 文件名 </ param >
        /// <param name = "filePath" > 文件路径 </ param >
        /// < returns > 成功返回true,否则返回false </ returns >
        bool Update(int id, int contactId, string fileName, string filePath);

        /// <summary>
        /// 更新合同附件
        /// </summary>
        /// <param name = "id" > Id </ param >
        /// < returns > 成功返回true,否则返回false </ returns >
        bool Deactivate(int id);


        /// <summary>
        /// 通过id获取合同附件
        /// </summary>
        /// <param name = "id" > Id </ param >
        /// < returns > 合同Entity </ returns >
        ContractAttachments GetContractAttachment(int id);

        bool UpdateContractAttachments(List<ContractAttachments> addContractAttachments, List<ContractAttachments> delContractAttachments, int version, bool needToAddHistory);
    }
}
