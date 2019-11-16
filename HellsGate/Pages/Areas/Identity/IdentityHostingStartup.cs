using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(HellsGate.Areas.Identity.IdentityHostingStartup))]

namespace HellsGate.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}