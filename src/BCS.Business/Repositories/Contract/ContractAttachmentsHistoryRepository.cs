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
    public partial class ContractAttachmentsHistoryRepository : RepositoryBase<ContractAttachmentsHistory>, IContractAttachmentsHistoryRepository
    {
        public ContractAttachmentsHistoryRepository(BCSContext dbContext)
           : base(dbContext)
        {

        }
        public static IContractAttachmentsHistoryRepository Instance
        {
            get { return AutofacContainerModule.GetService<ContractAttachmentsHistoryRepository>(); }
        }
    }
}
