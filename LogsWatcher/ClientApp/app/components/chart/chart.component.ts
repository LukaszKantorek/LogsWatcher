import { Component, OnInit } from '@angular/core';
import { Log, LogExtensions } from "../../models/log";
import { Http } from '@angular/http';
import { StateMediator, IHandler } from "../../statemanager/state.mediator";
import { LogsStateChangesSubscriber } from "../../statemanager/newlogsstate.subescriber";

@Component({
    selector: 'logs-chart',
    templateUrl: './chart.component.html',
    styleUrls: ['./chart.component.css']
})
export class LogsChartComponent implements OnInit {
  data: any;
  logsCount: number;

  logsChangesStateHandler: IHandler;
  logsStateSubscriber: LogsStateChangesSubscriber;

  constructor(http: Http, mediator: StateMediator) {
    this.data = {};
    this.logsCount = 0;
    this.logsStateSubscriber = new LogsStateChangesSubscriber((newState: Array<Log>) => {
      this.logsCount = newState.length;
      let stateGrouppedByType = this.groupLogsByType(newState);
      this.setupChartData(stateGrouppedByType);
    });
    this.logsChangesStateHandler = mediator;
  }

  ngOnInit(): void {
    this.logsCount = 0;

    this.logsChangesStateHandler
      .attachSubscriber(this.logsStateSubscriber);
  }

  private groupLogsByType(logs: Array<Log>) : any {
    let stateGrouppedByType: any = {}

    logs.forEach((log) => {
      let description = LogExtensions.getLogDescription(log);
      if (stateGrouppedByType[description]) {
        stateGrouppedByType[description] = stateGrouppedByType[description] + 1;
      } else {
        stateGrouppedByType[description] = 1;
      }
    });

    return stateGrouppedByType;
  }

  private setupChartData(stateGrouppedByType: any) {

    let legendLabels = this.getLogsDescriptions(stateGrouppedByType);
    let chartValues = this.getLogsCounts(stateGrouppedByType);

    this.data = {
      labels: legendLabels,
      datasets: [
        {
          data: chartValues,
          backgroundColor: [
            "#5cb85c",
            "#f0ad4e",
            "#d9534f"
          ],
          hoverBackgroundColor: [
            "#5cb85c",
            "#f0ad4e",
            "#d9534f"
          ]
        }]
    };
  }

  private getLogsDescriptions(stateGrouppedByType : any) : Array<string> {
    return Object.keys(stateGrouppedByType);
  }

  private getLogsCounts(stateGrouppedByType: any): Array<number> {
    let values = new Array<any>();
    this.getLogsDescriptions(stateGrouppedByType)
      .forEach((key: any) => {
        values.push(stateGrouppedByType[key]);
      });
    return values;
  }
}