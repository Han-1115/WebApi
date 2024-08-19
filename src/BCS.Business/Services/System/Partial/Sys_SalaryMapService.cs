/*
 *所有关于Sys_SalaryMap类的业务代码应在此处编写
*可使用repository.调用常用方法，获取EF/Dapper等信息
*如果需要事务请使用repository.DbContextBeginTransaction
*也可使用DBServerProvider.手动获取数据库相关信息
*用户信息、权限、角色等使用UserContext.Current操作
*Sys_SalaryMapService对增、删、改查、导入、导出、审核业务代码扩展参照ServiceFunFilter
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
using BCS.Core.EFDbContext;
using BCS.Core.ManageUser;
using BCS.Entity.DTO.Project;
using BCS.Entity.DTO.System;
using BCS.Core.Const;
using BCS.Core.Enums;
using Newtonsoft.Json;
using BCS.Core.DBManager;
using BCS.Core.ConverterContainer;
using System.Linq.Dynamic.Core;

namespace BCS.Business.Services
{
    public partial class Sys_SalaryMapService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISys_SalaryMapRepository _repository;//访问数据库

        [ActivatorUtilitiesConstructor]
        public Sys_SalaryMapService(
            ISys_SalaryMapRepository dbRepository,
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
        /// 根据职位、城市、级别获取薪资
        /// </summary>
        /// <param name="positionId"></param>
        /// <param name="cityId"></param>
        /// <param name="levelId"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> GetSysSalaryMap(int positionId, int cityId, int levelId)
        {
            var result = await _repository.FindAsync(x => x.PositionId == positionId && x.CityId == cityId && x.LevelId == levelId);
            if (result.Count > 0)
            {
                return WebResponseContent.Instance.OK("获取薪资成功", result);
            }
            else
            {
                return WebResponseContent.Instance.Error("未找到薪资信息");
            }
        }

        /// <summary>
        /// 项目列表
        /// </summary>
        /// <param name="pageDataOptions"></param>
        /// <returns></returns>
        public PageGridData<Sys_SalaryMapDTO> GetPagerList(PageDataOptions pageDataOptions)
        {
            BCSContext context = DBServerProvider.GetEFDbContext();
            var query = from salary in context.Set<BCS.Entity.DomainModels.Sys_SalaryMap>()
                        select new Sys_SalaryMapDTO
                        {
                            Id = salary.Id,
                            CityId = salary.CityId,
                            PositionId = salary.PositionId,
                            LevelId = salary.LevelId,
                            MinCost_Rate = salary.MinCost_Rate,
                            MaxCost_Rate = salary.MaxCost_Rate,
                            Remark = salary.Remark,
                            CreateID = salary.CreateID,
                            Creator = salary.Creator,
                            CreateDate = salary.CreateDate,
                            ModifyID = salary.ModifyID,
                            Modifier = salary.Modifier,
                            ModifyDate = salary.ModifyDate,
                        };

            // dynamic query conditions
            if (!string.IsNullOrEmpty(pageDataOptions.Wheres))
            {
                var whereConditions = JsonConvert.DeserializeObject<List<SearchParameters>>(pageDataOptions.Wheres);

                if (whereConditions != null && whereConditions.Any())
                {
                    foreach (var condition in whereConditions)
                    {
                        switch (condition.DisplayType)
                        {

                            case HtmlElementType.Equal:
                                query = query.Where($"{condition.Name}", condition.Value);
                                break;
                            case HtmlElementType.like:
                                query = query.Contains($"{condition.Name}", condition.Value);
                                break;
                            case HtmlElementType.lessorequal:
                                query = query.LessOrequal($"{condition.Name}", condition.Value);
                                break;
                            case HtmlElementType.thanorequal:
                                query = query.ThanOrEqual($"{condition.Name}", condition.Value);
                                break;
                            default:
                                query = query.Where($"{condition.Name}", condition.Value);
                                break;

                        }
                    }
                }
            }
            // sort
            if (!string.IsNullOrEmpty(pageDataOptions.Sort) && !string.IsNullOrEmpty(pageDataOptions.Order))
            {
                query = query.OrderBy($"{pageDataOptions.Sort} {pageDataOptions.Order}");
            }
            else
            {
                query = query.OrderByDescending(x => x.CreateID);
            }

            var totalCount = query.Count();
            var currentPage = query.Skip((pageDataOptions.Page - 1) * pageDataOptions.Rows).Take(pageDataOptions.Rows).ToList();
            currentPage.ForEach(item =>
            {
                item.Position = ConverterContainer.PositionConverter(item.PositionId);
                item.City = ConverterContainer.CityConverter(item.CityId);
                item.Level = ConverterContainer.LevelConverter(item.LevelId);
            });
            var result = new PageGridData<Sys_SalaryMapDTO>
            {
                rows = currentPage,
                total = totalCount
            };

            return result;

        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="sys_SalaryMapDTO">更新实体</param>
        /// <returns></returns>
        public async Task<WebResponseContent> SaveOrUpdate(Sys_SalaryMapDTO sys_SalaryMapDTO)
        {
            UserInfo userInfo = UserContext.Current.UserInfo;
            userInfo.UserName = string.IsNullOrEmpty(userInfo.UserName) ? string.Empty : userInfo.UserName;
            DateTime currentTime = DateTime.Now;
            int result = 0;
            if (await _repository.ExistsAsync(x => x.CityId == sys_SalaryMapDTO.CityId && x.PositionId == sys_SalaryMapDTO.PositionId && x.LevelId == sys_SalaryMapDTO.LevelId))
            {
                //update
                var existsItem = await _repository.FindFirstAsync(x => x.CityId == sys_SalaryMapDTO.CityId && x.PositionId == sys_SalaryMapDTO.PositionId && x.LevelId == sys_SalaryMapDTO.LevelId);

                existsItem.CityId = sys_SalaryMapDTO.CityId;
                existsItem.PositionId = sys_SalaryMapDTO.PositionId;
                existsItem.LevelId = sys_SalaryMapDTO.LevelId;
                existsItem.MinCost_Rate = sys_SalaryMapDTO.MinCost_Rate;
                existsItem.MaxCost_Rate = sys_SalaryMapDTO.MaxCost_Rate;
                existsItem.Remark = sys_SalaryMapDTO.Remark;
                existsItem.ModifyID = userInfo.User_Id;
                existsItem.Modifier = userInfo.UserName;
                existsItem.ModifyDate = currentTime;
                result = _repository.Update(existsItem, true);
                return result > 0 ? WebResponseContent.Instance.OK("更新薪资信息成功", existsItem) : WebResponseContent.Instance.Error("更新薪资信息失败");
            }
            else
            {
                //insert
                Sys_SalaryMap sys_DepartmentSetting = new Sys_SalaryMap()
                {
                    CityId = sys_SalaryMapDTO.CityId,
                    PositionId = sys_SalaryMapDTO.PositionId,
                    LevelId = sys_SalaryMapDTO.LevelId,
                    MinCost_Rate = sys_SalaryMapDTO.MinCost_Rate,
                    MaxCost_Rate = sys_SalaryMapDTO.MaxCost_Rate,
                    Remark = sys_SalaryMapDTO.Remark,
                    CreateID = userInfo.User_Id,
                    Creator = userInfo.UserName,
                    CreateDate = currentTime,
                    ModifyID = userInfo.User_Id,
                    Modifier = userInfo.UserName,
                    ModifyDate = currentTime,
                };
                _repository.Add(sys_DepartmentSetting, true);
                return WebResponseContent.Instance.OK("新增薪资信息成功", sys_DepartmentSetting);
            }
        }
    }
}
