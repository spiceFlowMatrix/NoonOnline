import { Component, OnInit, ViewChild } from '@angular/core';
import * as _ from 'lodash';
import { DELETE_TITLE, DELETE_MESSAGE, UtilService, CourseService } from '../../../shared';

@Component({
    selector: 'app-courses-list',
    templateUrl: './courses-list.component.html',
    styleUrls: ['./courses-list.component.scss']
})
export class CoursesListComponent implements OnInit {
    private allSubscribers: Array<any> = [];
    @ViewChild('listdialog') listCommonDialog: any;
    @ViewChild('coursePreviewDialog') coursePreviewDialog: any;
    @ViewChild('courseUsersPreviewDialog') courseUsersPreviewDialog: any;
    coursesList: Array<any> = [];
    coursePreviewObj: any = null;
    courseUsersPreviewObj: any = null;
    filterModel: any = {};
    isCoursesLoading: boolean;
    constructor(
        public utilService: UtilService,
        public courseService: CourseService
    ) { }


    ngOnInit() {
        this.filterModel = {
            pagenumber: 1,
            perpagerecord: 10,
            totalCount: 0
        }
        this.getCourses(this.filterModel);
    }


    search($event) {
        this.filterModel.pagenumber = 1;
        this.filterModel.search = $event.value;
        if (!this.filterModel.search)
            delete this.filterModel.search;
        this.getCourses(this.filterModel);
    }
    onPageChange(event) {
        this.filterModel.pagenumber = event;
        this.getCourses(this.filterModel);
    }

    openDeleteConfirmation(course, index) {
        course.index = index;
        this.listCommonDialog.open(1, DELETE_TITLE, DELETE_MESSAGE, course);
    }

    getCourses(filterObj) {
        this.isCoursesLoading = true;
        this.allSubscribers.push(this.courseService.getCourses(filterObj).subscribe(res => {
            this.isCoursesLoading = false;
            this.coursesList = res.data;
            this.filterModel.totalCount = res.totalcount;
        }, err => {
            this.isCoursesLoading = false;
            this.utilService.showErrorCall(err);
        }));
    }

    deleteCourse(data) {
        this.allSubscribers.push(this.courseService.deleteCourse(data.id).subscribe(res => {
            this.coursesList = _.remove(this.coursesList, (o) => {
                return !(o.id == data.id);
            });
        }, err => {
            this.utilService.showErrorCall(err);
        }));
    }

    // loadCoursePreview(courseId) {
    //     this.courseService.coursePreview(courseId).subscribe(res => {
    //         this.coursePreviewObj = res.data;
    //         this.coursePreviewDialog.open();
    //     }, err => {
    //         this.utilService.showErrorCall(err);
    //     });
    // }

    loadUsersPreview(courseId) {
        this.allSubscribers.push(this.courseService.courseUsersPreview(courseId).subscribe(res => {
            this.courseUsersPreviewObj = res.data;
            this.courseUsersPreviewDialog.open();
        }, err => {
            this.utilService.showErrorCall(err);
        }));
    }

    ngOnDestroy() {
        this.allSubscribers.map(value => value.unsubscribe());
    }
}
