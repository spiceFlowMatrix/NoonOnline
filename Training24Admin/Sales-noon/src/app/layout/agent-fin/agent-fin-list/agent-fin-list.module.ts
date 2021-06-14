import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';

import { AgentFinListRoutingModule } from './agent-fin-list-routing.module';
import { AgentFinListComponent } from './agent-fin-list.component';
import { PageHeaderModule, CommonDialogModule, AgentService, SharedModule, SearchModule, UsersService } from '../../../shared';
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
        AgentFinListRoutingModule
    ],
    declarations: [AgentFinListComponent],
    providers: [AgentService,UsersService]
})
export class AgentFinListModule { }
