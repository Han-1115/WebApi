/*
*所有关于Sys_WorkFlowTable类的业务代码接口应在此处编写
*/
using BCS.Core.BaseProvider;
using BCS.Entity.DomainModels;
using BCS.Core.Utilities;
using System.Linq.Expressions;
using System.Linq;
using System;
using System.Collections.Generic;
using BCS.Entity.DTO.Flow;
using System.Threading.Tasks;

namespace BCS.Business.IServices
{
    public partial interface ISys_WorkFlowTableService
    {

        /// <summary>
        /// 查询一个审批流程的所有节点详情信息
        /// </summary>
        /// <param name="workFlowTableId">审批流主键Id</param>
        /// <returns>所有节点详情信息列表</returns>
        public List<Sys_WorkFlowTableStep> GetStepList(Guid workFlowTableId);

        /// <summary>
        /// 我的审批，我的提交，审批流程列表
        /// </summary>
        /// <param name="pageDataOptions"></param>
        /// <returns></returns>

        new PageGridData<WorkFlowPagerModel> GetPageData(PageDataOptions pageDataOptions);

        /// <summary>
        /// 根据流程实例ID获取 与之对应的业务数据对比信息
        /// </summary>
        /// <param name="workFlowTableId">流程实例ID</param>
        /// <returns></returns>
        public Task<WebResponseContent> GetBusinessCompareInfo(Guid workFlowTable_Id);
    }
}
