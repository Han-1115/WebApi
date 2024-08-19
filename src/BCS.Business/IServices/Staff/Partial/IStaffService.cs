/*
*所有关于Staff类的业务代码接口应在此处编写
*/
using BCS.Core.BaseProvider;
using BCS.Entity.DomainModels;
using BCS.Core.Utilities;
using System.Linq.Expressions;
using BCS.Entity.DTO.Staff;

namespace BCS.Business.IServices
{
    public partial interface IStaffService
    {

        /// <summary>
        /// 人员查询列表
        /// </summary>
        /// <param name="pageDataOptions">分页参数</param>
        /// <returns></returns>
        PageGridData<StaffPagerModel> GetPagerList(PageDataOptions pageDataOptions);

        /// <summary>
        /// 导出人员查询列表
        /// </summary>
        /// <param name="pageDataOptions"></param>
        /// <param name="contentRootPath"></param>
        /// <returns></returns>
        Task<WebResponseContent> ExportPagerList(PageDataOptions pageDataOptions, string contentRootPath);

        /// <summary>
        /// 获取金蝶组织架构
        /// </summary>
        /// <returns></returns>
        Task<WebResponseContent> GetAdminOrgDataAsync();

        /// <summary>
        /// 同步金蝶员工信息
        /// </summary>
        /// <returns></returns>
        Task<WebResponseContent> SynchronizeStaff();


        /// <summary>
        /// 查询所有的交付部门的自有人员
        /// </summary>
        /// <returns></returns>
        PageGridData<Object> GetStaffData(PageDataOptions pageDataOptions);
    }
}
