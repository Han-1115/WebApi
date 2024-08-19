using BCS.Business.IRepositories;
using BCS.Business.IServices;
using BCS.Core.BaseProvider;
using BCS.Core.Extensions.AutofacManager;
using BCS.Core.Utilities;
using BCS.Entity.DomainModels;
using BCS.Entity.DTO.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCS.Business.Services
{
    public partial class ContractAttachmentsHistoryService : ServiceBase<ContractAttachmentsHistory, IContractAttachmentsHistoryRepository>
    , IContractAttachmentsHistoryService, IDependency
    {
        private WebResponseContent Response { get; set; }
        public ContractAttachmentsHistoryService(IContractAttachmentsHistoryRepository repository)
        : base(repository)
        {
            Init(repository);
            Response = new WebResponseContent(true);
        }
        public static IContractAttachmentsHistoryService Instance
        {
            get { return AutofacContainerModule.GetService<IContractAttachmentsHistoryService>(); }
        }

        public bool AddHistory(BCS.Entity.DomainModels.ContractAttachments contractAttachments, int version)
        {
            if (contractAttachments == null || contractAttachments.Id == 0) return false;

            repository.DbContextBeginTransaction(() =>
            {
                var history = new ContractAttachmentsHistory
                {
                    ContractAttachments_Id = contractAttachments.Id,
                    Contract_Id = contractAttachments.Contract_Id,
                    FileName = contractAttachments.FileName,
                    FilePath = contractAttachments.FilePath,
                    UploadTime = contractAttachments.UploadTime,
                    IsDelete = contractAttachments.IsDelete,
                    Version = version,
                    CreateTime = DateTime.Now
                };
                repository.Add(history);
                repository.DbContext.SaveChanges();
                return Response.OK();
            });
            return true;
        }
    }
}
