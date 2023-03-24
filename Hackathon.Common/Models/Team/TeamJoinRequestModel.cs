using Hackathon.Common.Abstraction;
using System;

namespace Hackathon.Common.Models.Team;

/// <summary>
/// Запрос на вступление в команду
/// </summary>
public class TeamJoinRequestModel: TeamJoinRequestParameters, IHasCreatedAt, IHasModifyAt
{
    /// <summary>
    /// Идентификатор запроса
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Наименование команды
    /// </summary>
    public string TeamName { get; set; }

    /// <summary>
    /// Наименование пользователя, автора заявки
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// Дата и время создания запроса
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Дата и время изменения запроса
    /// </summary>
    public DateTime? ModifyAt { get; set; }
}
