import {BaseApiClient} from "../base.client";
import {HttpClient} from "@angular/common/http";
import {IProject} from "../../models/Project/IProject";
import {Injectable} from "@angular/core";
import {Observable} from "rxjs";
import {IProjectUpdateFromGitBranch} from "../../models/Project/IProjectUpdateFromGitBranch";

@Injectable({
  providedIn: 'root'
})
export class ProjectClient extends BaseApiClient
{
  private baseRoute = `${this.api}/project`;

  protected constructor(http:HttpClient) {
    super(http)
  }

  public createAsync(project:IProject){
    return this.http.post(this.baseRoute, project);
  }

  public getAsync(eventId: number, teamId: number):Observable<IProject>{
    return this.http.get<IProject>(`${this.baseRoute}/${eventId}/${teamId}`);
  }

  public updateAsync(parameters: IProject) {
    return this.http.put(`${this.baseRoute}`, parameters);
  }

  public updateProjectFromGitBranch(parameters: IProjectUpdateFromGitBranch){
    return this.http.put(`${this.baseRoute}/branch`, parameters);
  }

  public remove(eventId: number, teamId: number) {
    return this.http.delete(`${this.baseRoute}/${eventId}/${teamId}`);
  }
}
