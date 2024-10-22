using Hackathon.Common.Abstraction.Auth;
using Microsoft.AspNetCore.SignalR;

namespace Hackathon.IntegrationEvents;

public class IntegrationEventsUserIdProvider: IUserIdProvider
{
    private readonly IAuthorizedUserContext _authorizedUserContext;

    public IntegrationEventsUserIdProvider(IAuthorizedUserContext authorizedUserContext)
    {
        _authorizedUserContext = authorizedUserContext;
    }

    public string GetUserId(HubConnectionContext connection)
        => _authorizedUserContext.GetAuthorizedUser()?.Id.ToString();
}
