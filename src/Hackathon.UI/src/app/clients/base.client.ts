import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from '../../environments/environment';

export abstract class BaseApiClient {
  protected api: string = environment.api;
  protected baseRoute: string;

  protected constructor(protected http: HttpClient, route:string | null | undefined) {
    const headers: HttpHeaders = new HttpHeaders().set('content-type', 'application/json');

    http.options(this.api, {
      headers: headers,
    });

    this.baseRoute = this.api;

    if (route !== undefined && route !== null) {
      this.baseRoute += `/${route}`;
    }

  }
}
