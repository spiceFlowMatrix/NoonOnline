import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgbCarouselModule, NgbAlertModule } from '@ng-bootstrap/ng-bootstrap';

import { DashboardRoutingModule } from './dashboard-routing.module';
import { DashboardComponent } from './dashboard.component';
import {
    NotificationComponent,
    ChatComponent
} from './components';
import { StatModule, SharedModule } from '../../shared';

@NgModule({
    imports: [
        CommonModule,
        NgbCarouselModule.forRoot(),
        NgbAlertModule.forRoot(),
        DashboardRoutingModule,
        StatModule,
        SharedModule.forRoot()
    ],
    declarations: [
        DashboardComponent,
        NotificationComponent,
        ChatComponent
    ]
})
export class DashboardModule { }
