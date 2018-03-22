import { Component, OnInit } from '@angular/core';
import { LogsRepository } from "../../repositories/logs.repository";

import { StateMediator } from "../../statemanager/state.mediator";
import { Http } from '@angular/http';

@Component({
    selector: 'home',
    templateUrl: './home.component.html',
    styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit{
  logsUpdater: LogsRepository;

  constructor(mediator: StateMediator, server: Http) {
    this.logsUpdater = new LogsRepository(mediator, server);
  }

  ngOnInit(): void {
    this.logsUpdater.init();
  }

  addTrace() {
    this.logsUpdater.appendTrace();
  }

  addWarning() {
    this.logsUpdater.appendWarning();
  }

  addError() {
    this.logsUpdater.appendError();
  }
}