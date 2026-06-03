import { Observable } from 'rxjs';
import { autorun, toJS } from 'mobx';

export function fromMobx<T>(expression: () => T): Observable<T> {
  return new Observable<T>((observer) => {
    // autorun сразу выполнит expression и будет следить за изменениями
    const disposer = autorun(() => {
      try {
        const rawValue = expression();
        // Преобразуем в чистый JS (toJS) и отправляем в поток RxJS
        observer.next(toJS(rawValue));
      } catch (error) {
        observer.error(error);
      }
    });

    // Возвращаем функцию отписки
    return () => disposer();
  });
}
