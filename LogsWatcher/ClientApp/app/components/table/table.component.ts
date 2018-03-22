import { Component, OnInit } from '@angular/core';
import { Log } from "../../models/log";
import { Http } from '@angular/http';
import { StateMediator, IHandler } from "../../statemanager/state.mediator";
import { LogsStateChangesSubscriber } from "../../statemanager/newlogsstate.subescriber";

@Component({
    selector: 'logs-table',
    templateUrl: './table.component.html',
    styleUrls: ['./table.component.css']
})
export class LogsTableComponent implements OnInit {
  logs: Array<Log>;
  logsChangesStateHandler: IHandler;
  logsStateSubscriber: LogsStateChangesSubscriber;

  constructor(http: Http, mediator: StateMediator) {
    this.logs = new Array<Log>();
    this.logsStateSubscriber = new LogsStateChangesSubscriber((newState: Array<Log>) => {
      this.logs = newState;
    });
    this.logsChangesStateHandler = mediator;
  }

  ngOnInit(): void {
    this.logsChangesStateHandler
      .attachSubscriber(this.logsStateSubscriber);
  }
}