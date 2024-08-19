using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using BCS.Core.Controllers.Basic;
using BCS.Core.Enums;
using BCS.Core.Filters;
using BCS.Entity.AttributeManager;
using BCS.Entity.DomainModels;
using BCS.Business.IServices;

namespace BCS.System.Controllers
{
    [Route("api/Sys_Role")]
    [PermissionTable(Name = "Sys_Role")]
    public partial class Sys_RoleController : ApiBaseController<ISys_RoleService>
    {
        public Sys_RoleController(ISys_RoleService service)
        : base("System", "System", "Sys_Role", service)
        {

        }
    }
}


