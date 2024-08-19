using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using BCS.Core.Const;
using BCS.Core.EFDbContext;
using BCS.Core.Enums;
using BCS.Core.Extensions;
using BCS.Core.ManageUser;
using BCS.Core.Services;

namespace BCS.Core.Middleware
{
    public class ExceptionHandlerMiddleWare
    {
        private readonly RequestDelegate next;
        public ExceptionHandlerMiddleWare(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                (context.RequestServices.GetService(typeof(ActionObserver)) as ActionObserver).RequestDate = DateTime.Now;
                await next(context);
                Logger.Info(LoggerType.System);
            }
            catch (Exception exception)
            {
                var env = context.RequestServices.GetService(typeof(IWebHostEnvironment)) as IWebHostEnvironment;
                string message = exception.Message + exception.InnerException;
                Logger.Error(LoggerType.Exception, message);
                //if (!env.IsDevelopment())
                //{
                //    message = "服务器处理异常";
                //}
                //else
                //{
                //    Console.WriteLine($"服务器处理出现异常:{message}");
                //}
                context.Response.StatusCode = 500;
                context.Response.ContentType = ApplicationContentType.JSON;
                await context.Response.WriteAsync(new { message, status = false }.Serialize(), Encoding.UTF8);
            }
        }
    }
}
