import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgbTypeaheadModule } from '@ng-bootstrap/ng-bootstrap';
import { RoleAssignmentComponent } from './role-assignment.component';
import { FormsModule } from '@angular/forms';
import { UtilService } from './../../../shared';
@NgModule({
    imports: [
        CommonModule,
        NgbTypeaheadModule.forRoot(),
        FormsModule
    ],
    declarations: [RoleAssignmentComponent],
    exports: [RoleAssignmentComponent],
    providers: [UtilService]
})
export class RoleAssignmentComponentModule { }