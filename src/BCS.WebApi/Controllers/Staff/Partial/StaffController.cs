/*
 *接口编写处...
*如果接口需要做Action的权限验证，请在Action上使用属性
*如: [ApiActionPermission("Staff",Enums.ActionPermissionOptions.Search)]
 */
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using BCS.Entity.DomainModels;
using BCS.Business.IServices;
using BCS.Core.Controllers.Basic;
using BCS.Core.Enums;
using BCS.Core.Extensions;
using BCS.Core.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;

namespace BCS.WebApi.Controllers.Staff
{
    public partial class StaffController
    {
        private readonly IStaffService _service;//访问业务代码
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _hostingEnvironment;
        [ActivatorUtilitiesConstructor]
        public StaffController(
            IStaffService service,
            IHttpContextAccessor httpContextAccessor
,
            IWebHostEnvironment hostingEnvironment)
        : base(service)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// 获取员工列表(新增自有人员弹窗用)
        /// </summary>
        /// <param name="loadData">分页参数</param>
        /// <returns></returns>
        [HttpPost, Route("GetStaffData")]
        public ActionResult GetStaffData([FromBody] PageDataOptions loadData) => Json(_service.GetStaffData(loadData));

        /// <summary>
        /// 人员查询列表
        /// </summary>
        /// <param name="loadData">分页参数</param>
        /// <returns></returns>
        [HttpPost, Route("GetPagerList")]
        public ActionResult GetPagerList([FromBody] PageDataOptions loadData) => Json(_service.GetPagerList(loadData));


        /// <summary>
        /// 导出人员列表
        /// </summary>
        /// <param name="pageDataOptions"></param>
        /// <returns></returns>
        [HttpPost, Route("ExportPagerList")]
        public async Task<IActionResult> ExportPagerList([FromBody] PageDataOptions pageDataOptions) => Json(await _service.ExportPagerList(pageDataOptions, _hostingEnvironment.ContentRootPath));

        /// <summary>
        /// 获取金蝶组织架构数据
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("GetAdminOrg")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAdminOrgDataAsync()
        {
            return Json(await _service.GetAdminOrgDataAsync());
        }

        /// <summary>
        /// 同步金蝶员工数据
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("SyncStaffFromKingdee")]
        [AllowAnonymous]
        public async Task<IActionResult> SyncStaffFromKingdee()
        {
            return Json(await _service.SynchronizeStaff());
        }
    }
}
