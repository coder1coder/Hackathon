import {Injectable, Input} from "@angular/core";

@Injectable()
export class AppStateService
{
  public title: string;
  public hideTitleBar: boolean = false;
  public hideContentWhileLoading: boolean = true;
  public isLoading: boolean = false;
  public showLoadingIndicator: boolean = true;
  public containerCssClasses: string = 'container container-full container-padding';
  public layoutCssClasses: string = '';
  public logoMinWidth: string = `initial`;
}
