import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PackageListRoutingModule } from './package-list-routing.module';
import { PackageListComponent } from './package-list.component';
import { PageHeaderModule, CommonDialogModule, SharedModule, SearchModule } from '../../../shared';
import { NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { PackageService } from '../../../shared/services/package.service';


@NgModule({
    imports: [
        CommonModule,
        PageHeaderModule,
        NgbPaginationModule.forRoot(),
        SharedModule.forRoot(),
        CommonDialogModule,
        SearchModule,
        PackageListRoutingModule
    ],
    declarations: [PackageListComponent],
    providers: [PackageService]
})
export class PackageListModule { }
