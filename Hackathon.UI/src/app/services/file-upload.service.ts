import {Injectable} from "@angular/core";
import {environment} from "../../environments/environment";
import {HttpClient} from "@angular/common/http";
import {Observable, of, switchMap} from "rxjs";
import {map} from "rxjs/operators";
import {UploadFileErrorMessages} from "../common/error-messages/upload-file-error-messages";
import {FileUtils} from "../common/FileUtils";

@Injectable({
  providedIn: 'root'
})
export class FileUploadService {

  private api: string = `${environment.api}`;

  constructor(private http: HttpClient) {}

  /** Загрузить веб-контент файл помещенных через селект или "drag and drop"
   * @param files Объекты типа элемента HTML input type="file"
   * @param uploadApiUrl Путь до контроллера который принимает объект file
   */
  public uploadFile(files: FileList, uploadApiUrl: string): Observable<string> {
    return of(files)
      .pipe(
        map((fileList: FileList) => {
          if (!(fileList?.length > 0)) throw new Error(UploadFileErrorMessages.FileUploadError);
          const file: File = fileList[0];
          if (!FileUtils.IsImage(file)) throw new Error(UploadFileErrorMessages.FileIsNotImage);
          if (file.size / FileUtils.Divider > FileUtils.MaxFileSize) throw new Error(UploadFileErrorMessages.FileSizeOutOfRange);
          return file;
        }),
        switchMap((validFile: File) => {
          const formData = new FormData();
          formData.append("file", validFile);
          return this.http.post<string>(`${this.api}${uploadApiUrl}`, formData);
        })
      );
  }
}
