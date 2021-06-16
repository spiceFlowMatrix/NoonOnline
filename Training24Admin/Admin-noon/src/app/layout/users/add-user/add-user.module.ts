import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgbModalModule, NgbPaginationModule, NgbDatepickerModule } from '@ng-bootstrap/ng-bootstrap';
import { AddUserRoutingModule } from './add-user-routing.module';
import { AddUserComponent } from './add-user.component';
import { PageHeaderModule, SharedModule, UsersService, SchoolService, CommonDialogModule, CourseService, GradeService } from '../../../shared';

@NgModule({
    imports: [
        CommonModule,
        PageHeaderModule,
        CommonDialogModule,
        SharedModule.forRoot(),
        NgbModalModule.forRoot(),
        NgbPaginationModule.forRoot(),
        NgbDatepickerModule.forRoot(),
        AddUserRoutingModule,
    ],
    declarations: [AddUserComponent],
    providers: [UsersService, CourseService, SchoolService, GradeService]
})
export class AddUserModule { }
