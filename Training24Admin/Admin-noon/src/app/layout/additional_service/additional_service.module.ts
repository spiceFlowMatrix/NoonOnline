import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { Additional_ServiceRoutingModule } from './additional_service-routing.module';
import { Additional_ServiceComponent } from './additional_service.component';

@NgModule({
    imports: [CommonModule, Additional_ServiceRoutingModule],
    declarations: [Additional_ServiceComponent]
})
export class Additional_ServiceModule { }
