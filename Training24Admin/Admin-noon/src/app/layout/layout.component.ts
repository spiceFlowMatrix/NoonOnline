import { Component, OnInit } from '@angular/core';
import { UtilService, AuthenticationService } from '../shared';
import { Router, NavigationStart, NavigationEnd } from '@angular/router';
import { AuthService } from './../shared';

@Component({
    selector: 'app-layout',
    templateUrl: './layout.component.html',
    styleUrls: ['./layout.component.scss']
})
export class LayoutComponent implements OnInit {
    showCourseHeader: boolean = false;
    collapedSideBar: boolean;
    isRoutingLoading: boolean;
    allSubscribers = [];
    shouldDisplayOverlay: boolean = true;

    constructor(
        public utilService: UtilService,
        public authenticationService: AuthenticationService,
        public router: Router,
        public auth: AuthService
    ) {
        this.router.events
            .subscribe((event) => {
                if (event instanceof NavigationEnd) {
                    if (this.router.url.includes('course-preview')) {
                        this.showCourseHeader = true;
                    }
                    else {
                        this.showCourseHeader = false;
                    }
                }
            });
        // auth.handleAuthentication();
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
        // this.allSubscribers.push(this.authenticationService.refreshToken().subscribe(res => {
        //     localStorage.setItem('token', res.token);
        // }, err => {
        //     this.utilService.showErrorToast('Sorry', 'Please login again.');
        //     this.utilService.logout();
        // }));
        // }

        this.allSubscribers.push(this.router.events.subscribe(nav => {
            if (nav instanceof NavigationStart) {
                this.isRoutingLoading = true;
            }
            if (nav instanceof NavigationEnd) {
                this.isRoutingLoading = false;
            }
        }));
    }

    receiveCollapsed($event) {
        this.collapedSideBar = $event;
    }

    ngOnDestroy() {
        this.allSubscribers.map(value => value.unsubscribe());
    }
}
