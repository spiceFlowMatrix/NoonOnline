import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AddPackageComponent } from './add-package.component';

const routes: Routes = [
    {
        path: '', component: AddPackageComponent,
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class AddPackageRoutingModule {
}
