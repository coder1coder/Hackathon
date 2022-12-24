import {EventStatus} from "./EventStatus";

export interface IEventListItem {
  id: number;
  name: string;
  description: string;
  start: Date;
  maxEventMembers: number;
  minTeamMembers: number;
  isCreateTeamsAutomatically: boolean;
  status: EventStatus;
  ownerId: number;
  ownerName: string;
  teamsCount: number;
  membersCount: number;
  eventImageId?: string;
  photoLink?: string;
}
