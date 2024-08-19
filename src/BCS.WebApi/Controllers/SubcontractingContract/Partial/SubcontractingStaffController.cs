/*
 *接口编写处...
*如果接口需要做Action的权限验证，请在Action上使用属性
*如: [ApiActionPermission("SubcontractingStaff",Enums.ActionPermissionOptions.Search)]
 */
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using BCS.Entity.DomainModels;
using BCS.Business.IServices;
using Microsoft.AspNetCore.Authorization;
using System.Net;
using BCS.Core.Utilities;
using Microsoft.AspNetCore.Hosting;

namespace BCS.Business.Controllers
{
    public partial class SubcontractingStaffController
    {
        private readonly ISubcontractingStaffService _service;//访问业务代码
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _hostingEnvironment;

        [ActivatorUtilitiesConstructor]
        public SubcontractingStaffController(
            ISubcontractingStaffService service,
            IHttpContextAccessor httpContextAccessor,
            IWebHostEnvironment hostingEnvironment
        )
        : base(service)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        ///分包人员档案库查询
        /// </summary>
        /// <param name="pageDataOptions">分页参数</param>
        /// <returns></returns>
        [HttpPost, Route("GetSubcontractingStaffPagerList")]
        public IActionResult GetSubcontractingStaffPagerList([FromBody] PageDataOptions pageDataOptions) => Json(_service.GetSubcontractingStaffPagerListService(pageDataOptions));

        /// <summary>
        ///导出分包人员档案库
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("ExportSubcontractingStaffPagerList")]
        public async Task<IActionResult> ExportSubcontractingStaffList([FromBody] PageDataOptions pageDataOptions) => Json(await _service.ExporSubcontractingStafftPagerListServiceAsync(pageDataOptions, _hostingEnvironment.ContentRootPath));

        /// <summary>
        ///导出分包人员档案库
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("GetSubcontractorStaffData")]
        public IActionResult GetSubcontractorStaffData([FromBody]PageDataOptions pageDataOptions) => Json(_service.GetSubcontractorStaffData(pageDataOptions));
    }
}
