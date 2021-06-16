import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CoursesListRoutingModule } from './courses-list-routing.module';
import { CoursesListComponent } from './courses-list.component';
import { PageHeaderModule, CommonDialogModule, CourseService, SharedModule, SearchModule } from '../../../shared';
import { CoursePreviewModule,CourseUsersModule } from './../../components';
import { NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
@NgModule({
    imports: [
        CommonModule,
        PageHeaderModule,
        NgbPaginationModule.forRoot(),
        SharedModule.forRoot(),
        CommonDialogModule,
        SearchModule,
        CoursePreviewModule,
        CourseUsersModule,
        CoursesListRoutingModule
    ],
    declarations: [CoursesListComponent],
    providers: [CourseService]
})
export class CoursesListModule { }
