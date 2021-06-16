import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AddFileDialogComponent } from './addfile-dialog.component';
import { FormsModule } from '@angular/forms';
@NgModule({
    imports: [
        CommonModule,
        FormsModule
    ],
    declarations: [AddFileDialogComponent],
    exports: [AddFileDialogComponent],
    providers: []
})
export class AddFileModule { }