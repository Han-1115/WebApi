/*
*所有关于ProjectOtherBudget类的业务代码接口应在此处编写
*/
using BCS.Core.BaseProvider;
using BCS.Entity.DomainModels;
using BCS.Core.Utilities;
using System.Linq.Expressions;
using BCS.Entity.DTO.Project;

namespace BCS.Business.IServices
{
    public partial interface IProjectOtherBudgetService
    {
        /// <summary>
        /// 保存-项目其它成本费用预算
        /// </summary>
        /// <param name="projectId">项目id</param>
        /// <param name="projectOtherBudgets">项目其它成本费用预算</param>
        /// <returns></returns>
        public Task<WebResponseContent> SaveOtherBudget(int projectId, ICollection<ProjectOtherBudgetDTO> projectOtherBudgets);
    }
 }
