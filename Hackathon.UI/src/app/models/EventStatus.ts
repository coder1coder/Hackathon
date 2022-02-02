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

export enum TranslatedEventStatus
{
  Черновик = 0,
  Опубликовано = 1,
  Началось = 2,
  Разработка = 3,
  Подготовка = 4,
  Презентация = 5,
  'Выбор победителя' = 6,
  Награждение = 7,
  Закончено = 8,
}

export class Status{

  public static getTranslatedStatus = (e: EventStatus | undefined) : TranslatedEventStatus => {
    if ( e == 0) {
      return  TranslatedEventStatus.Черновик;}
    if ( e == 1) {
      return  TranslatedEventStatus.Опубликовано;}
    if ( e == 2) {
      return  TranslatedEventStatus.Началось;}
    if ( e == 3) {
      return  TranslatedEventStatus.Разработка;}
    if ( e == 4) {
      return  TranslatedEventStatus.Подготовка;}
    if ( e == 5) {
      return  TranslatedEventStatus.Презентация;}
    if ( e == 6) {
      return  TranslatedEventStatus["Выбор победителя"];}
    if ( e == 7) {
      return  TranslatedEventStatus.Награждение;}
    else
      return  TranslatedEventStatus.Закончено;
  }

  public static getTranslatedStatusValue(status:TranslatedEventStatus){
    return TranslatedEventStatus[status];
  }

}




