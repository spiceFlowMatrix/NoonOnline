import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AddAdditionalComponent } from './add-additional.component';

const routes: Routes = [
    {
        path: '', component: AddAdditionalComponent,        
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class AddAdditionalRoutingModule {
}
