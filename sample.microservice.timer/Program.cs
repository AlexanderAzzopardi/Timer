using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Dapr.Client;
using Microsoft.AspNetCore.Mvc;

namespace sample.microservice.timer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            RedisClear();
            CreateHostBuilder(args).Build().Run();
        }

        public static async void RedisClear()
        {
            var client = new DaprClientBuilder().Build();
            await client.SaveStateAsync("statestore", "threadsOnline", 0);
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
