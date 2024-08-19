/*
 *代码由框架生成,任何更改都可能导致被代码生成器覆盖
 *如果要增加方法请在当前目录下Partial文件夹Sys_DepartmentMappingController编写
 */
using Microsoft.AspNetCore.Mvc;
using BCS.Core.Controllers.Basic;
using BCS.Entity.AttributeManager;
using BCS.Business.IServices;
namespace BCS.Business.Controllers
{
    [Route("api/Sys_DepartmentMapping")]
    [PermissionTable(Name = "Sys_DepartmentMapping")]
    public partial class Sys_DepartmentMappingController : ApiBaseController<ISys_DepartmentMappingService>
    {
        public Sys_DepartmentMappingController(ISys_DepartmentMappingService service)
        : base(service)
        {
        }
    }
}

