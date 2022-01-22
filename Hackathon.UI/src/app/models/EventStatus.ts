export enum EventStatus
{
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

export const EventStatusLabelMapping: Record<EventStatus, string> = {
  [EventStatus.Draft]: "Draft",
  [EventStatus.Published]: "Published",
  [EventStatus.Started]: "Started",
  [EventStatus.Development]: "Development",
  [EventStatus.Prepare]: "Prepare",
  [EventStatus.Presentation]: "Presentation",
  [EventStatus.Decision]: "Decision",
  [EventStatus.Award]: "Award",
  [EventStatus.Finished]: "Finished",
};
