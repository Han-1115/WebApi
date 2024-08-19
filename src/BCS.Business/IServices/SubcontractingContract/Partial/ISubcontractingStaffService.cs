/*
*所有关于SubcontractingStaff类的业务代码接口应在此处编写
*/
using BCS.Core.BaseProvider;
using BCS.Entity.DomainModels;
using BCS.Core.Utilities;
using System.Linq.Expressions;
using BCS.Entity.DTO.Contract;
using BCS.Entity.DTO.SubcontractingStaff;
namespace BCS.Business.IServices
{
    public partial interface ISubcontractingStaffService
    {
        /// <summary>
        /// 查询分包人员档案
        /// </summary>
        /// <param name="pageDataOptions"></param>
        /// <returns></returns>
        PageGridData<SubcontractingStaffListDetails> GetSubcontractingStaffPagerListService(PageDataOptions pageDataOptions);

        /// <summary>
        /// 导出分包人员档案
        /// </summary>
        /// <param name="pageDataOptions"></param>
        /// <param name="contentRootPath"></param>
        /// <returns></returns>
        Task<WebResponseContent> ExporSubcontractingStafftPagerListServiceAsync(PageDataOptions pageDataOptions, string contentRootPath);

        /// <summary>
        /// 查询所有分包人员数据
        /// </summary>
        /// <returns></returns>
        PageGridData<Object> GetSubcontractorStaffData(PageDataOptions pageDataOptions);
    }
 }
