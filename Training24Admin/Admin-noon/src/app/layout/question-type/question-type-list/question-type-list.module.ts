import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { QuestionTypeListRoutingModule } from './question-type-list-routing.module';
import { QuestionTypeListComponent } from './question-type-list.component';
import { PageHeaderModule, CommonDialogModule, QuestionTypeService, SharedModule, SearchModule } from '../../../shared';
import { NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';

@NgModule({
    imports: [
        CommonModule,
        PageHeaderModule,
        NgbPaginationModule.forRoot(),
        SharedModule.forRoot(),
        CommonDialogModule,
        SearchModule,
        QuestionTypeListRoutingModule
    ],
    declarations: [QuestionTypeListComponent],
    providers: [QuestionTypeService]
})
export class QuestionTypeListModule { }
