import { Component } from '@angular/core';
import { DefaultLayoutComponent } from '../layouts/default/default.layout.component';

@Component({
    selector: 'not-found',
    templateUrl: './not-found.component.html',
    styleUrls: ['./not-found.component.scss'],
    imports: [DefaultLayoutComponent]
})
export class NotFoundComponent {}
