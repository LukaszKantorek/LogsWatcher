import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { ChartModule } from 'primeng/chart';
import { OrderListModule } from 'primeng/orderlist';
import { HttpModule } from '@angular/http';
import { AppComponent } from './components/app/app.component';
import { NavMenuComponent } from './components/navmenu/navmenu.component';
import { HomeComponent } from './components/home/home.component';
import { LogsChartComponent } from './components/chart/chart.component';
import { LogsTableComponent } from './components/table/table.component';
import { StateMediator } from "./statemanager/state.mediator";

@NgModule({
    declarations: [
      AppComponent,
      NavMenuComponent,
      HomeComponent,
      LogsChartComponent,
      LogsTableComponent
    ],
    imports: [
      CommonModule,
      HttpModule,
      FormsModule,
      ChartModule,
      OrderListModule,
      RouterModule.forRoot([
          { path: '', redirectTo: 'home', pathMatch: 'full' },
          { path: 'home', component: HomeComponent },
          { path: '**', redirectTo: 'home' }
      ])
    ],
    providers: [StateMediator]
})
export class AppModuleShared {
}
