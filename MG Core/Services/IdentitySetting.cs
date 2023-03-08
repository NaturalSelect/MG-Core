using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;
using MG_Core.Models;

namespace MG_Core.Services
{
    public class IdentitySetting
    {
        private ConfigurationBuilder Builder = new ConfigurationBuilder();
        private readonly IConfigurationRoot Configuration;
        public IdentitySetting()
        {
            Builder.SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile(AppDomain.CurrentDomain.BaseDirectory + "appsettings.json");
            Configuration = Builder.Build();
        }
        public string GetEmailUserPassword()
        {
            return Configuration["Identity:Email:Password"];
        }
        public DbType GetDbType()
        {
            var Type = Configuration["Identity:DbType"];
            switch (Type.ToLower())
            {
                case "ms_sqlserver":
                    return DbType.MS_SqlServer;
                case "mysql":
                    return DbType.MySql;
            }
            return DbType.MS_SqlServer;
        }
        public string GetEmailAdress()
        {
            return Configuration["Identity:Email:EmailAdress"];
        }
        public string GetEmailServer()
        {
            return Configuration["Identity:Email:SMTP"];
        }
        public int GetSMTPPort()
        {
            int i;
            if (!int.TryParse(Configuration["Identity:Email:Port"],out i)){
                throw new Exception("邮箱端口配置错误");
            }
            return i;
        }
        public bool SMTPIsSSL()
        {
            bool b;
            if (!bool.TryParse(Configuration["Identity:Email:SSL"],out b)){
                throw new Exception("邮箱SSL配置错误");
            }
            return b;
        }
    }
}
