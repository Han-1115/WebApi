/*
 *所有关于Sys_DepartmentSetting类的业务代码应在此处编写
*可使用repository.调用常用方法，获取EF/Dapper等信息
*如果需要事务请使用repository.DbContextBeginTransaction
*也可使用DBServerProvider.手动获取数据库相关信息
*用户信息、权限、角色等使用UserContext.Current操作
*Sys_DepartmentSettingService对增、删、改查、导入、导出、审核业务代码扩展参照ServiceFunFilter
*/
using BCS.Core.BaseProvider;
using BCS.Core.Extensions.AutofacManager;
using BCS.Entity.DomainModels;
using System.Linq;
using BCS.Core.Utilities;
using System.Linq.Expressions;
using BCS.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using BCS.Business.IRepositories;
using BCS.Entity.DTO.System;
using static Dapper.SqlMapper;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using BCS.Core.ManageUser;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;

namespace BCS.Business.Services
{
    public partial class Sys_DepartmentSettingService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISys_DepartmentSettingRepository _repository;//访问数据库

        [ActivatorUtilitiesConstructor]
        public Sys_DepartmentSettingService(
            ISys_DepartmentSettingRepository dbRepository,
            IHttpContextAccessor httpContextAccessor
            )
        : base(dbRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = dbRepository;
            //多租户会用到这init代码，其他情况可以不用
            //base.Init(dbRepository);
        }

        /// <summary>
        /// 获取部门设置
        /// </summary>
        /// <param name="departmentId">部门ID</param>
        /// <param name="year">年份</param>
        /// <returns></returns>
        public async Task<WebResponseContent> Query(Guid departmentId, string year)
        {
            Expression<Func<Sys_DepartmentSetting, bool>> predicate = o => true;
            if (departmentId != Guid.Empty)
            {
                predicate = predicate.And(x => x.DepartmentId == departmentId);
            }
            if (!string.IsNullOrEmpty(year))
            {
                predicate = predicate.And(x => x.Year.ToString() == year);
            }
            var result = await _repository.FindAsync(predicate);
            if (result.Count > 0)
            {
                return WebResponseContent.Instance.OK("获取部门配置信息成功", result);
            }
            else
            {
                return WebResponseContent.Instance.Error("未找部门配置信息");
            }
        }

        /// <summary>
        /// 更新部门设置
        /// </summary>
        /// <param name="sys_DepartmentSettingDTO">配置实体</param>
        /// <returns></returns>
        public async Task<WebResponseContent> SaveOrUpdate(Sys_DepartmentSettingDTO sys_DepartmentSettingDTO)
        {
            UserInfo userInfo = UserContext.Current.UserInfo;
            userInfo.UserName = string.IsNullOrEmpty(userInfo.UserName) ? string.Empty : userInfo.UserName;
            DateTime currentTime = DateTime.Now;
            int result = 0;
            if (await _repository.ExistsAsync(x => x.DepartmentId == sys_DepartmentSettingDTO.DepartmentId && x.Year == sys_DepartmentSettingDTO.Year))
            {
                //update
                var existsItem = await _repository.FindFirstAsync(x => x.DepartmentId == sys_DepartmentSettingDTO.DepartmentId && x.Year == sys_DepartmentSettingDTO.Year);

                existsItem.Year = sys_DepartmentSettingDTO.Year;
                existsItem.LaborCostofOwnDelivery = sys_DepartmentSettingDTO.LaborCostofOwnDelivery;
                existsItem.ProjectGPM = sys_DepartmentSettingDTO.ProjectGPM;
                existsItem.Remark = sys_DepartmentSettingDTO.Remark;
                existsItem.ModifyID = userInfo.User_Id;
                existsItem.Modifier = userInfo.UserName;
                existsItem.ModifyDate = currentTime;
                result = _repository.Update(existsItem, true);
                return result > 0 ? WebResponseContent.Instance.OK("更新部门配置信息成功", existsItem) : WebResponseContent.Instance.Error("更新部门配置信息失败");
            }
            else
            {
                //insert
                Sys_DepartmentSetting sys_DepartmentSetting = new Sys_DepartmentSetting()
                {
                    DepartmentId = sys_DepartmentSettingDTO.DepartmentId,
                    Year = sys_DepartmentSettingDTO.Year,
                    LaborCostofOwnDelivery = sys_DepartmentSettingDTO.LaborCostofOwnDelivery,
                    ProjectGPM = sys_DepartmentSettingDTO.ProjectGPM,
                    Remark = sys_DepartmentSettingDTO.Remark,
                    CreateID = userInfo.User_Id,
                    Creator = userInfo.UserName,
                    CreateDate = currentTime,
                    ModifyID = userInfo.User_Id,
                    Modifier = userInfo.UserName,
                    ModifyDate = currentTime,
                };
                _repository.Add(sys_DepartmentSetting, true);
                return WebResponseContent.Instance.OK("新增部门配置信息成功", sys_DepartmentSetting);
            }
        }
    }
}
