/*
 *Author：jxx
 *Contact：283591387@qq.com
 *代码由框架生成,此处任何更改都可能导致被代码生成器覆盖
 *所有业务编写全部应在Partial文件夹下ProjectResourceBudgetHCHistoryService与IProjectResourceBudgetHCHistoryService中编写
 */
using BCS.Business.IRepositories;
using BCS.Business.IServices;
using BCS.Core.BaseProvider;
using BCS.Core.Extensions.AutofacManager;
using BCS.Entity.DomainModels;

namespace BCS.Business.Services
{
    public partial class ProjectResourceBudgetHCHistoryService : ServiceBase<ProjectResourceBudgetHCHistory, IProjectResourceBudgetHCHistoryRepository>
    , IProjectResourceBudgetHCHistoryService, IDependency
    {
    public ProjectResourceBudgetHCHistoryService(IProjectResourceBudgetHCHistoryRepository repository)
    : base(repository)
    {
    Init(repository);
    }
    public static IProjectResourceBudgetHCHistoryService Instance
    {
      get { return AutofacContainerModule.GetService<IProjectResourceBudgetHCHistoryService>(); } }
    }
 }
