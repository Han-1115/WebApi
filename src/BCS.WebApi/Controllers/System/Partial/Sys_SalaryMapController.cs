/*
 *接口编写处...
*如果接口需要做Action的权限验证，请在Action上使用属性
*如: [ApiActionPermission("Sys_SalaryMap",Enums.ActionPermissionOptions.Search)]
 */
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using BCS.Entity.DomainModels;
using BCS.Business.IServices;
using BCS.Core.Utilities;
using BCS.Entity.DTO.System;

namespace BCS.WebApi.Controllers
{
    public partial class Sys_SalaryMapController
    {
        private readonly ISys_SalaryMapService _service;//访问业务代码
        private readonly IHttpContextAccessor _httpContextAccessor;

        [ActivatorUtilitiesConstructor]
        public Sys_SalaryMapController(
            ISys_SalaryMapService service,
            IHttpContextAccessor httpContextAccessor
        )
        : base(service)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 根据职位、城市、级别获取薪资
        /// </summary>
        /// <param name="positionId"></param>
        /// <param name="cityId"></param>
        /// <param name="levelId"></param>
        /// <returns></returns> 
        [HttpGet, Route("GetSysSalaryMap")]
        public async Task<IActionResult> GetSysSalaryMap(int positionId, int cityId, int levelId)
        {
            return Json(await Service.GetSysSalaryMap(positionId, cityId, levelId));
        }

        /// <summary>
        ///查询分页列表
        /// </summary>
        /// <param name="pageDataOptions">分页参数</param>
        /// <returns></returns>
        [HttpPost, Route("GetPagerList")]
        public IActionResult GetPagerList([FromBody] PageDataOptions pageDataOptions) => Json(_service.GetPagerList(pageDataOptions));

        /// <summary>
        /// 更新或者新增
        /// </summary>
        /// <param name="sys_SalaryMapDTO">更新实体</param>
        /// <returns></returns>
        [HttpPost, Route("SaveOrUpdate")]
        public async Task<IActionResult> SaveOrUpdate([FromBody] Sys_SalaryMapDTO sys_SalaryMapDTO)
        {
            return Json(await Service.SaveOrUpdate(sys_SalaryMapDTO));
        }
    }
}
