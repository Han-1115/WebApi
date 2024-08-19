/*
*所有关于ProjectResourceBudgetHC类的业务代码接口应在此处编写
*/
using BCS.Core.BaseProvider;
using BCS.Entity.DomainModels;
using BCS.Core.Utilities;
using System.Linq.Expressions;
using BCS.Entity.DTO.Project;

namespace BCS.Business.IServices
{
    public partial interface IProjectResourceBudgetHCService
    {
        /// <summary>
        /// 计算资源预算人月信息
        /// </summary>
        /// <param name="calculateResourceBudgetHCInputDTO">项目ID,资源计划</param>
        /// <returns></returns>
        public Task<WebResponseContent> CalculateResourceBudgetHC(CalculateResourceBudgetHCInputDTO calculateResourceBudgetHCInputDTO);

        /// <summary>
        /// 保存资源预算+人月信息
        /// </summary>
        /// <param name="saveResourceBudgeInputDTO"></param>
        /// <returns></returns>
        public Task<WebResponseContent> SaveProjectResourceBudget(SaveResourceBudgeInputDTO saveResourceBudgeInputDTO);
    }
 }
