using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            Database.SetConnInfo(@"DataSource=database.db");
            /*Database.AddDrugEntry(new Shared.DrugEntry()
            {
                DrugName = "Co-codamol",
                Time = new DateTime(2000, 10, 15, 10, 10, 10, 300)
            });*/

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
