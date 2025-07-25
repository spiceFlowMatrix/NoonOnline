import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { LessonsRoutingModule } from './lessons-routing.module';
import { LessonsComponent } from './lessons.component';

@NgModule({
    imports: [CommonModule, LessonsRoutingModule],
    declarations: [LessonsComponent]
})
export class LessonsModule { }
