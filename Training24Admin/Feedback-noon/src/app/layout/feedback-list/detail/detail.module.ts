import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { FeedbackDetailRoutingModule } from './detail-routing.module';
import { FeedbackDetailComponent } from './detail.component';
import { FeedbackService, UtilService, DirectivesModule,CommonDialogModule } from './../../../shared';

import {
    RoleAssignmentComponentModule,
    TaskInputItemComponentModule,
    TaskManagementModule,
    PublicFeedbackFormModule
} from './../../components';
@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        PublicFeedbackFormModule,
        RoleAssignmentComponentModule,
        TaskInputItemComponentModule,
        FeedbackDetailRoutingModule,
        TaskManagementModule,
        CommonDialogModule,
        DirectivesModule
    ],
    declarations: [
        FeedbackDetailComponent,
    ],
    providers: [FeedbackService, UtilService]
})
export class FeedbackDetailModule { }
