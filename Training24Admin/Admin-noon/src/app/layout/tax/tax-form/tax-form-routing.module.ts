import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { TaxFormComponent } from './tax-form.component';

const routes: Routes = [
    {
        path: '', component: TaxFormComponent,
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class TaxFormRoutingModule {
}
