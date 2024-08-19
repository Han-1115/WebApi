/*
*所有关于ProjectHistory类的业务代码接口应在此处编写
*/
using BCS.Core.BaseProvider;
using BCS.Entity.DomainModels;
using BCS.Core.Utilities;
using System.Linq.Expressions;
namespace BCS.Business.IServices
{
    public partial interface IProjectHistoryService
    {
        /// <summary>
        /// 添加项目历史
        /// </summary>
        /// <param name="contract">项目</param>
        /// <param name="version">变更版本号</param>
        /// <returns>添加成返回true，否则false</returns>
        bool AddHistory(Project project, int version);
    }
 }
