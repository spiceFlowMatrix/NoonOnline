import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { FeedDetailRoutingModule } from './feed-detail-routing.module';
import { FeedDetailComponent } from './feed-detail.component';
import { FeedbackService, UtilService, DirectivesModule, CommonDialogModule, UsersService } from '../../../shared';

import {
    RoleAssignmentComponentModule,
    TaskInputItemComponentModule,
    TaskManagementModule,
    PublicFeedbackFormModule
} from '../../components';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatDividerModule } from '@angular/material/divider';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSelectModule } from '@angular/material/select';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatIconModule } from '@angular/material/icon';
@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        MatFormFieldModule,
        MatDividerModule,
        MatTableModule,
        MatPaginatorModule,
        MatSelectModule,
        MatCheckboxModule,
        MatIconModule,
        PublicFeedbackFormModule,
        RoleAssignmentComponentModule,
        TaskInputItemComponentModule,
        FeedDetailRoutingModule,
        TaskManagementModule,
        CommonDialogModule,
        DirectivesModule
    ],
    declarations: [
        FeedDetailComponent,
    ],
    providers: [FeedbackService, UtilService,UsersService]
})
export class FeedDetailModule { }
