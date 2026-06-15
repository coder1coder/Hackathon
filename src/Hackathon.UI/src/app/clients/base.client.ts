import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { inject } from '@angular/core';

export abstract class BaseApiClient {
  protected http = inject(HttpClient);
  protected api: string = environment.api;
  protected baseRoute: string;

  protected constructor(route: string | null | undefined) {
    const headers: HttpHeaders = new HttpHeaders().set('content-type', 'application/json');

    this.http.options(this.api, {
      headers: headers,
    });

    this.baseRoute = this.api;

    if (route !== undefined && route !== null) {
      this.baseRoute += `/${route}`;
    }
  }
}
