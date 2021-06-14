import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
    selector: 'app-course-users-preview',
    templateUrl: './course-users-preview.component.html',
    // styleUrls: ['./usercourse.component.scss']
})
export class CourseUsersComponent implements OnInit {
    @ViewChild('courseUsersPreviewDialog') courseUsersPreviewDialog;

    @Input('data')
    courseUsersPreviewObj: any = {};

    modalRef: any = null;
    constructor(
        private modalService: NgbModal
    ) { }

    ngOnInit() {

    }

    open() {
        this.modalRef = this.modalService.open(this.courseUsersPreviewDialog, { backdrop: 'static', size: 'lg' });
    }
}
