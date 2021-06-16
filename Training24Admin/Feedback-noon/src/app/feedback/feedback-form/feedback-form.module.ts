import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgbAccordionModule, NgbModalModule, NgbTimepickerModule } from '@ng-bootstrap/ng-bootstrap';
import { FileDropModule } from 'ngx-file-drop';
import { FeedbackFormRoutingModule } from './feedback-form-routing.module';
import { FeedbackFormComponent } from './feedback-form.component';
import { SharedModule, GradeService, FeedbackService } from '../../shared';
import { TranslateModule } from '@ngx-translate/core';
import {
  PublicFeedbackFormModule
} from './../../layout/components';
@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    FileDropModule,
    TranslateModule,
    SharedModule.forRoot(),
    NgbTimepickerModule.forRoot(),
    NgbModalModule.forRoot(),
    NgbAccordionModule.forRoot(),
    PublicFeedbackFormModule,
    FeedbackFormRoutingModule
  ],
  declarations: [FeedbackFormComponent],
  providers: [GradeService, FeedbackService]
})
export class FeedbackFormModule { }
