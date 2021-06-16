import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AppUpdateFormComponent } from './app-update-form.component';

const routes: Routes = [
    {
        path: '', component: AppUpdateFormComponent,
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class AppUpdateFormRoutingModule {
}
