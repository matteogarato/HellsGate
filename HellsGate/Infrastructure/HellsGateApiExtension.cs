using HellsGate.Services;
using HellsGate.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace HellsGate.Infrastructure
{
    public static class HellsGateApiExtension
    {
        public static void AddHellsGateApi(this IServiceCollection services)
        {
            services.AddSingleton<IAccessManagerService, AccessManagerService>();
            services.AddSingleton<IAsyncHelperService, AsyncHelperService>();
            services.AddSingleton<IAutorizationManagerService, AutorizationManagerService>();
            //services.AddSingleton<ILoginManagerService, LoginManagerService>();
            services.AddSingleton<IMenuService, MenuService>();
            services.AddSingleton<ISecurLibService, SecurLibService>();
        }

        public static void AddHellsGateApi(this IApplicationBuilder app)
        {
            //app.UseResponseCompression();
        }
    }
}