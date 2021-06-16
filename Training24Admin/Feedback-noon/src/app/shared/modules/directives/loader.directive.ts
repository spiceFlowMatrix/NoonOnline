import { Component, OnInit, Input, OnChanges } from '@angular/core';


declare var jQuery;
@Component({
    selector: 'noon-loader',
    template: `
    <div class="spinner" style="z-index: 999999;">
        <div class="bounce1"></div>
        <div class="bounce2"></div>
        <div class="bounce3"></div>
    </div>
    `
})
export class LoaderComponent {
    constructor() { }
}