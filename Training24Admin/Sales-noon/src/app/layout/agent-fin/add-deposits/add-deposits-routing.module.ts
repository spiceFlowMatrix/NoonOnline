import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AddDepositsComponent } from './add-deposits.component';

const routes: Routes = [
    {
        path: '', component: AddDepositsComponent,        
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class AddDepositsRoutingModule {
}
