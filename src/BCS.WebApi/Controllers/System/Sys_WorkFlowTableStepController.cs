/*
 *代码由框架生成,任何更改都可能导致被代码生成器覆盖
 *如果要增加方法请在当前目录下Partial文件夹Sys_WorkFlowTableStepController编写
 */
using Microsoft.AspNetCore.Mvc;
using BCS.Core.Controllers.Basic;
using BCS.Entity.AttributeManager;
using BCS.Business.IServices;
namespace BCS.System.Controllers
{
    [Route("api/Sys_WorkFlowTableStep")]
    [PermissionTable(Name = "Sys_WorkFlowTableStep")]
    public partial class Sys_WorkFlowTableStepController : ApiBaseController<ISys_WorkFlowTableStepService>
    {
        public Sys_WorkFlowTableStepController(ISys_WorkFlowTableStepService service)
        : base(service)
        {
        }
    }
}

