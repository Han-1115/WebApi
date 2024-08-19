/*
 *代码由框架生成,任何更改都可能导致被代码生成器覆盖
 *如果要增加方法请在当前目录下Partial文件夹Sys_CalendarController编写
 */
using Microsoft.AspNetCore.Mvc;
using BCS.Core.Controllers.Basic;
using BCS.Entity.AttributeManager;
using BCS.Business.IServices;
namespace BCS.WebApi.Controllers.Project
{
    [Route("api/Sys_Calendar")]
    [PermissionTable(Name = "Sys_Calendar")]
    public partial class Sys_CalendarController : ApiBaseController<ISys_CalendarService>
    {
        public Sys_CalendarController(ISys_CalendarService service)
        : base(service)
        {
        }
    }
}

