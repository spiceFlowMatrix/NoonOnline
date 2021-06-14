import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import * as _ from 'lodash';
import { PackageService } from '../../../shared/services/package.service';
import { DELETE_TITLE, DELETE_MESSAGE, UtilService, CourseService } from '../../../shared';


@Component({
    selector: 'app-add-package',
    templateUrl: './add-package.component.html',
    styleUrls: ['./add-package.component.scss']
})
export class AddPackageComponent implements OnInit {
    @ViewChild('assignCourseDialog') assignCourseDialog: any;
    @ViewChild('listdialog') listCommonDialog: any;
    
    private allSubscribers: Array<any> = [];
    public packageModel: any = {};
    isEditView: boolean= false;
    isCallingApi: boolean = false;
    isPackageCourseLoading: boolean =false;
    packageCourseList: any;
    filterCourseModel: any;
    coursesList: any;

    constructor(
        public utilService: UtilService,
        public packageService: PackageService,
        public activatedRoute: ActivatedRoute,
        public courseService: CourseService,
        public router: Router) 
        {
            this.allSubscribers.push(this.activatedRoute.params.subscribe((params: Params) => {
                if (params['id']) {
                    this.isEditView = true;
                    this.packageModel.id = params['id'];
                    this.getPackageById(this.packageModel.id);
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
        perpagerecord: 10
    }
 this.getPackageCourseById(this.packageModel.id,this.filterCourseModel);
}

getAllCourses() {
    this.allSubscribers.push(this.courseService.getCourses({}).subscribe(res => {    
        this.coursesList = res.data;
    }));
}


getPackageById(id) {
    this.isCallingApi = true;
    this.allSubscribers.push(this.packageService.getPackageById(id).subscribe(res => {
        this.isCallingApi = false;
        this.packageModel = _.clone(res.data);
    }, err => {
        this.isCallingApi = false;
        this.utilService.showErrorCall(err);
    }));
}

openAddCourseModal() {
    this.assignCourseDialog.openModal();
}

addCourseToPackage(model) {
    this.allSubscribers.push(this.packageService.addPackageCourse({
        "package_id": this.packageModel.id,
        "course_id": model.courseid
    }).subscribe(res => {
        this.assignCourseDialog.closeModal();
        this.utilService.showErrorSuccess('', "Course added successfully.");
        this.preInitData();
    }, err => {
        this.utilService.showErrorCall(err);
    }));
}

managePackage() {
    this.isCallingApi = true;
    this.packageModel.key = "Edit";
    this.allSubscribers.push(this.packageService.managePackage(this.packageModel).subscribe((res: any) => {
        this.isCallingApi = false;
        this.router.navigate(['/package/package-list']);
    }, err => {
        this.isCallingApi = false;
        this.utilService.showErrorCall(err);
    }));
}

getPackageCourseById(id, filter) {
    
    this.isPackageCourseLoading = true;
    this.allSubscribers.push(this.packageService.getPackageCourse(filter, id).subscribe(res => {
        this.isPackageCourseLoading = false;
        this.packageCourseList = res.data;
        this.filterCourseModel.totalCount = res.totalcount;
    }, err => {
        this.isPackageCourseLoading = false;
        this.utilService.showErrorCall(err);
    }));
}
openDeleteConfirmation(course, index) {
    course.index = index;
    this.listCommonDialog.open(1, DELETE_TITLE, DELETE_MESSAGE, course);
}

deleteCourseFromPackage(course) {
    this.allSubscribers.push(this.packageService.deleteCourseFromPackage(course.id).subscribe(res => {
        this.packageCourseList = _.remove(this.packageCourseList, (o: any) => {
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