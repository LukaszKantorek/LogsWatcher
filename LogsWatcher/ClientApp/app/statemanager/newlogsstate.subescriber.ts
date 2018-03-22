import { ISubscriber } from "./istate.subscriber";
import { Log } from "../models/log";
import { ChangeType } from './changetype';

export class LogsStateChangesSubscriber implements ISubscriber {
  updateLogsStatePridicate: Function;

  constructor(updateLogsStatePridicate : Function) {
    this.updateLogsStatePridicate = updateLogsStatePridicate;
  }

  canHandle(changeType: ChangeType): boolean {
    return changeType === ChangeType.LogsUpdated;
  }

  handleChange(change: any): void {
    let newLogsState = change as Array<Log>;
    if (newLogsState) {
      this.updateLogsStatePridicate(newLogsState);
    }
  }
}