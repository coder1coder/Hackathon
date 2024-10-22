import { EventStatus } from './EventStatus';

export class ChangeEventStatusMessage {
  status: EventStatus;
  message: string;

  constructor(status: EventStatus, message: string) {
    this.status = status;
    this.message = message;
  }
}
