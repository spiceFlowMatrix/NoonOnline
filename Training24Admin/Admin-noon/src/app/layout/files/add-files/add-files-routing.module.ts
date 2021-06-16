import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AddFileComponent } from './add-files.component';

const routes: Routes = [
    {
        path: '', component: AddFileComponent,        
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class AddFileRoutingModule {
}
