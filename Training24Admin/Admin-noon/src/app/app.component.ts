import { Component, OnInit } from '@angular/core';
import { AuthService } from './shared';
import { environment } from "../environments/environment";

// import Scrollbar from 'smooth-scrollbar';
@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
    shouldDisplayOverlay: boolean = true;

    constructor(public auth: AuthService) {
        // Attempt single sign-on authentication
        // if not authenticated
        // if (auth.isAuthenticated()) {
        //     this.shouldDisplayOverlay = false;
        // } else {            
        //     this.shouldDisplayOverlay = !auth.renewToken();
        // }

        // // Hide the overlay when single sign-on auth is complete
        // auth.ssoAuthComplete$.subscribe(res => {            
        //     this.shouldDisplayOverlay = !res
        // });
    }

    ngOnInit() {
        // Scrollbar.init(document.querySelector('html'));
        // setTimeout(() => {
        //     let element: HTMLDivElement = document.querySelector('html .scroll-content');
        //     element.style.position = 'absolute';
        //     element.style.top = '0px';
        //     element.style.width = '100%';
        //     element.style.height = '100vh';
        // }, 100);
    }
}
