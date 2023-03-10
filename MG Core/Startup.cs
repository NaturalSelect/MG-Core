using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using MG_Core.Data;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.Mvc;
using MG_Core.Models;
using MG_Core.Services;
using Microsoft.AspNetCore.ResponseCompression;
using UEditorNetCore;
using System.IO.Compression;

namespace MG_Core
{
    public class Startup
    {
        private readonly IdentitySetting identitySetting;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            identitySetting = new IdentitySetting();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            switch (identitySetting.GetDbType())
            {
                case DbType.MS_SqlServer:
                    services.AddDbContext<ApplicationDbContext>((o) =>
                    {
                        o.UseSqlServer(Configuration.GetConnectionString("Sql server Connection"));
                    });
                    break;
                case DbType.MySql:
                    services.AddDbContext<ApplicationDbContext>((o) =>
                    {
                        o.UseMySql(Configuration.GetConnectionString("Mysql Connection"));
                    });
                    break;
                default:
                    services.AddDbContext<ApplicationDbContext>((o) =>
                    {
                        o.UseSqlServer(Configuration.GetConnectionString("Sql server Connection"));
                    });
                    break;
            }
            services.AddResponseCompression(o=> {
                o.Providers.Add<GzipCompressionProvider>();
                o.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "image/svg+xml" });
            });
            services.Configure<GzipCompressionProviderOptions>(o=> {
                o.Level = CompressionLevel.Optimal;
            });
            services.Configure<IdentityOptions>((o) =>
            {
                //当密码连错10次时锁定其账户10分钟
                o.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                o.Lockout.MaxFailedAccessAttempts = 5;
                o.Password.RequireUppercase = false;
                
            });
            services.AddIdentity<ApplicationUser, IdentityRole>((o)=>{ o.SignIn.RequireConfirmedEmail = true; })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddSingleton<HtmlEncoder>(HtmlEncoder.Create(allowedRanges:
                    new[] {UnicodeRanges.CjkUnifiedIdeographs,UnicodeRanges.BasicLatin }
                ));
            services.AddTransient<IdentitySetting>();
            services.AddUEditorService();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
                
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            //if (identitySetting.AllRequestUseSSL())
            //{
            //    var ssl = new RewriteOptions().AddRedirectToHttps();
            //    app.UseRewriter(ssl);
            //}
            app.UseResponseCompression();
            app.UseStaticFiles();
            app.UseAuthentication();
            
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
