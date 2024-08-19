using BCS.Core.BaseProvider;
using BCS.Core.EFDbContext;
using BCS.Core.Extensions.AutofacManager;
using BCS.Entity.DomainModels;
using BCS.Business.IRepositories;

namespace BCS.Business.Repositories
{
    public partial class ContractRepository : RepositoryBase<Contract>, IContractRepository
    {
        public ContractRepository(BCSContext dbContext)
           : base(dbContext)
        {

        }
        public static IContractRepository Instance
        {
            get { return AutofacContainerModule.GetService<IContractRepository>(); }
        }
    }
}
