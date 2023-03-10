using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace MG_Core.Models
{
    public class AppsettingsReader
    {
        public static string Read(string Index)
        {
            var builer = new ConfigurationBuilder()
                .AddJsonFile(AppDomain.CurrentDomain.BaseDirectory+"/appsettings.json");
            var config = builer.Build();
            return  config["appsettings:"+Index];
        } 
    }
}
