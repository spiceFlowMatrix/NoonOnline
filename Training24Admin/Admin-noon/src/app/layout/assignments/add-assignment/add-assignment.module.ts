import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgbTypeaheadModule, NgbModalModule, NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { AddAssignmentRoutingModule } from './add-assignment-routing.module';
import { AddAssignmentComponent } from './add-assignment.component';
import {
    PageHeaderModule,
    CommonDialogModule,
    SharedModule,
    ChapterService,
    AssignmentService,
    FileService,
    UsersService,
    DataService
} from '../../../shared';
import { AddFileModule, UploadStatusModule, AddAllFileModule } from './../../components';
import { MatCommonModule } from '@angular/material/core';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressBarModule } from '@angular/material/progress-bar';
@NgModule({
    imports: [
        CommonModule,
        SharedModule.forRoot(),
        PageHeaderModule,
        AddAssignmentRoutingModule,
        CommonDialogModule,
        AddAllFileModule,
        MatCommonModule,
        MatInputModule,
        MatFormFieldModule,
        MatButtonModule,
        MatCardModule,
        MatProgressBarModule,
        UploadStatusModule,
        NgbPaginationModule.forRoot(),
        NgbModalModule.forRoot(),
        NgbTypeaheadModule.forRoot()
    ],
    declarations: [AddAssignmentComponent],
    providers: [AssignmentService, FileService, ChapterService, DataService, UsersService]
})
export class AddAssignmentModule { }
