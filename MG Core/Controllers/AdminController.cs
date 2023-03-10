using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using MG_Core.Models;
using MG_Core.Data;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace MG_Core.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private ApplicationDbContext Context { get; set; }
        private UserManager<ApplicationUser> UserManager { get; set; }
        private SignInManager<ApplicationUser> SignInManager { get; set; }
        private BBSDbConnect Connect {get; set;}
        public AdminController(ApplicationDbContext context,UserManager<ApplicationUser> manager,SignInManager<ApplicationUser> signIn)
        {
            Context = context;
            UserManager = manager;
            SignInManager = signIn;
            Connect = new BBSDbConnect(context);

        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<UserViewModel> list = new List<UserViewModel>();
            foreach(var u in Context.Users)
            {
                list.Add(new UserViewModel() { Id = u.Id, E_mail = u.Email, Lock = u.LockoutEnd.ToString(), Name = u.UserName, Role = (await UserManager.GetRolesAsync(u)).FirstOrDefault(),DisplayName = u.DisplayName,EmailConfirmed=u.EmailConfirmed });
            }
            ViewBag.List = list;
            ViewBag.HomeItem = Context.HomeItem.ToArray();
            List<string> FileList = new List<string>();
            var Files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + @"\wwwroot\homeitem\");
            foreach(var s in Files)
            {
               var File ="/homeitem/" + s.Split('\\').Last();
                FileList.Add(File);
            }
            ViewBag.FileList = FileList.ToArray();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddHomeItem(HomeItemViewModel model)
        {
            Context.HomeItem.Add(model);
            await Context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult AddHomeItem()
        {
            return View(new HomeItemViewModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveHomeItem(string Id)
        {
            var q = from c in Context.HomeItem
                    where c.Id == Id
                    select c;
            if(q.Count() != 0)
            {
                Context.HomeItem.Remove(q.First());
                await Context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(string UserId)
        {
            var u = await UserManager.FindByIdAsync(UserId);
            if(u == null || u.UserName == User.Identity.Name)
            {
                return RedirectToAction("Index");
            }
            var q = from c in Context.Users.Where(x=>x.Id == UserId)
                    join b in Context.Block
                    on c.Id equals b.Id
                    select b;
            var a = from f in Context.Users.Where(x=>x.Id == UserId)
                    join p in Context.Post
                    on f.Id equals p.User.Id
                    select p;
            var g = from h in Context.Users.Where(x => x.Id == UserId)
                    join r in Context.Reply
                    on h.Id equals r.UserName.Id
                    select r;
            foreach (var i in q)
            {
                i.PowerUser = await UserManager.FindByNameAsync(User.Identity.Name);
                Context.Block.Update(i);
            }
            foreach (var i in a)
            {
                Context.Post.Remove(i);
            }
            foreach (var i in g)
            {
                Context.Reply.Remove(i);
            }
            Context.Users.Remove(u);
            await Context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LockUser(string UserId)
        {
            var u = await UserManager.FindByIdAsync(UserId);
            if (u.UserName == User.Identity.Name)
            {
                return RedirectToAction("Index");
            }
            await UserManager.SetLockoutEndDateAsync(u,new DateTimeOffset(new DateTime(DateTime.Now.Year,DateTime.Now.Month+1,DateTime.Now.Day-1)));
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UnLockUser(string UserId)
        {
            await UserManager.SetLockoutEndDateAsync(await UserManager.FindByIdAsync(UserId),null);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> ChangeUserRole(string UserId)
        {
            var u = await UserManager.FindByIdAsync(UserId);
            return View(new UserRoleViewModel() {UserId = u.Id,UserName = u.UserName });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeUserRole(UserRoleViewModel model)
        {
            var u = await UserManager.FindByIdAsync(model.UserId);
            var q = from c in Context.Roles
                    where c.NormalizedName == model.RoleId
                    select c;
            if (q.Count() < 1)
            {
                ModelState.AddModelError("", "找不到Role");
                return View(model);
            }
            if (u == null)
            {
                ModelState.AddModelError("", "找不到指定User");
                return View(model);
            }
            IList<string> list = await UserManager.GetRolesAsync(u);
            if (list.Count > 1)
            {
                foreach (var i in list)
                {
                    await UserManager.RemoveFromRoleAsync(u, i);
                }
            }
            await UserManager.AddToRoleAsync(u, model.RoleId);
            await UserManager.UpdateAsync(u);
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpImg(IFormFile file)
        {
            var s = file.FileName.Split('.');
            var FileType = s[s.Length - 1].ToLower();
            var FileName = DateTime.Now.GetHashCode();
            var LocalPath = AppDomain.CurrentDomain.BaseDirectory + @"\wwwroot\homeitem\"+FileName+"."+FileType;
            if(FileType == "png"||FileType == "jpg"||FileType == "svg")
            {
                using (FileStream fs = new FileStream(LocalPath, FileMode.Create))
                {
                    await file.CopyToAsync(fs);
                    await fs.FlushAsync();
                }
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveImg(string FileName)
        {
            var Path = AppDomain.CurrentDomain.BaseDirectory + @"\wwwroot\homeitem\" + FileName;
            System.IO.File.Delete(Path);
            return RedirectToAction(nameof(Index));
        }
    }
}