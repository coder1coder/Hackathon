using System;
using System.Collections.Generic;
using System.Reflection;
using Hackathon.Configuration;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hackathon.API.Module;

/// <summary>
/// Модуль API.
/// </summary>
public interface IApiModule
{
    IList<Func<IServiceProvider, DbContext>> RegisteredDbContextFactories { get; }

    /// <summary>
    /// Сборка с подписчиками.
    /// </summary>
    /// <returns>NULL в случае, если модуль не содержит подписчиков.</returns>
    Assembly ConsumersAssembly { get; }

    void ConfigureServices(IServiceCollection serviceCollection, IConfiguration configuration);

    void ConfigureEndpoints(IEndpointRouteBuilder endpointRouteBuilder, AppSettings appSettings);
}
