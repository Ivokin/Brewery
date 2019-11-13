using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(Brewery.Web.Areas.Identity.IdentityHostingStartup))]
namespace Brewery.Web.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}