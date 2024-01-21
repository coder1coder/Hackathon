using Hackathon.Common.Models.User;
using Hackathon.DAL.Entities.Interfaces;
using System;
using System.Collections.Generic;
using Hackathon.DAL.Entities.Event;
using Hackathon.DAL.Entities.Block;

namespace Hackathon.DAL.Entities.User;

/// <summary>
/// Пользователь
/// </summary>
public class UserEntity: BaseEntity, ISoftDeletable
{
    /// <summary>
    /// Логин пользователя
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// Пароль пользователя
    /// </summary>
    public string PasswordHash { get; set; }

    /// <summary>
    /// Электронная почта пользователя
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Запрос на подтверждение Email
    /// </summary>
    public EmailConfirmationRequestEntity EmailConfirmationRequest { get; set; }

    /// <summary>
    /// Полное наименование пользователя
    /// </summary>
    public string FullName { get; set; }

    /// <summary>
    /// Идентификатор пользователя социальной сети Google
    /// </summary>
    public string GoogleAccountId { get; set; }

    /// <summary>
    /// Сведения о Google аккаунте пользователя
    /// </summary>
    public GoogleAccountEntity GoogleAccount { get; set; }

    /// <summary>
    /// Роль пользователя
    /// </summary>
    public UserRole Role { get; set; }

    /// <summary>
    /// Команды в которых участвует пользователь
    /// </summary>
    public List<MemberTeamEntity> Teams { get; set; } = new ();

    /// <summary>
    /// Запросы на вступление в команду
    /// </summary>
    public ICollection<TeamJoinRequestEntity> JoinRequests { get; set; } = new List<TeamJoinRequestEntity>();

    /// <summary>
    /// Принятые соглашения участия в мероприятии
    /// </summary>
    public ICollection<EventAgreementEntity> EventAgreements { get; set; } = new List<EventAgreementEntity>();

    /// <summary>
    /// Блокировка пользователя
    /// </summary>
    public BlockingEntity Block { get; set; }

    /// <summary>
    /// Идентификатор аватара профиля в файловом хранилище
    /// </summary>
    public Guid? ProfileImageId { get; set; }

    /// <summary>
    /// Признак удаления
    /// </summary>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Получить одно из имеющихся имен
    /// </summary>
    /// <returns></returns>
    public string GetAnyName()
        => FullName ?? UserName;
}
