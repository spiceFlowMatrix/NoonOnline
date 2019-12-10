import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { TaxComponent } from './tax.component';

const routes: Routes = [
    {
        path: '', component: TaxComponent,
        children: [
            { path: '', redirectTo: 'tax-form', pathMatch: 'prefix' },
            { path: 'tax-form', loadChildren: './tax-form/tax-form.module#TaxFormModule' },
        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class TaxRoutingModule {
}
