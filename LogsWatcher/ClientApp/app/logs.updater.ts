import { StateMediator, INotifier } from "./statemanager/state.mediator";
import { ChangeType } from "./statemanager/changetype";
import { Http } from '@angular/http';
import { Log } from './models/log';

export class LogsUpdater {

  server: Http;
  mediator: INotifier;

  constructor(mediator: StateMediator, server: Http) {
    this.mediator = mediator;
    this.server = server;
  }

  init(): void {
    this.getLogsFromServer();
  }

  getLogsFromServer() {
    this.server.get('http://localhost:61148/Domain/GetLogs').subscribe(result=> {
      if (result) {
        let logs = result.json();
        if (logs) {
          this.mediator.notifyAboutChange(ChangeType.LogsUpdated, logs);
        }
      }
    }, error => console.error(error));
  }

  appendTrace() {
    this.appendLog('AddTrace');
  }

  appendWarning() {
    this.appendLog('AddWarning');
  }

  appendError() {
    this.appendLog('AddError');
  }

  private appendLog(uriPostFix: string) {
    this.server.get('http://localhost:61148/Domain/' + uriPostFix).subscribe(() => {
      this.getLogsFromServer();
    }, error => console.error(error));
  }
}