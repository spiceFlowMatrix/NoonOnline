import { Component, OnInit, ViewChild, Output, EventEmitter } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
@Component({
    selector: 'app-common-dialog',
    templateUrl: './common-dialog.component.html',
    styleUrls: ['./common-dialog.component.scss']
})
export class CommonDialogComponent implements OnInit {
    @ViewChild('commonDialogModalContent') commonDialogModalContent: any;
    type: number; // 1 = delete
    title: string;
    message: string;
    data: any;
    @Output() deleteEvent: EventEmitter<any> = new EventEmitter();

    constructor(private modalService: NgbModal) { }

    ngOnInit() { }
    open(type, title, message, data?: any) {
        this.type = type;
        this.title = title;
        this.message = message;
        this.data = data;
        this.modalService.open(this.commonDialogModalContent, { backdrop: 'static' });
    }
}
