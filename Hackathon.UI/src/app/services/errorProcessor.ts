import {SnackService} from "./snack.service";
import {IProblemDetails} from "../models/IProblemDetails";
import {Injectable} from "@angular/core";

@Injectable({
  providedIn: 'root'
})
export class ErrorProcessor
{
  constructor(private snackService: SnackService) { }

  public Process(errorContext: any, defaultErrorMessage: string | null = null):void{
    let errorMessage = defaultErrorMessage ?? "Произошла непредвиденная ошибка";

    if (errorContext.error !== undefined) {
      const problemDetails: IProblemDetails = <IProblemDetails>errorContext.error;
      errorMessage = Boolean(problemDetails?.errors) ? Object.values(problemDetails.errors)[0] : 
        (problemDetails?.detail || errorContext.title || problemDetails["validation-error"]);
    }

    this.snackService.open(errorMessage);
  }
}
