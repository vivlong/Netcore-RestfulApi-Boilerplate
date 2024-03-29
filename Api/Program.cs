﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ServiceStack;

namespace Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
												.UseContentRoot(Directory.GetCurrentDirectory())
																.ConfigureAppConfiguration((context, config) =>
																{
																				var env = context.HostingEnvironment;
																				config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
																								.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
																				config.AddEnvironmentVariables();
																})
																.ConfigureLogging((hostingContext, logging) =>
																{
																				logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
																				logging.AddConsole();
																				logging.AddDebug();
																})
																.UseStartup<Startup>()
																.Build();
    }
}
