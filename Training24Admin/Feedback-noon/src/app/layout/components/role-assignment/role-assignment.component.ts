import { Component, OnInit, Input, Output, EventEmitter, OnChanges, SimpleChanges } from '@angular/core';
import { UtilService } from './../../../shared';
import { Observable, Subject, merge } from 'rxjs';
import { debounceTime, distinctUntilChanged, filter, map } from 'rxjs/operators';
@Component({
    selector: 'noon-role-assignment',
    templateUrl: './role-assignment.component.html',
})
export class RoleAssignmentComponent implements OnInit, OnChanges {
    @Input('data')
    data: any = [];

    @Input('dataModel')
    dataModel: any = [];

    @Input('fieldId')
    fieldId: any = 'typehead';

    @Input('avtcolor')
    avtcolor: any = 'white';

    @Input('abackground')
    abackground: any = 'red';

    @Input('title')
    title: any = '';

    @Input('subtitle')
    subtitle: any = '';

    @Input('userType')
    userType: any = '';

    @Input('actionable')
    isRoleActionable: boolean = true;

    formatter = (x: { username: string }) => x.username;
    rformatter = (result: any) => result.username.toUpperCase();

    focus$ = new Subject<string>();
    click$ = new Subject<string>();
    model: any;

    @Output() selectRoleEvent: EventEmitter<any> = new EventEmitter();
    constructor(
        public utilService: UtilService
    ) {
    }

    ngOnChanges(changes: SimpleChanges) {
        if (!this.data) {
            this.data = [];
        }
    }

    ngOnInit() {
    }

    search = (text$: Observable<string>) => {
        const debouncedText$ = text$.pipe(debounceTime(200), distinctUntilChanged());
        const inputFocus$ = this.focus$;

        return merge(debouncedText$, inputFocus$).pipe(
            map(term => (term === '' ? this.data
                : this.data.filter(v => v.username.toLowerCase().indexOf(term.toLowerCase()) > -1)))
        );
    }

    focusOnAccountSelection() {
        this.utilService.focusInput(this.fieldId);
    }

    selectItem($event) {
        $event.item['type'] = this.userType;
        this.selectRoleEvent.emit({ data: $event.item });
    }
}
