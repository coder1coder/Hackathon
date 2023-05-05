export interface IProjectUpdateFromGitBranch
{
  /** Идентификатор мероприятия */
  eventId: number;

  /** Идентификатор команды */
  teamId: number;

  /* Ссылка на репозиторий с указанием ветки (https://github.com/coder1coder/Hackathon/tree/develop) */
  linkToGitBranch?: string;
}
