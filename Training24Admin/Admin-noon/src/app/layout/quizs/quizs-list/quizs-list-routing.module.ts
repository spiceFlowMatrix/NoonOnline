import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { QuizsListComponent } from './quizs-list.component';

const routes: Routes = [
    {
        path: '', component: QuizsListComponent,
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class QuizsListRoutingModule {
}
