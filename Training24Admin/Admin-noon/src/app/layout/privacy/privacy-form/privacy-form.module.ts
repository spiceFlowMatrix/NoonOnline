import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PageHeaderModule, CommonDialogModule, SharedModule, SearchModule } from '../../../shared';
import { NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { PrivacyFormComponent } from './privacy-form.component';
import { PrivacyFormRoutingModule } from './privacy-form-routing.module';
import { QuillModule } from 'ngx-quill';
import { PrivacyService } from '../../../shared/services/privacy.services';

@NgModule({
    imports: [
        CommonModule,
        PageHeaderModule,
        NgbPaginationModule.forRoot(),
        SharedModule.forRoot(),
        QuillModule,
        CommonDialogModule,
        SearchModule,
        PrivacyFormRoutingModule
    ],
    declarations: [PrivacyFormComponent],
    providers: [PrivacyService]
})
export class PrivacyFormModule { }
