/*
*所有关于Project类的业务代码接口应在此处编写
*/
using BCS.Core.BaseProvider;
using BCS.Entity.DomainModels;
using BCS.Core.Utilities;
using System.Linq.Expressions;
using System.Collections.Generic;
using BCS.Entity.DTO.Contract;
using BCS.Entity.DTO.Project;

namespace BCS.Business.IServices
{
    public partial interface IProjectService
    {
        /// <summary>
        /// 添加或者更改项目
        /// </summary>
        /// <returns>更新成功返回项目的id，否则返回0</returns>
        int AddOrUpdate(int id, string relationship, string code, string name, decimal projectAmount, string type, string departmentId, string department, int managerId, string manager, string remark);

        /// <summary>
        /// 通过id获取项目
        /// </summary>
        /// <returns>项目Entity</returns>
        Project GetProject(int id);


        /// <summary>
        /// 通过id列表来获取所有项目
        /// </summary>
        /// <returns>项目Entity列表</returns>
        List<Project> FindProjectsWithIds(List<int> ids);

        /// <summary>
        /// 查询合同分页列表
        /// </summary>
        /// <param name="pageDataOptions"></param>
        /// <returns></returns>
        PageGridData<ProjectListOutPutDTO> GetPagerList(PageDataOptions pageDataOptions);
        /// <summary>
        /// 查询合同分页列表-Stable版
        /// </summary>
        /// <param name="pageDataOptions"></param>
        /// <returns></returns>
        PageGridData<ProjectListOutPutDTO> GetStablePagerList(PageDataOptions pageDataOptions);
        /// <summary>
        /// 查询已同意的合同分页列表
        /// </summary>
        /// <param name="pageDataOptions"></param>
        /// <returns></returns>
        PageGridData<ApprovedProjectListOutPutDTO> GetApprovedPagerList(PageDataOptions pageDataOptions);
        /// <summary>
        /// 查询合同分页列表
        /// </summary>
        /// <param name="pageDataOptions"></param>
        /// <returns></returns>
        PageGridData<ProjectListOutPutDTO> GetContractPagerList(PageDataOptions pageDataOptions);
        /// <summary>
        /// 合同项目列表-stable 版本
        /// </summary>
        /// <param name="pageDataOptions"></param>
        /// <returns></returns>
        PageGridData<ProjectListOutPutDTO> GetStableContractPagerList(PageDataOptions pageDataOptions);
        /// <summary>
        /// 导出项目
        /// </summary>
        /// <param name="pageDataOptions"></param>
        /// <param name="contentRootPath"></param>
        /// <returns></returns>
        public Task<WebResponseContent> ExportPagerList(PageDataOptions pageDataOptions, string contentRootPath);
        /// <summary>
        /// 导出项目-Stable版
        /// </summary>
        /// <param name="pageDataOptions"></param>
        /// <param name="contentRootPath"></param>
        /// <returns></returns>
        public Task<WebResponseContent> ExportStablePagerList(PageDataOptions pageDataOptions, string contentRootPath);
        /// <summary>
        /// 导出合同项目
        /// </summary>
        /// <param name="pageDataOptions"></param>
        /// <param name="contentRootPath"></param>
        /// <returns></returns>
        public Task<WebResponseContent> ExportContractPagerList(PageDataOptions pageDataOptions, string contentRootPath);
        /// <summary>
        /// 导出合同项目-Stable版
        /// </summary>
        /// <param name="pageDataOptions"></param>
        /// <param name="contentRootPath"></param>
        /// <returns></returns>
        public Task<WebResponseContent> ExportStableContractPagerList(PageDataOptions pageDataOptions, string contentRootPath);

        /// <summary>
        /// 导出项目资源预算
        /// </summary>
        /// <param name="pageDataOptions"></param>
        /// <param name="contentRootPath"></param>
        /// <returns></returns>
        public Task<WebResponseContent> ExportProjectResourceBudgetPagerList(PageDataOptions pageDataOptions, string contentRootPath);
        /// <summary>
        /// 导出项目资源预算-Stable版
        /// </summary>
        /// <param name="pageDataOptions"></param>
        /// <param name="contentRootPath"></param>
        /// <returns></returns>
        public Task<WebResponseContent> ExportStableProjectResourceBudgetPagerList(PageDataOptions pageDataOptions, string contentRootPath);

        /// <summary>
        /// 根据项目ID获取项目详情
        /// </summary>
        /// <param name="projectId">项目Id</param>
        /// <returns></returns>
        public Task<WebResponseContent> GetProjectDetail(int projectId);

        /// <summary>
        /// 根据项目ID获取项目历史详情
        /// </summary>
        /// <param name="projectHistoryId">项目历史Id</param>
        /// <returns></returns>
        public Task<WebResponseContent> GetProjectHistoryDetail(int projectHistoryId);

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
        public Task<WebResponseContent> GetProjectExtraInfo(int projectId);

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
        public Task<WebResponseContent> GetProjectHistoryExtraInfo(int projectHistoryId);

        /// <summary>
        /// 保存项目信息
        /// </summary>
        /// <param name="projectDTO">项目实体</param>
        /// <returns></returns>
        public Task<WebResponseContent> SaveProject(ProjectDTO projectDTO);

        /// <summary>
        /// 项目变更
        /// <para>UpdateType</para>
        /// <para>1:变更项目经理</para>
        /// <para>2:变更项目总监</para>
        /// </summary>
        /// <param name="projectUpdateDTO"></param>
        /// <returns></returns>
        public Task<WebResponseContent> UpdateProject(ProjectUpdateDTO projectUpdateDTO);

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
        public Task<WebResponseContent> SaveProjectExtraInfo(int projectId, ProjectExtraInfoDTO projectExtraInfoDTO);

        /// <summary>
        /// 项目提交审批[包括项目ID,项目预算汇总信息]
        /// <para>1:项目预算汇总|ProjectBudgetSummary</para>
        /// </summary>
        /// <param name="projectSubmitInputDTO"></param>
        /// <returns></returns>
        public Task<WebResponseContent> Submit(ProjectSubmitInputDTO projectSubmitInputDTO);

        /// <summary>
        /// 撤回
        /// </summary>
        /// <param name="projectId">项目Id</param>
        /// <returns></returns>
        public Task<WebResponseContent> ReCall(int projectId);

        /// <summary>
        /// 撤回变更
        /// </summary>
        /// <param name="projectId">项目Id</param>
        /// <returns></returns>
        public Task<WebResponseContent> ReCallChange(int projectId);

        /// <summary>
        /// 查询项目变更前后对比信息
        /// </summary>
        /// <param name="projectHistoryId">项目历史Id</param>
        /// <returns></returns>
        public WebResponseContent GetProjectCompareInfo(int projectHistoryId);

        /// <summary>
        /// 查询项目变更前后对比信息_审计
        /// </summary>
        /// <param name="projectId">项目历史Id</param>
        /// <returns></returns>
        public WebResponseContent GetProjectCompareInfoForAudit(int projectId);

        /// <summary>
        /// 合同项目列表
        /// </summary>
        /// <param name="pageDataOptions"></param>
        /// <returns></returns>
        public WebResponseContent GetProjectHistoryListByProjectId(int projectId);

        //生成项目编码
        public string GenerateProjectCode();
    }
}
