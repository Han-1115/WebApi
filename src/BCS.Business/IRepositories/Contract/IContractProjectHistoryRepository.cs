using BCS.Core.BaseProvider;
using BCS.Core.Extensions.AutofacManager;
using BCS.Entity.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCS.Business.IRepositories
{
    public partial interface IContractProjectHistoryRepository : IDependency, IRepository<ContractProjectHistory>
    {
    }
}
