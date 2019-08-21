using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Funq;
using ServiceStack;
using ServiceStack.Common;
using ServiceStack.VirtualPath;
using ServiceStack.Configuration;
using ServiceStack.Auth;
using ServiceStack.Caching;
using ServiceStack.Api.OpenApi;
using ServiceStack.Data;
using ServiceStack.Logging;
using ServiceStack.Logging.NLogger;
using ServiceStack.OrmLite;
using ServiceStack.Redis;
using ServiceStack.Text;
using Api.ServiceInterface;

namespace Api
{
				public class AppHost : AppHostBase, IConfigureApp
				{
								private static string ver = Assembly.GetExecutingAssembly().GetName().Version.ToString();
								public AppHost() : base("API v" + ver, typeof(HelloServices).Assembly)
								{
												LogManager.LogFactory = new NLogFactory();
								}

								public void Configure(IApplicationBuilder app)
								{
												app.UseServiceStack(new AppHost
												{
																AppSettings = new NetCoreAppSettings(Configuration)
												});
								}

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
												Plugins.Add(new OpenApiFeature
												{
																// LogoUrl = string.Empty,
																DisableAutoDtoInBodyParam = true
												});
												Plugins.Add(new PostmanFeature());
												Plugins.Add(new CorsFeature(
																allowedHeaders: "Content-Type,Authorization"
												));
												// 开启 auto query 特性
												Plugins.Add(new AutoQueryFeature
												{
																MaxLimit = 100,
																IncludeTotal = true,
																EnableAutoQueryViewer = false
												});
												//var redisManager = container.Resolve<IRedisClientsManager>();
								}

								public override void Configure(IServiceCollection services)
								{
												//services.AddSingleton<IRedisClientsManager>(
												//				new RedisManagerPool(Configuration.GetConnectionString("redis")));
								}

								public override void OnExceptionTypeFilter(Exception ex, ResponseStatus responseStatus)
								{
												var log = LogManager.GetLogger("Exception Handlers");
												log.Error($"{responseStatus.ToJson()}", ex);
												base.OnExceptionTypeFilter(ex, responseStatus);
								}
				}
}
