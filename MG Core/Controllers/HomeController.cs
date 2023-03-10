using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MG_Core.Models;
using MG_Core.Data;

namespace MG_Core.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext Context;
        private readonly BBSDbConnect connect;
        public HomeController(ApplicationDbContext context)
        {
            Context = context;
            connect = new BBSDbConnect(context);
        }
        public IActionResult Index()
        {
            ViewBag.List = connect.GetBlockList();
            ViewData["QQ"] = AppsettingsReader.Read("QQ");
            ViewBag.NEWS = connect.GetNEWS();
            return View();
        }

        public IActionResult Error()
        {
            var error = new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier };
            return View(error);
        }
        public IActionResult TryMigrate()
        {
            Context.TryMigrate();
            return Content("OK");
        }
    }
}
