import { Component, OnInit, Input, Output, ViewChild, EventEmitter } from '@angular/core';
import { CourseService, UtilService } from '../../../shared';

@Component({
    selector: 'app-usercourse',
    templateUrl: './usercourse-table.component.html',
    // styleUrls: ['./usercourse.component.scss']
})
export class UserCourseComponent implements OnInit {
    allSubscribers: any = [];
    @ViewChild('coursePreviewDialog') coursePreviewDialog: any;
    @ViewChild('courseUsersPreviewDialog') courseUsersPreviewDialog: any;

    @Input('data')
    userCourseList: any = [];

    @Input('roleName')
    roleName: any = '';


    @Output() openUserPreviewModal: EventEmitter<any> = new EventEmitter<any>();
    coursePreviewObj: any = null;

    courseUsersPreviewObj: any = null;
    constructor(
        public courseService: CourseService,
        public utilService: UtilService
    ) { }

    ngOnInit() {
    }

    openCoursePreviewModal(course) {
        this.allSubscribers.push(this.courseService.coursePreview(course.courseid).subscribe(res => {
            this.coursePreviewObj = res.data;
            this.coursePreviewDialog.open();
        }, err => {
            this.utilService.showErrorCall(err);
        }));
    }

    openCourseUserPreviewModal(course) {
        this.allSubscribers.push(this.courseService.courseUsersPreview(course.courseid).subscribe(res => {
            this.courseUsersPreviewObj = res.data[0];
            this.courseUsersPreviewDialog.open();
        }, err => {
            this.utilService.showErrorCall(err);
        }));
    }
}
