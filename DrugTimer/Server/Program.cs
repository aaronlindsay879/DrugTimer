using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DrugTimer.Server.Communication;
using DrugTimer.Server.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DrugTimer.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var sharedClient = new HttpClient();
            Database.SetConnInfo(@"DataSource=database.db");
            Discord.HttpClient = sharedClient;


            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
