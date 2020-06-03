import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PageHeaderModule, CommonDialogModule, SharedModule, SearchModule } from '../../../shared';
import { NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import {  AppUpdateFormRoutingModule } from './app-update-form-routing.module';
import {  AppUpdateFormComponent } from './app-update-form.component';
import { AppUpdateService } from '../../../shared/services/app-update.services';

@NgModule({
    imports: [
        CommonModule,
        PageHeaderModule,
        NgbPaginationModule.forRoot(),
        SharedModule.forRoot(),
        CommonDialogModule,
        SearchModule,
        AppUpdateFormRoutingModule
    ],
    declarations: [AppUpdateFormComponent],
    providers: [AppUpdateService]
})
export class AppUpdateFormModule { }
