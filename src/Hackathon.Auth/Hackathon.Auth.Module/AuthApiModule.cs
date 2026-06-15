using Hackathon.API.Module;
using Hackathon.Auth.Abstraction;
using Hackathon.Auth.Abstraction.Configuration;
using Hackathon.Auth.BL;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hackathon.Auth.Module;

/// <summary>
/// Модуль авторизации и аутентификации.
/// </summary>
/// <remarks>
/// Модуль владеет своими настройками (<see cref="AuthenticateSettings"/>) и отвечает за
/// <b>выпуск</b> токена (sign-in). Конвейер <b>проверки</b> токена (JWT bearer middleware)
/// остаётся сквозным и настраивается централизованно в Startup, так как от него зависит
/// авторизация всех остальных модулей.
/// </remarks>
public class AuthApiModule: ApiModule
{
    public override void ConfigureServices(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.Configure<AuthenticateSettings>(configuration.GetSection("Auth"));
        serviceCollection.AddScoped<IAuthService, AuthService>();
    }
}
