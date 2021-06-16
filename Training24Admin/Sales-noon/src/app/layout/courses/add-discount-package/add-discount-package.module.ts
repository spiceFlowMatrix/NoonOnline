import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AddDiscountRoutingModule } from './add-discount-package-routing.module';
import { AddDiscountPackageComponent } from './add-discount-package.component';
import { PageHeaderModule, CommonDialogModule, SharedModule, CourseService } from '../../../shared';

@NgModule({
    imports: [
        CommonModule,
        PageHeaderModule,
        CommonDialogModule,
        SharedModule.forRoot(),
        AddDiscountRoutingModule
    ],
    declarations: [AddDiscountPackageComponent],
    providers: [CourseService]
})
export class AddDiscountPackageModule { }
