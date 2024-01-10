import { Observable, of } from 'rxjs';
import { computed, IComputedValue, Lambda, toJS } from 'mobx';
import { switchMap } from 'rxjs/operators';

export function fromMobx<T>(expression: () => T): Observable<T> {
  return new Observable((observer) => {
    const computedValue: IComputedValue<T> = computed(expression);
    const disposer: Lambda = computedValue.observe_((changes) => {
      observer.next(changes.newValue);
    }, true);

    return () => {
      disposer && disposer();
    };
  }).pipe(switchMap((value) => of(toJS(value as any))));
}
