import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PackageRoutingModule } from './package-routing.module';
import { PackageComponent } from './package.component';

@NgModule({
    imports: [CommonModule, PackageRoutingModule],
    declarations: [PackageComponent]
})
export class PackageModule { }
