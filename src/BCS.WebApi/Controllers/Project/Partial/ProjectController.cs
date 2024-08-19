/*
 *接口编写处...
*如果接口需要做Action的权限验证，请在Action上使用属性
*如: [ApiActionPermission("Project",Enums.ActionPermissionOptions.Search)]
 */
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using BCS.Entity.DomainModels;
using BCS.Business.IServices;
using BCS.Entity.DTO.Project;
using BCS.Business.Services;
using BCS.Core.Extensions;
using BCS.Core.Filters;
using BCS.Core.Utilities;
using Autofac.Core;
using BCS.Core.BaseProvider;
using BCS.Core.Enums;
using BCS.Entity.DTO;
using System.Text.RegularExpressions;
using System.Linq;
using BCS.Entity.DTO.Contract;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using BCS.Entity.DTO.Flow;

namespace BCS.WebApi.Controllers.Project
{
    public partial class ProjectController
    {
        private readonly IProjectService _service;//访问业务代码
        private readonly IProjectBudgetSummaryService _projectBudgetSummaryService;//访问业务代码
        private readonly IProjectResourceBudgetHCService _projectResourceBudgetHCService;//访问业务代码

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _hostingEnvironment;
        [ActivatorUtilitiesConstructor]
        public ProjectController(
            IProjectService service,
            IProjectBudgetSummaryService projectBudgetSummaryService,
            IProjectResourceBudgetHCService projectResourceBudgetHCService,
            IHttpContextAccessor httpContextAccessor,
            IWebHostEnvironment hostingEnvironment = null)
        : base(service)
        {
            _service = service;
            _projectBudgetSummaryService = projectBudgetSummaryService;
            _projectResourceBudgetHCService = projectResourceBudgetHCService;
            _httpContextAccessor = httpContextAccessor;
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        ///查询项目分页列表
        /// </summary>
        /// <param name="pageDataOptions">分页参数</param>
        /// <returns></returns>
        [HttpPost, Route("GetPagerList")]
        public IActionResult GetPagerList([FromBody] PageDataOptions pageDataOptions) => Json(_service.GetPagerList(pageDataOptions));

        /// <summary>
        ///查询已通过的项目分页列表
        /// </summary>
        /// <param name="pageDataOptions">分页参数</param>
        /// <returns></returns>
        [HttpPost, Route("GetApprovedPagerList")]
        public IActionResult GetApprovedPagerList([FromBody] PageDataOptions pageDataOptions) => Json(_service.GetApprovedPagerList(pageDataOptions));

        /// <summary>
        ///查询项目分页列表 -Stable版
        /// </summary>
        /// <param name="pageDataOptions">分页参数</param>
        /// <returns></returns>
        [HttpPost, Route("GetStablePagerList")]
        public IActionResult GetStablePagerList([FromBody] PageDataOptions pageDataOptions) => Json(_service.GetStablePagerList(pageDataOptions));

        /// <summary>
        ///查询合同项目分页列表
        /// </summary>
        /// <param name="pageDataOptions">分页参数</param>
        /// <returns></returns>
        [HttpPost, Route("GetContractPagerList")]
        public IActionResult GetContractPagerList([FromBody] PageDataOptions pageDataOptions) => Json(_service.GetContractPagerList(pageDataOptions));

        /// <summary>
        ///查询合同项目分页列表 -Stable版
        /// </summary>
        /// <param name="pageDataOptions">分页参数</param>
        /// <returns></returns>
        [HttpPost, Route("GetStableContractPagerList")]
        public IActionResult GetStableContractPagerList([FromBody] PageDataOptions pageDataOptions) => Json(_service.GetStableContractPagerList(pageDataOptions));

        /// <summary>
        /// 根据项目ID获取项目详情(包括项目合同和客户信息)
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [HttpGet, Route("GetProjectDetail")]
        public async Task<IActionResult> GetProjectDetail([FromQuery] int projectId)
        {
            return Json(await _service.GetProjectDetail(projectId));
        }


        /// <summary>
        /// 根据项目ID获取项目历史详情(包括项目合同和客户信息)
        /// </summary>
        /// <param name="projectHistoryId"></param>
        /// <returns></returns>
        [HttpGet, Route("GetProjectHistoryDetail")]
        public async Task<IActionResult> GetProjectHistoryDetail([FromQuery] int projectHistoryId)
        {
            return Json(await _service.GetProjectHistoryDetail(projectHistoryId));
        }

        /// <summary>
        /// 根据项目ID获取项目额外信息详情
        /// <para>1:项目计划信息|ProjectPlanInfo</para>
        /// <para>2:项目资源预算|ProjectResourceBudget</para>
        /// <para>3:项目附件列表|ProjectAttachmentList</para>
        /// <para>4:项目其它成本费用预算|ProjectOtherBudget</para>
        /// <para>5:项目预算汇总|ProjectBudgetSummary</para>
        /// <para>6:项目资源预算|ProjectResourceBudgetHC</para>
        /// </summary>
        /// <param name="projectId">项目Id</param>
        /// <returns></returns>
        [HttpGet, Route("GetProjectExtraInfo")]
        public async Task<IActionResult> GetProjectExtraInfo([FromQuery] int projectId)
        {
            return Json(await _service.GetProjectExtraInfo(projectId));
        }

        /// <summary>
        /// 根据项目ID获取项目历史额外信息详情
        /// <para>1:项目计划信息|ProjectPlanInfo</para>
        /// <para>2:项目资源预算|ProjectResourceBudget</para>
        /// <para>3:项目附件列表|ProjectAttachmentList</para>
        /// <para>4:项目其它成本费用预算|ProjectOtherBudget</para>
        /// <para>5:项目预算汇总|ProjectBudgetSummary</para>
        /// <para>6:项目资源预算|ProjectResourceBudgetHC</para>
        /// </summary>
        /// <param name="projectHistoryId">项目历史Id</param>
        /// <returns></returns>
        [HttpGet, Route("GetProjectHistoryExtraInfo")]
        public async Task<IActionResult> GetProjectHistoryExtraInfo([FromQuery] int projectHistoryId)
        {
            return Json(await _service.GetProjectHistoryExtraInfo(projectHistoryId));
        }

        /// <summary>
        /// 计算资源预算人月信息
        /// </summary>
        /// <param name="calculateResourceBudgetHCInputDTO">项目ID,资源计划</param>
        /// <returns></returns>
        [HttpPost, Route("CalculateResourceBudgetHC")]
        public async Task<IActionResult> CalculateResourceBudgetHC([FromBody] CalculateResourceBudgetHCInputDTO calculateResourceBudgetHCInputDTO)
        {
            return Json(await _projectResourceBudgetHCService.CalculateResourceBudgetHC(calculateResourceBudgetHCInputDTO));
        }

        /// <summary>
        /// 保存项目节段+资源预算信息
        /// <para>项目其它费用，只是用来上传计算，本次接口不保存</para>
        /// </summary>
        /// <param name="saveResourceBudgeInputDTO">Parameters:项目ID,项目节段,资源计划,项目其它费用</param>
        /// <returns></returns>
        [HttpPost, Route("SaveProjectResourceBudget")]
        public async Task<IActionResult> SaveProjectResourceBudget([FromBody] SaveResourceBudgeInputDTO saveResourceBudgeInputDTO)
        {
            return Json(await _projectResourceBudgetHCService.SaveProjectResourceBudget(saveResourceBudgeInputDTO));
        }

        /// <summary>
        /// 根据项目ID获取项目预算汇总信息
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [HttpGet, Route("GetProjectBudgetSummary")]
        public async Task<IActionResult> GetProjectBudgetSummary([FromQuery] int projectId)
        {
            return Json(await _projectBudgetSummaryService.GetProjectBudgetSummary(projectId));
        }

        /// <summary>
        /// 保存项目信息
        /// </summary>
        /// <param name="projectDTO">项目实体</param>
        /// <returns></returns>
        [HttpPost, Route("SaveProject")]
        public async Task<IActionResult> SaveProject([FromBody] ProjectDTO projectDTO)
        {
            return Json(await _service.SaveProject(projectDTO));
        }

        /// <summary>
        /// 项目变更
        /// <para>UpdateType</para>
        /// <para>1:变更项目经理</para>
        /// <para>2:变更项目总监</para>
        /// </summary>
        /// <param name="projectUpdateDTO"></param>
        /// <returns></returns>
        [HttpPost, Route("UpdateProject")]
        public async Task<IActionResult> UpdateProject([FromBody] ProjectUpdateDTO projectUpdateDTO)
        {
            return Json(await _service.UpdateProject(projectUpdateDTO));
        }

        /// <summary>
        /// 保存项目额外信息
        /// <para>1:项目计划信息|ProjectPlanInfo</para>
        /// <para>2:项目资源预算|ProjectResourceBudget</para>
        /// <para>3:项目附件列表|ProjectAttachmentList</para>
        /// <para>4:项目其它成本费用预算|ProjectOtherBudget</para>
        /// <para>5:项目资源预算|ProjectResourceBudgetHC</para>
        /// </summary>
        /// <param name="projectId">项目Id</param>
        /// <param name="projectExtraInfoDTO">项目额外信息</param>
        /// <returns></returns>
        [HttpPost, Route("SaveProjectExtraInfo")]
        public async Task<IActionResult> SaveProjectExtraInfo([FromForm] ProjectExtraInfoDTO projectExtraInfoDTO)
        {
            var files = Request.Form.Files;
            foreach (var file in files)
            {
                string input = file.Name;
                string pattern = @"\[(\d+)\]";

                Match match = Regex.Match(input, pattern);

                string indexStr = match.Groups[1].Value;
                int index = int.Parse(indexStr);
                projectExtraInfoDTO.ProjectAttachmentList.ElementAt(index).File = file;
            }
            return Json(await _service.SaveProjectExtraInfo(projectExtraInfoDTO.Project_Id, projectExtraInfoDTO));
        }

        /// <summary>
        /// 项目提交审批[包括项目ID,项目预算汇总信息]
        /// <para>1:项目预算汇总|ProjectBudgetSummary</para>
        /// </summary>
        /// <param name="projectWorkFlowDTO"></param>
        /// <returns></returns>
        [HttpPost, Route("Submit")]
        public async Task<IActionResult> Submit([FromBody] ProjectSubmitInputDTO projectWorkFlowDTO)
        {
            return Json(await _service.Submit(projectWorkFlowDTO));
        }

        /// <summary>
        /// 审核
        /// <para><paramref name="keys"/> 业务主键,多条记录用','号分隔</para>
        /// <para><paramref name="auditStatus"/> 审批状态 (1:审批通过,2:审批驳回,3:审批中 ,4:未发起 ,5:已撤回)</para>
        /// <para><paramref name="auditReason"/> 审批意见</para>
        /// </summary>
        /// <param name="keys">业务主键keys</param>
        /// <param name="auditStatus">审批状态 (1:审批通过,2:审批驳回,3:审批中 ,4:未发起 ,5:已撤回)</param>
        /// <param name="auditReason">审批意见</param>
        /// <returns></returns>
        //[ApiActionPermission(Enums.ActionPermissionOptions.Audit)]
        [HttpPost, Route("Audit")]
        [ApiExplorerSettings(IgnoreApi = false)]
        public override ActionResult Audit([FromBody] WorkFlowAuditDTO workFlowAudit)
        {
            return base.Audit(workFlowAudit);
        }

        /// <summary>
        /// 撤回-工作流
        /// <para><paramref name="projectId"/> 业务主键ID</para>
        /// </summary>
        /// <param name="projectId">项目Id</param>
        /// <returns></returns>
        [HttpPost, Route("Recall/{projectId}")]
        public async Task<IActionResult> Recall([FromRoute] int projectId) => Json(await _service.ReCall(projectId));

        /// <summary>
        /// 变更撤回
        /// <para><paramref name="projectId"/> 业务主键ID</para>
        /// </summary>
        /// <param name="projectId">项目Id</param>
        /// <returns></returns>
        [HttpPost, Route("ReCallChange/{projectId}")]
        public async Task<IActionResult> ReCallChange([FromRoute] int projectId) => Json(await _service.ReCallChange(projectId));

        /// <summary>
        /// 查询项目变更前后对比信息
        /// </summary>
        /// <param name="projectHistoryId">项目历史Id</param>
        /// <returns></returns>
        [HttpGet, Route("GetProjectCompareInfo")]
        public IActionResult GetContractCompareInfo(int projectHistoryId) => Json(_service.GetProjectCompareInfo(projectHistoryId));

        /// <summary>
        /// 查询项目变更前后对比信息_审计
        /// </summary>
        /// <param name="projectHistoryId">项目Id</param>
        /// <returns></returns>
        [HttpGet, Route("GetProjectCompareInfoForAudit")]
        public IActionResult GetProjectCompareInfoForAudit(int projectId) => Json(_service.GetProjectCompareInfoForAudit(projectId));

        /// <summary>
        /// 根据项目id查询项目历史信息
        /// </summary>
        /// <param name="projectId">项目Id</param>
        /// <returns></returns>
        /// 
        [HttpGet, Route("GetProjectHistoryListByProjectId")]
        public IActionResult GetProjectHistoryListByProjectId(int projectId) => Json(_service.GetProjectHistoryListByProjectId(projectId));

        /// <summary>
        /// 导出项目
        /// </summary>
        /// <param name="pageDataOptions"></param>
        /// <returns></returns>
        [HttpPost, Route("ExportPagerList")]
        public async Task<IActionResult> ExportPagerList([FromBody] PageDataOptions pageDataOptions) => Json(await _service.ExportPagerList(pageDataOptions, _hostingEnvironment.ContentRootPath));

        /// <summary>
        /// 导出项目 -Stable版
        /// </summary>
        /// <param name="pageDataOptions"></param>
        /// <returns></returns>
        [HttpPost, Route("ExportStablePagerList")]
        public async Task<IActionResult> ExportStablePagerList([FromBody] PageDataOptions pageDataOptions) => Json(await _service.ExportStablePagerList(pageDataOptions, _hostingEnvironment.ContentRootPath));

        /// <summary>
        /// 导出合同项目
        /// </summary>
        /// <param name="pageDataOptions"></param>
        /// <returns></returns>
        [HttpPost, Route("ExportContractPagerList")]
        public async Task<IActionResult> ExportContractPagerList([FromBody] PageDataOptions pageDataOptions) => Json(await _service.ExportContractPagerList(pageDataOptions, _hostingEnvironment.ContentRootPath));

        /// <summary>
        /// 导出合同项目 -Stable版
        /// </summary>
        /// <param name="pageDataOptions"></param>
        /// <returns></returns>
        [HttpPost, Route("ExportStableContractPagerList")]
        public async Task<IActionResult> ExportStableContractPagerList([FromBody] PageDataOptions pageDataOptions) => Json(await _service.ExportStableContractPagerList(pageDataOptions, _hostingEnvironment.ContentRootPath));

        /// <summary>
        /// 导出项目资源预算
        /// </summary>
        /// <param name="pageDataOptions"></param>
        /// <returns></returns>
        [HttpPost, Route("ExportProjectResourceBudgetPagerList")]
        public async Task<IActionResult> ExportProjectResourceBudgetPagerList([FromBody] PageDataOptions pageDataOptions) => Json(await _service.ExportProjectResourceBudgetPagerList(pageDataOptions, _hostingEnvironment.ContentRootPath));

        /// <summary>
        /// 导出项目资源预算 -Stable版
        /// </summary>
        /// <param name="pageDataOptions"></param>
        /// <returns></returns>
        [HttpPost, Route("ExportStableProjectResourceBudgetPagerList")]
        public async Task<IActionResult> ExportStableProjectResourceBudgetPagerList([FromBody] PageDataOptions pageDataOptions) => Json(await _service.ExportStableProjectResourceBudgetPagerList(pageDataOptions, _hostingEnvironment.ContentRootPath));

        //生成项目编码
        [HttpGet, Route("GenerateProjectCode")]
        public IActionResult GenerateProjectCode() => Json(_service.GenerateProjectCode());
    }
}
