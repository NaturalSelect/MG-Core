using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MG_Core
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }
        
        public static IWebHost BuildWebHost(string[] args)
        {
            var b = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile(AppDomain.CurrentDomain.BaseDirectory+"/appsettings.json");
            var c = b.Build();
            return WebHost.CreateDefaultBuilder(args).ConfigureLogging((h, l) => { l.AddConfiguration(h.Configuration.GetSection("Logging")); l.AddConsole(); l.AddDebug(); })
                .UseUrls(c["appsettings:Url"])
                .UseStartup<Startup>()
                .Build();
        }
            
            
    }
}
