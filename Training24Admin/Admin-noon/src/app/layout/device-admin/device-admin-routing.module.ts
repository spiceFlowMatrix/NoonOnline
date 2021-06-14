import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { DeviceAdminComponent } from './device-admin.component';

const routes: Routes = [
    {
        path: '', component: DeviceAdminComponent,
        children: [
            { path: '', redirectTo: 'device-administration', pathMatch: 'prefix' },
            { path: 'device-administration', loadChildren: './device-administartion/device-administration.module#DeviceAdministartionModule' },
        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class DeviceAdminRoutingModule {
}
