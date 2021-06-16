import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { FilesListRoutingModule } from './files-list-routing.module';
import { FilesListComponent } from './files-list.component';
import { PageHeaderModule, CommonDialogModule, FileService, SharedModule, SearchModule } from '../../../shared';
import { NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';

@NgModule({
    imports: [
        CommonModule,
        PageHeaderModule,
        NgbPaginationModule.forRoot(),
        SharedModule.forRoot(),
        CommonDialogModule,
        SearchModule,
        FilesListRoutingModule
    ],
    declarations: [FilesListComponent],
    providers: [FileService]
})
export class FilesListModule { }
