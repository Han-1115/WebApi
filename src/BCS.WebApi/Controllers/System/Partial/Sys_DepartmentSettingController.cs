/*
 *接口编写处...
*如果接口需要做Action的权限验证，请在Action上使用属性
*如: [ApiActionPermission("Sys_DepartmentSetting",Enums.ActionPermissionOptions.Search)]
 */
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using BCS.Entity.DomainModels;
using BCS.Business.IServices;
using BCS.Entity.DTO.System;

namespace BCS.WebApi.Controllers
{
    public partial class Sys_DepartmentSettingController
    {
        private readonly ISys_DepartmentSettingService _service;//访问业务代码
        private readonly IHttpContextAccessor _httpContextAccessor;

        [ActivatorUtilitiesConstructor]
        public Sys_DepartmentSettingController(
            ISys_DepartmentSettingService service,
            IHttpContextAccessor httpContextAccessor
        )
        : base(service)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 获取部门设置
        /// </summary>
        /// <param name="departmentId">部门ID</param>
        /// <param name="year">年份</param>
        /// <returns></returns>
        [HttpGet, Route("Query")]
        public async Task<IActionResult> Query(Guid departmentId, string year)
        {
            return Json(await Service.Query(departmentId, year));
        }

        /// <summary>
        /// 更新或者新增
        /// </summary>
        /// <param name="sys_DepartmentSettingDTO">更新实体</param>
        /// <returns></returns>
        [HttpPost, Route("SaveOrUpdate")]
        public async Task<IActionResult> SaveOrUpdate([FromBody] Sys_DepartmentSettingDTO sys_DepartmentSettingDTO)
        {
            return Json(await Service.SaveOrUpdate(sys_DepartmentSettingDTO));
        }
    }
}
