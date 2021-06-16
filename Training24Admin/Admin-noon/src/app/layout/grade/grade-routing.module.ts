import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { GradeComponent } from './grade.component';

const routes: Routes = [
    {
        path: '', component: GradeComponent,
        children: [
            { path: '', redirectTo: 'grade-list', pathMatch: 'prefix' },
            { path: 'grade-list', loadChildren: './grade-list/grade-list.module#GradeListModule' },
            { path: 'add-grade', loadChildren: './add-grade/add-grade.module#AddGradeModule' },
            { path: 'edit-grade/:id', loadChildren: './add-grade/add-grade.module#AddGradeModule' }
        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class GradeRoutingModule {
}
