import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SchoolRoutingModule } from './school-routing.module';
import { SchoolComponent } from './school.component';

@NgModule({
    imports: [CommonModule, SchoolRoutingModule],
    declarations: [SchoolComponent]
})
export class SchoolModule { } 
