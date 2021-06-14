import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CoursesListRoutingModule } from './courses-list-routing.module';
import { CoursesListComponent } from './courses-list.component';
import { PageHeaderModule, CommonDialogModule, CourseService, SharedModule, SearchModule } from '../../../shared';
import { NgbPaginationModule, NgbAccordionModule } from '@ng-bootstrap/ng-bootstrap';

@NgModule({
    imports: [
        CommonModule,
        PageHeaderModule,
        NgbAccordionModule.forRoot(),
        NgbPaginationModule.forRoot(),
        SharedModule.forRoot(),
        CommonDialogModule,
        SearchModule,
        CoursesListRoutingModule
    ],
    declarations: [CoursesListComponent],
    providers: [CourseService]
})
export class CoursesListModule { }
