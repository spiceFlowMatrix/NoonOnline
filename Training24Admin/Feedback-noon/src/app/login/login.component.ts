import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthenticationService, UtilService, EmailPattern, PasswordPattern } from '../shared';
import { AuthService } from './../shared';
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
    private allSubscribers: Array<any> = [];

    public emailPattern: string = EmailPattern;
    public passwordPattern: string = PasswordPattern;
    public isLoading: boolean = false;
    shouldDisplayOverlay: boolean = true;
    isFormLoading: boolean = true;
    profile: any = {};

    constructor(
        public router: Router,
        public route: ActivatedRoute,
        private authenticationService: AuthenticationService,
        public _utilService: UtilService,
        public auth: AuthService
    ) {
        // auth.logout();
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

        // Attempt single sign-on authentication
        // if not authenticated
        // if (auth.isAuthenticated()) {
        //     // this.shouldDisplayOverlay = false;
        //     if (this.auth.userProfile) {
        //         this.profile = this.auth.userProfile;
        //         debugger
        //     } else {
        //         this.auth.getProfile((err, profile) => {
        //             this.profile = profile;
        //             debugger
        //         });
        //     }
        // }
        //  else {            
        //     auth.renewToken();
        // }

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
            // localStorage.setItem('token', res.token); 
            if (this._utilService.getRole()
                && this._utilService.getRole()
                && (this._utilService.getRole().indexOf('admin') > -1
                    || this._utilService.getRole().indexOf('aafmanager') > -1
                    || this._utilService.getRole().indexOf('coordinator') > -1
                    || this._utilService.getRole().indexOf('edit_team_leader') > -1
                    || this._utilService.getRole().indexOf('shooting_team_leader') > -1
                    || this._utilService.getRole().indexOf('graphics_team_leader') > -1
                    || this._utilService.getRole().indexOf('quality_assurance') > -1
                    || this._utilService.getRole().indexOf('feedback_edge_team') > -1
                    || this._utilService.getRole().indexOf('filming_staff') > -1
                    || this._utilService.getRole().indexOf('editing_staff') > -1
                    || this._utilService.getRole().indexOf('graphics_staff') > -1
                    || this._utilService.getRole().indexOf('aafmanager') > -1)
            ) {
                this.router.navigate(['feed-list']);
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
        //     this.router.navigate(['dashboard']);
        // }, err => {
        //     this.isLoading = false;
        //     this._utilService.showErrorCall(err);
        // }));
    }

    

    ngOnDestroy() {
        this.allSubscribers.map(value => value.unsubscribe());
    }
}
