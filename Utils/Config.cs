using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace protecta.laft.api.Utils
{
    public class Config
    {
        public static IConfiguration AppSetting = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();
    }
}