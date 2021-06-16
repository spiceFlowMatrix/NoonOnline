import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ListsRoutingModule } from './lists-routing.module';
import { ListsComponent } from './lists.component';
import { FeedbackService, UtilService, DirectivesModule, CommonDialogModule } from '../../../shared';

import {
    RoleAssignmentComponentModule,
    TaskInputItemComponentModule,
    TaskManagementModule,
    PublicFeedbackFormModule
} from '../../components';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatDividerModule } from '@angular/material/divider';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatTabsModule } from '@angular/material/tabs';
import { MatSelectModule } from '@angular/material/select';
import {MatDatepickerModule} from '@angular/material/datepicker';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { SatDatepickerModule, SatNativeDateModule, DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE } from 'saturn-datepicker';
import { MAT_MOMENT_DATE_FORMATS, MomentDateAdapter } from '@angular/material-moment-adapter'
import { MatNativeDateModule } from '@angular/material/core';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        MatFormFieldModule,
        MatDividerModule,
        MatTableModule,
        MatPaginatorModule,
        MatNativeDateModule,
        MatInputModule,
        MatDatepickerModule,
        SatDatepickerModule,
        MatTabsModule,
        SatNativeDateModule,
        MatSelectModule,
        MatCheckboxModule,
        MatIconModule,
        PublicFeedbackFormModule,
        RoleAssignmentComponentModule,
        TaskInputItemComponentModule,
        ListsRoutingModule,
        TaskManagementModule,
        CommonDialogModule,
        DirectivesModule
    ],
    declarations: [
        ListsComponent,
    ],
    providers: [FeedbackService, UtilService
    ,  {provide: DateAdapter, useClass: MomentDateAdapter, deps: [MAT_DATE_LOCALE]},
    {provide: MAT_DATE_FORMATS, useValue: MAT_MOMENT_DATE_FORMATS},
]
})
export class ListsModule { }
