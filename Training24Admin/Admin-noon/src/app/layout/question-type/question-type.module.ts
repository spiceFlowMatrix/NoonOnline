import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { QuestionTypesRoutingModule } from './question-type-routing.module';
import { QuestionTypesComponent } from './question-type.component';

@NgModule({
    imports: [CommonModule, QuestionTypesRoutingModule],
    declarations: [QuestionTypesComponent]
})
export class QuestionTypesModule { }
