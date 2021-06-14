import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgbTypeaheadModule, NgbModalModule, NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { AddBooksRoutingModule } from './add-books-routing.module';
import { AddBooksComponent } from './add-books.component';
import {
    PageHeaderModule,
    SharedModule,
    QuizService,
    QuestionsService,
    CommonDialogModule,
    DirectivesModule,
    GradeService,
    CourseService,
    FileService
} from '../../../shared';
import { QuillModule } from 'ngx-quill';
import { LibraryService } from '../../../shared/services/library.services';
import { SelectDropDownModule } from 'ngx-select-dropdown'
import { AddFileModule, UploadStatusModule } from './../../components';

@NgModule({
    imports: [
        CommonModule,
        PageHeaderModule,
        CommonDialogModule,
        SharedModule.forRoot(),
        NgbModalModule.forRoot(),
        NgbPaginationModule.forRoot(),
        NgbTypeaheadModule.forRoot(),
        SelectDropDownModule,
        AddFileModule,
        UploadStatusModule,
        DirectivesModule,
        QuillModule,
        AddBooksRoutingModule,
    ],
    declarations: [AddBooksComponent],
    providers: [GradeService,LibraryService,CourseService, FileService]
})
export class AddBooksModule { }
