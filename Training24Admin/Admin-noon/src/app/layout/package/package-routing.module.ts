import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { PackageComponent } from './package.component';

const routes: Routes = [
    {
        path: '', component: PackageComponent,
        children: [
            { path: '', redirectTo: 'package-list', pathMatch: 'prefix' },
            { path: 'package-list', loadChildren: './package-list/package-list.module#PackageListModule' },
            { path: 'add-package', loadChildren: './add-package/add-package.module#AddPackageModule' },
            { path: 'edit-package/:id', loadChildren: './add-package/add-package.module#AddPackageModule' }
        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class PackageRoutingModule {
}
