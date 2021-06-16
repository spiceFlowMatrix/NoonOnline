import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { QuillModule } from 'ngx-quill'
import { TextManagementRoutingModule } from './text-management-routing.module';
import { TextManagementPageComponent } from './text-management.component';
import { DirectivesModule, UsersService, CourseService, UtilService } from '../../shared';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        QuillModule,
        DirectivesModule,
        TextManagementRoutingModule
    ],
    declarations: [TextManagementPageComponent],
    providers: [
        UsersService,
        UtilService
    ]
})
export class TextManagementPageModule { }
