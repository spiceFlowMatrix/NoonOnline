import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {  SalesConfigureComponent } from './sales-configure.component';
import { SalesConfigureRoutingModule } from './sales-configure-routing.module';

@NgModule({
    imports: [CommonModule, SalesConfigureRoutingModule],
    declarations: [SalesConfigureComponent],
})
export class SalesConfigureModule { }
