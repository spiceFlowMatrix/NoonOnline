import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AddCoursesRoutingModule } from './add-courses-routing.module';
import { AddCoursesComponent } from './add-courses.component';
import { PageHeaderModule, SharedModule, CommonDialogModule, CourseService, UsersService, GradeService } from '../../../shared';
import { NgbModalModule, NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';

@NgModule({
    imports: [
        CommonModule,
        PageHeaderModule,
        SharedModule.forRoot(),
        NgbModalModule.forRoot(),
        NgbPaginationModule.forRoot(),
        CommonDialogModule,
        AddCoursesRoutingModule,
    ],
    declarations: [AddCoursesComponent],
    providers: [CourseService, UsersService, GradeService]
})
export class AddCoursesModule { }
