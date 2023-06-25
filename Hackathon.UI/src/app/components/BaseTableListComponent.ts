import {Directive} from "@angular/core";
import {PageResultComponent} from "./page-result.component";

@Directive()
export abstract class BaseTableListComponent<T> extends PageResultComponent<T> {

  public override items: T[] = [];
  public abstract getDisplayColumns():string[];
}
