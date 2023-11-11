import { Injectable } from "@angular/core";
import { environment } from "../../environments/environment";
import { HttpClient } from "@angular/common/http";
import { filter, Observable, of, switchMap } from "rxjs";
import { map } from "rxjs/operators";
import { UploadFileErrorMessages } from "../common/error-messages/upload-file-error-messages";
import { FileUtils } from "../common/interfaces/file-utils";
import { ErrorProcessorService } from "./error-processor.service";

@Injectable({
  providedIn: 'root'
})
export class FileUploadService {

  private api: string = `${environment.api}`;

  constructor(
    private http: HttpClient,
    private errorProcessor: ErrorProcessorService,
  ) {}

  /** Загрузить веб-контент файл помещенных через селект или "drag and drop"
   * @param files Объекты типа элемента HTML input type="file"
   * @param uploadApiUrl Путь до контроллера который принимает объект file
   */
  public uploadFile(files: FileList, uploadApiUrl: string): Observable<string> {
    return of(files)
      .pipe(
        map((fileList: FileList) => {
          const file: File = fileList[0];
          let errorMsg: string;
          if (fileList?.length <= 0) {
            errorMsg = UploadFileErrorMessages.FileUploadError;
          }
          if (!FileUtils.IsImage(file)) {
            errorMsg = UploadFileErrorMessages.FileIsNotImage;
          }
          if (file.size / FileUtils.Divider > FileUtils.MaxFileSize) {
            errorMsg = UploadFileErrorMessages.FileSizeOutOfRange;
          }
          if (errorMsg) {
            files = null;
            this.errorProcessor.Process(null, errorMsg);
          }
          return errorMsg ? null : file;
        }),
        filter((v) => !!v),
        switchMap((validFile: File) => {
          const formData = new FormData();
          formData.append("file", validFile);
          return this.http.post<string>(`${this.api}${uploadApiUrl}`, formData);
        })
      );
  }
}
