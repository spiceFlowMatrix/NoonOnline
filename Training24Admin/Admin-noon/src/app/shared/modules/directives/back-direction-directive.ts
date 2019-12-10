import { Directive, EventEmitter, HostListener, OnInit, Output } from '@angular/core';
import { Location } from '@angular/common';

@Directive({
    selector: '[backDirection]'
})
export class BackDirectionDirective implements OnInit {

    constructor(private location: Location) {
     }

    ngOnInit() {
    }

    @HostListener('click', ['$event'])
    clickEvent(event) {
        event.preventDefault();
        event.stopPropagation();
        setTimeout(() => {
            this.location.back();
        }, 100);
    }
}