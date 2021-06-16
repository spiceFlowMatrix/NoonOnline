import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AddBundlesComponent } from './add-bundles.component';

const routes: Routes = [
    {
        path: '', component: AddBundlesComponent,        
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class AddBundlesRoutingModule {
}
