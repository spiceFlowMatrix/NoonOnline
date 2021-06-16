import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgbTypeaheadModule, NgbModalModule, NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { AddAdditionalRoutingModule } from './add-additional-routing.module';
import { AddAdditionalComponent } from './add-additional.component';
import {
    PageHeaderModule,
    CommonDialogModule,
    SharedModule,
    ChapterService,
    AssignmentService,
    FileService,
    UsersService
} from '../../../shared';
import {AdditionalService} from '../../../shared/services/additionalservice.services';
import { AddFileModule, UploadStatusModule } from './../../components';
import { from } from 'rxjs';
@NgModule({
    imports: [
        CommonModule,
        PageHeaderModule,
        CommonDialogModule,
        AddFileModule,
        UploadStatusModule,
        NgbPaginationModule.forRoot(),
        NgbModalModule.forRoot(),
        NgbTypeaheadModule.forRoot(),
        SharedModule.forRoot(),
        AddAdditionalRoutingModule
    ],
    declarations: [AddAdditionalComponent],
    providers: [AdditionalService]
})
export class AddAdditionalModule { }
