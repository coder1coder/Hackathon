import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { IStorageFile } from '../models/FileStorage/IStorageFile';
import {Bucket} from "../common/Bucket";
import {DomSanitizer, SafeUrl} from "@angular/platform-browser";
import {map} from "rxjs/operators";

@Injectable({
  providedIn: 'root'
})

export class FileStorageService {
  private api: string = `${environment.api}/fileStorage`;
  private cache = new Map<string, Observable<ArrayBuffer>>();

  constructor(private sanitizer: DomSanitizer, private http: HttpClient) {
    const headers = new HttpHeaders()
      .set('content-type', 'application/json');

    http.options(this.api, {
      'headers': headers
    });
  }

  public getById(id: string): Observable<SafeUrl> {

    const key = JSON.stringify(id);
    if (!this.cache.has(key)) {
      const response = this.http.get(`${this.api}/get/${id}`, { responseType: 'arraybuffer' })
      this.cache.set(key, response);
    }

    return (this.cache.get(key) as Observable<ArrayBuffer>)
      .pipe(map((x: ArrayBuffer) => this.getSafeUrlFromByteArray(x)));
  }

  public upload(bucket: Bucket, file: File) : Observable<IStorageFile>{

    const formData = new FormData();
    formData.append("file", file);

    return this.http.post<IStorageFile>(`${this.api}/upload/${bucket}`, formData, {
      headers: new HttpHeaders().set('Content-Disposition', 'multipart/event-create-edit-card-data')
    })
  }

  public getSafeUrlFromByteArray(buffer: ArrayBuffer): SafeUrl {
    let TYPED_ARRAY = new Uint8Array(buffer);
    const STRING_CHAR = TYPED_ARRAY.reduce((data, byte)=> data + String.fromCharCode(byte), '');
    let base64String = btoa(STRING_CHAR);

    return this.sanitizer.bypassSecurityTrustUrl('data:image/jpg;base64,' + base64String);
  }
}
