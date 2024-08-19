/*
 *接口编写处...
*如果接口需要做Action的权限验证，请在Action上使用属性
*如: [ApiActionPermission("StaffProject",Enums.ActionPermissionOptions.Search)]
 */
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using BCS.Entity.DomainModels;
using BCS.Business.IServices;
using BCS.Core.Filters;
using BCS.Entity.DTO.Staff;
using BCS.Core.Enums;
using BCS.Core.Utilities;
using Microsoft.AspNetCore.Hosting;
using System.Net.Mime;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace BCS.WebApi.Controllers.Staff
{
    public partial class StaffProjectController
    {
        private readonly IStaffProjectService _service;//访问业务代码
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _hostingEnvironment;

        [ActivatorUtilitiesConstructor]
        public StaffProjectController(
            IStaffProjectService service,
            IHttpContextAccessor httpContextAccessor,
            IWebHostEnvironment hostingEnvironment = null
        )
        : base(service)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// 下载导入Excel模板
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("DownLoadStaffProjectTemplate")]
        [ApiExplorerSettings(IgnoreApi = false)]
        public async Task<ActionResult> DownLoadStaffProjectTemplate([FromQuery] int projectId)
        {
            return Json(await _service.DownLoadTemplate<StaffProjectDetails>((c) => new { c.StaffNo, c.StaffName, c.ChargeRate, c.InputStartDate, c.InputEndDate }, _hostingEnvironment.ContentRootPath, projectId));
        }

        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="fileInput"></param>
        /// <param name="StaffProjecId"></param>
        /// <returns></returns>
        [HttpPost, Route("ImportStaffProject")]
        [ApiExplorerSettings(IgnoreApi = false)]
        public ActionResult ImportStaffProject(List<IFormFile> fileInput)
        {
            return Json(_service.Import<StaffProjectDetails>(fileInput, _service.CompleteAllField, c => new { c.StaffNo, c.StaffName, c.ChargeRate, c.InputStartDate, c.InputEndDate }));
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="fileInput"></param>
        /// <returns></returns>
        [HttpPost, Route("ExportStaffProjectDetail")]
        [ApiExplorerSettings(IgnoreApi = false)]
        public async Task<IActionResult> ExportStaffProjectDetail([FromBody] PageDataOptions pageDataOptions) => Json(await _service.ExportStaffProjectDetail(pageDataOptions, _hostingEnvironment.ContentRootPath));

        /// <summary>
        ///人员进出项查询列表
        /// </summary>
        /// <param name="pageDataOptions">分页参数</param>
        /// <returns></returns>
        [HttpPost, Route("GetPagerList")]
        public IActionResult GetPagerList([FromBody] PageDataOptions pageDataOptions) => Json(_service.GetPagerList(pageDataOptions));

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="fileInput"></param>
        /// <returns></returns>
        [HttpPost, Route("ExportStaffPagerList")]
        public async Task<IActionResult> ExportStaffPagerList([FromBody] PageDataOptions pageDataOptions) => Json(await _service.ExportPagerList(pageDataOptions, _hostingEnvironment.ContentRootPath));

        [HttpGet, Route("ProjectDetailsById")]
        public IActionResult GetProjectDetailsById([FromQuery] int projectId)
        {
            return Json(_service.GetProjectDetailsById(projectId));
        }

        [HttpGet, Route("Edit")]
        public IActionResult Edit([FromQuery] int projectId)
        {
            return Json(_service.Edit(projectId));
        }

        [HttpPost, Route("StaffProjetDetails")]
        public IActionResult GetStaffProjetDetails([FromBody] PageDataOptions pageDataOptions)
        {
            return Json(_service.GetStaffProjetDetails(pageDataOptions));
        }

        [HttpPost, Route("Save")]
        public IActionResult Save(int projectId, [FromBody] List<StaffProjectDetailsV2> staffProjectDetails)
            => Json(_service.Save(projectId, staffProjectDetails));


        [HttpPost, Route("Submit")]
        public IActionResult Submit(int projectId, [FromBody] List<StaffProjectDetailsV2> staffProjectDetails)
            => Json(_service.Submit(projectId, staffProjectDetails));


        [HttpGet, Route("StaffProjetPutDetails")]
        public IActionResult GetStaffProjetPutDetails([FromQuery] int StaffId)
        {
            return Json(_service.GetstaffProjectPutInfo(StaffId));
        }

        /// <summary>
        /// 特殊项目人员进出项
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("StaffEntryExistOtherProject")]
        [AllowAnonymous]
        public async Task<IActionResult> StaffEntryExistOtherProject()
        {
            return Json(await _service.StaffEntryExistOtherProject());
        }

        /// <summary>
        /// 校验某人是否和其他项目有时间冲突
        /// </summary>
        /// <param name="staffProjectVerification"></param>
        /// <returns></returns>
        [HttpPost, Route("CheckStaffProjet")]
        public IActionResult CheckStaffProjet([FromBody] StaffProjectVerification staffProjectVerification)
            => Json(_service.CheckStaffProjet(staffProjectVerification));

        /// <summary>
        /// 查询人员ChargeRate变动记录
        /// </summary>
        /// <param name="pageDataOptions"></param>
        /// <returns></returns>
        [HttpPost, Route("GetStaffRecordChanges")]
        public IActionResult GetStaffChargeRateChanges([FromBody] PageDataOptions pageDataOptions)
        {
            return Json(_service.QueryStaffChargeRateChanges(pageDataOptions));
        }

        /// <summary>
        /// 导出人员ChargeRate变动记录
        /// </summary>
        /// <param name="fileInput"></param>
        /// <returns></returns>
        [HttpPost, Route("ExportStaffChargeRateChanges")]
        public async Task<IActionResult> ExportStaffChargeRateChanges([FromBody] PageDataOptions pageDataOptions) => Json(await _service.ExportStaffChargeRateChanges(pageDataOptions, _hostingEnvironment.ContentRootPath));


    }
}
