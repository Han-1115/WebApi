using BCS.Core.BaseProvider;
using BCS.Entity.DomainModels;
using BCS.Core.Utilities;
using System.Linq.Expressions;
using BCS.Entity.DTO.Contract;
using ContractAttachments = BCS.Entity.DomainModels.ContractAttachments;

namespace BCS.Business.IServices
{
    public partial interface IContractSerevice
    {
        /// <summary>
        /// 获取所有的框架合同
        /// </summary>
        /// <returns>所有的客户实体</returns>
        WebResponseContent GetFramContractList();

        /// <summary>
        /// 删除合同
        /// </summary>
        /// <param name="id">合同Id</param>
        /// <returns></returns>
        WebResponseContent DeleContract(int contract_id);

        /// <summary>
        /// 撤回
        /// </summary>
        /// <param name="id">合同Id</param>
        /// <returns></returns>
        WebResponseContent ReCall(int contract_id);

        /// <summary>
        /// 撤回变更
        /// </summary>
        /// <param name="id">合同Id</param>
        /// <returns></returns>
        WebResponseContent ReCallChange(int contract_id);


        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="id">合同Id</param>
        /// <returns></returns>
        WebResponseContent Close(int contract_id);

        /// <summary>
        /// 查询合同分页列表
        /// </summary>
        /// <param name="pageDataOptions"></param>
        /// <returns></returns>
        PageGridData<ContractPagerModel> GetPagerList(PageDataOptions pageDataOptions);

        /// <summary>
        /// 导出附件
        /// </summary>
        /// <param name="pageDataOptions"></param>
        /// <returns></returns>
        WebResponseContent ExportFile(PageDataOptions pageDataOptions, string contentRootPath);

        /// <summary>
        /// 通过id获取合同
        /// </summary>
        /// <returns>合同Entity</returns>
        Contract GetContract(int id);

        /// <summary>
        /// 更新合同表
        /// </summary>
        /// <returns>返回合同id</returns>
        int UpdateContractInfo(Contract contract, byte workflowAction, List<Project> addProjects, List<Project> updateProjects, List<ContractProject> deleteProjects, List<ContractAttachments> addContractAttachments, List<ContractAttachments> delContractAttachments);


        /// <summary>
        /// 获取合并对比信息
        /// </summary>
        /// <param name="contractHistoryId">合同历史Id</param>
        /// <returns></returns>
        WebResponseContent GetContractCompareInfo(int contractHistoryId);

        /// <summary>
        /// 查询合同变更前后信息审批使用
        /// </summary>
        /// <param name="contract_id">合同Id</param>
        /// <returns></returns>
        public WebResponseContent GetContractCompareInfoForAudit(int contract_id);

        /// <summary>
        /// 获取合同详情
        /// </summary>
        /// <param name="contract_id"></param>
        /// <returns></returns>
        WebResponseContent GetContractDetail(int contract_id);

        /// <summary>
        /// 查询历史合同详情
        /// </summary>
        /// <param name="contractHistoryId"></param>
        /// <returns></returns>
        WebResponseContent GetContractHistoryDetail(int contractHistoryId);

        /// <summary>
        /// 校验合同是否可以变更
        /// </summary>
        /// <param name="contract_id"></param>
        /// <returns></returns>
        bool CheckContractChange(int contract_id);

        /// <summary>
        /// 获取合同详情-stable 版本
        /// <para>从历史表获取最后一版数据进行详情展示</para>
        /// </summary>
        /// <param name="contractHistoryId"></param>
        /// <returns></returns>
        WebResponseContent GetStableContractDetail(int contractHistoryId);

        /// <summary>
        /// 查询合同分页列表-stable 版本
        /// <para>从历史表获取最后一版数据进行列表展示</para>
        /// </summary>
        /// <param name="pageDataOptions"></param>
        /// <returns></returns>
        PageGridData<ContractPagerModel> GetStablePagerList(PageDataOptions pageDataOptions);

        /// <summary>
        /// 导出附件-stable 版本
        /// <para>从历史表获取最后一版数据进行导出</para>
        /// </summary>
        /// <param name="pageDataOptions"></param>
        /// <returns></returns>
        WebResponseContent ExportStableFile(PageDataOptions pageDataOptions, string contentRootPath);
    }
}
