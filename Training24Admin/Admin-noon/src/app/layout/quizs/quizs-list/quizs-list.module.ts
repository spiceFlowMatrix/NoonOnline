import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { QuizsListRoutingModule } from './quizs-list-routing.module';
import { QuizsListComponent } from './quizs-list.component';
import { PageHeaderModule, CommonDialogModule, QuizService, SharedModule, SearchModule } from '../../../shared';
import { NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { QuizPreviewModule } from './../../components';

@NgModule({
    imports: [
        CommonModule,
        PageHeaderModule,
        NgbPaginationModule.forRoot(),
        SharedModule.forRoot(),
        CommonDialogModule,
        SearchModule,
        QuizPreviewModule,
        QuizsListRoutingModule
    ],
    declarations: [QuizsListComponent],
    providers: [QuizService]
})
export class QuizsListModule { }
