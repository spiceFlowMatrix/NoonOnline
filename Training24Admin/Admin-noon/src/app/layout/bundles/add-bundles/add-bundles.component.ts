import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { DELETE_TITLE, DELETE_MESSAGE, UtilService, BundleService, CourseService } from '../../../shared';
import * as _ from 'lodash';

@Component({
    selector: 'app-add-bundles',
    templateUrl: './add-bundles.component.html',
    styleUrls: ['./add-bundles.component.scss']
})
export class AddBundlesComponent implements OnInit {
    private allSubscribers: Array<any> = [];
    @ViewChild('manageBundleForm') manageBundleForm: NgForm;
    @ViewChild('listdialog') listCommonDialog: any;
    @ViewChild('assignCourseDialog') assignCourseDialog: any;

    public bundleModel: any = {};
    public coursesList: any = [];
    public bundleCourseList: any = [];
    public filterCourseModel: any = {};
    isEditView: boolean = false;
    isCallingApi: boolean = false;
    isBundleCourseLoading: boolean = false;

    constructor(
        private activatedRoute: ActivatedRoute,
        public utilService: UtilService,
        public bundleService: BundleService,
        public courseService: CourseService,
        // private modalService: NgbModal,
        public router: Router
    ) {
        this.allSubscribers.push(this.activatedRoute.params.subscribe((params: Params) => {
            if (params['id']) {
                this.isEditView = true;
                this.bundleModel.id = params['id'];
                this.getBundleById(this.bundleModel.id);
                setTimeout(() => {
                    this.getAllCourses();
                    this.preInitData();
                }, 1000);
            } else {
                this.isEditView = false;
            }
        }));
    }

    ngOnInit() {

    }

    preInitData() {
        this.filterCourseModel = {
            pagenumber: 1,
            perpagerecord: 10,
            totalCount: 0
        }
        this.getBundleCourseById(this.bundleModel.id);
    }

    getAllCourses() {
        this.allSubscribers.push(this.courseService.getCourses({}).subscribe(res => {
            this.coursesList = res.data;
        }));
    }

    getBundleById(id) {
        this.isCallingApi = true;
        this.allSubscribers.push(this.bundleService.getBundleById(id).subscribe(res => {
            this.isCallingApi = false;
            this.bundleModel = _.clone(res.data);
        }, err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err);
        }));
    }

    manageBundle() {
        this.isCallingApi = true;
        this.allSubscribers.push(this.bundleService.manageBundle(this.bundleModel).subscribe((res: any) => {
            this.isCallingApi = false;
            this.router.navigate(['bundles']);
        }, err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err);
        }));
    }

    getBundleCourseById(id) {
        this.isBundleCourseLoading = true;
        this.allSubscribers.push(this.bundleService.getBundleCourse(this.filterCourseModel, id).subscribe(res => {
            this.isBundleCourseLoading = false;
            this.bundleCourseList = res.data;
            this.filterCourseModel.totalCount = res.totalcount;
        }, err => {
            this.isBundleCourseLoading = false;
            this.utilService.showErrorCall(err);
        }));
    }

    onPageChange(event) {
        this.filterCourseModel.pagenumber = event;
        this.getBundleCourseById(this.bundleModel.id);
    }

    openAddCourseModal() {
        this.assignCourseDialog.openModal();
    }

    addCourseToBundle(model) {
        this.allSubscribers.push(this.bundleService.addBundleCourse({
            "bundleid": this.bundleModel.id,
            "courseid": model.courseid
        }).subscribe(res => {
            this.assignCourseDialog.closeModal();
            this.utilService.showErrorSuccess('', "Course added successfully.");
            this.preInitData();
        }, err => {
            this.utilService.showErrorCall(err);
        }));
    }

    openDeleteConfirmation(course, index) {
        course.index = index;
        this.listCommonDialog.open(1, DELETE_TITLE, DELETE_MESSAGE, course);
    }

    deleteCourseFromBundle(course) {
        this.allSubscribers.push(this.bundleService.deleteCourseFromBundle(course.id).subscribe(res => {
            this.bundleCourseList = _.remove(this.bundleCourseList, (o: any) => {
                return !(o.id == course.id);
            });
        }, err => {
            this.utilService.showErrorCall(err);
        }));
    }

    ngOnDestroy() {
        this.allSubscribers.map(value => value.unsubscribe());
    }
}
