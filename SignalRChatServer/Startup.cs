using Application.Db;
using Application.Services;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SignalRChatServer.Hubs;
using System;
using System.Linq;

namespace SignalRChatServer
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationServices();
            services.AddSignalR();
            services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());
            services.AddResponseCompression(opts =>
            {
                opts.MimeTypes =
                   ResponseCompressionDefaults.MimeTypes.Concat(
                       new[] { "application/octet-stream" });
            });

            // Use in-memony SQL database
            services.AddDbContext<GameDbContext>(opt => opt.UseInMemoryDatabase("InMemoryTestDb"));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseResponseCompression();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<GameHub>("/gamehub");
            });
        }
    }
}
