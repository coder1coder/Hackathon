export interface IEventLogModel {
  id: string;
  type: EventLogType;
  description: string;
  userId: number;
  userName: string;
  timestamp: Date;
}

export enum EventLogType {
  Default = 0,
  Created = 1,
  Updated = 2,
  Deleted = 3,
}
