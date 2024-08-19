/*
 *代码由框架生成,任何更改都可能导致被代码生成器覆盖
 *如果要增加方法请在当前目录下Partial文件夹ProjectResourceBudgetHCHistoryController编写
 */
using Microsoft.AspNetCore.Mvc;
using BCS.Core.Controllers.Basic;
using BCS.Entity.AttributeManager;
using BCS.Business.IServices;
namespace BCS.WebApi.Controllers.Project
{
    [Route("api/ProjectResourceBudgetHCHistory")]
    [PermissionTable(Name = "ProjectResourceBudgetHCHistory")]
    public partial class ProjectResourceBudgetHCHistoryController : ApiBaseController<IProjectResourceBudgetHCHistoryService>
    {
        public ProjectResourceBudgetHCHistoryController(IProjectResourceBudgetHCHistoryService service)
        : base(service)
        {
        }
    }
}

