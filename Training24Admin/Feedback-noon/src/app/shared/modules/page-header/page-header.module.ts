import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { PageHeaderComponent } from './page-header.component';
import { NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import { UtilService } from './../../services/utils.services';
@NgModule({
    imports: [CommonModule,
        TranslateModule,
        NgbDropdownModule.forRoot(),
        RouterModule],
    declarations: [PageHeaderComponent],
    exports: [PageHeaderComponent],
    providers: [UtilService]
})
export class PageHeaderModule { }
