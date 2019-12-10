import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CoursePreviewComponent } from './course-preview.component';
import { FormsModule } from '@angular/forms';
import { NgbTypeaheadModule, NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap'
import { CoursePreviewRoutingModule } from './course-preview-routing.module'
import { SharedModule, CommonDialogModule, CourseService, ChapterService, LessonService ,QuizService, AssignmentService} from '../../../shared';
import { DragulaModule } from 'ng2-dragula';
import { CourseHeaderComponent } from '../../components/course-header/course-header.component';
import { TranslateModule } from '@ngx-translate/core';
import { MatCommonModule } from '@angular/material/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        SharedModule,
        MatButtonModule,
        TranslateModule,
        MatCardModule,
        CommonDialogModule,
        NgbTypeaheadModule.forRoot(),
        NgbDropdownModule,
        DragulaModule.forRoot(),
        CoursePreviewRoutingModule
    ],
    declarations: [CoursePreviewComponent,CourseHeaderComponent],
    providers: [CourseService, ChapterService, LessonService,QuizService,AssignmentService]
})
export class CoursePreviewModule { }