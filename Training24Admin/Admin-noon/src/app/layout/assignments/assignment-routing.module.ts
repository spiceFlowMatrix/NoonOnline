import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AssignmentsComponent } from './assignment.component';

const routes: Routes = [
    {
        path: '', component: AssignmentsComponent,
        children: [
            { path: '', redirectTo: 'assignment-list', pathMatch: 'prefix' },
            { path: 'assignment-list', loadChildren: './assignment-list/assignment-list.module#AssignmentListModule' },
            { path: 'add-assignment', loadChildren: './add-assignment/add-assignment.module#AddAssignmentModule' },
            { path: 'edit-assignment/:assignid', loadChildren: './add-assignment/add-assignment.module#AddAssignmentModule' }
        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class AssignmentsRoutingModule {
}
