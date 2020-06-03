import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AppUpdateComponent } from './app-update.component';

const routes: Routes = [
    {
        path: '', component: AppUpdateComponent,
        children: [
            { path: '', redirectTo: 'app-update-form', pathMatch: 'prefix' },
            { path: 'app-update-form', loadChildren: './app-update-form/app-update-form.module#AppUpdateFormModule' },
            // { path: 'add-user', loadChildren: './add-user/add-user.module#AddUserModule' },
            // { path: 'edit-user/:id', loadChildren: './add-user/add-user.module#AddUserModule' }
        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class AppUpdateRoutingModule {
}
