export enum EventStatus {
  Draft = 0,
  OnModeration = 3,
  Published = 1,
  Started = 2,
  Finished = 8,
}

export class EventStatusTranslator {
  public static GetName = (e:EventStatus) : string => EventStatus[e].toLowerCase();
  public static Translate = (e: EventStatus) : string => {
    switch (e) {
      case EventStatus.Draft: return 'Черновик'
      case EventStatus.OnModeration: return 'На модерации'
      case EventStatus.Published: return 'Опубликовано'
      case EventStatus.Started: return 'Начато'
      case EventStatus.Finished: return 'Завершено'

      default: return EventStatus[e];
    }
  }
}
