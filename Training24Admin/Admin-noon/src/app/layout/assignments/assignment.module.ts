import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AssignmentsRoutingModule } from './assignment-routing.module';
import { AssignmentsComponent } from './assignment.component';

@NgModule({
    imports: [CommonModule, AssignmentsRoutingModule],
    declarations: [AssignmentsComponent]
})
export class AssignmentsModule { }
