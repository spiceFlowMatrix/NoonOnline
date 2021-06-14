import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CourseSelectionComponent } from './course-selection.component';
import { FormsModule } from '@angular/forms';
@NgModule({
    imports: [
        CommonModule,
        FormsModule
    ],
    declarations: [CourseSelectionComponent],
    exports: [CourseSelectionComponent],
    providers: []
})
export class CourseSelectionModule { }