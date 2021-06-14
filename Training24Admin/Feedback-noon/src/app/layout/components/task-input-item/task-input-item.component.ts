import { Component, OnInit, Input, Output, EventEmitter, OnChanges, SimpleChanges } from '@angular/core';
import { UtilService } from './../../../shared';
@Component({
    selector: 'noon-task-input-item',
    templateUrl: './task-input-item.component.html',
    styleUrls: ['./task-input-item.component.scss']
})
export class TaskInputItemComponent implements OnInit, OnChanges {
    @Input('data')
    data: any = {};

    @Input('fieldId')
    fieldId: any;

    @Input('taskType')
    taskType: any;

    @Input('isNotificationNeeded')
    isNotificationNeeded: boolean;

    @Input('isDisabled')
    isDisabled: boolean = false;

    @Input('isFeedback')
    isFeedback: boolean = false;

    @Input('placeholderText')
    placeholderText: string = 'Add new task';

    @Output() doneEvent: EventEmitter<any> = new EventEmitter();
    @Output() taskActionEvent: EventEmitter<any> = new EventEmitter();

    constructor(
        public utilService: UtilService
    ) {
    }

    ngOnChanges(changes: SimpleChanges) {
        if (this.isFeedback) {
            this.data.description = this.utilService.getTagName(this.data);
        }        
    }

    ngOnInit() {
    }

    changeInput(e) {
        if (e.keyCode == 13) {
            this.doneEvent.emit({ data: this.data });
        }
    }

    focusInput() {
        setTimeout(() => {
            let element: any = document.getElementById(this.fieldId);
            if (element)
                element.focus();
        }, 300);
    }

    actionOnTask() {
        if (this.data.id)
            this.taskActionEvent.emit({ data: this.data });
    }
}
