import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { BundlesComponent } from './bundles.component';

const routes: Routes = [
    {
        path: '', component: BundlesComponent,
        children: [
            { path: '', redirectTo: 'bundles-list', pathMatch: 'prefix' },
            { path: 'bundles-list', loadChildren: './bundles-list/bundles-list.module#BundlesListModule' },
            { path: 'add-bundle', loadChildren: './add-bundles/add-bundles.module#AddBundlesModule' },
            { path: 'edit-bundle/:id', loadChildren: './add-bundles/add-bundles.module#AddBundlesModule' }
        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class BundlesRoutingModule {
}
