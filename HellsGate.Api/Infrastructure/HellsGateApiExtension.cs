using HellsGate.Models.Context;
using HellsGate.Models.DatabaseModel;
using HellsGate.Services;
using HellsGate.Services.Interfaces;
using log4net;
using log4net.Config;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Reflection;

namespace HellsGate.Infrastructure
{
    public static class HellsGateApiExtension
    {
        public static void AddHellsGateApi(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddDbContext<HellsGateContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("HellsGateContext")));
            //services.AddDefaultIdentity<PeopleAnagraphicModel>(options => options.SignIn.RequireConfirmedAccount = true)
            //    .AddEntityFrameworkStores<HellsGateContext>();
            services.AddScoped<IAccessManagerService, AccessManagerService>();
            services.AddScoped<IAsyncHelperService, AsyncHelperService>();
            services.AddScoped<IAutorizationManagerService, AutorizationManagerService>();
            services.AddScoped<INodeService, NodeService>();
            //services.AddSingleton<ILoginManagerService, LoginManagerService>();
            services.AddScoped<IMenuService, MenuService>();
            services.AddScoped<ISecurLibService, SecurLibService>();

            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            //Locator = new Locator();
        }
    }
}