import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgbAccordionModule, NgbModalModule, NgbTimepickerModule } from '@ng-bootstrap/ng-bootstrap';
import { FileDropModule } from 'ngx-file-drop';
import { PublicFeedbackFormComponent } from './feedback-form.component';
import { SharedModule, GradeService, FeedbackService } from './../../../shared';
import { TranslateModule } from '@ngx-translate/core';
@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    FileDropModule,
    TranslateModule,    
    SharedModule.forRoot(),
    NgbTimepickerModule.forRoot(),
    NgbModalModule.forRoot(),
    NgbAccordionModule.forRoot()
  ],
  declarations: [PublicFeedbackFormComponent],
  exports: [PublicFeedbackFormComponent],
  providers: [GradeService, FeedbackService]
})
export class PublicFeedbackFormModule { }
