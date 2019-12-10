import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { BundlesListComponent } from './bundles-list.component';

const routes: Routes = [
    {
        path: '', component: BundlesListComponent,
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class BundlesListRoutingModule {
}
