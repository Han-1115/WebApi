using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCS.Core.BaseProvider;
using BCS.Entity.DomainModels;
using BCS.Core.Extensions.AutofacManager;
namespace BCS.Builder.IRepositories
{
    public partial interface ISys_TableInfoRepository : IDependency,IRepository<Sys_TableInfo>
    {
    }
}

