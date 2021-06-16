import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TaskInputItemComponent } from './task-input-item.component';
import { FormsModule } from '@angular/forms';
import { UtilService } from './../../../shared';

@NgModule({
    imports: [
        CommonModule,
        FormsModule
    ],
    declarations: [TaskInputItemComponent],
    exports: [TaskInputItemComponent],
    providers: [UtilService]
})
export class TaskInputItemComponentModule { }