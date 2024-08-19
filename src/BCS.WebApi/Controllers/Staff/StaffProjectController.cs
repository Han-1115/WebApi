/*
 *代码由框架生成,任何更改都可能导致被代码生成器覆盖
 *如果要增加方法请在当前目录下Partial文件夹StaffProjectController编写
 */
using Microsoft.AspNetCore.Mvc;
using BCS.Core.Controllers.Basic;
using BCS.Entity.AttributeManager;
using BCS.Business.IServices;
namespace BCS.WebApi.Controllers.Staff
{
    [Route("api/StaffProject")]
    [PermissionTable(Name = "StaffProject")]
    public partial class StaffProjectController : ApiBaseController<IStaffProjectService>
    {
        public StaffProjectController(IStaffProjectService service)
        : base(service)
        {
        }
    }
}

