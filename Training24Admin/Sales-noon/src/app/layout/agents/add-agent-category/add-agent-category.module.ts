import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AddCategoryRoutingModule } from './add-agent-category-routing.module';
import { AddCategoryComponent } from './add-agent-category.component';
import { PageHeaderModule, CommonDialogModule, SharedModule, AgentService } from '../../../shared';

@NgModule({
    imports: [
        CommonModule,
        PageHeaderModule,
        CommonDialogModule,
        SharedModule.forRoot(),
        AddCategoryRoutingModule
    ],
    declarations: [AddCategoryComponent],
    providers: [AgentService]
})
export class AddCategoryModule { }
