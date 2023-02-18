using Hackathon.API;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Hackathon.Tests.Integration;

public class TestWebApplicationFactory : WebApplicationFactory<Startup>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Tests");

        base.ConfigureWebHost(builder);
        // builder.ConfigureKestrel(x => x.ListenLocalhost(7100));
    }
}