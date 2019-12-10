import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AddAgentRoutingModule } from './add-agent-routing.module';
import { AddAgentComponent } from './add-agent.component';
import { PageHeaderModule, CommonDialogModule, SharedModule, AgentService } from '../../../shared';

@NgModule({
    imports: [
        CommonModule,
        PageHeaderModule,
        CommonDialogModule,
        SharedModule.forRoot(),
        AddAgentRoutingModule
    ],
    declarations: [AddAgentComponent],
    providers: [AgentService]
})
export class AddAgentModule { }
