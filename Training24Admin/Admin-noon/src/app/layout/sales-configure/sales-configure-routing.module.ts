import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { SalesConfigureComponent } from './sales-configure.component';

const routes: Routes = [
    {
        path: '', component: SalesConfigureComponent,
        children: [
            { path: '', redirectTo: 'accountlist', pathMatch: 'prefix' },
            { path: 'accountlist', loadChildren: './accountlist/accountlist.module#AccountListModule' },
        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class SalesConfigureRoutingModule {
}
