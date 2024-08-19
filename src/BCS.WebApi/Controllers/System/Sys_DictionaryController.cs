using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BCS.Core.Controllers.Basic;
using BCS.Core.Extensions;
using BCS.Core.Filters;
using BCS.Business.IServices;

namespace BCS.System.Controllers
{
    [Route("api/Sys_Dictionary")]
    public partial class Sys_DictionaryController : ApiBaseController<ISys_DictionaryService>
    {
        public Sys_DictionaryController(ISys_DictionaryService service)
        : base("System", "System", "Sys_Dictionary", service)
        {
        }
    }
}
