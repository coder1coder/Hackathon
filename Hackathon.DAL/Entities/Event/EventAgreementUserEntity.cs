using Hackathon.DAL.Entities.User;

namespace Hackathon.DAL.Entities.Event;

public class EventAgreementUserEntity
{
    /// <summary>
    /// Идентификатор соглашения
    /// </summary>
    public long AgreementId { get; set; }
    public EventAgreementEntity Agreement { get; set; }

    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public long UserId { get; set; }
    public UserEntity User { get; set; }
}
