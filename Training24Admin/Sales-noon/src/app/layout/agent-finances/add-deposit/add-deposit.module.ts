import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgbDatepickerModule } from '@ng-bootstrap/ng-bootstrap';
import { AddDepositRoutingModule } from './add-deposit-routing.module';
import { AddDepositComponent } from './add-deposit.component';
import { PageHeaderModule, CommonDialogModule, SharedModule, AgentService } from '../../../shared';

@NgModule({
    imports: [
        CommonModule,
        PageHeaderModule,
        CommonDialogModule,
        NgbDatepickerModule.forRoot(),
        SharedModule.forRoot(),
        AddDepositRoutingModule
    ],
    declarations: [AddDepositComponent],
    providers: [AgentService]
})
export class AddDepositModule { }
