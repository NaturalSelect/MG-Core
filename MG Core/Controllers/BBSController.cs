using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MG_Core.Models;
using Microsoft.AspNetCore.Identity;
using MG_Core.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using System.Text.Encodings.Web;
using System.IO;

namespace MG_Core.Controllers
{
    public class BBSController : Controller
    {
        private readonly BBSDbConnect connect;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly HtmlEncoder Encoder;
        public BBSController(ApplicationDbContext context,UserManager<ApplicationUser> manager, HtmlEncoder encoder)
        {
            Encoder = encoder;
            connect = new BBSDbConnect(context);
            userManager = manager;
        }
        
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.BlockList = connect.GetBlockList();
            return View();
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult AddBlock()
        {
            return View(new BlockViewModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> AddBlock(BlockViewModel model)
        {
            var u = await userManager.FindByIdAsync(model.PowerUserId);
            if (u == null)
            {
                ModelState.AddModelError("", "找不到用户");
                return View(model);
            }
            var ImgPath = "/images/Block.jpg";
            if (model.Img != null)
            {
                var path = AppDomain.CurrentDomain.BaseDirectory + @"\wwwroot\";
                var a = model.Img.FileName.Split('.');
                var filetype = a[a.Count() - 1];
                if (filetype != "jpg" && filetype != "png")
                {
                    return RedirectToAction("Index");
                }
                var filename = DateTime.Now.GetHashCode() + "." + filetype;
                var LocalPath = path + @"\images\" + filename;
                var ServerPath = "/images/" + filename;
                using (FileStream fs = new FileStream(LocalPath, FileMode.Create))
                {
                    await model.Img.CopyToAsync(fs);
                    await fs.FlushAsync();
                }
                ImgPath = ServerPath;
            }
            var s = await connect.CreateBlock(new Block() {
           Name = model.Name,
           PowerUser = u,
           ImgPath = ImgPath,
           Introduction = model.Introduction
            });
            
            if (s != null)
            {
                ModelState.AddModelError("", s);
                return View(model);
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        [Authorize]
        [Route("/Block/{Id}")]
        public IActionResult GetBlockPost(string Id,int Page=1)
        {
            if (string.IsNullOrEmpty(Id))
            {
                return RedirectToAction("Index");
            }
            var b = connect.FindBlockById(Id);
            if(b == null)
            {
                return RedirectToAction("Index");
            }
            var p = connect.GetBlockAllPost(Id);
            if(p != null )
            {
                int? MaxPage = p.Count() / 10;
                if ((p.Count() % 10) != 0)
                {
                    MaxPage = MaxPage + 1;
                }
                ViewBag.MaxPage = MaxPage;
                if (Page > MaxPage)
                {
                    Page = 1;
                }
                ViewBag.NowPage = Page;
            }
            b.Post = p.ToArray();
            ViewBag.NowPage = Page;
            return View(b);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> RemoveBlock(string Id)
        {
            await connect.RemoveBlock(Id);
            return RedirectToAction("Index");
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult EditBlock(string Id)
        {
            ViewData["Id"] = Id;
            var b = connect.FindBlockById(Id);
            var u = connect.GetBlockPowerUser(Id);
            return View(new BlockViewModel() {Name=b.Name,PowerUserId = u.Id });
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBlock(BlockViewModel block,string Id)
        {
            var b = new Block();
            b.Name = block.Name;
            b.PowerUser = await userManager.FindByIdAsync(block.PowerUserId);
            if (block.Img != null)
            {
                var path = AppDomain.CurrentDomain.BaseDirectory + @"\wwwroot\";
                var a = block.Img.FileName.Split('.');
                var filetype = a[a.Count() - 1];
                if (!(filetype == "jpg" || filetype == "png"))
                {
                    return RedirectToAction("Index");
                }
                var filename = DateTime.Now.GetHashCode()+"."+filetype;
                var LocalPath = path + @"\images\"+filename;
                var ServerPath = "/images/" + filename;
                using (FileStream fs = new FileStream(LocalPath, FileMode.Create))
                {
                    await block.Img.CopyToAsync(fs);
                    await fs.FlushAsync();
                }
                b.ImgPath = ServerPath;
            }
            b.Id = Id;
            b.Introduction = block.Introduction;
            var s = await connect.EditBlock(b);
            if (!string.IsNullOrEmpty(s))
            {
                ViewData["Id"] = Id;
                ModelState.AddModelError("", s);
                return View(block);
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        [Authorize]
        public IActionResult AddPost(string BlockId)
        {
            ViewData["BlockId"] = BlockId;
            return View(new PostViewModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> AddPost(string BlockId,PostViewModel model)
        {
            var b = connect.FindBlockById(BlockId);
            if(b == null)
            {
                ModelState.AddModelError("", "ERROR:找不到版块");
                return View(model);
            }
            ViewData["BlockId"] = BlockId;
            Post p = new Post();
            p.Block = b;
            p.Time = DateTime.Now;
            p.Title = model.Tittle;
            p.Body = model.Body;
            p.User = await userManager.FindByNameAsync(User.Identity.Name);
            var s = await connect.AddPostToBlock(p,b.Id);
            if (!string.IsNullOrEmpty(s))
            {
                ModelState.AddModelError("", s);
                return View(model);
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        [Route("/Post/{PostId}")]
        public async Task<IActionResult> GetPost(int PostId)
        {
            var p = connect.FindPostById(PostId);
            if(p == null)
            {
                RedirectToAction("Index","Home");
            }
            if(p.Reply != null)
            {
                int? MaxPage = p.Reply.Count() / 10;
                if (p.Reply.Count() % 10 != 0)
                {
                    MaxPage = MaxPage + 1;
                    ViewBag.MaxPage = MaxPage;
                }
            }
            ViewBag.IsPowerUser = connect.IsBlockPowerUser((await userManager.FindByNameAsync(User.Identity.Name)), p.Block.Id);
            return View(p);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> RemovePost(int PostId,string ReturnUrl)
        {
            var BlockID = connect.FindBlockByPostId(PostId).Id;
            if (!User.IsInRole("Admin") && connect.IsBlockPowerUser((await userManager.FindByNameAsync(User.Identity.Name)),BlockID))
            {
                return StatusCode(410, "您无权这么做");
            }
            var s = await connect.RemovePostFormBlock(PostId,BlockID);
            if(s == null)
            {
                if (Url.IsLocalUrl(ReturnUrl))
                {
                    return RedirectToAction(ReturnUrl);
                }
                return RedirectToAction("Index");
            }
            return Content(s);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> AddReply( string Body,int PostId,string ReturnUrl)
        {
            Reply reply = new Reply();
            reply.Body = Body;
            if (string.IsNullOrEmpty(reply.Body))
            {
                ModelState.AddModelError("", "内容不能为空");
            }
            reply.UserName = await userManager.FindByNameAsync(User.Identity.Name);
            reply.Time = DateTime.Now;
            await connect.AddReplyToPost(reply, PostId);
            if (Url.IsLocalUrl(ReturnUrl))
            {
                return Redirect(ReturnUrl);
            }
            return RedirectToAction("Index","Home");
        }
        [HttpGet]
        public IActionResult AddRelpy(Reply model)
        {
            return PartialView(model);
        } 
        [HttpGet]
        public IActionResult ListPartia()
        {
            return PartialView(connect.GetBlockList());
        }
    }  
}