import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PageHeaderModule, CommonDialogModule, SharedModule, SearchModule } from '../../../shared';
import { NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { TimeOutFormComponent } from './timeout-form.component';
import { TimeOutFormRoutingModule } from './timeout-form-routing.module';
import { TimeoutService } from '../../../shared/services/timeout.services';

@NgModule({
    imports: [
        CommonModule,
        PageHeaderModule,
        NgbPaginationModule.forRoot(),
        SharedModule.forRoot(),
        CommonDialogModule,
        SearchModule,
        TimeOutFormRoutingModule
    ],
    declarations: [TimeOutFormComponent],
    providers: [TimeoutService]
})
export class TimeOutFormModule { }
