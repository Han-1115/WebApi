/*
 *接口编写处...
*如果接口需要做Action的权限验证，请在Action上使用属性
*如: [ApiActionPermission("StaffAttendance",Enums.ActionPermissionOptions.Search)]
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
using Microsoft.AspNetCore.Hosting;

namespace BCS.Business.Controllers
{
    public partial class StaffAttendanceController
    {
        private readonly IStaffAttendanceService _service;//访问业务代码
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _hostingEnvironment;
        [ActivatorUtilitiesConstructor]
        public StaffAttendanceController(
            IStaffAttendanceService service,
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
        ///同步本月明细考勤数据
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("SynchronizeCurrentMonthAttendance")]
        [AllowAnonymous]
        public IActionResult SynchronizeCurrentMonthAttendance() => Json(_service.SynchronizeCurrentMonthAttendance());

        /// <summary>
        ///同步上月明细考勤数据
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("SynchronizeLastMonthAttendance")]
        [AllowAnonymous]
        public IActionResult SynchronizeLastMonthAttendance() => Json(_service.SynchronizeLastMonthAttendance());
        /// <summary>
        /// 考勤看板列表分页查询
        /// </summary>
        /// <param name="pageDataOptions"></param>
        /// <returns></returns>
        [HttpPost, Route("DashboardPagerList")]
        public IActionResult GetStaffAttendanceDashboardPagerList([FromBody] PageDataOptions pageDataOptions) => Json(_service.GetStaffAttendanceDashboardPagerList(pageDataOptions));

        /// <summary>
        /// 导出员工考勤列表
        /// </summary>
        /// <param name="pageDataOptions"></param>
        /// <returns></returns>
        [HttpPost, Route("ExportStaffAttendanceDashboardPagerList")]
        public async Task<IActionResult> ExportStaffAttendanceDashboardPagerList([FromBody] PageDataOptions pageDataOptions) => Json(await _service.ExportStaffAttendanceDashboardPagerList(pageDataOptions, _hostingEnvironment.ContentRootPath));

        /// <summary>
        /// 考勤汇总列表分页查询
        /// </summary>
        /// <param name="pageDataOptions"></param>
        /// <returns></returns>
        [HttpPost, Route("SummaryPagerList")]
        public IActionResult GetStaffAttendanceSummaryPagerList([FromBody] PageDataOptions pageDataOptions) => Json(_service.GetStaffAttendanceSummaryPagerList(pageDataOptions));

        /// <summary>
        /// 导出员工考勤汇总列表
        /// </summary>
        /// <param name="pageDataOptions"></param>
        /// <returns></returns>
        [HttpPost, Route("ExportStaffAttendanceSummaryPagerList")]
        public async Task<IActionResult> ExportStaffAttendanceSummaryPagerList([FromBody] PageDataOptions pageDataOptions) => Json(await _service.ExportStaffAttendanceSummaryPagerList(pageDataOptions, _hostingEnvironment.ContentRootPath));

        /// <summary>
        /// 人力成本表分页查询
        /// <para>期间：YearMonth required, 传递年月 比如: 202403</para>
        /// <para>部门：DepartmentId optional, equal 查询。目前筛选条件选择到叶子结点</para>
        /// <para>项目编码：Project_Code optional, 模糊查询</para>
        /// <para>项目名称：Project_Name optional, 模糊查询</para>
        /// <para>人员工号：StaffNo optional, 模糊查询</para>
        /// <para>人员姓名：StaffName optional, 模糊查询</para>
        /// </summary>
        /// <param name="pageDataOptions"></param>
        /// <returns></returns>
        [HttpPost, Route("GetStaffCostSummaryPagerList")]
        public IActionResult GetStaffCostSummaryPagerList([FromBody] PageDataOptions pageDataOptions) => Json(_service.GetStaffCostSummaryPagerList(pageDataOptions));

        /// <summary>
        /// 导出人力成本表
        /// <para>期间：YearMonth required, 传递年月 比如: 202403</para>
        /// <para>部门：DepartmentId optional, equal 查询。目前筛选条件选择到叶子结点</para>
        /// <para>项目编码：Project_Code optional, 模糊查询</para>
        /// <para>项目名称：Project_Name optional, 模糊查询</para>
        /// <para>人员工号：StaffNo optional, 模糊查询</para>
        /// <para>人员姓名：StaffName optional, 模糊查询</para>
        /// </summary>
        /// <param name="pageDataOptions"></param>
        /// <returns></returns>
        [HttpPost, Route("ExportStaffCostSummaryPagerList")]
        public async Task<IActionResult> ExportStaffCostSummaryPagerList([FromBody] PageDataOptions pageDataOptions) => Json(await _service.ExportStaffCostSummaryPagerList(pageDataOptions, _hostingEnvironment.ContentRootPath));
    }
}
