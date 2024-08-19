using BCS.Business.IRepositories;
using BCS.Business.IServices;
using BCS.Core.BaseProvider;
using BCS.Core.DBManager;
using BCS.Core.EFDbContext;
using BCS.Core.Enums;
using BCS.Core.Extensions.AutofacManager;
using BCS.Core.Utilities;
using BCS.Entity.DomainModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCS.Business.Services
{
    public partial class ContractAttachmentsService : ServiceBase<ContractAttachments, IContractAttachmentsRepository>
    , IContractAttachmentsService, IDependency
    {
        private WebResponseContent Response { get; set; }

        private IContractAttachmentsHistoryRepository _attachmentsHistroyRepository;

        public ContractAttachmentsService(IContractAttachmentsRepository repository, IContractAttachmentsHistoryRepository attachmentsHistroyRepository)
        : base(repository)
        {
            Init(repository);
            Response = new WebResponseContent(true);
            _attachmentsHistroyRepository = attachmentsHistroyRepository;
        }
        public static IContractAttachmentsService Instance
        {
            get { return AutofacContainerModule.GetService<IContractAttachmentsService>(); }
        }


        public List<ContractAttachments> GetContractAttachmentsByContactId(int contactId)
        {
            return repository.FindAsIQueryable(x => x.Contract_Id == contactId && x.IsDelete == 0).ToList();
        }

        public int Add(int contactId, string fileName, string filePath)
        {
            if (contactId == 0) return 0;

            var contractAttachments = new ContractAttachments
            {
                Contract_Id = contactId,
                FileName = fileName,
                FilePath = filePath,
                UploadTime = DateTime.Now,
                IsDelete = (int)DeleteEnum.Not_Deleted
            };

            repository.DbContextBeginTransaction(() =>
            {
                repository.Add(contractAttachments);
                repository.DbContext.SaveChanges();
                return Response.OK();
            });
            return contractAttachments.Id;
        }

        public bool Update(int id, int contactId, string fileName, string filePath)
        {
            var return_id = 0;
            var contractAttachment = GetContractAttachment(id);
            if (contractAttachment == null || contractAttachment.Id == 0) return false;

            contractAttachment.Contract_Id = contactId;
            contractAttachment.FileName = fileName;
            contractAttachment.FilePath = filePath;

            repository.DbContextBeginTransaction(() =>
            {
                return_id = repository.Update(contractAttachment, x => new { x.Contract_Id, x.FileName, x.FilePath }, true);

                if (return_id != 0)
                {
                    return Response.OK();
                }
                return Response.Error("合同附件更新失败!");
            });

            return return_id != 0;
        }

        public bool Deactivate(int id)
        {
            var return_id = 0;
            var contractAttachment = GetContractAttachment(id);
            if (contractAttachment == null || contractAttachment.Id == 0) return false;

            contractAttachment.IsDelete = (int)DeleteEnum.Deleted;

            repository.DbContextBeginTransaction(() =>
            {
                return_id = repository.Update(contractAttachment, x => new { x.IsDelete }, true);

                if (return_id != 0)
                {
                    return Response.OK();
                }
                return Response.Error("合同附件更新失败!");
            });

            return return_id != 0;
        }

        public ContractAttachments GetContractAttachment(int id)
        {
            return repository.FindFirst(x => x.Id == id);
        }

        public bool UpdateContractAttachments(List<ContractAttachments> addContractAttachments, List<ContractAttachments> delContractAttachments, int version, bool needToAddHistory)
        {
            BCSContext dbContext = DBServerProvider.GetEFDbContext();
            using (var transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    repository.AddRange(addContractAttachments);
                    repository.UpdateRange(delContractAttachments);

                    dbContext.SaveChanges();

                    // TODO: 在审批通过后才会更新历史
                    //if (needToAddHistory)
                    //{
                    //    addContractAttachments.ForEach(contractAttachments =>
                    //    {
                    //        var history = new ContractAttachmentsHistory
                    //        {
                    //            ContractAttachments_Id = contractAttachments.Id,
                    //            Contract_Id = contractAttachments.Contract_Id,
                    //            FileName = contractAttachments.FileName,
                    //            FilePath = contractAttachments.FilePath,
                    //            UploadTime = contractAttachments.UploadTime,
                    //            IsDelete = contractAttachments.IsDelete,
                    //            Version = version,
                    //            CreateTime = DateTime.Now
                    //        };
                    //        _attachmentsHistroyRepository.Add(history);
                    //    });

                    //    delContractAttachments.ForEach(contractAttachments =>
                    //    {
                    //        var history = new ContractAttachmentsHistory
                    //        {
                    //            ContractAttachments_Id = contractAttachments.Id,
                    //            Contract_Id = contractAttachments.Contract_Id,
                    //            FileName = contractAttachments.FileName,
                    //            FilePath = contractAttachments.FilePath,
                    //            UploadTime = contractAttachments.UploadTime,
                    //            IsDelete = contractAttachments.IsDelete,
                    //            Version = version,
                    //            CreateTime = DateTime.Now
                    //        };
                    //        _attachmentsHistroyRepository.Add(history);
                    //    });
                    //    dbContext.SaveChanges();
                    //}
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }
    }
}
