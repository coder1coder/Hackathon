import { UserRole } from "../models/User/UserRole";

export class MenuItem {
  public unique: number;
  public routeLink: string;
  public text: string;
  public onlyForRoles: UserRole[] | undefined;
  public children: MenuItem[];

  constructor(routeLink:string, text:string, onlyForRoles: UserRole[] | undefined = undefined, children: MenuItem[] = []) {
    this.unique = new Date().getTime();

    this.routeLink = routeLink;
    this.text = text;
    this.onlyForRoles = onlyForRoles;
    this.children = children;
  }
}
