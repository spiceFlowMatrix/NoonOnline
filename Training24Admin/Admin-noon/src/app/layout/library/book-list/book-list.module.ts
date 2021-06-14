import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { BookListRoutingModule } from './book-list-routing.module';
import { BookListComponent } from './book-list.component';
import { PageHeaderModule, CommonDialogModule, SharedModule, SearchModule, GradeService } from '../../../shared';
import { NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { QuizPreviewModule } from './../../components';
import { LibraryService } from '../../../shared/services/library.services';

@NgModule({
    imports: [
        CommonModule,
        PageHeaderModule,
        NgbPaginationModule.forRoot(),
        SharedModule.forRoot(),
        CommonDialogModule,
        SearchModule,
        QuizPreviewModule,
        BookListRoutingModule
    ],
    declarations: [BookListComponent],
    providers: [LibraryService,GradeService]
})
export class BookListModule { }
