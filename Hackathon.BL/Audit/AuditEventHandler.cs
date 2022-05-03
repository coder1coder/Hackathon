using System;
using System.Threading.Tasks;
using Hackathon.Abstraction.Audit;
using Hackathon.Abstraction.User;
using Hackathon.Common.Models.Audit;
using Hackathon.Entities;

namespace Hackathon.BL.Audit;

public class AuditEventHandler: IAuditEventHandler
{
    private readonly IAuditRepository _repository;
    private readonly IUserRepository _userRepository;

    public AuditEventHandler(
        IAuditRepository repository, 
        IUserRepository userRepository)
    {
        _repository = repository;
        _userRepository = userRepository;
    }

    public async Task Handle(AuditEventModel model)
    {
        var entity = new AuditEventEntity
        {
            Id = Guid.NewGuid(),
            Type = model.Type,
            Description = model.Description,
            Timestamp = model.Timestamp,
            UserId = model.UserId
        };

        if (model.UserId.HasValue)
        {
            var userEntity = await _userRepository.GetAsync(model.UserId.Value);
            entity.UserName = userEntity?.UserName;
        }
        
        await _repository.AddAsync(entity);
    }
}