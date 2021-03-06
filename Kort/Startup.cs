using DataLibrary;
using Kort.Data;
using Kort.MyStates;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Kort
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var connString = Configuration.GetConnectionString("default");
            if (connString == null)
                throw new ArgumentNullException("Connection string cannot be null");

            services.AddMvc(options => options.EnableEndpointRouting = false)
                        .SetCompatibilityVersion(CompatibilityVersion.Version_3_0); // ----Controller

            services.AddSignalR();
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddSingleton<WeatherForecastService>();

            services.AddSingleton<IDataAccess>(s => new DataAccess(connString));
            services.AddSingleton<MySingletonState>();
            services.AddScoped<MyAppState>();
            services.AddTransient<IMailService, MailService>();

            services.AddDevExpressBlazor();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                ///app.UseHsts();
            }

            ///app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseMvcWithDefaultRoute();       // ----Controller

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapHub<Hubs.DataHub>("/DataHub");
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
