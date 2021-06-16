import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgbModalModule } from '@ng-bootstrap/ng-bootstrap';

import { DashboardRoutingModule } from './dashboard-routing.module';
import { DashboardComponent } from './dashboard.component';
import { SharedModule, UsersService } from '../../shared';

@NgModule({
    imports: [
        CommonModule,
        NgbModalModule.forRoot(),
        SharedModule.forRoot(),
        DashboardRoutingModule
    ],
    declarations: [
        DashboardComponent,
    ],
    providers: [UsersService]
})
export class DashboardModule { }
