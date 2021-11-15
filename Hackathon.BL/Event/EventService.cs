using System;
using System.Threading.Tasks;
using FluentValidation;
using Hackathon.Common.Abstraction;
using Hackathon.Common.Exceptions;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Event;
using MapsterMapper;

namespace Hackathon.BL.Event
{
    public class EventService: IEventService
    {
        private readonly IMapper _mapper;
        private readonly IValidator<CreateEventModel> _createEventModelValidator;
        private readonly IEventRepository _eventRepository;

        public EventService(IMapper mapper, IValidator<CreateEventModel> createEventModelValidator,
            IEventRepository eventRepository)
        {
            _mapper = mapper;
            _createEventModelValidator = createEventModelValidator;
            _eventRepository = eventRepository;
        }
        public async Task<long> CreateAsync(CreateEventModel createEventModel)
        {
            await _createEventModelValidator.ValidateAndThrowAsync(createEventModel);

            var eventModel = _mapper.Map<EventModel>(createEventModel);
            return await _eventRepository.CreateAsync(eventModel);
        }

        public async Task<EventModel> GetAsync(long eventId)
        {
            return await GetEventThrowIfNotExist(eventId);
        }

        public async Task<BaseCollectionModel<EventModel>> GetAsync(GetFilterModel<EventFilterModel> getFilterModel)
        {
            if (getFilterModel.Page.HasValue && getFilterModel.Page < 1)
                throw new ServiceException("Номер страницы должен быть больше 0");

            if (getFilterModel.PageSize.HasValue && getFilterModel.PageSize < 1)
                throw new ServiceException("Размер страницы должен быть больше 0");

            return await _eventRepository.GetAsync(getFilterModel);
        }

        public async Task SetStatusAsync(long eventId, EventStatus eventStatus)
        {
            var eventModel = await GetEventThrowIfNotExist(eventId);
            eventModel.Status = eventStatus;
            await _eventRepository.UpdateAsync(eventModel);
        }

        public async Task DeleteAsync(long eventId)
        {
            await GetEventThrowIfNotExist(eventId);
            await _eventRepository.DeleteAsync(eventId);
        }

        public async Task SetStartMemberRegistrationAsync(long eventId, DateTime eventStartMemberRegistration)
        {
            var eventModel = await GetEventThrowIfNotExist(eventId);
            eventModel.StartMemberRegistration = eventStartMemberRegistration; 
            await _eventRepository.UpdateAsync(eventModel);
        }

        public async Task SetMinTeamMembersAsync(long eventId, int eventMinTeamMembers)
        {
            var eventModel = await GetEventThrowIfNotExist(eventId);
            eventModel.MinTeamMembers = eventMinTeamMembers; 
            await _eventRepository.UpdateAsync(eventModel);
        }

        public async Task SetMaxEventMembersAsync(long eventId, int eventMaxEventMembers)
        {
            var eventModel = await GetEventThrowIfNotExist(eventId);
            eventModel.MaxEventMembers = eventMaxEventMembers; 
            await _eventRepository.UpdateAsync(eventModel);
        }

        private async Task<EventModel> GetEventThrowIfNotExist(long eventId)
        {
            var eventModel = await _eventRepository.GetAsync(eventId);

            if (eventModel == null)
                throw new ServiceException("Событие с таким идентификатором не найдено");

            return eventModel;
        }
    }
}