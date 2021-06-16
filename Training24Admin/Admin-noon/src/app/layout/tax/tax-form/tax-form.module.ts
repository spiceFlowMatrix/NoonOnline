import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PageHeaderModule, CommonDialogModule, SharedModule, SearchModule } from '../../../shared';
import { NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { TaxFormRoutingModule } from './tax-form-routing.module';
import { TaxFormComponent } from './tax-form.component';
import { TaxService } from '../../../shared/services/tax.services';

@NgModule({
    imports: [
        CommonModule,
        PageHeaderModule,
        NgbPaginationModule.forRoot(),
        SharedModule.forRoot(),
        CommonDialogModule,
        SearchModule,
        TaxFormRoutingModule
    ],
    declarations: [TaxFormComponent],
    providers: [TaxService]
})
export class TaxFormModule { }
