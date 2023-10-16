import { SnackService } from "./snack.service";
import { IProblemDetails } from "../models/IProblemDetails";
import { Injectable } from "@angular/core";

@Injectable({
  providedIn: 'root'
})
export class ErrorProcessorService {

  private readonly UNKNOWN_ERROR: string = "Произошла непредвиденная ошибка";
  constructor(private snackService: SnackService) {}

  public Process(errorContext: any, defaultErrorMessage: string | null = null):void{
    let errorMessage = defaultErrorMessage ?? this.UNKNOWN_ERROR;

    if (errorContext.error !== undefined) {
      const problemDetails: IProblemDetails = <IProblemDetails>errorContext.error;
      errorMessage = problemDetails?.errors ? Object.values(problemDetails.errors)[0] :
        (problemDetails?.detail || errorContext.title || problemDetails["validation-error"] || this.UNKNOWN_ERROR);
    }

    this.snackService.open(errorMessage);
  }
}
