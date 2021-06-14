import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AgentFianancesRoutingModule } from './agent-finances-routing.module';
import { AgentFinancesComponent } from './agent-finances.component';

@NgModule({
  imports: [
    CommonModule,
    AgentFianancesRoutingModule
  ],
  declarations: [AgentFinancesComponent]
})
export class AgentFianancesModule { }
