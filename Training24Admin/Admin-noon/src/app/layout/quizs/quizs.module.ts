import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { QuizsRoutingModule } from './quizs-routing.module';
import { QuizsComponent } from './quizs.component';

@NgModule({
    imports: [CommonModule, QuizsRoutingModule],
    declarations: [QuizsComponent]
})
export class QuizsModule { }
