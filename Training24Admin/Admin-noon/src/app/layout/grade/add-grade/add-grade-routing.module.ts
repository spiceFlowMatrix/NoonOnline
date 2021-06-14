import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AddGradeComponent } from './add-grade.component';

const routes: Routes = [
    {
        path: '', component: AddGradeComponent,        
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class AddGradeRoutingModule {
}
