import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AgentFinRoutingModule } from './agent-fin-routing.module';
import { AgentFinComponent } from './agent-fin.component';

@NgModule({
  imports: [
    CommonModule,
    AgentFinRoutingModule
  ],
  declarations: [AgentFinComponent]
})
export class AgentFinModule { }
