import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgbDatepickerModule } from '@ng-bootstrap/ng-bootstrap';
import { AddDepositsRoutingModule } from './add-deposits-routing.module';
import { AddDepositsComponent } from './add-deposits.component';
import { PageHeaderModule, CommonDialogModule, SharedModule, AgentService, UsersService, FileService } from '../../../shared';

@NgModule({
    imports: [
        CommonModule,
        PageHeaderModule,
        CommonDialogModule,
        NgbDatepickerModule.forRoot(),
        SharedModule.forRoot(),
        AddDepositsRoutingModule
    ],
    declarations: [AddDepositsComponent],
    providers: [AgentService, UsersService, FileService]
})
export class AddDepositsModule { }
