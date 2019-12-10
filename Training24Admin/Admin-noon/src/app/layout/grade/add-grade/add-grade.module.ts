import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AddGradeRoutingModule } from './add-grade-routing.module';
import { AddGradeComponent } from './add-grade.component';
import { CourseSelectionModule } from './../../components';
import { PageHeaderModule, CommonDialogModule, SharedModule, GradeService, CourseService } from '../../../shared';
import {SchoolService} from '../../../shared/services/school.services';
import { NgbModalModule, NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
@NgModule({
    imports: [
        CommonModule,
        PageHeaderModule,
        CommonDialogModule,
        CourseSelectionModule,
        SharedModule.forRoot(),
        NgbModalModule.forRoot(),
        NgbPaginationModule.forRoot(),
        AddGradeRoutingModule,
    ],
    declarations: [AddGradeComponent],
    providers: [GradeService, CourseService,SchoolService]
})
export class AddGradeModule { }
