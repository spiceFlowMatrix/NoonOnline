import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PageHeaderModule, CommonDialogModule, SharedModule, SearchModule } from '../../../shared';
import { NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { AccountListRoutingModule } from './accountlist-routing.module';
import { AccountListComponent } from './accountlist.component';
import { SalesConfigService } from '../../../shared/services/salesconfig.services';

@NgModule({
    imports: [
        CommonModule,
        PageHeaderModule,
        NgbPaginationModule.forRoot(),
        SharedModule.forRoot(),
        CommonDialogModule,
        SearchModule,
        AccountListRoutingModule
    ],
    declarations: [AccountListComponent],
    providers: [SalesConfigService]
})
export class AccountListModule { }
