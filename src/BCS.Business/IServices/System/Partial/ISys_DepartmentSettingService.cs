/*
*所有关于Sys_DepartmentSetting类的业务代码接口应在此处编写
*/
using BCS.Core.BaseProvider;
using BCS.Entity.DomainModels;
using BCS.Core.Utilities;
using System.Linq.Expressions;
using BCS.Entity.DTO.System;
namespace BCS.Business.IServices
{
    public partial interface ISys_DepartmentSettingService
    {
        /// <summary>
        /// 获取部门设置
        /// </summary>
        /// <param name="departmentId">部门ID</param>
        /// <param name="year">年份</param>
        /// <returns></returns>
        public Task<WebResponseContent> Query(Guid departmentId, string year);

        /// <summary>
        /// 更新部门设置
        /// </summary>
        /// <param name="sys_DepartmentSettingDTO">配置实体</param>
        /// <returns></returns>
        public Task<WebResponseContent> SaveOrUpdate(Sys_DepartmentSettingDTO sys_DepartmentSettingDTO);
    }
}
