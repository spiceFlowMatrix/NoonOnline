import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { BundlesRoutingModule } from './bundles-routing.module';
import { BundlesComponent } from './bundles.component';

@NgModule({
    imports: [CommonModule, BundlesRoutingModule],
    declarations: [BundlesComponent]
})
export class BundlesModule { }
