/*
 *接口编写处...
*如果接口需要做Action的权限验证，请在Action上使用属性
*如: [ApiActionPermission("Sys_Calendar",Enums.ActionPermissionOptions.Search)]
 */
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using BCS.Entity.DomainModels;
using BCS.Business.IServices;
using BCS.Business.Services;
using BCS.Entity.DTO.Project;

namespace BCS.WebApi.Controllers.Project
{
    public partial class Sys_CalendarController
    {
        private readonly ISys_CalendarService _service;//访问业务代码
        private readonly IHttpContextAccessor _httpContextAccessor;

        [ActivatorUtilitiesConstructor]
        public Sys_CalendarController(
            ISys_CalendarService service,
            IHttpContextAccessor httpContextAccessor
        )
        : base(service)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 初始化系统日历
        /// </summary>
        /// <param name="holiday_SystemIdS">假期系统类型ID 字符串</param>
        /// <param name="year">年份</param>
        /// <returns></returns>
        [HttpPost, Route("InitSysCalendar/{holiday_SystemIdS}/{year}")]
        public async Task<IActionResult> InitSysCalendar([FromRoute] string holiday_SystemIdS, [FromRoute] int year)
        {
            return Json(await _service.InitSysCalendar(holiday_SystemIdS, year));
        }

        /// <summary>
        /// 获取系统日历
        /// </summary>
        /// <param name="holiday_SystemId">假期系统类型ID</param>
        /// <param name="year">年份</param>
        /// <returns></returns>
        [HttpGet, Route("GetSysCalendar")]
        public async Task<IActionResult> GetSysCalendar(int holiday_SystemId, int year)
        {
            return Json(await _service.GetSysCalendar(holiday_SystemId, year));
        }

        /// <summary>
        /// 保存或者更新系统日历
        /// </summary>
        /// <param name="sys_CalendarDTO"></param>
        /// <returns></returns>
        [HttpPost, Route("SaveOrUpdate")]
        public async Task<IActionResult> SaveOrUpdate([FromBody] Sys_CalendarDTO sys_CalendarDTO)
        {
            return Json(await _service.SaveOrUpdate(sys_CalendarDTO));
        }
    }
}
