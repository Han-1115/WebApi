using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BCS.Core.Const;
using BCS.Core.DBManager;
using BCS.Core.EFDbContext;
using BCS.Core.Enums;
using BCS.Core.Extensions;
using BCS.Core.ManageUser;
using BCS.Core.Services;
using BCS.Core.Utilities;
using BCS.Core.WorkFlow;
using BCS.Entity;
using BCS.Entity.DomainModels;
using BCS.Entity.DTO.Flow;

namespace BCS.Business.Services
{
    public partial class Sys_MenuService
    {
        /// <summary>
        /// 菜单静态化处理，每次获取菜单时先比较菜单是否发生变化，如果发生变化从数据库重新获取，否则直接获取_menus菜单
        /// </summary>
        private static List<Sys_Menu> _menus { get; set; }

        /// <summary>
        /// 从数据库获取菜单时锁定的对象
        /// </summary>
        private static object _menuObj = new object();

        /// <summary>
        /// 当前服务器的菜单版本
        /// </summary>
        private static string _menuVersionn = "";

        private const string _menuCacheKey = "inernalMenu";

        /// <summary>
        /// 编辑修改菜单时,获取所有菜单
        /// </summary>
        /// <returns></returns>
        public async Task<object> GetMenu()
        {
            //  DBServerProvider.SqlDapper.q
            return (await repository.FindAsync(x => 1 == 1, a =>
             new
             {
                 id = a.Menu_Id,
                 parentId = a.ParentId,
                 name = a.MenuName,
                 a.MenuType,
                 a.OrderNo
             })).OrderByDescending(a => a.OrderNo)
                .ThenByDescending(q => q.parentId).ToList();

        }

        private List<Sys_Menu> GetAllMenu()
        {
            //每次比较缓存是否更新过，如果更新则重新获取数据
            string _cacheVersion = CacheContext.Get(_menuCacheKey);
            if (_menuVersionn != "" && _menuVersionn == _cacheVersion)
            {
                return _menus ?? new List<Sys_Menu>();
            }
            lock (_menuObj)
            {
                if (_menuVersionn != "" && _menus != null && _menuVersionn == _cacheVersion) return _menus;
                //2020.12.27增加菜单界面上不显示，但可以分配权限
                _menus = repository.FindAsIQueryable(x => x.Enable == 1 || x.Enable == 2)
                    .OrderByDescending(a => a.OrderNo)
                    .ThenByDescending(q => q.ParentId).ToList();

                _menus.ForEach(x =>
                {
                    // 2022.03.26增移动端加菜单类型
                    x.MenuType ??= 0;
                    if (!string.IsNullOrEmpty(x.Auth) && x.Auth.Length > 10)
                    {
                        try
                        {
                            x.Actions = x.Auth.DeserializeObject<List<Sys_Actions>>();
                        }
                        catch { }
                    }
                    if (x.Actions == null) x.Actions = new List<Sys_Actions>();
                });

                string cacheVersion = CacheContext.Get(_menuCacheKey);
                if (string.IsNullOrEmpty(cacheVersion))
                {
                    cacheVersion = DateTime.Now.ToString("yyyyMMddHHMMssfff");
                    CacheContext.Add(_menuCacheKey, cacheVersion);
                }
                else
                {
                    _menuVersionn = cacheVersion;
                }
            }
            return _menus;
        }

        /// <summary>
        /// 获取当前用户有权限查看的菜单
        /// </summary>
        /// <returns></returns>
        public List<Sys_Menu> GetCurrentMenuList()
        {
            int roleId = UserContext.Current.RoleId;
            return GetUserMenuList(roleId);
        }


        public List<Sys_Menu> GetUserMenuList(int roleId)
        {
            if (UserContext.IsRoleIdSuperAdmin(roleId))
            {
                return GetAllMenu();
            }
            List<int> menuIds = UserContext.Current.GetPermissions(roleId).Select(x => x.Menu_Id).ToList();
            return GetAllMenu().Where(x => menuIds.Contains(x.Menu_Id)).ToList();
        }

        /// <summary>
        /// 获取当前用户所有菜单与权限
        /// </summary>
        /// <returns></returns>
        public async Task<object> GetCurrentMenuActionList()
        {
            return await GetMenuActionList(UserContext.Current.RoleId);
        }

        /// <summary>
        /// 根据角色ID获取菜单与权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task<object> GetMenuActionList(int roleId)
        {
            //2020.12.27增加菜单界面上不显示，但可以分配权限
            if (UserContext.IsRoleIdSuperAdmin(roleId))
            {
                return await Task.Run(() => GetAllMenu()
                .Where(c => c.MenuType == UserContext.MenuType)
                .Select(x =>
                new
                {
                    id = x.Menu_Id,
                    name = x.MenuName,
                    url = x.Url,
                    parentId = x.ParentId,
                    icon = x.Icon,
                    x.Enable,
                    x.TableName, // 2022.03.26增移动端加菜单类型
                    permission = x.Actions.Select(s => s.Value).ToArray()
                }).ToList());
            }

            var user = UserContext.Current.UserInfo;
            var deptIds = user.DeptIds?.Select(s => s.ToString());
            BCSContext context = DBServerProvider.GetEFDbContext();
            var workFlowTableQuery = context.Set<Sys_WorkFlowTable>() as IQueryable<Sys_WorkFlowTable>;
            var stepQuery = _stepRepository.FindAsIQueryable(x => (x.StepType == (int)AuditType.用户审批 && x.StepValue == user.User_Id.ToString())
              || (x.StepType == (int)AuditType.角色审批 && x.StepValue == user.Role_Id.ToString()));

            stepQuery = from a in stepQuery
                        where a.AuditStatus == null
                        join b in workFlowTableQuery on a.WorkFlowTable_Id equals b.WorkFlowTable_Id
                        where b.AuditStatus != (int)ApprovalStatus.Rejected && b.AuditStatus != (int)ApprovalStatus.Recalled && b.AuditStatus != (int)ApprovalStatus.Approved && b.AuditStatus != (int)ApprovalStatus.NotInitiated
                        join c in context.Set<Sys_WorkFlowTableStep>() on new { WorkFlowTable_Id = a.WorkFlowTable_Id, OrderId = a.OrderId } equals new { WorkFlowTable_Id = c.WorkFlowTable_Id, OrderId = c.OrderId + 1 }
                        where (a.OrderId != 1 ? c.AuditStatus == (int)ApprovalStatus.Approved : true)
                        select a;

            workFlowTableQuery = workFlowTableQuery.Where(x => stepQuery.Any(c => x.WorkFlowTable_Id == c.WorkFlowTable_Id));
            if (user.RoleName == CommonConst.DeliveryManager)
            {
                var contractIds = (from project in context.Set<Project>()
                                   where deptIds.Contains(project.Delivery_Department_Id)
                                   join contractProject in context.Set<ContractProject>().Where(s => s.Status == (int)Status.Active) on project.Id equals contractProject.Project_Id
                                   select contractProject.Contract_Id).Distinct().Select(x => x.ToString()).ToList();
                var workFlowTableQueryContract = workFlowTableQuery.Where((x => contractIds.Contains(x.WorkTableKey) && (x.BusinessType == (int)BusinessTypeEnum.CongractRigster || x.BusinessType == (int)BusinessTypeEnum.ContractChange)));

                var projectIds = (from project in context.Set<Project>() where deptIds.Contains(project.Delivery_Department_Id) select project.Id).Select(x => x.ToString()).ToList();
                var workFlowTableQueryProject = workFlowTableQuery.Where((x => projectIds.Contains(x.WorkTableKey) && (x.BusinessType == (int)BusinessTypeEnum.ProjectApplication || x.BusinessType == (int)BusinessTypeEnum.ProjectChange)));
                
                var subcontractIds = (from subcontract in context.Set<SubContractFlow>() where (deptIds != null ? deptIds.Contains(subcontract.Delivery_Department_Id) : true) select subcontract.Id).Select(x => x.ToString()).ToList();
                var workFlowTableQuerySubcontract = workFlowTableQuery.Where((x => subcontractIds.Contains(x.WorkTableKey) && (x.BusinessType == (int)BusinessTypeEnum.SubContractRigster || x.BusinessType == (int)BusinessTypeEnum.SubContractChange || x.BusinessType == (int)BusinessTypeEnum.SubContractorRigster || x.BusinessType == (int)BusinessTypeEnum.SubContractorChange)));
                workFlowTableQuery = workFlowTableQueryContract.Union(workFlowTableQueryProject).Union(workFlowTableQuerySubcontract);
            }

            if (user.RoleName == CommonConst.SeniorProgramManager)
            {
                var contractIds = (from project in context.Set<Project>()
                                   where deptIds.Contains(project.Delivery_Department_Id)
                                   join contractProject in context.Set<ContractProject>().Where(s => s.Status == (int)Status.Active) on project.Id equals contractProject.Project_Id
                                   select contractProject.Contract_Id).Distinct().Select(x => x.ToString()).ToList();
                var workFlowTableQueryContract = workFlowTableQuery.Where((x => contractIds.Contains(x.WorkTableKey) && (x.BusinessType == (int)BusinessTypeEnum.CongractRigster || x.BusinessType == (int)BusinessTypeEnum.ContractChange)));

                var projectIds = (from project in context.Set<Project>() where project.Project_Director_Id == user.User_Id select project.Id).Select(x => x.ToString()).ToList();
                var workFlowTableQueryProject = workFlowTableQuery.Where((x => projectIds.Contains(x.WorkTableKey) && (x.BusinessType == (int)BusinessTypeEnum.ProjectApplication || x.BusinessType == (int)BusinessTypeEnum.ProjectChange)));

                var subcontractIds = (from subcontract in context.Set<SubContractFlow>() where subcontract.Contract_Director_Id == user.User_Id select subcontract.Id).Select(x => x.ToString()).ToList();
                var workFlowTableQuerySubcontract = workFlowTableQuery.Where((x => subcontractIds.Contains(x.WorkTableKey) && (x.BusinessType == (int)BusinessTypeEnum.SubContractRigster || x.BusinessType == (int)BusinessTypeEnum.SubContractChange || x.BusinessType == (int)BusinessTypeEnum.SubContractorRigster || x.BusinessType == (int)BusinessTypeEnum.SubContractorChange)));
                workFlowTableQuery = workFlowTableQueryContract.Union(workFlowTableQueryProject).Union(workFlowTableQuerySubcontract);
            }

            var todoCount = workFlowTableQuery.Count();

            var menu = from a in UserContext.Current.Permissions
                       join b in GetAllMenu().Where(c => c.MenuType == UserContext.MenuType)
                       on a.Menu_Id equals b.Menu_Id
                       orderby b.OrderNo descending
                       select new
                       {
                           id = a.Menu_Id,
                           name = b.MenuName,
                           url = b.Url,
                           parentId = b.ParentId,
                           icon = b.Icon,
                           b.Enable,
                           b.TableName, // 2022.03.26增移动端加菜单类型
                           permission = a.UserAuthArr,
                           extra = b.MenuName == "To-do Tasks" || b.MenuName == "My Tasks" ? new { todoCount = todoCount } : null
                       };
            return menu.ToList();
        }

        /// <summary>
        /// 新建或编辑菜单
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> Save(Sys_Menu menu)
        {
            WebResponseContent webResponse = new WebResponseContent();
            if (menu == null) return webResponse.Error("没有获取到提交的参数");
            if (menu.Menu_Id > 0 && menu.Menu_Id == menu.ParentId) return webResponse.Error("父级ID不能是当前菜单的ID");
            try
            {
                webResponse = menu.ValidationEntity(x => new { x.MenuName, x.TableName });
                if (!webResponse.Status) return webResponse;
                if (menu.TableName != "/" && menu.TableName != ".")
                {
                    // 2022.03.26增移动端加菜单类型判断
                    Sys_Menu sysMenu = await repository.FindAsyncFirst(x => x.TableName == menu.TableName);
                    if (sysMenu != null)
                    {
                        sysMenu.MenuType ??= 0;
                        if (sysMenu.MenuType == menu.MenuType)
                        {
                            if ((menu.Menu_Id > 0 && sysMenu.Menu_Id != menu.Menu_Id)
                            || menu.Menu_Id <= 0)
                            {
                                return webResponse.Error($"视图/表名【{menu.TableName}】已被其他菜单使用");
                            }
                        }
                    }
                }
                bool _changed = false;
                if (menu.Menu_Id <= 0)
                {
                    repository.Add(menu.SetCreateDefaultVal());
                }
                else
                {
                    //2020.05.07新增禁止选择上级角色为自己
                    if (menu.Menu_Id == menu.ParentId)
                    {
                        return webResponse.Error($"父级id不能为自己");
                    }
                    if (repository.Exists(x => x.ParentId == menu.Menu_Id && menu.ParentId == x.Menu_Id))
                    {
                        return webResponse.Error($"不能选择此父级id，选择的父级id与当前菜单形成依赖关系");
                    }

                    _changed = repository.FindAsIQueryable(c => c.Menu_Id == menu.Menu_Id).Select(s => s.Auth).FirstOrDefault() != menu.Auth;

                    repository.Update(menu.SetModifyDefaultVal(), p => new
                    {
                        p.ParentId,
                        p.MenuName,
                        p.Url,
                        p.Auth,
                        p.OrderNo,
                        p.Icon,
                        p.Enable,
                        p.MenuType,// 2022.03.26增移动端加菜单类型
                        p.TableName,
                        p.ModifyDate,
                        p.Modifier
                    });
                }
                await repository.SaveChangesAsync();

                CacheContext.Add(_menuCacheKey, DateTime.Now.ToString("yyyyMMddHHMMssfff"));
                if (_changed)
                {
                    UserContext.Current.RefreshWithMenuActionChange(menu.Menu_Id);
                }
                _menus = null;
                webResponse.OK("保存成功", menu);
            }
            catch (Exception ex)
            {
                webResponse.Error(ex.Message);
            }
            finally
            {
                Logger.Info($"表:{menu.TableName},菜单：{menu.MenuName},权限{menu.Auth},{(webResponse.Status ? "成功" : "失败")}{webResponse.Message}");
            }
            return webResponse;

        }

        public async Task<WebResponseContent> DelMenu(int menuId)
        {
            WebResponseContent webResponse = new WebResponseContent();

            if (await repository.ExistsAsync(x => x.ParentId == menuId))
            {
                return webResponse.Error("当前菜单存在子菜单,请先删除子菜单!");
            }
            repository.Delete(new Sys_Menu()
            {
                Menu_Id = menuId
            }, true);
            CacheContext.Add(_menuCacheKey, DateTime.Now.ToString("yyyyMMddHHMMssfff"));
            return webResponse.OK("删除成功");
        }
        /// <summary>
        /// 编辑菜单时，获取菜单信息
        /// </summary>
        /// <param name="menuId"></param>
        /// <returns></returns>
        public async Task<object> GetTreeItem(int menuId)
        {
            var sysMenu = (await base.repository.FindAsync(x => x.Menu_Id == menuId))
                .Select(
                p => new
                {
                    p.Menu_Id,
                    p.ParentId,
                    p.MenuName,
                    p.Url,
                    p.Auth,
                    p.OrderNo,
                    p.Icon,
                    p.Enable,
                    // 2022.03.26增移动端加菜单类型
                    MenuType = p.MenuType ?? 0,
                    p.CreateDate,
                    p.Creator,
                    p.TableName,
                    p.ModifyDate
                }).FirstOrDefault();
            return sysMenu;
        }
    }
}

