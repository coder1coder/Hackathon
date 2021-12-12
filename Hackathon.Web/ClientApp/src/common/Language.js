export class Language {
}

Language.Event = {
    id: 'Идентификатор',
    name: 'Название события',
    start: 'Начало события',
    developmentMinutes: 'Продолжительность этапа разработки (минуты)',
    teamPresentationMinutes: 'Продолжительность этапа презентации командами (минуты)',
    memberRegistrationMinutes: 'Продолжительность регистрации участников (минуты)',
    minTeamMembers: 'Минимальное количество участников в команде',
    maxEventMembers: 'Максимальное количество участников',
    isCreateTeamsAutomatically: 'Создавать команды автоматически',
    changeEventStatusMessages: 'Сообщения смены статуса события'
}

Language.Team = {
    id: 'Идентификатор',
    name: 'Название команды',
    eventId: 'Идентификатор события',
    members: 'Список участников'
}