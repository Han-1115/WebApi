/*
*所有关于ProjectPlanInfo类的业务代码接口应在此处编写
*/
using BCS.Core.BaseProvider;
using BCS.Entity.DomainModels;
using BCS.Core.Utilities;
using System.Linq.Expressions;
using BCS.Entity.DTO.Project;

namespace BCS.Business.IServices
{
    public partial interface IProjectPlanInfoService
    {
        /// <summary>
        /// 保存项目计划信息，项目资源预算
        /// </summary>
        /// <param name="projectId">项目id</param>
        /// <param name="projectExtraInfos">项目计划信息</param>
        /// <param name="projectResourceBudgets">项目预算信息</param>
        /// <param name="projectResourceBudgetHCs">项目预算信息HC</param>
        /// <returns></returns>
        public Task<WebResponseContent> SaveProjectPlanInfo(int projectId, ICollection<ProjectPlanInfoDTO> projectPlanInfos, ICollection<ProjectResourceBudgetDTO> projectResourceBudgets, ICollection<ProjectResourceBudgetHCDTO> projectResourceBudgetHCs);
    }
 }
