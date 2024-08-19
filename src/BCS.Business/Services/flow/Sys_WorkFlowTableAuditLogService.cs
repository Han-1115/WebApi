/*
 *Author：jxx
 *Contact：283591387@qq.com
 *代码由框架生成,此处任何更改都可能导致被代码生成器覆盖
 *所有业务编写全部应在Partial文件夹下Sys_WorkFlowTableAuditLogService与ISys_WorkFlowTableAuditLogService中编写
 */
using BCS.Business.IRepositories;
using BCS.Business.IServices;
using BCS.Core.BaseProvider;
using BCS.Core.Extensions.AutofacManager;
using BCS.Entity.DomainModels;

namespace BCS.Business.Services
{
    public partial class Sys_WorkFlowTableAuditLogService : ServiceBase<Sys_WorkFlowTableAuditLog, ISys_WorkFlowTableAuditLogRepository>
    , ISys_WorkFlowTableAuditLogService, IDependency
    {
    public Sys_WorkFlowTableAuditLogService(ISys_WorkFlowTableAuditLogRepository repository)
    : base(repository)
    {
    Init(repository);
    }
    public static ISys_WorkFlowTableAuditLogService Instance
    {
      get { return AutofacContainerModule.GetService<ISys_WorkFlowTableAuditLogService>(); } }
    }
 }
