import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AddCourseDefinitionComponent } from './add-course-definition.component';

const routes: Routes = [
    {
        path: '', component: AddCourseDefinitionComponent,        
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class AddCourseDefinitionRoutingModule {
}
