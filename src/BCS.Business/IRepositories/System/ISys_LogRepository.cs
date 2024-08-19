using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCS.Core.BaseProvider;
using BCS.Entity.DomainModels;
using BCS.Core.Extensions.AutofacManager;
namespace BCS.Business.IRepositories
{
    public partial interface ISys_LogRepository : IDependency,IRepository<Sys_Log>
    {
    }
}

