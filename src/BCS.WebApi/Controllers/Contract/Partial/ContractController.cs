using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BCS.Core.Controllers.Basic;
using BCS.Core.Enums;
using BCS.Core.Filters;
using BCS.Entity.DomainModels;
using BCS.Business.IServices;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using BCS.Entity.DTO.Contract;
using BCS.Core.Utilities;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Microsoft.Extensions.Hosting.WindowsServices;
using System.IO;
using System;

namespace BCS.System.Controllers
{

    public partial class ContractController
    {
        /// <summary>
        /// 删除合同
        /// </summary>
        /// <param name="contract_id">合同Id</param>
        /// <returns></returns>
        [HttpGet, Route("DelContract")]
        public IActionResult DelContract(int contract_id) => Json(_service.DeleContract(contract_id));

        /// <summary>
        /// 撤回-工作流
        /// <para><paramref name="contract_id"/> 合同Id</para>
        /// </summary>
        /// <param name="contract_id">合同Id</param>
        /// <returns></returns>
        [HttpPost, Route("Recall/{contract_id}")]
        public IActionResult Recall(int contract_id) => Json(_service.ReCall(contract_id));

        /// <summary>
        /// 变更撤回
        /// <para><paramref name="contract_id"/> 合同Id</para>
        /// </summary>
        /// <param name="contract_id">合同Id</param>
        /// <returns></returns>
        [HttpPost, Route("ReCallChange/{contract_id}")]
        public IActionResult ReCallChange(int contract_id) => Json(_service.ReCallChange(contract_id));

        /// <summary>
        ///关闭
        /// </summary>
        /// <param name="contract_id">合同Id</param>
        /// <returns></returns>
        [HttpGet, Route("Close")]
        public IActionResult Close(int contract_id) => Json(_service.Close(contract_id));

        /// <summary>
        /// 获取所有的框架合同信息（用于绑定下拉框）
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("GetFramContractList")]
        public IActionResult GetFramContractList() => Json(_service.GetFramContractList());

        /// <summary>
        ///查询合同分页列表
        /// </summary>
        /// <param name="pageDataOptions">分页参数</param>
        /// <returns></returns>
        [HttpPost, Route("GetPagerList")]
        public IActionResult GetPagerList([FromBody] PageDataOptions pageDataOptions) => Json(_service.GetPagerList(pageDataOptions));

        /// <summary>
        ///导出
        /// </summary>
        /// <param name="pageDataOptions">分页参数</param>
        /// <returns></returns>
        [HttpPost, Route("ExportFile")]
        public IActionResult ExportFile([FromBody] PageDataOptions pageDataOptions)
        {
            var contentRootPath = _hostingEnvironment.ContentRootPath;
            return Json(_service.ExportFile(pageDataOptions, contentRootPath));
        }

        /// <summary>
        ///查询变更记录（用于查看详情页面）
        /// </summary>
        /// <param name="contract_id"></param>
        /// <returns></returns>
        [HttpGet, Route("GetContractHistoryListByContractId")]
        public IActionResult GetContractHistoryListByContractId(int contract_id) => Json(_contractHistoryService.GetContractHistories(contract_id));

        /// <summary>
        /// 查询合同详情（编辑或者查看带出合同信息）
        /// </summary>
        /// <param name="contract_id"></param>
        /// <returns></returns>
        [HttpGet, Route("GetContractDetail")]
        public IActionResult GetContractDetail(int contract_id) => Json(_service.GetContractDetail(contract_id));

        /// <summary>
        /// 查询历史合同详情
        /// </summary>
        /// <param name="contractHistoryId"></param>
        /// <returns></returns>
        [HttpGet, Route("GetContractHistoryDetail")]
        public IActionResult GetContractHistoryDetail(int contractHistoryId) => Json(_service.GetContractHistoryDetail(contractHistoryId));

        /// <summary>
        /// 查询合同变更前后信息
        /// </summary>
        /// <param name="contractHistoryId">合同历史Id</param>
        /// <returns></returns>
        [HttpGet, Route("GetContractCompareInfo")]
        public IActionResult GetContractCompareInfo(int contractHistoryId)
        {
            var contract = _contractHistoryService.GetContractHistory(contractHistoryId);
            if (contract == null) return Json(new WebResponseContent().Error("合同历史不存在!"));
            return Json(_service.GetContractCompareInfo(contractHistoryId));
        }

        /// <summary>
        /// 查询合同变更前后信息审批使用
        /// </summary>
        /// <param name="contract_id">合同Id</param>
        /// <returns></returns>
        [HttpGet, Route("GetContractCompareInfoForAudit")]
        public IActionResult GetContractCompareInfoForAudit(int contract_id)
        {
            return Json(_service.GetContractCompareInfoForAudit(contract_id));
        }

        /// <summary>
        /// 校验合同是否可以变更
        /// </summary>
        /// <param name="contract_id">合同Id</param>
        /// <returns></returns>
        [HttpGet, Route("CheckContractChange")]
        public IActionResult CheckContractChange(int contract_id) => Json(_service.CheckContractChange(contract_id));

        /// <summary>
        /// 获取合同详情-stable 版本
        /// <para>从历史表获取最后一版数据进行详情展示</para>
        /// </summary>
        /// <param name="contractHistoryId"></param>
        /// <returns></returns>
        [HttpGet, Route("GetStableContractDetail")]
        public IActionResult GetStableContractDetail(int contractHistoryId) => Json(_service.GetStableContractDetail(contractHistoryId));

        /// <summary>
        /// 查询合同分页列表-stable 版本
        /// <para>从历史表获取最后一版数据进行列表展示</para>
        /// </summary>
        /// <param name="pageDataOptions"></param>
        /// <returns></returns>
        [HttpPost, Route("GetStablePagerList")]
        public IActionResult GetStablePagerList([FromBody] PageDataOptions pageDataOptions) => Json(_service.GetStablePagerList(pageDataOptions));

        /// <summary>
        /// 导出附件-stable 版本
        /// <para>从历史表获取最后一版数据进行导出</para>
        /// </summary>
        /// <param name="pageDataOptions"></param>
        /// <returns></returns>
        [HttpPost, Route("ExportStableFile")]
        public IActionResult ExportStableFile([FromBody] PageDataOptions pageDataOptions)
        {
            var contentRootPath = _hostingEnvironment.ContentRootPath;
            return Json(_service.ExportStableFile(pageDataOptions, contentRootPath));
        }
    }
}
