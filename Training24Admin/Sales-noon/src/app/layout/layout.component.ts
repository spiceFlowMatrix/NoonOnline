import { Component, OnInit } from '@angular/core';
import { UtilService, AuthenticationService } from '../shared';
import { AuthService } from './../shared';
@Component({
    selector: 'app-layout',
    templateUrl: './layout.component.html',
    styleUrls: ['./layout.component.scss']
})
export class LayoutComponent implements OnInit {

    collapedSideBar: boolean;
    shouldDisplayOverlay: boolean = true;

    constructor(
        public utilService: UtilService,
        public authenticationService: AuthenticationService,
        public auth: AuthService
    ) { 
        if (auth.isAuthenticated()) {
            this.shouldDisplayOverlay = false;
        } else {            
            this.shouldDisplayOverlay = !auth.renewToken();
        }

        // Hide the overlay when single sign-on auth is complete
        auth.ssoAuthComplete$.subscribe(res => {            
            this.shouldDisplayOverlay = !res
        });
    }

    ngOnInit() {
        // if (this.utilService.checkRefreshToken()) {
        //     this.authenticationService.refreshToken().subscribe(res => {
        //         localStorage.setItem('access_token', res.token);
        //     }, err => {
        //         this.utilService.showErrorToast('Sorry', 'Please login again.');
        //         this.utilService.logout();
        //     });
        // }
    }

    receiveCollapsed($event) {
        this.collapedSideBar = $event;
    }
}
