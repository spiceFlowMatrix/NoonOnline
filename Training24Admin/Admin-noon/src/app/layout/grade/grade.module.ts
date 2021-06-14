import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { GradeRoutingModule } from './grade-routing.module';
import { GradeComponent } from './grade.component';

@NgModule({
    imports: [CommonModule, GradeRoutingModule],
    declarations: [GradeComponent]
})
export class GradeModule { }
