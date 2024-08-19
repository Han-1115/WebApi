/*
 *所有关于ProjectOtherBudget类的业务代码应在此处编写
*可使用repository.调用常用方法，获取EF/Dapper等信息
*如果需要事务请使用repository.DbContextBeginTransaction
*也可使用DBServerProvider.手动获取数据库相关信息
*用户信息、权限、角色等使用UserContext.Current操作
*ProjectOtherBudgetService对增、删、改查、导入、导出、审核业务代码扩展参照ServiceFunFilter
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
using BCS.Entity.DTO.Project;
using BCS.Core.ManageUser;
using BCS.Business.Repositories;
using BCS.Core.EFDbContext;
using AutoMapper;
using BCS.Core.DBManager;

namespace BCS.Business.Services
{
    public partial class ProjectOtherBudgetService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IProjectOtherBudgetRepository _repository;//访问数据库
        private readonly IMapper _mapper;

        [ActivatorUtilitiesConstructor]
        public ProjectOtherBudgetService(
            IProjectOtherBudgetRepository dbRepository,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper
            )
        : base(dbRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = dbRepository;
            _mapper = mapper;
            //多租户会用到这init代码，其他情况可以不用
            //base.Init(dbRepository);
        }
        /// <summary>
        /// 保存-项目其它成本费用预算
        /// </summary>
        /// <param name="projectId">项目id</param>
        /// <param name="projectOtherBudgets">项目其它成本费用预算</param>
        /// <returns></returns>
        public async Task<WebResponseContent> SaveOtherBudget(int projectId, ICollection<ProjectOtherBudgetDTO> projectOtherBudgets)
        {
            UserInfo userInfo = UserContext.Current.UserInfo;
            DateTime currentTime = DateTime.Now;
            var updatePlanInfoList = new List<ProjectPlanInfo>();
            userInfo.UserName = string.IsNullOrEmpty(userInfo.UserName) ? string.Empty : userInfo.UserName;
            //项目资源预算

            var existsOtherBudget = await _repository.FindAsync(x => x.Project_Id == projectId);
            var insertList = new List<ProjectOtherBudget>();
            var updateList = new List<ProjectOtherBudget>();
            var removeList = new List<ProjectOtherBudget>();

            foreach (var item in projectOtherBudgets)
            {
                var projectOtherBudget = _mapper.Map<ProjectOtherBudget>(item);
                if (projectOtherBudget.Id <= 0)
                {
                    //新增业务
                    projectOtherBudget.Project_Id = projectId;
                    projectOtherBudget.CreateID = userInfo.User_Id;
                    projectOtherBudget.Creator = userInfo.UserName;
                    projectOtherBudget.CreateDate = currentTime;
                    projectOtherBudget.ModifyID = userInfo.User_Id;
                    projectOtherBudget.Modifier = userInfo.UserName;
                    projectOtherBudget.ModifyDate = currentTime;
                    insertList.Add(projectOtherBudget);
                }
                else
                {
                    //更新业务
                    projectOtherBudget.ModifyID = userInfo.User_Id;
                    projectOtherBudget.Modifier = userInfo.UserName;
                    projectOtherBudget.ModifyDate = currentTime;
                    updateList.Add(projectOtherBudget);
                }
            }
            //删除业务
            foreach (var item in existsOtherBudget)
            {
                if (projectOtherBudgets.Any(o => o.Id == item.Id))
                {
                    continue;
                }
                var projectResourceBudget = _mapper.Map<ProjectOtherBudget>(item);
                removeList.Add(projectResourceBudget);
            }
            BCSContext dbContext = DBServerProvider.GetEFDbContext();
            using (var transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    repository.AddRange(insertList);
                    repository.UpdateRange(updatePlanInfoList);
                    if (removeList.Count > 0)
                    {
                        repository.DeleteWithKeys(removeList.Select(o => (object)o.Id).ToArray());
                    }
                    dbContext.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return WebResponseContent.Instance.Error($"项目其它成本费用预算异常:[{ex.Message}]");
                }
            }
            return WebResponseContent.Instance.OK("项目其它成本费用预算成功");
        }
    }
}
