import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgbModalModule } from '@ng-bootstrap/ng-bootstrap';
import { NgbTabsetModule } from '@ng-bootstrap/ng-bootstrap';
import { FeedbackDetailRoutingModule } from './feedback-detail-routing.module';
import { FeedbackDetailComponent } from './feedback-detail.component';
import { ChatBoxComponent } from './chatbox/chatbox.component';
import { MetadataComponent } from './metadata/metadata.component';
import { TimestampsComponent } from './timestamps/timestamps.component';
import { InformerDetailComponent } from './informer-detail/informer-detail.component';
import { FilmingStatusComponent } from './filming-status/filming-status.component';
import { GraphicsStatusComponent } from './graphics-status/graphics-status.component';
import { EditingStatusComponent } from './editing-status/editing-status.component';
import { FeedbackService, SharedModule } from '../../shared';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    SharedModule.forRoot(),
    NgbModalModule.forRoot(),
    NgbTabsetModule.forRoot(),
    FeedbackDetailRoutingModule
  ],
  declarations: [
    FeedbackDetailComponent,
    ChatBoxComponent,
    MetadataComponent,
    TimestampsComponent,
    InformerDetailComponent,
    FilmingStatusComponent,
    GraphicsStatusComponent,
    EditingStatusComponent
  ],
  providers: [FeedbackService]
})
export class FeedbackDetailModule { }
