import { EventEmitter, ElementRef, OnInit, Directive, Input, Output } from '@angular/core';
import { fromEvent } from 'rxjs';
import { map, debounceTime } from 'rxjs/operators';
import { NgModel } from '@angular/forms';

@Directive({ selector: '[debounce]', providers: [NgModel] })
export class DebounceDirective implements OnInit {
    @Input('delay')
    delay: number = 700;

    @Output() func: EventEmitter<any> = new EventEmitter();
    eventStream: any;
    constructor(private elementRef: ElementRef, private model: NgModel) {
    }

    ngOnInit() {
        this.eventStream = fromEvent(this.elementRef.nativeElement, 'input').pipe(
            map(() => this.model.value),
            debounceTime(this.delay)
        );

        this.eventStream.subscribe(input => this.func.emit({ input: this.elementRef.nativeElement }));
    }
}