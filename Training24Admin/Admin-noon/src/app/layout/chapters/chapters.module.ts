import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ChaptersRoutingModule } from './chapters-routing.module';
import { ChaptersComponent } from './chapters.component';

@NgModule({
    imports: [CommonModule, ChaptersRoutingModule],
    declarations: [ChaptersComponent]
})
export class ChaptersModule { }
