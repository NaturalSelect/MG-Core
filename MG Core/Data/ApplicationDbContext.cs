using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MG_Core.Models;

namespace MG_Core.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public virtual DbSet<Block> Block { get; set; }
        public virtual DbSet<Post> Post { get; set; }
        public virtual DbSet<Reply> Reply { get; set; }
        public virtual DbSet<HomeItemViewModel> HomeItem { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Block>().ToTable("Block");
            builder.Entity<ApplicationUser>().ToTable("AspNetUsers");
            builder.Entity<Post>().ToTable("Post");
            builder.Entity<Reply>().ToTable("Reply");
            builder.Entity<HomeItemViewModel>().ToTable("HomeItem");
            //builder.Entity<ApplicationUser>().ToTable("");
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
        public void TryMigrate()
        {
            try
            {
                
                Database.Migrate();
            }
            catch(Exception e)
            {
                throw new Exception("在尝试迁移数据库的时候发生了错误!"+e.Source+e.Message+e.StackTrace);
            }
            
            
        }
    }
}
