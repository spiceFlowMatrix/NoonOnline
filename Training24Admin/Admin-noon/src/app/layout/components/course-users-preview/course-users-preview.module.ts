import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CourseUsersComponent } from './course-users-preview.component';
import { FormsModule } from '@angular/forms';
import { CoursePreviewModule } from '../course-preview/course-preview.module';
import { NgbModalModule } from '@ng-bootstrap/ng-bootstrap';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        NgbModalModule,
        CoursePreviewModule
    ],
    declarations: [CourseUsersComponent],
    exports: [CourseUsersComponent],
    providers: []
})
export class CourseUsersModule { }