import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UploadStatusDialogComponent } from './upload-status.component';
import { FormsModule } from '@angular/forms';
@NgModule({
    imports: [
        CommonModule,
        FormsModule
    ],
    declarations: [UploadStatusDialogComponent],
    exports: [UploadStatusDialogComponent],
    providers: []
})
export class UploadStatusModule { }