import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ChaptersListRoutingModule } from './chapters-list-routing.module';
import { ChaptersListComponent } from './chapters-list.component';
import { PageHeaderModule, CommonDialogModule, ChapterService, SharedModule, SearchModule } from '../../../shared';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

@NgModule({
    imports: [
        CommonModule,
        PageHeaderModule,
        NgbModule.forRoot(),
        SharedModule.forRoot(),
        CommonDialogModule,
        SearchModule,
        ChaptersListRoutingModule
    ],
    declarations: [ChaptersListComponent],
    providers: [ChapterService]
})
export class ChaptersListModule { }
