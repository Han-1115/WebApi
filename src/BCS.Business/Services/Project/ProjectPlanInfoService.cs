/*
 *Author：jxx
 *Contact：283591387@qq.com
 *代码由框架生成,此处任何更改都可能导致被代码生成器覆盖
 *所有业务编写全部应在Partial文件夹下ProjectPlanInfoService与IProjectPlanInfoService中编写
 */
using BCS.Business.IRepositories;
using BCS.Business.IServices;
using BCS.Core.BaseProvider;
using BCS.Core.Extensions.AutofacManager;
using BCS.Entity.DomainModels;

namespace BCS.Business.Services
{
    public partial class ProjectPlanInfoService : ServiceBase<ProjectPlanInfo, IProjectPlanInfoRepository>
    , IProjectPlanInfoService, IDependency
    {
    public ProjectPlanInfoService(IProjectPlanInfoRepository repository)
    : base(repository)
    {
    Init(repository);
    }
    public static IProjectPlanInfoService Instance
    {
      get { return AutofacContainerModule.GetService<IProjectPlanInfoService>(); } }
    }
 }
