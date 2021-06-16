import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { LoginRoutingModule } from './login-routing.module';
import { LoginComponent } from './login.component';
import { AuthenticationService, SharedModule } from '../shared';

@NgModule({
    imports: [CommonModule, FormsModule, LoginRoutingModule,SharedModule.forRoot()],
    declarations: [LoginComponent],
    providers: [AuthenticationService]
})
export class LoginModule { }
