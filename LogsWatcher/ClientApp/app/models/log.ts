import {LogType} from "./logtype"

export interface Log {
  value: string;
  insertDate: Date;
  type: LogType;
  stackTrace: string;
}

export class LogExtensions {
  static getLogDescription(log: Log): string {
    if (log && log.type) {
      if (log.type.typeNumber === 0) {
        return "Info";
      } else if (log.type.typeNumber === 1) {
        return "Warning";
      } else if (log.type.typeNumber === 2) {
        return "Error";
      }
    }
    return "undefined";
  }
}