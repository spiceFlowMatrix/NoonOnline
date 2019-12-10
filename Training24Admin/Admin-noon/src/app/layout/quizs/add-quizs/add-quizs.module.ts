import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgbModalModule, NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { AddQuizsRoutingModule } from './add-quizs-routing.module';
import { AddQuizsComponent } from './add-quizs.component';
import {
    PageHeaderModule,
    SharedModule,
    QuizService,
    QuestionsService,
    CommonDialogModule,
    DirectivesModule
} from '../../../shared';
import { QuillModule } from 'ngx-quill';
import { MatCommonModule } from '@angular/material/core';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatCardModule } from '@angular/material/card';
import {MatCheckboxModule} from '@angular/material/checkbox';
import { KatexModule } from 'ng-katex';

@NgModule({
    imports: [
        CommonModule,
        SharedModule.forRoot(),
        PageHeaderModule,
        CommonDialogModule,
        MatCommonModule,
        MatInputModule,
        MatButtonModule,
        MatFormFieldModule,
        MatCardModule,
        MatCheckboxModule,
        NgbModalModule.forRoot(),
        NgbPaginationModule.forRoot(),
        DirectivesModule,
        QuillModule,
        AddQuizsRoutingModule,
    ],
    declarations: [AddQuizsComponent],
    providers: [QuizService, QuestionsService]
})
export class AddQuizsModule { }
