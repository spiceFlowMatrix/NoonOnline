import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { PrivacyComponent } from './privacy.component';

const routes: Routes = [
    {
        path: '', component: PrivacyComponent,
        children: [
            { path: '', redirectTo: 'privacy-form', pathMatch: 'prefix' },
            { path: 'privacy-form', loadChildren: './privacy-form/privacy-form.module#PrivacyFormModule' },
            // { path: 'add-user', loadChildren: './add-user/add-user.module#AddUserModule' },
            // { path: 'edit-user/:id', loadChildren: './add-user/add-user.module#AddUserModule' }
        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class PrivacyRoutingModule {
}
