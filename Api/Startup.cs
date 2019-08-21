using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Funq;
using ServiceStack;
using ServiceStack.Common;
using ServiceStack.VirtualPath;
using ServiceStack.Configuration;
using ServiceStack.Api.OpenApi;
using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Extensions.PlatformAbstractions;
using Api.ServiceInterface;
using System.Reflection;

namespace Api
{
    public class Startup : ModularStartup
    {
        public Startup(IConfiguration configuration) : base(configuration){}

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public new void ConfigureServices(IServiceCollection services)
        {
								}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

												app.UseServiceStack(new AppHost
												{
																AppSettings = new NetCoreAppSettings(Configuration)
												});
								}
    }

    public class AppHost : AppHostBase
    {
								private static string ver = Assembly.GetExecutingAssembly().GetName().Version.ToString();
								public AppHost() : base("API v" + ver, typeof(HelloServices).Assembly) { }

								// Configure your AppHost with the necessary configuration and dependencies your App needs
								public override void Configure(Container container)
        {
												SetConfig(new HostConfig
												{
																ApiVersion = "1.0",
																DefaultRedirectPath = "/metadata",
																DebugMode = AppSettings.Get(nameof(HostConfig.DebugMode), false),
																EnableFeatures = Feature.Json | Feature.Metadata,
																WebHostPhysicalPath = "~".MapServerPath(),
																GlobalResponseHeaders = new Dictionary<string, string> {
																				{ "Vary", "Accept" },
																				{ "X-Powered-By", "api" },
																},
																AllowFilePaths = new List<string>
																{
																				".well-known/**/*",        //LetsEncrypt
																},
																AddMaxAgeForStaticMimeTypes = new Dictionary<string, TimeSpan> {
																				{ "image/gif", TimeSpan.FromHours(1) },
																				{ "image/png", TimeSpan.FromHours(1) },
																				{ "image/jpeg", TimeSpan.FromHours(1) },
																},
																EnableOptimizations = true
												});
												Plugins.Add(new OpenApiFeature());
												Plugins.Add(new CorsFeature());
								}
    }
}
