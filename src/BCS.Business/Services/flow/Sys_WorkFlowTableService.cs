/*
 *Author：jxx
 *Contact：283591387@qq.com
 *代码由框架生成,此处任何更改都可能导致被代码生成器覆盖
 *所有业务编写全部应在Partial文件夹下Sys_WorkFlowTableService与ISys_WorkFlowTableService中编写
 */
using BCS.Business.IRepositories;
using BCS.Business.IServices;
using BCS.Core.BaseProvider;
using BCS.Core.Extensions.AutofacManager;
using BCS.Entity.DomainModels;

namespace BCS.Business.Services
{
    public partial class Sys_WorkFlowTableService : ServiceBase<Sys_WorkFlowTable, ISys_WorkFlowTableRepository>
    , ISys_WorkFlowTableService, IDependency
    {
    public Sys_WorkFlowTableService(ISys_WorkFlowTableRepository repository)
    : base(repository)
    {
    Init(repository);
    }
    public static ISys_WorkFlowTableService Instance
    {
      get { return AutofacContainerModule.GetService<ISys_WorkFlowTableService>(); } }
    }
 }
