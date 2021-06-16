import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgbAccordionModule } from '@ng-bootstrap/ng-bootstrap';
import { AgentsRoutingModule } from './agents-routing.module';
import { AgentsComponent } from './agents.component';

@NgModule({
  imports: [
    CommonModule,
    AgentsRoutingModule
  ],
  declarations: [AgentsComponent]
})
export class AgentsModule { }
