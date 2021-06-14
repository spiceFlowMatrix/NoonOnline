import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserCourseComponent } from './usercourse-table.component';
import { FormsModule } from '@angular/forms';
import { CoursePreviewModule } from './../course-preview/course-preview.module';
import { CourseUsersModule } from './../course-users-preview/course-users-preview.module';
import { CourseService, UtilService } from '../../../shared';
@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        CourseUsersModule,
        CoursePreviewModule
    ],
    declarations: [UserCourseComponent],
    exports: [UserCourseComponent],
    providers: [CourseService, UtilService]
})
export class UserCourseModule { }