import { Component, OnInit, ViewChild } from '@angular/core';
import * as _ from 'lodash';
import { DELETE_TITLE, DELETE_MESSAGE, UtilService, CourseService } from '../../../shared';

@Component({
    selector: 'app-courses-list',
    templateUrl: './courses-list.component.html',
    styleUrls: ['./courses-list.component.scss']
})
export class CoursesListComponent implements OnInit {
    @ViewChild('listdialog') listCommonDialog: any;
    activePanels: string[] = ['course_definitions', 'discount_packages']
    cdFilterModel: any = {};
    dpFilterModel: any = {};
    isCallingApi: boolean;
    courseDefinitions: Array<any> = [];
    discountPackages: Array<any> = [];
    private allSubscribers: Array<any> = [];
    
    constructor(
        public utilService: UtilService,
        public courseService: CourseService
    ) { }

    ngOnInit() {
        this.cdFilterModel = {
            pagenumber: 1,
            perpagerecord: 10,
            totalCount: 0
        }
        this.dpFilterModel = _.clone(this.cdFilterModel);
        this.getcourseDefinitions(this.cdFilterModel);
        this.getDiscountPackages(this.dpFilterModel);
    }

    openDeleteConfirmation(course, index, type) {
        course.index = index;
        course.type = type;
        this.listCommonDialog.open(1, DELETE_TITLE, DELETE_MESSAGE, course);
    }

    search($event) {
        this.cdFilterModel.search = $event.value;
        if (!this.cdFilterModel.search)
            delete this.cdFilterModel.search;
        this.getcourseDefinitions(this.cdFilterModel);
    }

    onPageChange(event, type) {
        if (type == 'cd') {
            this.cdFilterModel.pagenumber = event;
            this.getcourseDefinitions(this.cdFilterModel);
        } else if (type = 'dp') {
            this.dpFilterModel.pagenumber = event;
            this.getDiscountPackages(this.cdFilterModel);
        }
    }

    getcourseDefinitions(filterObj) {
        this.isCallingApi = true;
        this.allSubscribers.push(this.courseService.getCourseDefinitions(filterObj).subscribe(res => {
            this.isCallingApi = false;
            this.courseDefinitions = res.data;
            this.cdFilterModel.totalCount = res.totalcount;
        }, err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err);
        }));
    }

    deleteItem(data) {
        if (data.type == 'cd') {
            this.allSubscribers.push(this.courseService.deleteCourseDefinition(data.id).subscribe(res => {
                this.courseDefinitions = _.remove(this.courseDefinitions, (o) => {
                    return !(o.id == data.id);
                });
            }, err => {
                this.utilService.showErrorCall(err);
            }));
        } else if (data.type == 'dp') {
            this.allSubscribers.push(this.courseService.deleteDiscount(data.id).subscribe(res => {
                this.discountPackages = _.remove(this.discountPackages, (o) => {
                    return !(o.id == data.id);
                });
            }, err => {
                this.utilService.showErrorCall(err);
            }));
        }
    }

    getDiscountPackages(filterObj) {
        this.isCallingApi = true;
        this.allSubscribers.push(this.courseService.getDiscounts(filterObj).subscribe(res => {
            this.isCallingApi = false;
            this.discountPackages = res.data;
            this.dpFilterModel.totalCount = res.totalcount;
        }, err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err);
        }));
    }

    ngOnDestroy() {
        this.allSubscribers.map(value => value.unsubscribe());
    }
}
