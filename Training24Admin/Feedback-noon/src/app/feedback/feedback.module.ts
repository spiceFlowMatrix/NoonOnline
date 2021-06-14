import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FeedbackRoutingModule } from './feedback-routing.module';
import { FeedbackComponent } from './feedback.component';
import { PageHeaderModule } from '../shared';
@NgModule({
  imports: [
    CommonModule,
    PageHeaderModule,
    FeedbackRoutingModule
  ],
  declarations: [FeedbackComponent]
})
export class FeedbackModule { }
