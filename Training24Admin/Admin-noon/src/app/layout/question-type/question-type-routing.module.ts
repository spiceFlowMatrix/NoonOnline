import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { QuestionTypesComponent } from './question-type.component';

const routes: Routes = [
    {
        path: '', component: QuestionTypesComponent,
        children: [
            { path: '', redirectTo: 'question-type-list', pathMatch: 'prefix' },
            { path: 'question-type-list', loadChildren: './question-type-list/question-type-list.module#QuestionTypeListModule' },
            { path: 'add-question-type', loadChildren: './add-question-type/add-question-type.module#AddQuestionTypeModule' },
            { path: 'edit-question-type/:id', loadChildren: './add-question-type/add-question-type.module#AddQuestionTypeModule' }
        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class QuestionTypesRoutingModule {
}
