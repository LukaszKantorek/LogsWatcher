import { Injectable } from '@angular/core';
import { ChangeType } from "./changetype";
import { ISubscriber } from "./istate.subscriber";

@Injectable()
export class StateMediator implements IHandler, INotifier {
  private subscribers: Array<ISubscriber>;

  constructor() {
    this.subscribers = new Array<ISubscriber>();
  }

  attachSubscriber(subscriber: ISubscriber) {
    this.subscribers.push(subscriber);
  }

  notifyAboutChange(changeType: ChangeType, change: any) {
    this.subscribers.forEach((subscriber) => {
      if (subscriber.canHandle(changeType)) {
        subscriber.handleChange(change);
      }
    });
  }
}

export interface IHandler {
  attachSubscriber(subscriber: ISubscriber): void
}

export interface INotifier {
  notifyAboutChange(changeType: ChangeType, change: any): void;
}