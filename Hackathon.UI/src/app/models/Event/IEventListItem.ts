import {EventStatus} from "../EventStatus";

export interface IEventListItem {
  id: number;
  name: string;
  start: Date;
  maxEventMembers: number;
  minTeamMembers: number;
  isCreateTeamsAutomatically: boolean;
  status: EventStatus;
  ownerId: number;
  ownerName: string;
  teamsCount: number;
  membersCount: number;
}
