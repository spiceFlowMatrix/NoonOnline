import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { QuizsComponent } from './quizs.component';

const routes: Routes = [
    {
        path: '', component: QuizsComponent,
        children: [
            { path: '', redirectTo: 'quizs-list', pathMatch: 'prefix' },
            { path: 'quizs-list', loadChildren: './quizs-list/quizs-list.module#QuizsListModule' },
            { path: 'add-quiz', loadChildren: './add-quizs/add-quizs.module#AddQuizsModule' },
            { path: 'edit-quiz/:id', loadChildren: './add-quizs/add-quizs.module#AddQuizsModule' }
        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class QuizsRoutingModule {
}
