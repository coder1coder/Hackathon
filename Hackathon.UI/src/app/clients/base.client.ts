import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from '../../environments/environment';

export abstract class BaseApiClient {
  protected api: string = environment.api;

  protected constructor(protected http: HttpClient) {
    const headers: HttpHeaders = new HttpHeaders().set('content-type', 'application/json');

    http.options(this.api, {
      headers: headers,
    });
  }
}
