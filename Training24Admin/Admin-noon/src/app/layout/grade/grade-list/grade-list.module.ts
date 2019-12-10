import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { GradeListRoutingModule } from './grade-list-routing.module';
import { GradeListComponent } from './grade-list.component';
import { PageHeaderModule, CommonDialogModule, GradeService, SharedModule, SearchModule } from '../../../shared';
import { NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';

@NgModule({
    imports: [
        CommonModule,
        PageHeaderModule,
        NgbPaginationModule.forRoot(),
        SharedModule.forRoot(),
        SearchModule,
        CommonDialogModule,
        GradeListRoutingModule
    ],
    declarations: [GradeListComponent],
    providers: [GradeService]
})
export class GradeListModule { }
