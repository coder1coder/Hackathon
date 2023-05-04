import { Observable, of } from 'rxjs';
import { computed, toJS } from 'mobx';
import { switchMap } from 'rxjs/operators';

export function fromMobx<T>( expression: () => T ) : Observable<T> {

  return new Observable(observer => {
    const computedValue = computed(expression);
    const disposer = computedValue.observe_(changes => {
      observer.next(changes.newValue);
    }, true);

    return () => {
      disposer && disposer();
    }
  }).pipe(switchMap((value) => of(toJS(value as any))));
}
