import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AgentListRoutingModule } from './agents-list-routing.module';
import { AgentListComponent } from './agents-list.component';
import { PageHeaderModule, CommonDialogModule, AgentService, SharedModule, SearchModule } from '../../../shared';
import { NgbPaginationModule, NgbAccordionModule } from '@ng-bootstrap/ng-bootstrap';

@NgModule({
    imports: [
        CommonModule,
        PageHeaderModule,
        NgbAccordionModule.forRoot(),
        NgbPaginationModule.forRoot(),
        SharedModule.forRoot(),
        CommonDialogModule,
        SearchModule,
        AgentListRoutingModule
    ],
    declarations: [AgentListComponent],
    providers: [AgentService]
})
export class AgentListModule { }
