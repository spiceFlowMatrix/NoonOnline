import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CsvInputComponent } from './csvinput.component';

const routes: Routes = [
    {
        path: '', component: CsvInputComponent,
        children: [
            { path: '', redirectTo: 'csvinput-form', pathMatch: 'prefix' },
            { path: 'csvinput-form', loadChildren: './csvinput-form/csvinput-form.module#CsvInputFormModule' },
            // { path: 'add-user', loadChildren: './add-user/add-user.module#AddUserModule' },
            // { path: 'edit-user/:id', loadChildren: './add-user/add-user.module#AddUserModule' }
        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class CsvInputRoutingModule {
}
