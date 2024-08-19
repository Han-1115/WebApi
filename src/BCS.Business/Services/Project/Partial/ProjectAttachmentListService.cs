/*
 *所有关于ProjectAttachmentList类的业务代码应在此处编写
*可使用repository.调用常用方法，获取EF/Dapper等信息
*如果需要事务请使用repository.DbContextBeginTransaction
*也可使用DBServerProvider.手动获取数据库相关信息
*用户信息、权限、角色等使用UserContext.Current操作
*ProjectAttachmentListService对增、删、改查、导入、导出、审核业务代码扩展参照ServiceFunFilter
*/
using BCS.Core.BaseProvider;
using BCS.Core.Extensions.AutofacManager;
using BCS.Entity.DomainModels;
using System.Linq;
using BCS.Core.Utilities;
using System.Linq.Expressions;
using BCS.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using BCS.Business.IRepositories;
using BCS.Entity.DTO.Project;
using AutoMapper;
using BCS.Core.ManageUser;
using BCS.Core.EFDbContext;
using BCS.Core.DBManager;

namespace BCS.Business.Services
{
    public partial class ProjectAttachmentListService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IProjectAttachmentListRepository _repository;//访问数据库
        private readonly IMapper _mapper;

        [ActivatorUtilitiesConstructor]
        public ProjectAttachmentListService(
            IProjectAttachmentListRepository dbRepository,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper
            )
        : base(dbRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = dbRepository;
            _mapper = mapper;
            //多租户会用到这init代码，其他情况可以不用
            //base.Init(dbRepository);
        }

        /// <summary>
        /// 上传附件，只考虑 新增+删除
        /// </summary>
        /// <param name="projectId">项目ID</param>
        /// <param name="attachmentList">附件列表</param>
        /// <returns></returns>
        public async Task<WebResponseContent> UploadAttachment(int projectId, ICollection<ProjectAttachmentListDTO> attachmentList)
        {
            UserInfo userInfo = UserContext.Current.UserInfo;
            DateTime currentTime = DateTime.Now;
            userInfo.UserName = string.IsNullOrEmpty(userInfo.UserName) ? string.Empty : userInfo.UserName;
            var existsAttachment = await _repository.FindAsync(x => x.Project_Id == projectId);
            var insertAttachment = new List<ProjectAttachmentList>();
            var updateAttachment = new List<ProjectAttachmentList>();
            var removeAttachment = new List<ProjectAttachmentList>();
            foreach (var item in attachmentList)
            {
                var attachment = _mapper.Map<ProjectAttachmentList>(item);
                if (attachment.Id <= 0)
                {
                    attachment.Project_Id = projectId;
                    attachment.FileName = item.File.FileName;
                    attachment.UploadTime = currentTime;
                    var uploadFiles = new List<IFormFile>() { item.File };
                    var uploadResult = Upload(uploadFiles);
                    if (uploadResult.Status)
                    {
                        attachment.FilePath = $"{uploadResult.Data.ToString()}{item.File.FileName}";
                    }
                    else
                    {
                        throw new Exception($"上传附件异常:[{uploadResult.Message}]");
                    }
                    attachment.IsDelete = 0;
                    attachment.Remark = string.Empty;
                    attachment.CreateID = userInfo.User_Id;
                    attachment.Creator = userInfo.UserName;
                    attachment.CreateDate = currentTime;
                    attachment.ModifyID = userInfo.User_Id;
                    attachment.Modifier = userInfo.UserName;
                    attachment.ModifyDate = currentTime;
                    insertAttachment.Add(attachment);
                }
                else
                {
                    var updateItem = existsAttachment.Find(x => x.Id == attachment.Id);
                    ArgumentNullException.ThrowIfNull(updateItem, $"未找到ID为[{attachment.Id}]的附件信息");
                    updateItem.ModifyID = userInfo.User_Id;
                    updateItem.Modifier = userInfo.UserName;
                    updateItem.ModifyDate = currentTime;
                    updateAttachment.Add(updateItem);
                }
            }
            //删除业务
            foreach (var item in existsAttachment)
            {
                if (attachmentList.Any(o => o.Id == item.Id))
                {
                    continue;
                }
                item.IsDelete = 1;
                item.ModifyID = userInfo.User_Id;
                item.Modifier = userInfo.UserName;
                item.ModifyDate = currentTime;
                removeAttachment.Add(item);
            }

            BCSContext dbContext = DBServerProvider.GetEFDbContext();
            using (var transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    repository.AddRange(insertAttachment);
                    repository.UpdateRange(updateAttachment);
                    repository.UpdateRange(removeAttachment);

                    dbContext.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception($"插入附件异常:[{ex.Message}]");
                }
            }
            return WebResponseContent.Instance.OK("上传附件成功");
        }
    }
}
