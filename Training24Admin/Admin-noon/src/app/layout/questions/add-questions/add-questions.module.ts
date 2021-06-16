import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AddQuestionRoutingModule } from './add-questions-routing.module';
import { AddQuestionComponent } from './add-questions.component';
import { PageHeaderModule, CommonDialogModule, SharedModule, QuestionsService, QuestionTypeService, FileService } from '../../../shared';

@NgModule({
    imports: [
        CommonModule,
        PageHeaderModule,
        CommonDialogModule,
        SharedModule.forRoot(),
        AddQuestionRoutingModule
    ],
    declarations: [AddQuestionComponent],
    providers: [QuestionsService, QuestionTypeService, FileService]
})
export class AddQuestionModule { }
