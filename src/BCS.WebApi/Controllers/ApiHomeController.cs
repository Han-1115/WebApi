using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BCS.Core.EFDbContext;
using Microsoft.AspNetCore.Authorization;

namespace BCS.WebApi.Controllers
{
    [AllowAnonymous]
    public class ApiHomeController : Controller
    {
        public IActionResult Index()
        {
            
            return new RedirectResult("/swagger/");
        }

    }
}