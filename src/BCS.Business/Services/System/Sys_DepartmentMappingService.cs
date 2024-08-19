/*
 *Author：jxx
 *Contact：283591387@qq.com
 *代码由框架生成,此处任何更改都可能导致被代码生成器覆盖
 *所有业务编写全部应在Partial文件夹下Sys_DepartmentMappingService与ISys_DepartmentMappingService中编写
 */
using BCS.Business.IRepositories;
using BCS.Business.IServices;
using BCS.Core.BaseProvider;
using BCS.Core.Extensions.AutofacManager;
using BCS.Entity.DomainModels;

namespace BCS.Business.Services
{
    public partial class Sys_DepartmentMappingService : ServiceBase<Sys_DepartmentMapping, ISys_DepartmentMappingRepository>
    , ISys_DepartmentMappingService, IDependency
    {
    public Sys_DepartmentMappingService(ISys_DepartmentMappingRepository repository)
    : base(repository)
    {
    Init(repository);
    }
    public static ISys_DepartmentMappingService Instance
    {
      get { return AutofacContainerModule.GetService<ISys_DepartmentMappingService>(); } }
    }
 }
