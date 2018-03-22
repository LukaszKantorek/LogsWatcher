import { ChangeType } from './changetype';

export interface ISubscriber {
  canHandle(changeType: ChangeType): boolean;
  handleChange(change: any): void;
}

