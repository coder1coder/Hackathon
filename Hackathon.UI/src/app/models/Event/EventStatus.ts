export enum EventStatus {
  Draft = 0,
  Published = 1,
  Started = 2,
  Development = 3,
  Prepare = 4,
  Presentation = 5,
  Decision = 6,
  Award = 7,
  Finished = 8,
}

export class EventStatusTranslator {
  public static GetName = (e:EventStatus) : string => EventStatus[e].toLowerCase();
  public static Translate = (e: EventStatus) : string => {
    switch (e) {
      case EventStatus.Draft: return 'Черновик'
      case EventStatus.Published: return 'Опубликовано'
      case EventStatus.Started: return 'Начато'
      case EventStatus.Development: return 'Разработка'
      case EventStatus.Prepare: return 'Подготовка'
      case EventStatus.Presentation: return 'Презентация'
      case EventStatus.Decision: return 'Выбор победителя'
      case EventStatus.Award: return 'Награждение'
      case EventStatus.Finished: return 'Завершено'

      default: return EventStatus[e];
    }
  }
}
