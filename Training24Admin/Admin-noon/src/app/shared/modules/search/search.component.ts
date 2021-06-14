import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

@Component({
    selector: 'app-noon-search',
    templateUrl: './search.component.html',
    styleUrls: ['./search.component.scss']
})
export class SearchComponent implements OnInit {
    @Input() search: string = '';
    delay = 5000;
    @Output() func: EventEmitter<any> = new EventEmitter();
    constructor() { }

    ngOnInit() {
    }

    searchItems(event) {
        this.func.emit({ input: event, value: this.search });
    }
}
