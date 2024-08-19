/*
 *代码由框架生成,任何更改都可能导致被代码生成器覆盖
 *如果要增加方法请在当前目录下Partial文件夹SubcontractingContractPaymentPlanController编写
 */
using Microsoft.AspNetCore.Mvc;
using BCS.Core.Controllers.Basic;
using BCS.Entity.AttributeManager;
using BCS.Business.IServices;
namespace BCS.Business.Controllers
{
    [Route("api/SubcontractingContractPaymentPlan")]
    [PermissionTable(Name = "SubcontractingContractPaymentPlan")]
    public partial class SubcontractingContractPaymentPlanController : ApiBaseController<ISubcontractingContractPaymentPlanService>
    {
        public SubcontractingContractPaymentPlanController(ISubcontractingContractPaymentPlanService service)
        : base(service)
        {
        }
    }
}

