import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { QuestionListRoutingModule } from './questions-list-routing.module';
import { QuestionListComponent } from './questions-list.component';
import { PageHeaderModule, CommonDialogModule, QuestionsService, SharedModule, SearchModule } from '../../../shared';
import { NgbPaginationModule, NgbModalModule } from '@ng-bootstrap/ng-bootstrap';

@NgModule({
    imports: [
        CommonModule,
        PageHeaderModule,
        FormsModule,
        NgbModalModule.forRoot(),
        NgbPaginationModule.forRoot(),
        SharedModule.forRoot(),
        CommonDialogModule,
        SearchModule,
        QuestionListRoutingModule
    ],
    declarations: [QuestionListComponent],
    providers: [QuestionsService]
})
export class QuestionListModule { }
