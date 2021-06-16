import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { TimeoutComponent } from './timeout.component';

const routes: Routes = [
    {
        path: '', component: TimeoutComponent,
        children: [
            { path: '', redirectTo: 'timeout-form', pathMatch: 'prefix' },
            { path: 'timeout-form', loadChildren: './timeout-form/timeout-form.module#TimeOutFormModule' },
            // { path: 'add-user', loadChildren: './add-user/add-user.module#AddUserModule' },
            // { path: 'edit-user/:id', loadChildren: './add-user/add-user.module#AddUserModule' }
        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class TimeOutRoutingModule {
}
