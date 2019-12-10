import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { QuestionTypeListComponent } from './question-type-list.component';

const routes: Routes = [
    {
        path: '', component: QuestionTypeListComponent,
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class QuestionTypeListRoutingModule {
}
