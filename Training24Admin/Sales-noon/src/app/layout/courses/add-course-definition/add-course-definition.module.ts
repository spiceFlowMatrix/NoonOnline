import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AddCourseDefinitionRoutingModule } from './add-course-definition-routing.module';
import { AddCourseDefinitionComponent } from './add-course-definition.component';
import { PageHeaderModule, CommonDialogModule, SharedModule, GradeService, CourseService } from '../../../shared';

@NgModule({
    imports: [
        CommonModule,
        PageHeaderModule,
        CommonDialogModule,
        SharedModule.forRoot(),
        AddCourseDefinitionRoutingModule
    ],
    declarations: [AddCourseDefinitionComponent],
    providers: [CourseService,GradeService]
})
export class AddCourseDefinitionModule { }
