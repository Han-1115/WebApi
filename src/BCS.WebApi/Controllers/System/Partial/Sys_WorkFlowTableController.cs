/*
 *接口编写处...
*如果接口需要做Action的权限验证，请在Action上使用属性
*如: [ApiActionPermission("Sys_WorkFlowTable",Enums.ActionPermissionOptions.Search)]
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

namespace BCS.System.Controllers
{
    public partial class Sys_WorkFlowTableController
    {
        private readonly ISys_WorkFlowTableService _service;//访问业务代码
        private readonly IHttpContextAccessor _httpContextAccessor;

        [ActivatorUtilitiesConstructor]
        public Sys_WorkFlowTableController(
            ISys_WorkFlowTableService service,
            IHttpContextAccessor httpContextAccessor
        )
        : base(service)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 我的审批，我的提交，审批流程列表
        /// </summary>
        /// <param name="loadData"></param>
        /// <returns></returns>
        [ApiActionPermission()]
        public override ActionResult GetPageData([FromBody] PageDataOptions loadData)
        {
            return Json(_service.GetPageData(loadData));
        }

        /// <summary>
        /// 查询一个审批流程的所有节点详情信息
        /// </summary>
        /// <param name="workFlowTableId">审批流主键Id</param>
        /// <returns>所有节点详情信息列表</returns>
        [HttpGet, Route("GetStepList")]
        public ActionResult GetStepList(Guid workFlowTableId) => Json(_service.GetStepList(workFlowTableId));

        /// <summary>
        /// 根据流程实例ID获取 与之对应的业务数据对比信息,目前涉及以下业务
        /// <para>Contract</para>
        /// <para>Project</para>
        /// </summary>
        /// <param name="workFlowTable_Id">流程实例ID</param>
        /// <returns></returns>
        [HttpGet, Route("GetBusinessCompareInfo")]
        public async Task<IActionResult> GetBusinessCompareInfo(Guid workFlowTable_Id) => Json(await _service.GetBusinessCompareInfo(workFlowTable_Id));
    }
}
