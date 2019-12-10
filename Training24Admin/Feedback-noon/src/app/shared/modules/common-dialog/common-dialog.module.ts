import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CommonDialogComponent } from './common-dialog.component';

@NgModule({
    imports: [CommonModule],
    declarations: [CommonDialogComponent],
    exports: [CommonDialogComponent]
})
export class CommonDialogModule {}
