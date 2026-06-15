import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable, of } from 'rxjs';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { map } from 'rxjs/operators';
import { BaseApiClient } from './base.client';

@Injectable({
  providedIn: 'root',
})
export class FileStorageClient extends BaseApiClient {
  private sanitizer = inject(DomSanitizer);
  private cache = new Map<string, SafeUrl>();

  constructor() {
    super('fileStorage');
  }

  public getById(id: string): Observable<SafeUrl> {
    const key: string = JSON.stringify(id);
    if (!this.cache.has(key)) {
      return this.http.get(`${this.baseRoute}/${id}`, { responseType: 'arraybuffer' }).pipe(
        map((arrayBuffer: ArrayBuffer) => {
          const safeUrl: SafeUrl = this.getSafeUrlFromByteArray(arrayBuffer);
          this.cache.set(key, safeUrl);
          return safeUrl;
        }),
      );
    } else {
      //TODO: remove type assertion
      return of(this.cache.get(key)) as Observable<SafeUrl>;
    }
  }

  private getSafeUrlFromByteArray(buffer: ArrayBuffer): SafeUrl {
    const TYPED_ARRAY: Uint8Array = new Uint8Array(buffer);
    const STRING_CHAR: string = TYPED_ARRAY.reduce(
      (data, byte) => data + String.fromCharCode(byte),
      '',
    );
    const base64String: string = btoa(STRING_CHAR);
    return this.sanitizer.bypassSecurityTrustUrl('data:image/jpg;base64,' + base64String);
  }
}
