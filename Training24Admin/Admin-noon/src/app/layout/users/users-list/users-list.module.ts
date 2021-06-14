import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { UsersListRoutingModule } from './users-list-routing.module';
import { UsersListComponent } from './users-list.component';
import { PageHeaderModule, CommonDialogModule, UsersService, SharedModule, SearchModule, AuthenticationService } from '../../../shared';
import { NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';

@NgModule({
    imports: [
        CommonModule,
        PageHeaderModule,
        NgbPaginationModule.forRoot(),
        SharedModule.forRoot(),
        CommonDialogModule,
        SearchModule,
        UsersListRoutingModule
    ],
    declarations: [UsersListComponent],
    providers: [UsersService,AuthenticationService]
})
export class UsersListModule { }
