using System.Collections.Generic;

namespace Hackathon.Common.ErrorMessages;

public static class TeamMessages
{
    public const string UserAlreadyOwnerOfTeam = "Пользователь уже является владельцем команды";
    public const string UserIsNotOnTeam = "Пользователь не состоит в команде";
    public const string CreateTeamAccessDenied = "Создать команду для события может только владелец события";
    public const string UserAlreadyIsTheTeamMember = "Пользователь уже является участником команды";
    public const string SelectedTeamIsNotPublic = "Команда не является публичной";
    public const string MaximumNumberTeamMembersReached = "Достигнуто максимальное количество участников в команде";
    public const string TeamDoesNotExists = "Команда не существует";
    public const string TeamIsFull = "Команда содержит максимально количество участников";
}
