import { Directive, ViewContainerRef } from '@angular/core';

@Directive({
  selector: '[event-item]'
})
export class EventDirective {

  constructor(
    public viewContainerRef: ViewContainerRef,
  ) { }

}
