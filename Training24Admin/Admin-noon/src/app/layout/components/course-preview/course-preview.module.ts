import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CoursePreviewComponent } from './course-preview.component';
import { FormsModule } from '@angular/forms';
import { NgbModalModule } from '@ng-bootstrap/ng-bootstrap';
@NgModule({
    imports: [
        CommonModule,
        NgbModalModule.forRoot(),
        FormsModule
    ],
    declarations: [CoursePreviewComponent],
    exports: [CoursePreviewComponent],
    providers: []
})
export class CoursePreviewModule { }