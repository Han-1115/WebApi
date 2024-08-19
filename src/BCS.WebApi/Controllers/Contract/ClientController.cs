using Autofac.Core;
using BCS.Business.IServices;
using BCS.Core.Controllers.Basic;
using BCS.Core.Filters;
using BCS.Business.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BCS.WebApi.Controllers
{
    /// <summary>
    /// 客户实体接口
    /// </summary>
    [JWTAuthorize]
    [Route("/api/Client")]
    public partial class ClientController : Controller
    {
        private readonly IClientService _service;

        public ClientController(IClientService service)
        {
            _service = service;
        }

        /// <summary>
        /// 获取所有客户实体（用于绑定下拉框）
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpGet, Route("GetClientList")]
        public IActionResult GetClientList() => Json(_service.GetClientList());
    }
}
