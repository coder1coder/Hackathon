import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, ReplaySubject, share } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})

export class FileStorageService {
  private api: string = environment.api;
  private storage: Storage = sessionStorage;
  private cache = new Map<string, Observable<ArrayBuffer>>();

  constructor(private http: HttpClient) {
    const headers = new HttpHeaders()
      .set('content-type', 'application/json');

    http.options(this.api, {
      'headers': headers
    });
  }

  public getImage(id: string): Observable<ArrayBuffer> {
    const key = JSON.stringify(id);
    if (!this.cache.has(key)) {
      const response = this.http.get(this.api + `/filestorage/get/${id}`, { responseType: 'arraybuffer' })
      .pipe(
        share({
          connector: () => new ReplaySubject(1),
          resetOnError: false,
          resetOnComplete: false,
          resetOnRefCountZero: false,
        })
      );
      this.cache.set(key, response);
    }
    return this.cache.get(key) as Observable<ArrayBuffer>;
  }

  public setImage(file: File): Observable<string> {
    const formData = new FormData();
    formData.append("file", file);

    return this.http.post<string>(this.api+'/User/profile/image/upload', formData, {
      headers: new HttpHeaders().set('Content-Disposition', 'multipart/form-data')
    })
  }
}
