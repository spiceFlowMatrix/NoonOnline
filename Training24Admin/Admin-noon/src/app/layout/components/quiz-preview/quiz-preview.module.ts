import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { QuizPreviewComponent } from './quiz-preview.component';
import { FormsModule } from '@angular/forms';
import { NgbModalModule } from '@ng-bootstrap/ng-bootstrap';
@NgModule({
    imports: [
        CommonModule,
        NgbModalModule.forRoot(),
        FormsModule
    ],
    declarations: [QuizPreviewComponent],
    exports: [QuizPreviewComponent],
    providers: []
})
export class QuizPreviewModule { }