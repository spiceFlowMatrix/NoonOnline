import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { EmailPattern } from '../shared';

@Component({
    selector: 'app-forgot-password',
    templateUrl: './forgot-password.component.html',
    styleUrls: ['./forgot-password.component.scss']
})
export class ForgotPasswordComponent implements OnInit {
    @ViewChild('forgotPasswordForm') forgotPasswordForm: NgForm;
    public forgotPasswordModel: any = {};
    public emailPattern: string = EmailPattern;
    constructor(
        public router: Router
    ) {

    }

    ngOnInit() { }

    checkForgotPassword() {       
        this.router.navigate(['admin']);
    }
}
