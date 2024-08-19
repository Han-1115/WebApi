using BCS.Business.IRepositories;
using BCS.Business.IServices;
using BCS.Core.BaseProvider;
using BCS.Core.Extensions.AutofacManager;
using BCS.Core.Utilities;
using BCS.Entity.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCS.Business.Services
{
    public partial class ContractProjectHistoryService : ServiceBase<ContractProjectHistory, IContractProjectHistoryRepository>
      , IContractProjectHistoryService, IDependency
    {
        private WebResponseContent Response { get; set; }

        public ContractProjectHistoryService(IContractProjectHistoryRepository repository)
        : base(repository)
        {
            Init(repository);
            Response = new WebResponseContent(true);
        }
        public static IContractProjectHistoryRepository Instance
        {
            get { return AutofacContainerModule.GetService<IContractProjectHistoryRepository>(); }
        }

        public bool AddHistory(ContractProject contractProject, int version)
        {
            if (contractProject == null || contractProject.Id == 0) return false;

            repository.DbContextBeginTransaction(() =>
            {
                var history = new ContractProjectHistory
                {
                    Contract_Id = contractProject.Contract_Id,
                    Project_Id = contractProject.Project_Id,
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
