import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FeedbackListRoutingModule } from './feedback-list-routing.module';
import { FeedbackListComponent } from './feedback-list.component';
import { SharedModule, FeedbackService, UsersService, GradeService, DirectivesModule } from '../../shared';
import { TaskInputItemComponentModule } from './../components';
import { NgbPopoverModule, NgbDatepickerModule } from '@ng-bootstrap/ng-bootstrap';

@NgModule({
  imports: [
    CommonModule,
    SharedModule.forRoot(),
    NgbPopoverModule.forRoot(),
    NgbDatepickerModule.forRoot(),
    TaskInputItemComponentModule,
    DirectivesModule,
    FeedbackListRoutingModule
  ],
  declarations: [FeedbackListComponent],
  providers: [FeedbackService, UsersService, GradeService]
})
export class FeedbackListModule { }
