import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { TimeOutFormComponent } from './timeout-form.component';

const routes: Routes = [
    {
        path: '', component: TimeOutFormComponent,
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class TimeOutFormRoutingModule {
}
