import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AddSchoolRoutingModule } from './add-school-routing.module';
import { AddSchoolComponent } from './add-school.component';
import { PageHeaderModule, CommonDialogModule, SharedModule,SchoolService } from '../../../shared';

@NgModule({
    imports: [
        CommonModule,
        PageHeaderModule,
        CommonDialogModule,
        SharedModule.forRoot(),
        AddSchoolRoutingModule
    ],
    declarations: [AddSchoolComponent],
    providers: [SchoolService]
})
export class AddSchoolModule { }
