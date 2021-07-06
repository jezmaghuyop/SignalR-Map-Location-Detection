using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SignalR.MapMarker.Hubs;

namespace SignalR.MapMarker
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options => options.AddPolicy("CorsPolicy", builder =>
                builder
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .SetIsOriginAllowed(hostName => true)
            ));


            services.AddSignalR(options =>
            {
                options.MaximumReceiveMessageSize = null;
                options.EnableDetailedErrors = true;
            });
         
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("CorsPolicy");

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<MapHub>("/map");
            });

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("This is a SignalR Demo");
            });
        }
    }
}
