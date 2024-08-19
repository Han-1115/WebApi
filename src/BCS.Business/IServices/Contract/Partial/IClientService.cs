using BCS.Core.BaseProvider;
using BCS.Entity.DomainModels;
using BCS.Core.Utilities;
using System.Linq.Expressions;

namespace BCS.Business.IServices
{
    public partial interface IClientService
    {
        /// <summary>
        /// 获取所有的客户实体
        /// </summary>
        /// <returns>所有的客户实体</returns>
        WebResponseContent GetClientList();


        /// <summary>
        /// 获取客户实体信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Client GetClientById(int id);
    
    }
}
