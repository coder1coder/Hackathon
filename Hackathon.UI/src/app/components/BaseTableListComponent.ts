import {PageEvent} from "@angular/material/paginator";
import {AfterViewInit, Directive} from "@angular/core";
import {PageSettingsDefaults} from "../models/PageSettings";
import {GetListParameters} from "../models/GetListParameters";

@Directive()
export abstract class BaseTableListComponent<T> implements AfterViewInit {

  public items: T[] = [];
  private readonly componentName: string | undefined;

  pageSettings: PageEvent = new PageEvent();

  protected constructor(private name:string) {

    this.componentName = name;

    let pageSettingsJson = sessionStorage.getItem(`${this.componentName}${PageEvent.name}`);

    if (pageSettingsJson != null)
      this.pageSettings = JSON.parse(pageSettingsJson)
    else
    {
      this.pageSettings.pageSize = PageSettingsDefaults.Limit;
      this.pageSettings.pageIndex = 0;
    }
  }

  abstract getDisplayColumns():string[];
  abstract rowClick(item: T):any;
  abstract fetch(getFilterModel?: GetListParameters<T>):any;

  ngAfterViewInit(): void {
    this.fetch();
  }

  public paginatorChanges(event:PageEvent){
    this.setPageSettings(event);
    this.fetch();
  }

  setPageSettings(event:PageEvent){
    this.pageSettings = event;
    sessionStorage.setItem(`${this.componentName}${PageEvent.name}`, JSON.stringify(event));
    this.fetch();
  }

}
