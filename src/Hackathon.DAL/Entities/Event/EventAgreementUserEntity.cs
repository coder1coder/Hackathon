using Hackathon.DAL.Entities.Users;

namespace Hackathon.DAL.Entities.Event;

public class EventAgreementUserEntity
{
    /// <summary>
    /// Идентификатор соглашения
    /// </summary>
    public long AgreementId { get; set; }
    
    /// <summary>
    /// Соглашение
    /// </summary>
    public EventAgreementEntity Agreement { get; set; }

    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public long UserId { get; set; }
    
    /// <summary>
    /// Пользователь
    /// </summary>
    public UserEntity User { get; set; }
}
