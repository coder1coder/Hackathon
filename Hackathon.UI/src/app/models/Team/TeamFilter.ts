import {TeamType} from "./Team";

export class TeamFilter {
  ids!: number[];
  name!: string;
  owner!: string;
  ownerId!: number;
  hasOwner!: boolean;
  quantityMembersFrom!: number;
  quantityMembersTo!: number;
  eventId!: number;
  projectId!: number;
  type!:TeamType;
}
