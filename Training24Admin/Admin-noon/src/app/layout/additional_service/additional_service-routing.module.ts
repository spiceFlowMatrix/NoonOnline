import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { Additional_ServiceComponent } from './additional_service.component';

const routes: Routes = [
    {
        path: '', component: Additional_ServiceComponent,
        children: [
            { path: '', redirectTo: 'additional-list', pathMatch: 'prefix' },
            { path: 'additional-list', loadChildren: './additional-list/additional-list.module#AdditionalListModule' },
            { path: 'add-additional', loadChildren: './add-additional/add-additional.module#AddAdditionalModule' },
            { path: 'edit-additional/:id', loadChildren: './add-additional/add-additional.module#AddAdditionalModule' }
        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class Additional_ServiceRoutingModule {
}
