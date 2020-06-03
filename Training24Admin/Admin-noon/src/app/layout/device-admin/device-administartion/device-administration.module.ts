import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PageHeaderModule, CommonDialogModule, FileService, SharedModule, SearchModule } from '../../../shared';
import { DeviceAdministartionComponent } from './device-administartion.component';
import { DeviceAdministartionRoutingModule } from './device-administartion-routing';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatDividerModule } from '@angular/material/divider';
import { MatSelectModule } from '@angular/material/select';
import { MatTableModule } from '@angular/material/table';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatTabsModule } from '@angular/material/tabs';
import { MatListModule } from '@angular/material/list';
import { MatMenuModule } from '@angular/material/menu';
import { MatInputModule } from '@angular/material/input';
import {MatDatepickerModule} from '@angular/material/datepicker';
import { MatIconModule } from '@angular/material/icon';
import { DeviceComponent } from '../device/device.component';
import { DeviceQuotaExtensionComponent } from '../device-quota-extension/device-quota-extension.component';
import { DeviceService } from '../../../shared/services/device.service';
import { MatNativeDateModule } from '@angular/material/core';
import { MAT_MOMENT_DATE_FORMATS, MomentDateAdapter } from '@angular/material-moment-adapter'
import { SatDatepickerModule, SatNativeDateModule, DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE } from 'saturn-datepicker';
import { FormsModule } from '@angular/forms';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        CommonDialogModule,
        MatToolbarModule,
        MatButtonModule,
        MatSidenavModule,
        MatIconModule,
        MatInputModule,
        MatMenuModule,
        MatListModule,
        MatNativeDateModule,
        SatDatepickerModule,
        MatDatepickerModule,
        SatNativeDateModule,
        MatFormFieldModule,
        MatTabsModule,
        MatTableModule,
        MatSelectModule,
        MatDividerModule,
        DeviceAdministartionRoutingModule
    ],
    declarations: [DeviceAdministartionComponent,DeviceComponent,DeviceQuotaExtensionComponent],
    providers: [DeviceService, {provide: DateAdapter, useClass: MomentDateAdapter, deps: [MAT_DATE_LOCALE]},
    {provide: MAT_DATE_FORMATS, useValue: MAT_MOMENT_DATE_FORMATS},]
})
export class DeviceAdministartionModule { }
