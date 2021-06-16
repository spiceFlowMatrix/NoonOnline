import { EventEmitter, ElementRef, OnInit, Directive, Input, Output } from '@angular/core';
import { fromEvent } from 'rxjs';
import { map, debounceTime } from 'rxjs/operators';
import { NgModel } from '@angular/forms';

@Directive({ selector: '[debounce]' })
export class DebounceDirective implements OnInit {
    @Input() delay: number = 700;
    @Output() func: EventEmitter<any> = new EventEmitter();

    constructor(private elementRef: ElementRef, private model: NgModel) {
    }

    ngOnInit(): void {
        let eventStream = fromEvent(this.elementRef.nativeElement, 'keyup').pipe(
            map(() => this.model.value),
            debounceTime(this.delay)
        );

        eventStream.subscribe(input => this.func.emit({ input: this.elementRef.nativeElement, fired: event }));
    }

}