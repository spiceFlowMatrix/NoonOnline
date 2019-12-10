import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { LessonsListRoutingModule } from './lessons-list-routing.module';
import { LessonsListComponent } from './lessons-list.component';
import { PageHeaderModule, CommonDialogModule, LessonService, SharedModule, SearchModule } from '../../../shared';
import { NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';

@NgModule({
    imports: [
        CommonModule,
        PageHeaderModule,
        NgbPaginationModule.forRoot(),
        SharedModule.forRoot(),
        CommonDialogModule,
        SearchModule,
        LessonsListRoutingModule
    ],
    declarations: [LessonsListComponent],
    providers: [LessonService]
})
export class LessonListModule { }
