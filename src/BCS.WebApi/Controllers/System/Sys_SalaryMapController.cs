/*
 *代码由框架生成,任何更改都可能导致被代码生成器覆盖
 *如果要增加方法请在当前目录下Partial文件夹Sys_SalaryMapController编写
 */
using Microsoft.AspNetCore.Mvc;
using BCS.Core.Controllers.Basic;
using BCS.Entity.AttributeManager;
using BCS.Business.IServices;
namespace BCS.WebApi.Controllers
{
    [Route("api/Sys_SalaryMap")]
    [PermissionTable(Name = "Sys_SalaryMap")]
    public partial class Sys_SalaryMapController : ApiBaseController<ISys_SalaryMapService>
    {
        public Sys_SalaryMapController(ISys_SalaryMapService service)
        : base(service)
        {
        }
    }
}

