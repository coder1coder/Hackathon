import {PageEvent} from "@angular/material/paginator";
import {AfterViewInit, Directive} from "@angular/core";
import {PageSettingsDefaults} from "../models/PageSettings";
import {GetFilterModel} from "../models/GetFilterModel";

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
      this.pageSettings.pageSize = PageSettingsDefaults.PageSize;
      this.pageSettings.pageIndex = PageSettingsDefaults.PageIndex;
    }
  }

  abstract getDisplayColumns():string[];
  abstract rowClick(item: T):any;
  abstract fetch(getFilterModel?: GetFilterModel<T>):any;

  ngAfterViewInit(): void {
    this.fetch();
  }

  public setPageSettings(event:PageEvent){
    this.pageSettings = event;
    sessionStorage.setItem(`${this.componentName}${PageEvent.name}`, JSON.stringify(event));
    this.fetch();
  }

}
