import { Component, Input } from '@angular/core';
@Component({
    selector: 'noon-loader',
    template: `
    <div class="spinner" style="z-index: 999999;">
        <div class="bounce1"></div>
        <div class="bounce2"></div>
        <div class="bounce3"></div>
        <h3 *ngIf="withmessage">Please wait</h3>
        <div *ngIf="withmessage">
            <div class="bounce1"></div>
            <div class="bounce2"></div>
            <div class="bounce3"></div>
        </div>
    </div>
    `
})
export class LoaderComponent {
    @Input('withmessage')
    withmessage: boolean = false;
    constructor() { }
}