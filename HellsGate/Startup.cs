using System.IO;
using System.Reflection;
using log4net;
using log4net.Config;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using HellsGate.Models;
using System;
using System.Diagnostics;

namespace HellsGate
{
    public class Startup
    {
        public EventHandler<MailEventArgs> SendMailEvent;
        public EventHandler<LogEventArgs> LogEvent;
        private LogEventArgs _logEvent;
        public Locator Locator { get; set; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            Locator = new Locator(this);
        }

        public void Log(TraceLevel p_Trace, string p_Message, MethodBase p_Method, Exception p_Ex = null)
        {
            _logEvent = new LogEventArgs(p_Trace, p_Message, p_Method, p_Ex);
            LogEvent?.Invoke(null, _logEvent);
        }

        public void SendMail(MailEventArgs p_mailEventArgs)
        {
            SendMailEvent?.Invoke(null, p_mailEventArgs);
        }
    }
}
