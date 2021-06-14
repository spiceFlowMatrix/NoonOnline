import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CsvInputComponent } from './csvinput.component';
import { CsvInputRoutingModule } from './csvinput-routing.module';

@NgModule({
    imports: [CommonModule, CsvInputRoutingModule],
    declarations: [CsvInputComponent],
})
export class CsvInputModule { }
