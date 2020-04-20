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
            services.AddScoped<IAccessManagerService, AccessManagerService>();
            services.AddScoped<IAsyncHelperService, AsyncHelperService>();
            services.AddScoped<IAutorizationManagerService, AutorizationManagerService>();
            //services.AddSingleton<ILoginManagerService, LoginManagerService>();
            services.AddScoped<IMenuService, MenuService>();
            services.AddScoped<ISecurLibService, SecurLibService>();
        }

        public static void AddHellsGateApi(this IApplicationBuilder app)
        {
            //app.UseResponseCompression();
        }
    }
}