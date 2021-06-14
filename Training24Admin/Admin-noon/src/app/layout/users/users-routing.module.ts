import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { UsersComponent } from './users.component';

const routes: Routes = [
    {
        path: '', component: UsersComponent,
        children: [
            { path: '', redirectTo: 'users-list', pathMatch: 'prefix' },
            { path: 'users-list', loadChildren: './users-list/users-list.module#UsersListModule' },
            { path: 'add-user', loadChildren: './add-user/add-user.module#AddUserModule' },
            { path: 'edit-user/:id', loadChildren: './add-user/add-user.module#AddUserModule' }
        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class UsersRoutingModule {
}
