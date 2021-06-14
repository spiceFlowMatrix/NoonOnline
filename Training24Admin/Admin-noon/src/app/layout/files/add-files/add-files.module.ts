import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AddFileRoutingModule } from './add-files-routing.module';
import { AddFileComponent } from './add-files.component';
import { PageHeaderModule, CommonDialogModule, SharedModule, FileService } from '../../../shared';
import { UploadStatusModule } from './../../components';

@NgModule({
    imports: [
        CommonModule,
        PageHeaderModule,
        CommonDialogModule,
        UploadStatusModule,
        SharedModule.forRoot(),
        AddFileRoutingModule
    ],
    declarations: [AddFileComponent],
    providers: [FileService]
})
export class AddFilesModule { }
