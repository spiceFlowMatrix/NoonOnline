import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
    selector: 'noon-coursepreview',
    templateUrl: './course-preview.component.html',
    // styleUrls: ['./coursepreview.component.scss']
})
export class CoursePreviewComponent implements OnInit {
    @ViewChild('coursePreviewDialog') coursePreviewDialog;

    @Input('data')
    coursePreviewObj: any = {};

    modalRef: any = null;
    constructor(
        private modalService: NgbModal
    ) { }

    ngOnInit() {

    }

    open() {
        this.modalRef = this.modalService.open(this.coursePreviewDialog, { backdrop: 'static', size: 'lg' });
    }
}
