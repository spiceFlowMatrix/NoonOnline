import { Injectable } from '@angular/core';
import { CommonAPIService } from './common.service';
import { Router } from '@angular/router';

@Injectable()
export class AuthenticationService {
    constructor(
        private commonApiService: CommonAPIService,
        public router: Router,
    ) {

    }

    public checkLogin(data: any) {
        return this.commonApiService.post('v1/Account', data);
    }

    public refreshToken() {
        return this.commonApiService.get('v1/Account/RefreshAccessToken');
    }

    public logout() {
        this.commonApiService.get('v1/Account').subscribe(res => {
            this.refreshStorage();
        }, err => {
            this.refreshStorage();
        });
    }

    public refreshStorage() {
        this.router.navigateByUrl('/feed', { replaceUrl: true });
        localStorage.removeItem('access_token');
        localStorage.removeItem('token');
        localStorage.removeItem('isLoggedin');
        localStorage.removeItem('expires_at');
        localStorage.removeItem('id_token');
        localStorage.removeItem('state');
    }

}