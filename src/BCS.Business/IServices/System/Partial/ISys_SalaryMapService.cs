/*
*所有关于Sys_SalaryMap类的业务代码接口应在此处编写
*/
using BCS.Core.BaseProvider;
using BCS.Entity.DomainModels;
using BCS.Core.Utilities;
using System.Linq.Expressions;
using BCS.Entity.DTO.System;
namespace BCS.Business.IServices
{
    public partial interface ISys_SalaryMapService
    {
        /// <summary>
        /// 根据职位、城市、级别获取薪资
        /// </summary>
        /// <param name="positionId"></param>
        /// <param name="cityId"></param>
        /// <param name="levelId"></param>
        /// <returns></returns>
        public Task<WebResponseContent> GetSysSalaryMap(int positionId, int cityId, int levelId);

        public PageGridData<Sys_SalaryMapDTO> GetPagerList(PageDataOptions pageDataOptions);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="sys_SalaryMapDTO">更新实体</param>
        /// <returns></returns>
        public Task<WebResponseContent> SaveOrUpdate(Sys_SalaryMapDTO sys_SalaryMapDTO);
    }
}
