import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AdditionalListRoutingModule } from './additional-list-routing.module';
import { AdditionalListComponent } from './additional-list.component';
import { PageHeaderModule, CommonDialogModule, AssignmentService, SharedModule, SearchModule } from '../../../shared';
import { NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { AdditionalService } from '../../../shared/services/additionalservice.services';

@NgModule({
    imports: [
        CommonModule,
        PageHeaderModule,
        NgbPaginationModule.forRoot(),
        SharedModule.forRoot(),
        CommonDialogModule,
        SearchModule,
        AdditionalListRoutingModule
    ],
    declarations: [AdditionalListComponent],
    providers: [AdditionalService]
})
export class AdditionalListModule { }
