import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SchoolListRoutingModule } from './school-list-routing.module';
import { SchoolListComponent} from './school-list.component';
import { PageHeaderModule, CommonDialogModule, SharedModule, SearchModule,SchoolService } from '../../../shared';
import { NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';

@NgModule({
    imports: [
        CommonModule,
        PageHeaderModule,
        NgbPaginationModule.forRoot(),
        SharedModule.forRoot(),
        CommonDialogModule,
        SearchModule,
        SchoolListRoutingModule
    ],
    declarations: [SchoolListComponent],
    providers: [SchoolService]
})
export class SchoolListModule { }
