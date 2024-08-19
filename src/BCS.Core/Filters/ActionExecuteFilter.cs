using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using BCS.Core.Enums;
using BCS.Core.Extensions;
using BCS.Core.ObjectActionValidator;
using BCS.Core.Services;
using BCS.Core.Utilities;

namespace BCS.Core.Filters
{
    public class ActionExecuteFilter : IActionFilter
    {

        public void OnActionExecuting(ActionExecutingContext context)
        {
            //验证方法参数
            context.ActionParamsValidator();
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }
    }
}