import { EventStatus } from './EventStatus';
import { SafeUrl } from '@angular/platform-browser';

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
  imageId?: string;
  /** не возвращается с бэка, инициализируется после получения imageId */
  imageUrl?: SafeUrl;
}
