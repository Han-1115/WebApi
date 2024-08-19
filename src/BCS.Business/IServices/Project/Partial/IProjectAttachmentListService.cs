/*
*所有关于ProjectAttachmentList类的业务代码接口应在此处编写
*/
using BCS.Core.BaseProvider;
using BCS.Entity.DomainModels;
using BCS.Core.Utilities;
using System.Linq.Expressions;
using BCS.Entity.DTO.Project;

namespace BCS.Business.IServices
{
    public partial interface IProjectAttachmentListService
    {
        /// <summary>
        /// 上传附件，只考虑 新增+删除
        /// </summary>
        /// <param name="projectId">项目ID</param>
        /// <param name="attachmentList">附件列表</param>
        /// <returns></returns>
        public Task<WebResponseContent> UploadAttachment(int projectId, ICollection<ProjectAttachmentListDTO> attachmentList);
    }
}
