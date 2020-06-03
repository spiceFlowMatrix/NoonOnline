import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AppUpdateComponent } from './app-update.component';
import { AppUpdateRoutingModule } from './app-update-routing.module';

@NgModule({
    imports: [CommonModule, AppUpdateRoutingModule],
    declarations: [AppUpdateComponent],
})
export class AppUpdateModule { }
