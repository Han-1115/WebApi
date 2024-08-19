using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BCS.Core.Controllers.Basic;
using BCS.Core.Enums;
using BCS.Core.Filters;
using BCS.Entity.DomainModels;
using BCS.Business.IServices;
using Microsoft.AspNetCore.Authorization;

namespace BCS.System.Controllers
{
    [Route("api/menu")]
    [ApiController, JWTAuthorize()]
    public partial class Sys_MenuController : ApiBaseController<ISys_MenuService>
    {
        private ISys_MenuService _service { get; set; }
        public Sys_MenuController(ISys_MenuService service) :
            base("System", "System", "Sys_Menu", service)
        {
            _service = service;
        } 
    }
}
