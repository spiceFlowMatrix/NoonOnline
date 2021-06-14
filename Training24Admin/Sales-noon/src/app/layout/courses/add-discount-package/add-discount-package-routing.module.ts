import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AddDiscountPackageComponent } from './add-discount-package.component';

const routes: Routes = [
    {
        path: '', component: AddDiscountPackageComponent,        
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class AddDiscountRoutingModule {
}
