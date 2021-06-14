import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { EmailPattern, PasswordPattern } from '../shared';

@Component({
    selector: 'app-signup',
    templateUrl: './signup.component.html',
    styleUrls: ['./signup.component.scss']
})
export class SignupComponent implements OnInit {
    @ViewChild('signUpForm') signUpForm: NgForm;
    public registrationModel: any = {};
    public emailPattern: string = EmailPattern;
    public passwordPattern: string = PasswordPattern;
    constructor(
        public router: Router
    ) {

    }

    ngOnInit() { }

    register() {
        localStorage.setItem('isLoggedin', 'true');
        this.router.navigate(['dashboard']);
    }
}
