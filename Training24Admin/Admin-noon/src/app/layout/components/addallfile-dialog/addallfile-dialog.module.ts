import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AddAllFileDialogComponent } from './addallfile-dialog.component';
@NgModule({
    imports: [
        CommonModule,
        FormsModule
    ],
    declarations: [AddAllFileDialogComponent],
    exports: [AddAllFileDialogComponent],
    providers: []
})
export class AddAllFileModule { }