
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;
using BCS.Core.Controllers.Basic;
using BCS.Entity.AttributeManager;
using BCS.Entity.DomainModels;
using BCS.Business.IServices;

namespace BCS.System.Controllers
{
    [Route("api/Sys_User")]
    [PermissionTable(Name = "Sys_User")]
    public partial class Sys_UserController : ApiBaseController<ISys_UserService>
    {
        public Sys_UserController(ISys_UserService service)
        : base("System", "System", "Sys_User", service)
        {
            //, IMemoryCache cache
        }
    }
}
