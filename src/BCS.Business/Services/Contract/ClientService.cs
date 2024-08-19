using BCS.Business.IRepositories;
using BCS.Business.IServices;
using BCS.Core.BaseProvider;
using BCS.Core.Extensions.AutofacManager;
using BCS.Entity.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCS.Business.Services
{
    public partial class ClientService : ServiceBase<Client, IClientRepository>
     , IClientService, IDependency
    {
        public ClientService(IClientRepository repository)
        : base(repository)
        {
            Init(repository);
        }
        public static IClientRepository Instance
        {
            get { return AutofacContainerModule.GetService<IClientRepository>(); }
        }
    }
}
