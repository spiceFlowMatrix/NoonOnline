import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { QuestionsComponent } from './questions.component';

const routes: Routes = [
    {
        path: '', component: QuestionsComponent,
        children: [
            { path: '', redirectTo: 'question-list', pathMatch: 'prefix' },
            { path: 'question-list', loadChildren: './questions-list/questions-list.module#QuestionListModule' },
            { path: 'add-question', loadChildren: './add-questions/add-questions.module#AddQuestionModule' },
            { path: 'edit-question/:id', loadChildren: './add-questions/add-questions.module#AddQuestionModule' }
        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class QuestionsRoutingModule {
}
