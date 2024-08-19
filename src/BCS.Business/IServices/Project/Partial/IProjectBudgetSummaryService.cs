/*
*所有关于ProjectBudgetSummary类的业务代码接口应在此处编写
*/
using BCS.Core.BaseProvider;
using BCS.Entity.DomainModels;
using BCS.Core.Utilities;
using System.Linq.Expressions;
using BCS.Entity.DTO.Project;

namespace BCS.Business.IServices
{
    public partial interface IProjectBudgetSummaryService
    {
        /// <summary>
        /// 获取项目预算汇总
        /// </summary>
        /// <param name="projectId">项目Id</param>
        /// <returns></returns>
        public Task<WebResponseContent> GetProjectBudgetSummary(int projectId);

        /// <summary>
        /// 保存-项目预算汇总
        /// </summary>
        /// <param name="projectId">项目id</param>
        /// <param name="projectBudgetSumary">项目预算汇总</param>
        /// <returns></returns>
        public Task<WebResponseContent> SaveProjectBudgetSummary(int projectId, ICollection<ProjectBudgetSummaryDTO> projectBudgetSumary);
    }
}
