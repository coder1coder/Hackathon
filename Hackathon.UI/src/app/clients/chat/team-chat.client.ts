import {HttpClient} from "@angular/common/http";
import {BaseApiClient} from "../base.client";
import {TeamChatMessage} from "../../models/chat/TeamChatMessage";
import {Observable} from "rxjs";
import {BaseCollection} from "../../models/BaseCollection";
import {Injectable} from "@angular/core";

@Injectable({
  providedIn: 'root'
})

export class TeamChatClient extends BaseApiClient
{
  baseRoute = `${this.api}/chat/team`;

  protected constructor(http:HttpClient) {
    super(http)
  }

  public sendAsync(message:TeamChatMessage){
    return this.http.post(this.baseRoute + `/send`, message);
  }

  public getAsync(messageId: string):Observable<TeamChatMessage>{
    return this.http.get<TeamChatMessage>(`${this.baseRoute}/messages/${messageId}`);
  }

  public getListAsync(teamId:number, offset:number = 0, limit:number = 300): Observable<BaseCollection<TeamChatMessage>> {
    return this.http.post<BaseCollection<TeamChatMessage>>(this.baseRoute + `/${teamId}/list?offset=${offset}&limit=${limit}`, null);
  }
}
