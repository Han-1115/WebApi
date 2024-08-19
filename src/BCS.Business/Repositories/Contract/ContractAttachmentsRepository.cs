using BCS.Business.IRepositories;
using BCS.Core.BaseProvider;
using BCS.Core.EFDbContext;
using BCS.Core.Extensions.AutofacManager;
using BCS.Entity.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCS.Business.Repositories
{
    public partial class ContractAttachmentsRepository : RepositoryBase<ContractAttachments>, IContractAttachmentsRepository
    {
        public ContractAttachmentsRepository(BCSContext dbContext)
           : base(dbContext)
        {

        }
        public static IContractAttachmentsRepository Instance
        {
            get { return AutofacContainerModule.GetService<ContractAttachmentsRepository>(); }
        }
    }
}
