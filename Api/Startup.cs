using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using ServiceStack;
using Microsoft.AspNetCore.Http;

namespace Api
{
    public class Startup : ModularStartup
    {
        public Startup(IConfiguration configuration) : base(configuration){}

        public new void ConfigureServices(IServiceCollection services)
        {
												services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
												services.AddMemoryCache();
								}

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
												}
								}
    }

}
