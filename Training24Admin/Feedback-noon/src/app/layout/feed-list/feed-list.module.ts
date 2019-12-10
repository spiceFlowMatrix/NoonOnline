import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FeedListRoutingModule } from './feed-list-routing.module';
import { FeedListComponent } from './feed-list.component';
import { SharedModule, FeedbackService, UsersService, GradeService, DirectivesModule } from '../../shared';
import { TaskInputItemComponentModule } from '../components';
import { NgbPopoverModule, NgbDatepickerModule } from '@ng-bootstrap/ng-bootstrap';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatIconModule} from '@angular/material/icon';
import {MatDividerModule} from '@angular/material/divider';
import {MatTabsModule} from '@angular/material/tabs';
import {MatSelectModule} from '@angular/material/select';
import {MatTableModule} from '@angular/material/table';
import {MatCheckboxModule} from '@angular/material/checkbox';
import {MatPaginatorModule} from '@angular/material/paginator';

@NgModule({
  imports: [
    CommonModule,
    SharedModule.forRoot(),
    NgbPopoverModule.forRoot(),
    NgbDatepickerModule.forRoot(),
    MatFormFieldModule,
    MatDividerModule,
    MatTableModule,
    MatPaginatorModule,
    MatTabsModule,
    MatSelectModule,
    MatCheckboxModule,
    MatIconModule,
    TaskInputItemComponentModule,
    DirectivesModule,
    FeedListRoutingModule
  ],
  declarations: [FeedListComponent],
  providers: [FeedbackService, UsersService, GradeService]
})
export class FeedListModule { }
