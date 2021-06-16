import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MyCoursesListRoutingModule } from './mycourses-list-routing.module';
import { MyCoursesListComponent } from './mycourses-list.component';
import { PageHeaderModule, CourseService, SharedModule } from '../../../shared';
import { UserCourseModule } from '../../components';
import { NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
@NgModule({
    imports: [
        CommonModule,
        PageHeaderModule,
        UserCourseModule,
        NgbPaginationModule.forRoot(),
        SharedModule.forRoot(),
        MyCoursesListRoutingModule
    ],
    declarations: [MyCoursesListComponent],
    providers: [CourseService]
})
export class MyCoursesListModule { }
