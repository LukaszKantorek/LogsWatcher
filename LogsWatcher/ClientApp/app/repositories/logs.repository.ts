import { StateMediator, INotifier } from "../statemanager/state.mediator";
import { ChangeType } from "../statemanager/changetype";
import { Http } from '@angular/http';
import { LogTypeEnum } from '../models/logtypeenum';

export class LogsRepository {

  server: Http;
  mediator: INotifier;
  urlprefix: string;

  constructor(mediator: StateMediator, server: Http) {
    this.urlprefix = 'http://localhost:61148';
    this.mediator = mediator;
    this.server = server;
  }

  init(): void {
    this.getLogsFromServer();
  }

  getLogsFromServer() {
    this.server.get(this.urlprefix + '/Logger/GetLogs').subscribe(result=> {
      if (result) {
        let logs = result.json();
        if (logs) {
          this.mediator.notifyAboutChange(ChangeType.LogsUpdated, logs);
        }
      }
    }, error => console.error(error));
  }

  appendTrace() {
    this.appendLog('AddLog?logType=' + LogTypeEnum.Trace);
  }

  appendWarning() {
    this.appendLog('AddLog?logType=' + LogTypeEnum.Warning);
  }

  appendError() {
    this.appendLog('AddLog?logType=' + LogTypeEnum.Error);
  }

  private appendLog(uriPostFix: string) {
    this.server.get(this.urlprefix + '/Logger/' + uriPostFix).subscribe(() => {
      this.getLogsFromServer();
    }, error => console.error(error));
  }
}