import {environment} from "../../environments/environment";
import {Injectable} from "@angular/core";

@Injectable({ providedIn: 'root' })
export class Endpoints {

  private api = environment.api

  public Team: TeamEndpoints = new TeamEndpoints(this.api);
}

export class TeamEndpoints
{
  constructor(private api: string){}

  Create:string = `${this.api}/Team`;
  GetTeams:string = `${this.api}/Team/getTeams`;
  GetMy:string = `${this.api}/Team/My`;
  GetById = (id:number):string => `${this.api}/Team/${id}`;
}
