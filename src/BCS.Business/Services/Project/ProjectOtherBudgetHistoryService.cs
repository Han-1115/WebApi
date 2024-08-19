/*
 *Author：jxx
 *Contact：283591387@qq.com
 *代码由框架生成,此处任何更改都可能导致被代码生成器覆盖
 *所有业务编写全部应在Partial文件夹下ProjectOtherBudgetHistoryService与IProjectOtherBudgetHistoryService中编写
 */
using BCS.Business.IRepositories;
using BCS.Business.IServices;
using BCS.Core.BaseProvider;
using BCS.Core.Extensions.AutofacManager;
using BCS.Entity.DomainModels;

namespace BCS.Business.Services
{
    public partial class ProjectOtherBudgetHistoryService : ServiceBase<ProjectOtherBudgetHistory, IProjectOtherBudgetHistoryRepository>
    , IProjectOtherBudgetHistoryService, IDependency
    {
    public ProjectOtherBudgetHistoryService(IProjectOtherBudgetHistoryRepository repository)
    : base(repository)
    {
    Init(repository);
    }
    public static IProjectOtherBudgetHistoryService Instance
    {
      get { return AutofacContainerModule.GetService<IProjectOtherBudgetHistoryService>(); } }
    }
 }
