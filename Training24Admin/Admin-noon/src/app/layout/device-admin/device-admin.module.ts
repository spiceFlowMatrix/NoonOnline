import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DeviceAdminComponent } from './device-admin.component';
import { MatTabsModule } from '@angular/material/tabs';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatDividerModule } from '@angular/material/divider';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatListModule } from '@angular/material/list';
import { MatMenuModule } from '@angular/material/menu';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';
import { DeviceComponent } from './device/device.component';
import { DeviceQuotaExtensionComponent } from './device-quota-extension/device-quota-extension.component';
import { DeviceAdministartionComponent } from './device-administartion/device-administartion.component';
import { DeviceAdminRoutingModule } from './device-admin-routing.module';
import { DeviceService } from '../../shared/services/device.service';
@NgModule({
    imports: [CommonModule,
        DeviceAdminRoutingModule],
    declarations: [DeviceAdminComponent],
    providers: [DeviceService]
})
export class DeviceAdminModule { }
