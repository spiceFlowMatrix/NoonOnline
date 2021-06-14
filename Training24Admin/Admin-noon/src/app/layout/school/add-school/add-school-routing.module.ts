import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AddSchoolComponent } from './add-school.component';

const routes: Routes = [
    {
        path: '', component: AddSchoolComponent,        
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class AddSchoolRoutingModule {
}
