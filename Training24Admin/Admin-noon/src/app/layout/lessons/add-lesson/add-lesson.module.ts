import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AddLessonRoutingModule } from './add-lesson-routing.module';
import { AddLessonComponent } from './add-lesson.component';
// import { SelectDropDownModule } from 'ngx-select-dropdown'
import { NgbTypeaheadModule, NgbModalModule } from '@ng-bootstrap/ng-bootstrap';
import { PageHeaderModule, CommonDialogModule, SharedModule, LessonService, FileService, ChapterService } from '../../../shared';
import { UploadStatusModule } from './../../components';
import { MatCommonModule } from '@angular/material/core';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatCardModule } from '@angular/material/card';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatSelectModule } from '@angular/material/select';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import {MatProgressBarModule} from '@angular/material/progress-bar';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        PageHeaderModule,
        MatCommonModule,
        MatButtonModule,
        MatAutocompleteModule,
        MatInputModule,
        MatCardModule,
        MatProgressBarModule,
        MatSelectModule,
        CommonDialogModule,
        SharedModule.forRoot(),
        UploadStatusModule,
        NgbTypeaheadModule.forRoot(),
        // SelectDropDownModule,
        AddLessonRoutingModule,
    ],
    declarations: [AddLessonComponent],
    providers: [LessonService, FileService, ChapterService]
})
export class AddLessonModule { }
