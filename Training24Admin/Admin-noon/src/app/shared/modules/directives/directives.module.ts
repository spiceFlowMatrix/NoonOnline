import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import {
    BackDirectionDirective,
    DebounceDirective,
    LoaderComponent
} from '../directives';
import { DragDirective } from './dragDrop.directive';

@NgModule({
    imports: [
        CommonModule, 
        RouterModule
    ],
    declarations: [
        BackDirectionDirective, 
        DebounceDirective,
        DragDirective,
        LoaderComponent
    ],
    exports: [
        BackDirectionDirective, 
        DebounceDirective,
        DragDirective,
        LoaderComponent
    ]
})
export class DirectivesModule { }
