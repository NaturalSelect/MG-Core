using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MG_Core.Data;

namespace MG_Core.Models
{
    [ViewComponent(Name ="HomeItem")]
    public class HomeItemComponent:ViewComponent
    {
        private readonly ApplicationDbContext Context;
        public HomeItemComponent( ApplicationDbContext context)
        {
            Context = context;
        }
        public IViewComponentResult Invoke()
        {
            return View(Context.HomeItem.ToArray());
        }
    }
}
