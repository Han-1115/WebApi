using BCS.Core.BaseProvider;
using BCS.Entity.DomainModels;
using BCS.Core.Utilities;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace BCS.Business.IServices
{
    public partial interface IContractProjectService
    {
        /// <summary>
        /// 根据合同Id找到对应的所有项目
        /// </summary>
        /// <param name="id">合同Id</param>
        /// <returns>项目list</returns>
        List<ContractProject> GetContractProjectsByContactId(int contactId);

        /// <summary>
        /// 根据Id找到项目合同关系
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>项目合同关系</returns>
        ContractProject GetContractProject(int id);

        /// <summary>
        /// 增加项目合同关系
        /// </summary>
        /// <param name="id">合同Id</param>
        /// <returns>项目合同关系 id</returns>
        int Add(int contactId, int projectId);

        /// <summary>
        /// 删除或者加回来项目合同关系
        /// </summary>
        /// <param name="id">项目合同关系Id</param>
        /// <returns>成功则返回true,否则返回false</returns>
        bool UpdateStatus(int id, int status);

        /// <summary>
        /// 更新合同中所有项目
        /// </summary>
        /// <param name="contractId">项目合同关系Id</param>
        /// <param name="addProjects">需要添加的项目</param>
        /// <param name="updateProjects">需要更新的项目</param>
        /// <param name="deleteProjects">需要删除的项目</param>
        /// <param name="historyVersion">历史记录版本号</param>
        /// <param name="needToUpdateHistory">是否需要记录历史</param>
        /// <returns>成功则返回true,否则返回false</returns>
        bool UpdateContractProjects(int contractId, List<Project> addProjects, List<Project> updateProjects, List<ContractProject> deleteProjects, int historyVersion, bool needToUpdateHistory);
    }
}
