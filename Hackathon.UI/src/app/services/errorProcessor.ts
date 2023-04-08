import {SnackService} from "./snack.service";
import {IProblemDetails} from "../models/IProblemDetails";
import {Injectable} from "@angular/core";

@Injectable({
  providedIn: 'root'
})
export class ErrorProcessor
{
  constructor(private snackService: SnackService) {
  }

  public Process(errorContext: any, defaultErrorMessage: string | null = null):void{

    let errorMessage = defaultErrorMessage ?? "Произошла непредвиденная ошибка";

    if (errorContext.error !== undefined) {
      let problemDetails: IProblemDetails = <IProblemDetails>errorContext.error;
      errorMessage = (problemDetails.errors) ? Object.values(problemDetails.errors)[0] : errorContext.title;
    }

    this.snackService.open(errorMessage);
  }
}
