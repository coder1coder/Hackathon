import { Directive, ViewContainerRef } from '@angular/core';

@Directive({
    selector: '[event-item]',
    standalone: false
})
export class EventDirective {
  constructor(public viewContainerRef: ViewContainerRef) {}
}
