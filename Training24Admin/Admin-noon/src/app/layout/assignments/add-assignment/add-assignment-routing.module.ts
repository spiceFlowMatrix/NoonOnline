import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AddAssignmentComponent } from './add-assignment.component';

const routes: Routes = [
    {
        path: '', component: AddAssignmentComponent,        
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class AddAssignmentRoutingModule {
}
