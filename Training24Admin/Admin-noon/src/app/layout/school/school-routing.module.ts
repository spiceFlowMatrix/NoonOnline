import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { SchoolComponent } from './school.component';

const routes: Routes = [
    {
        path: '', component: SchoolComponent,
        children: [
            { path: '', redirectTo: 'school-list', pathMatch: 'prefix' },
            { path: 'school-list', loadChildren: './school-list/school-list.module#SchoolListModule' },
            { path: 'add-school', loadChildren: './add-school/add-school.module#AddSchoolModule' },
            { path: 'edit-school/:id', loadChildren: './add-school/add-school.module#AddSchoolModule' }
        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class SchoolRoutingModule {
}
