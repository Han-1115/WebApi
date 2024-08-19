/*
 *接口编写处...
*如果接口需要做Action的权限验证，请在Action上使用属性
*如: [ApiActionPermission("SubContractFlow",Enums.ActionPermissionOptions.Search)]
 */
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using BCS.Entity.DomainModels;
using BCS.Business.IServices;
using BCS.Core.Enums;
using BCS.Core.Utilities;
using BCS.Entity.DTO.Contract;
using Microsoft.AspNetCore.Authorization;
using System.Text.RegularExpressions;
using BCS.Entity.DTO.SubcontractingContract;
using BCS.Entity.DTO.Flow;
using BCS.Core.Extensions;

namespace BCS.Business.Controllers
{
    public partial class SubContractFlowController
    {
        private readonly ISubContractFlowService _service;//访问业务代码
        private readonly IHttpContextAccessor _httpContextAccessor;

        [ActivatorUtilitiesConstructor]
        public SubContractFlowController(
            ISubContractFlowService service,
            IHttpContextAccessor httpContextAccessor
        )
        : base(service)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 根据ID获取分包合同流程详情
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [HttpGet, Route("GetSubContractFlow")]
        public async Task<IActionResult> GetSubContractFlow([FromQuery] int id)
        {
            return Json(await _service.GetSubContractFlow(id));
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="SubContractFlowSaveModel"></param>
        /// <returns></returns>
        [HttpPost, Route("Save")]
        public async Task<IActionResult> Save([FromForm] SubContractFlowSaveModel model)
        {
            var files = Request.Form.Files;
            foreach (var file in files)
            {
                string input = file.Name;
                string pattern = @"\[(\d+)\]";

                Match match = Regex.Match(input, pattern);

                string indexStr = match.Groups[1].Value;
                int index = int.Parse(indexStr);
                model.SubContractFlowAttachmentList[index].File = file;
            }

            return Json(await _service.UpdateSubContract(model, (byte)WorkflowActions.Edit));
        }

        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="SubContractFlowSaveModel"></param>
        /// <returns></returns>
        [HttpPost, Route("Submit")]
        public async Task<IActionResult> Submit([FromForm] SubContractFlowSaveModel model)
        {
            var files = Request.Form.Files;
            foreach (var file in files)
            {
                string input = file.Name;
                string pattern = @"\[(\d+)\]";

                Match match = Regex.Match(input, pattern);

                string indexStr = match.Groups[1].Value;
                int index = int.Parse(indexStr);
                model.SubContractFlowAttachmentList[index].File = file;
            }

            return Json(await _service.UpdateSubContract(model, (byte)WorkflowActions.Submit));
        }

        /// <summary>
        /// 保存人员备案
        /// </summary>
        /// <param name="ContractSaveModel"></param>
        /// <returns></returns>
        [HttpPost, Route("SaveStaff")]
        public async Task<IActionResult> SaveStaff([FromForm] SubContractFlowSaveModel model)
        {
            WebResponseContent responseContent = new WebResponseContent();
            var updatedContractId = await _service.UpdateSubContractStaff(model, (byte)WorkflowActions.Edit);

            if (updatedContractId == 0) return Json(responseContent.Error("Failed to save personnel registration!"));

            return Json(new WebResponseContent
            {
                Code = "200",
                Data = null,
                Message = "保存成功",
                Status = true,
            });
        }

        /// <summary>
        /// 提交人员备案
        /// </summary>
        /// <param name="ContractSaveModel"></param>
        /// <returns></returns>
        [HttpPost, Route("SubmitStaff")]
        public async Task<IActionResult> SubmitStaff([FromForm] SubContractFlowSaveModel model)
        {
            WebResponseContent responseContent = new WebResponseContent();
            var updatedContractId = await _service.UpdateSubContractStaff(model, (byte)WorkflowActions.Submit);

            if (updatedContractId == 0) return Json(responseContent.Error("Failed to submit personnel registration!"));

            return Json(new WebResponseContent
            {
                Code = "200",
                Data = null,
                Message = "提交成功",
                Status = true,
            });
        }

        /// <summary>
        /// 审核
        /// <para><paramref name="keys"/> 业务主键,多条记录用','号分隔</para>
        /// <para><paramref name="auditStatus"/> 审批状态 (1:审批通过,2:审批驳回,3:审批中 ,4:未发起 ,5:已撤回)</para>
        /// <para><paramref name="auditReason"/> 审批意见</para>
        /// </summary>
        //[ApiActionPermission(Enums.ActionPermissionOptions.Audit)]
        [HttpPost, Route("Audit")]
        [ApiExplorerSettings(IgnoreApi = false)]
        public override ActionResult Audit([FromBody] WorkFlowAuditDTO workFlowAudit)
        {
            return base.Audit(workFlowAudit);
        }

        /// <summary>
        /// 撤回
        /// </summary>
        /// <param name="subContractFlowId"></param>
        /// <returns></returns>
        [HttpPost, Route("Recall/{subContractFlowId}")]
        public async Task<IActionResult> Recall([FromRoute] int subContractFlowId) => Json(await _service.ReCall(subContractFlowId));

        /// <summary>
        /// 关闭分包合同
        /// </summary>
        /// <param name="subContractFlowId"></param>
        /// <returns></returns>
        [HttpPost, Route("Close/{subContractFlowId}")]
        public async Task<IActionResult> Close([FromRoute] int subContractFlowId) => Json(await _service.Close(subContractFlowId));


        /// <summary>
        /// 删除草稿
        /// </summary>
        /// <param name="subContractFlowId"></param>
        /// <returns></returns>
        [HttpPost, Route("DeleteDraft/{subContractFlowId}")]
        public async Task<IActionResult> DeleteDraft([FromRoute] int subContractFlowId) => Json(await _service.DeleteDraft(subContractFlowId));

        /// <summary>
        /// 审核staff
        /// <para><paramref name="keys"/> 业务主键,多条记录用','号分隔</para>
        /// <para><paramref name="auditStatus"/> 审批状态 (1:审批通过,2:审批驳回,3:审批中 ,4:未发起 ,5:已撤回)</para>
        /// <para><paramref name="auditReason"/> 审批意见</para>
        /// </summary>
        //[ApiActionPermission(Enums.ActionPermissionOptions.Audit)]
        [HttpPost, Route("AuditStaff")]
        [ApiExplorerSettings(IgnoreApi = false)]
        public ActionResult AuditStaff([FromBody] WorkFlowAuditDTO workFlowAudit)
        {
            return Json(_service.AuditStaff(workFlowAudit));
        }

        /// <summary>
        /// 关闭分包人员
        /// </summary>
        /// <param name="subContractFlowId"></param>
        /// <returns></returns>
        [HttpPost, Route("CloseStaff/{subContractFlowId}")]
        public async Task<IActionResult> CloseStaff([FromRoute] int subContractFlowId) => Json(await _service.CloseStaff(subContractFlowId));

        /// <summary>
        /// 撤回staff
        /// </summary>
        /// <param name="subContractFlowId"></param>
        /// <returns></returns>
        [HttpPost, Route("RecallStaff/{subContractFlowId}")]
        public async Task<IActionResult> RecallStaff([FromRoute] int subContractFlowId) => Json(await _service.ReCallStaff(subContractFlowId));


        /// <summary>
        /// 删除staff草稿
        /// </summary>
        /// <param name="subContractFlowId"></param>
        /// <returns></returns>
        [HttpPost, Route("DeleteStaffDraft/{subContractFlowId}")]
        public async Task<IActionResult> DeleteStaffDraft([FromRoute] int subContractFlowId) => Json(await _service.DeleteStaffDraft(subContractFlowId));


        /// <summary>
        /// 分包合同历史记录列表
        /// </summary>
        /// <param name="SubContractId"></param>
        /// <returns></returns>
        [HttpGet, Route("GetSubcontractHistory")]
        public IActionResult GetSubcontractHistory([FromQuery] int SubcontractId)
        {
            return Json(_service.GetSubcontractHistory(SubcontractId));
        }

        /// <summary>
        /// 分包合同人员变更历史记录列表
        /// </summary>
        /// <param name="SubContractId"></param>
        /// <returns></returns>
        [HttpGet, Route("GetSubcontractHistoryForStaff")]
        public IActionResult GetSubcontractHistoryForStaff([FromQuery] int SubcontractId)
        {
            return Json(_service.GetSubcontractHistoryForStaff(SubcontractId));
        }

        [HttpGet, Route("SubcontractHistoryDetails")]
        public IActionResult SubcontractHistoryDetails([FromQuery] int SubContractFlowId)
        {
            return Json(_service.SubcontractHistoryDetails(SubContractFlowId));
        }
    }
}
