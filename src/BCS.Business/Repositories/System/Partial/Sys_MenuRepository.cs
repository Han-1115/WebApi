using BCS.Business.IRepositories;
using BCS.Core.BaseProvider;
using BCS.Core.Extensions.AutofacManager;
using BCS.Core.EFDbContext;
using BCS.Entity.DomainModels;

namespace BCS.Business.Repositories
{
    public partial class Sys_MenuRepository
    {
        public override BCSContext DbContext => base.DbContext;
    }
}

