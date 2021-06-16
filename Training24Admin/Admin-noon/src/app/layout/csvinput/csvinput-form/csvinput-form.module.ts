import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PageHeaderModule, CommonDialogModule, SharedModule, SearchModule } from '../../../shared';
import { NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { CsvInputFormRoutingModule } from './csvinput-form-routing.module';
import { CsvInputFormComponent } from './csvinput-form.component';
import { CsvService } from '../../../shared/services/csv.services';

@NgModule({
    imports: [
        CommonModule,
        PageHeaderModule,
        NgbPaginationModule.forRoot(),
        SharedModule.forRoot(),
        CommonDialogModule,
        SearchModule,
        CsvInputFormRoutingModule
    ],
    declarations: [CsvInputFormComponent],
    providers: [CsvService]
})
export class CsvInputFormModule { }
