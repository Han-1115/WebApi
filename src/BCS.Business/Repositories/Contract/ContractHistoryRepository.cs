using BCS.Core.BaseProvider;
using BCS.Core.EFDbContext;
using BCS.Core.Extensions.AutofacManager;
using BCS.Entity.DomainModels;
using BCS.Business.IRepositories;

namespace BCS.Business.Repositories
{
    public partial class ContractHistoryRepository : RepositoryBase<ContractHistory>, IContractHistoryRepository
    {
        public ContractHistoryRepository(BCSContext dbContext)
           : base(dbContext)
        {

        }
        public static IContractHistoryRepository Instance
        {
            get { return AutofacContainerModule.GetService<IContractHistoryRepository>(); }
        }
    }
}
