import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { environment } from 'src/environments/environment';
import { DomSanitizer, SafeUrl } from "@angular/platform-browser";
import { map } from "rxjs/operators";

@Injectable({
  providedIn: 'root'
})
export class FileStorageService {
  private api: string = `${environment.api}/fileStorage`;
  private cache = new Map<string, SafeUrl>();

  constructor(private sanitizer: DomSanitizer, private http: HttpClient) {}

  public getById(id: string): Observable<SafeUrl> {
    const key = JSON.stringify(id);
    if (!this.cache.has(key)) {
      return this.http.get(`${this.api}/get/${id}`, { responseType: 'arraybuffer' })
        .pipe(map((arrayBuffer: ArrayBuffer) => {
          const safeUrl = this.getSafeUrlFromByteArray(arrayBuffer);
          this.cache.set(key, safeUrl);
          return safeUrl;
        }));
    } else {
      return of(this.cache.get(key));
    }
  }

  private getSafeUrlFromByteArray(buffer: ArrayBuffer): SafeUrl {
    const TYPED_ARRAY = new Uint8Array(buffer);
    const STRING_CHAR = TYPED_ARRAY.reduce((data, byte)=> data + String.fromCharCode(byte), '');
    const base64String = btoa(STRING_CHAR);
    return this.sanitizer.bypassSecurityTrustUrl('data:image/jpg;base64,' + base64String);
  }
}
