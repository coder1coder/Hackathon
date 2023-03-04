using Hackathon.API;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace Hackathon.Tests.Integration;

public class TestWebApplicationFactory : WebApplicationFactory<Startup>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(x =>
            x.AddUserSecrets<Program>(true));

        builder.UseEnvironment("Tests");

        base.ConfigureWebHost(builder);
        // builder.ConfigureKestrel(x => x.ListenLocalhost(7100));
    }
}
