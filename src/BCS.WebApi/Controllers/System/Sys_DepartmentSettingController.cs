/*
 *代码由框架生成,任何更改都可能导致被代码生成器覆盖
 *如果要增加方法请在当前目录下Partial文件夹Sys_DepartmentSettingController编写
 */
using Microsoft.AspNetCore.Mvc;
using BCS.Core.Controllers.Basic;
using BCS.Entity.AttributeManager;
using BCS.Business.IServices;
namespace BCS.WebApi.Controllers
{
    [Route("api/Sys_DepartmentSetting")]
    //[PermissionTable(Name = "Sys_DepartmentSetting")]
    public partial class Sys_DepartmentSettingController : ApiBaseController<ISys_DepartmentSettingService>
    {
        public Sys_DepartmentSettingController(ISys_DepartmentSettingService service)
        : base(service)
        {
        }
    }
}

