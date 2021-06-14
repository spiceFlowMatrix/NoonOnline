import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AddQuestionTypeRoutingModule } from './add-question-type-routing.module';
import { AddQuestionTypeComponent } from './add-question-type.component';
import { PageHeaderModule, CommonDialogModule, SharedModule, QuestionTypeService } from '../../../shared';

@NgModule({
    imports: [
        CommonModule,
        PageHeaderModule,
        CommonDialogModule,
        SharedModule.forRoot(),
        AddQuestionTypeRoutingModule
    ],
    declarations: [AddQuestionTypeComponent],
    providers: [QuestionTypeService]
})
export class AddQuestionTypeModule { }
