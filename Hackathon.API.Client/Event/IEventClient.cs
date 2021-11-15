﻿using System.Threading.Tasks;
using Hackathon.Common.Models.Event;
using Hackathon.Contracts.Requests.Event;
using Hackathon.Contracts.Responses;

namespace Hackathon.API.Client.Event
{
    public interface IEventApiClient
    {
        Task<BaseCreateResponse> Create(CreateEventRequest createEventRequest);
        Task SetStatus(SetStatusRequest<EventStatus> setStatusRequest);
        Task SetStartRegistration(SetStartRegistrationRequest setStartRegistrationRequest);
        Task SetMaxTeamMembers(SetMaxTeamMembersRequest setMaxTeamMembersRequest);
        Task SetMinMembers(SetMinTeamMembersRequest setMinTeamMembersRequest);
        Task<EventModel> Get(long id);
    }
}