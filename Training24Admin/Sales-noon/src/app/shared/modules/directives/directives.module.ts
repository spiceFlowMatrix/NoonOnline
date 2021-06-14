import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import {
    BackDirectionDirective,
    DebounceDirective,
    LoaderComponent
} from '../directives';

@NgModule({
    imports: [
        CommonModule, 
        RouterModule
    ],
    declarations: [
        BackDirectionDirective, 
        DebounceDirective,
        LoaderComponent
    ],
    exports: [
        BackDirectionDirective, 
        DebounceDirective,
        LoaderComponent
    ]
})
export class DirectivesModule { }
