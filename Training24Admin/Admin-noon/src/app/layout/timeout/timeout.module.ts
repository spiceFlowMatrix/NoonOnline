import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TimeoutComponent } from './timeout.component';
import { TimeOutRoutingModule } from './timeout-routing.module';

@NgModule({
    imports: [CommonModule, TimeOutRoutingModule],
    declarations: [TimeoutComponent],
})
export class TimeOutModule { }
