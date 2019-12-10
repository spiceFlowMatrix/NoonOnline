import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AddQuizsComponent } from './add-quizs.component';

const routes: Routes = [
    {
        path: '', component: AddQuizsComponent,
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class AddQuizsRoutingModule {
}
