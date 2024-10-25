using Hackathon.Common.Extensions;
using Microsoft.AspNetCore.SignalR;

namespace Hackathon.IntegrationEvents;

public class IntegrationEventsUserIdProvider: IUserIdProvider
{
    public string GetUserId(HubConnectionContext connection)
        => connection.GetHttpContext()?.User?.GetUserId()?.ToString();
}
