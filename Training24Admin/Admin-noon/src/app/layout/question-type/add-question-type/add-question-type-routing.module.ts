import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AddQuestionTypeComponent } from './add-question-type.component';

const routes: Routes = [
    {
        path: '', component: AddQuestionTypeComponent,        
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class AddQuestionTypeRoutingModule {
}
