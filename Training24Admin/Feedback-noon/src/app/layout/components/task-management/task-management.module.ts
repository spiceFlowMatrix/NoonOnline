import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TaskManagementComponent } from './task-management.component';
import { FormsModule } from '@angular/forms';
import { TaskInputItemComponentModule } from './../task-input-item/task-input-item.module';
import { RoleAssignmentComponentModule } from './../role-assignment/role-assignment.module';
import { NgbPopoverModule, NgbTypeaheadModule, NgbModalModule } from '@ng-bootstrap/ng-bootstrap';
import { SharedPipesModule } from './../../../shared/pipes/shared-pipes.module';
import { FeedbackService, DirectivesModule, UtilService } from './../../../shared';
@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        NgbTypeaheadModule.forRoot(),
        NgbPopoverModule.forRoot(),
        NgbModalModule.forRoot(),
        SharedPipesModule,
        TaskInputItemComponentModule,
        RoleAssignmentComponentModule,
        DirectivesModule
    ],
    declarations: [TaskManagementComponent],
    exports: [TaskManagementComponent],
    providers: [UtilService, FeedbackService]
})
export class TaskManagementModule { }