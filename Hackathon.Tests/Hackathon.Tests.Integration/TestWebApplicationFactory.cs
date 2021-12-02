using Hackathon.API;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Hackathon.Tests.Integration
{
    public class TestWebApplicationFactory : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Tests.Unit");
            base.ConfigureWebHost(builder);
            builder.UseEnvironment("Tests.Unit");

            // builder.ConfigureKestrel(x => x.ListenLocalhost(7100));
        }
    }
}