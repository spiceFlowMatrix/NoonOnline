import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AssignmentListRoutingModule } from './assignment-list-routing.module';
import { AssignmentsComponent } from './assignment-list.component';
import { PageHeaderModule, CommonDialogModule, AssignmentService, SharedModule, SearchModule } from '../../../shared';
import { NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';

@NgModule({
    imports: [
        CommonModule,
        PageHeaderModule,
        NgbPaginationModule.forRoot(),
        SharedModule.forRoot(),
        CommonDialogModule,
        SearchModule,
        AssignmentListRoutingModule
    ],
    declarations: [AssignmentsComponent],
    providers: [AssignmentService]
})
export class AssignmentListModule { }
