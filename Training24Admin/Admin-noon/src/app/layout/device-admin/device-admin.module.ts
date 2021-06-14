import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DeviceAdminComponent } from './device-admin.component';
import { DeviceAdminRoutingModule } from './device-admin-routing.module';
import { DeviceService } from '../../shared/services/device.service';
@NgModule({
    imports: [CommonModule,
        DeviceAdminRoutingModule],
    declarations: [DeviceAdminComponent],
    providers: [DeviceService]
})
export class DeviceAdminModule { }
