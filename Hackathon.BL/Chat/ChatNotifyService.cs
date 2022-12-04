using System.Linq;
using System.Threading.Tasks;
using Hackathon.Abstraction.Chat;
using Hackathon.Abstraction.IntegrationEvents;
using Hackathon.Abstraction.Notification;
using Hackathon.Abstraction.Team;
using Hackathon.Abstraction.User;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Chat;
using Hackathon.Common.Models.Event;
using Hackathon.Common.Models.Notification;
using Hackathon.Entities;
using Hackathon.IntegrationEvents;
using Hackathon.IntegrationEvents.IntegrationEvent;

namespace Hackathon.BL.Chat;

public class ChatNotifyService : IChatNotifyService
{
    private readonly ITeamRepository _teamRepository;
    private readonly INotificationService _notificationService;

    public ChatNotifyService(
        ITeamRepository teamRepository,
        INotificationService notificationService)
    {
        _teamRepository = teamRepository;
        _notificationService = notificationService;
    }

    /// <inheritdoc cref="IChatNotifyService.SendMessage"/>
    public async Task SendMessage(CreateTeamChatMessage createTeamChatMessage)
    {
        var team = await _teamRepository.GetAsync(createTeamChatMessage.TeamId);
        
        // логика отправки уведомлений для тех кто в команде
        var usersIds = team.Members
                .Where(x=> x.Id != createTeamChatMessage.OwnerId)
                .Select(x => x.Id)
                .ToArray();

        if (usersIds.Any())
        {
            var notificationModels = usersIds.Select(x =>
                NotificationFactory.InfoNotification(createTeamChatMessage.Message, x));

            await _notificationService.PushMany(notificationModels);
        }
    }
}
