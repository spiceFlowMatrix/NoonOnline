import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgbModalModule, NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { AddPackageRoutingModule } from './add-package-routing.module';
import { AddPackageComponent } from './add-package.component';
import {
    PageHeaderModule,
    SharedModule,
    CommonDialogModule,
    DirectivesModule,
    GradeService,
    CourseService
} from '../../../shared';
import { QuillModule } from 'ngx-quill';
import { PackageService } from '../../../shared/services/package.service';
import { CourseSelectionModule } from '../../components';

@NgModule({
    imports: [
        CommonModule,
        PageHeaderModule,
        CommonDialogModule,
        SharedModule.forRoot(),
        NgbModalModule.forRoot(),
        NgbPaginationModule.forRoot(),
        CourseSelectionModule,
        DirectivesModule,
        QuillModule,
        AddPackageRoutingModule,
    ],
    declarations: [AddPackageComponent],
    providers: [GradeService, PackageService,CourseService]
})
export class AddPackageModule { }
