using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MG_Core.Models
{
    [ViewComponent(Name ="BlockList")]
    public class BlockComponent:ViewComponent
    {
        private readonly BBSDbConnect connect;
        public BlockComponent(MG_Core.Data.ApplicationDbContext context) {
            connect = new BBSDbConnect(context);
        }
        public IViewComponentResult Invoke() {
            return View(connect.GetBlockList());
        }
    }
}
