import { Component, OnInit, ViewChild } from '@angular/core';
import * as _ from 'lodash';
import { UtilService, CourseService } from '../../../shared';

@Component({
    selector: 'app-mycourses-list',
    templateUrl: './mycourses-list.component.html',
    styleUrls: ['./mycourses-list.component.scss']
})
export class MyCoursesListComponent implements OnInit {
    private allSubscribers: Array<any> = [];
    coursesList: Array<any> = [];
    filterModel: any = {};
    isCoursesLoading: boolean;
    constructor(
        public utilService: UtilService,
        public courseService: CourseService
    ) {

    }
    
    ngOnInit() {
        this.filterModel = {
            pagenumber: 1,
            perpagerecord: 10,
            totalCount: 0
        }
        this.getUserCourseById(this.utilService.getAccountDetails()["https://noon-online/uid"]);
    }

    onPageChange(event) {
        this.filterModel.pagenumber = event;
        this.getUserCourseById(this.utilService.getAccountDetails()["https://noon-online/uid"]);
    }

    getUserCourseById(id) { 
        this.isCoursesLoading = true;
        this.allSubscribers.push(this.courseService.getUserCourse(this.filterModel, id).subscribe(res => {
            this.isCoursesLoading = false;
            this.coursesList = res.data;
            this.filterModel.totalCount = res.totalcount;
        }, err => {
            this.utilService.showErrorCall(err);
        }));
    }

    ngOnDestroy() {
        this.allSubscribers.map(value => value.unsubscribe());
    }
}
