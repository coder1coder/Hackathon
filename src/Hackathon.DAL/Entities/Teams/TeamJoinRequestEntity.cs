using System;
using Hackathon.Common.Abstraction;
using Hackathon.Common.Models.Teams;
using Hackathon.DAL.Entities.Users;

namespace Hackathon.DAL.Entities.Teams;

/// <summary>
/// Запрос на вступление в команду
/// </summary>
public class TeamJoinRequestEntity: BaseEntity, IHasCreatedAt, IHasModifyAt
{
    /// <summary>
    /// Идентификатор команды
    /// </summary>
    public long TeamId { get; set; }

    /// <summary>
    /// Сведения о команде
    /// </summary>
    public TeamEntity Team { get; set; }

    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// Пользователь, автор заявки
    /// </summary>
    public UserEntity User { get; set; }

    /// <summary>
    /// Статус запроса
    /// </summary>
    public TeamJoinRequestStatus Status { get; set; }

    /// <summary>
    /// Дата и время создания запроса
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Дата и время изменения запроса
    /// </summary>
    public DateTime? ModifyAt { get; set; }

    /// <summary>
    /// Комментарий
    /// </summary>
    public string Comment { get; set; }
}
