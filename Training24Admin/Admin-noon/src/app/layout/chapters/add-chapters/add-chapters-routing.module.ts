import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AddChaptersComponent } from './add-chapters.component';

const routes: Routes = [
    {
        path: '', component: AddChaptersComponent,
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class AddChaptersRoutingModule {
}
