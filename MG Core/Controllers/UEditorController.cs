using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UEditorNetCore;

namespace MG_Core.Controllers
{
    [Route("api/[controller]")]
    public class UEditorController : Controller
    {
        private UEditorService Service;
        public UEditorController(UEditorService service)
        {
            Service = service;
        }
        public void Do()
        {
            Service.DoAction(this.HttpContext);
        }
    }
}