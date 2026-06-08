import { Directive, ViewContainerRef, inject } from '@angular/core';

@Directive({ selector: '[event-item]' })
export class EventDirective {
  viewContainerRef = inject(ViewContainerRef);


}
