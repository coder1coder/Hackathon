using System;
using System.Collections.Generic;
using Hackathon.Configuration;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hackathon.API.Module;

public interface IApiModule
{
    IList<Func<IServiceProvider, DbContext>> RegisteredDbContextFactories { get; }

    void ConfigureServices(IServiceCollection serviceCollection, IConfiguration configuration);

    void ConfigureEndpoints(IEndpointRouteBuilder endpointRouteBuilder, AppSettings appSettings);
}
