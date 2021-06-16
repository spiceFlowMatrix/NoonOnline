import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AdditionalListComponent } from './additional-list.component';

const routes: Routes = [
    {
        path: '', component: AdditionalListComponent,
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class AdditionalListRoutingModule {
}
