import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthenticationService, UtilService, EmailPattern, PasswordPattern, AuthService } from '../shared';
import { AUTH_CONFIG } from './../shared/services';
import { environment } from '../../environments/environment';
@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.scss'],
})
export class LoginComponent implements OnInit {
    @ViewChild('loginForm') loginForm: NgForm;
    public loginModel: any = {
        deviceType: 'browser',
        deviceToken: '',
        version: ''
    };
    shouldDisplayOverlay: boolean = true;
    isFormLoading: boolean = true;
    private allSubscribers: Array<any> = [];

    public emailPattern: string = EmailPattern;
    public passwordPattern: string = PasswordPattern;
    public isLoading: boolean = false;
    constructor(
        public router: Router,
        public route: ActivatedRoute,
        private authenticationService: AuthenticationService,
        public _utilService: UtilService,
        public auth: AuthService
    ) {
        this.isFormLoading = true;
        this.route.queryParams.subscribe(res => {
            if (res.logout) {
                this.onLoggedin();
            } else {
                auth.handleAuthentication().then((res: any) => {
                    this.isFormLoading = false;
                    if (res.key == 1) {
                        this.loginModel = res;
                        this.proceedAuthorization(res);
                    }
                }).catch(err => {
                    this.isFormLoading = false;
                    console.log(err);
                });
            }
        });
    }

    ngOnInit() {
        this.loginModel.deviceType = this.auth.get_browser().name;
        this.loginModel.version = this.auth.get_browser().version;
    }

    logout() {
        localStorage.setItem("data", JSON.stringify(this.loginModel));
        window.location.href = AUTH_CONFIG.audience + 'v2/logout?returnTo=' + environment.logoutcallbackURL;
    }

    proceedAuthorization(data) {
        this.isLoading = true;
        this.allSubscribers.push(this.authenticationService.checkLogin(this.loginModel).subscribe(res => {
            this.isLoading = false;
            localStorage.setItem('isLoggedin', 'true');
            localStorage.setItem('userid',res.userid);
            // localStorage.setItem('token', res.token); 
            if (this._utilService.getRole()
                && this._utilService.getRole()
                && (this._utilService.getRole().indexOf('admin') > -1) || 
                 (this._utilService.getRole().indexOf('sales_admin') > -1) || 
                 (this._utilService.getRole().indexOf('sales_agent') > -1)
            ) {
                this.router.navigate(['purchases']);
            } else {
                this._utilService.showErrorToast("Unauthorized", "You are not authorized person to access");
                localStorage.removeItem('isLoggedin');
                localStorage.removeItem('access_token');
                localStorage.removeItem('token');
            }
        }, err => {
            this.isLoading = false;
            this._utilService.showErrorCall(err);
        }));
    }



    onLoggedin() {
        this.auth.login();
        // this.isLoading = true;
        // this.allSubscribers.push(this.authenticationService.checkLogin(this.loginModel).subscribe(res => {
        //     this.isLoading = false;
        //     localStorage.setItem('isLoggedin', 'true');
        //     localStorage.setItem('access_token', res.token);
        //     if (this._utilService.getAccountDetails()
        //         && this._utilService.getAccountDetails().RoleId
        //         && (this._utilService.getAccountDetails().RoleId.indexOf('1') > -1
        //             || this._utilService.getAccountDetails().RoleId.indexOf('13') > -1)
        //     ) {
        //         this.router.navigate(['dashboard']);
        //     } else {
        //         this._utilService.showErrorToast("Unauthorized", "You are not authorized person to access");
        //         localStorage.removeItem('isLoggedin');
        //         localStorage.removeItem('access_token');
        //     }
        // }, err => {
        //     this.isLoading = false;
        //     this._utilService.showErrorCall(err);
        // }));
    }

    ngOnDestroy() {
        this.allSubscribers.map(value => value.unsubscribe());
    }
}
