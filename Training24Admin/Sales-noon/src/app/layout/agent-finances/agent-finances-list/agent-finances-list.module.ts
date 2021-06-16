import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';

import { AgentFinancesListRoutingModule } from './agent-finances-list-routing.module';
import { AgentFinancesListComponent } from './agent-finances-list.component';
import { PageHeaderModule, CommonDialogModule, AgentService, SharedModule, SearchModule } from '../../../shared';
import { NgbPaginationModule, NgbAccordionModule } from '@ng-bootstrap/ng-bootstrap';

@NgModule({
    imports: [
        CommonModule,
        PageHeaderModule,
        HttpClientModule,
        NgbAccordionModule.forRoot(),
        NgbPaginationModule.forRoot(),
        SharedModule.forRoot(),
        CommonDialogModule,
        SearchModule,
        AgentFinancesListRoutingModule
    ],
    declarations: [AgentFinancesListComponent],
    providers: [AgentService]
})
export class AgentFinancesListModule { }
