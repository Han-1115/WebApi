/*
 *接口编写处...
*如果接口需要做Action的权限验证，请在Action上使用属性
*如: [ApiActionPermission("SubcontractingContract",Enums.ActionPermissionOptions.Search)]
 */
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using BCS.Entity.DomainModels;
using BCS.Business.IServices;
using Autofac.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;

namespace BCS.Business.Controllers
{
    public partial class SubcontractingContractController
    {
        private readonly ISubcontractingContractService _service;//访问业务代码
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _hostingEnvironment;

        [ActivatorUtilitiesConstructor]
        public SubcontractingContractController(
            ISubcontractingContractService service,
            IHttpContextAccessor httpContextAccessor,
            IWebHostEnvironment hostingEnvironment = null
        )
        : base(service)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost,Route("GetSubcontractsList")]
        public IActionResult GetSubcontractsList([FromBody]PageDataOptions pageDataOptions)
        {
            return Json(_service.GetSubcontractsList(pageDataOptions));
        }

        /// <summary>
        /// 根据ID获取分包合同详情
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [HttpGet, Route("GetSubContractDetail")]
        public async Task<IActionResult> GetSubContractDetail([FromQuery] int id)
        {
            return Json(await _service.GetSubContractDetail(id));
        }

        /// <summary>
        /// 根据ID获取分包人员详情
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [HttpGet, Route("GetSubContractStaffDetail")]
        public async Task<IActionResult> GetSubContractStaffDetail([FromQuery] int id)
        {
            return Json(await _service.GetSubContractStaffDetail(id));
        }

        [HttpPost, Route("GetSubcontractsListForRegist")]
        public IActionResult GetSubcontractsListForRegist([FromBody] PageDataOptions pageDataOptions)
        {
            return Json(_service.GetSubcontractsListForRegist(pageDataOptions));
        }

        [HttpPost, Route("GetSubcontractsListForStaffRegist")]
        public IActionResult GetSubcontractsListForStaffRegist([FromBody] PageDataOptions pageDataOptions)
        {
            return Json(_service.GetSubcontractsListForStaffRegist(pageDataOptions));
        }

        [HttpPost, Route("ExportSubcontractsList")]
        public async Task<IActionResult> ExportSubcontractsList([FromBody] PageDataOptions pageDataOptions)
        {
            return Json(await _service.ExportSubcontractsList(pageDataOptions, _hostingEnvironment.ContentRootPath));
        }
    }
}
