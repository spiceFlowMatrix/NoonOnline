import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { DeviceAdministartionComponent } from './device-administartion.component';

const routes: Routes = [
    {
        path: '', component: DeviceAdministartionComponent,
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class DeviceAdministartionRoutingModule {
}
