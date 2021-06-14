import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {  TaxComponent } from './tax.component';
import { TaxRoutingModule } from './tax-routing.module';

@NgModule({
    imports: [CommonModule, TaxRoutingModule],
    declarations: [TaxComponent],
})
export class TaxModule { }
