import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { DirectivesModule } from './../directives/directives.module';
import { SearchComponent } from './search.component';

@NgModule({
    imports: [CommonModule, FormsModule, DirectivesModule],
    declarations: [SearchComponent],
    exports: [SearchComponent]
})
export class SearchModule { }
