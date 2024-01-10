import { IStorageFile } from '../FileStorage/IStorageFile';

export interface IProject {
  imageId?: string;

  /** Наименование */
  name: string;

  /** Идентификатор мероприятия */
  description?: string;

  /** Идентификатор мероприятия */
  eventId: number;

  /** Идентификатор команды */
  teamId: number;

  /** Файлы проекта */
  files?: IStorageFile[];

  /** Ссылка на ветку GIT репозитория */
  linkToGitBranch?: string;
}
