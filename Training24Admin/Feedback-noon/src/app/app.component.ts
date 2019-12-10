import { Component, OnInit } from '@angular/core';

// import Scrollbar from 'smooth-scrollbar';
@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {

    constructor() {        
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
