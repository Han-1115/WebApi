/*
 *Author：jxx
 *Contact：283591387@qq.com
 *代码由框架生成,此处任何更改都可能导致被代码生成器覆盖
 *所有业务编写全部应在Partial文件夹下EmailSendLogService与IEmailSendLogService中编写
 */
using BCS.Business.IRepositories.System;
using BCS.Business.IServices;
using BCS.Core.BaseProvider;
using BCS.Core.Extensions.AutofacManager;
using BCS.Entity.DomainModels;

namespace BCS.Business.Services
{
    public partial class EmailSendLogService : ServiceBase<EmailSendLog, IEmailSendLogRepository>
    , IEmailSendLogService, IDependency
    {
    public EmailSendLogService(IEmailSendLogRepository repository)
    : base(repository)
    {
    Init(repository);
    }
    public static IEmailSendLogService Instance
    {
      get { return AutofacContainerModule.GetService<IEmailSendLogService>(); } }
    }
 }
