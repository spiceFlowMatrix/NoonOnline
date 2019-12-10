import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DELETE_TITLE, DELETE_MESSAGE, UtilService, GradeService, CourseService,SchoolService } from '../../../shared';
import * as _ from 'lodash';

@Component({
    selector: 'app-add-grade',
    templateUrl: './add-grade.component.html',
    styleUrls: ['./add-grade.component.scss']
})
export class AddGradeComponent implements OnInit {
    private allSubscribers: Array<any> = [];
    @ViewChild('manageGradeForm') manageGradeForm: NgForm;
    @ViewChild('listdialog') listCommonDialog: any;
    public gradeModel: any = {};
    public SchoolList: any = [];
    isEditView: boolean = false;
    isCallingApi: boolean = false;

    @ViewChild('assignCourseDialog') assignCourseDialog: any;
    isGradeCourseLoading: boolean = false;
    public gradeCourseList: any = [];
    public filterCourseModel: any = {};
    public coursesList: any = [];
    public filterModel: any;
    constructor(
        private activatedRoute: ActivatedRoute,
        public utilService: UtilService,
        public gradeService: GradeService,
        public courseService: CourseService,
        public schoolService: SchoolService,
        public modalService: NgbModal,
        public router: Router
    ) {
        this.allSubscribers.push(this.activatedRoute.params.subscribe((params: Params) => {
            if (params['id']) {
                this.isEditView = true;
                this.gradeModel.id = params['id'];
                this.getGradeById(this.gradeModel.id);
                // setTimeout(() => {
                this.getAllCourses();
                this.preInitData();
                // }, 1000);
            } else {
                this.isEditView = false;
            }
        }));
    }

    ngOnInit() {
        this.filterModel = {
            totalCount: 0
        }
        this.getSchool(this.filterModel);
    }

    preInitData() {
        this.filterCourseModel = {
            pagenumber: 1,
            perpagerecord: 10,
            totalCount: 0
        }
        this.getGradeCourseById(this.gradeModel.id);
    }

    getAllCourses() {
        this.allSubscribers.push(this.courseService.getMiniCourseslist({}).subscribe(res => {
            this.coursesList = res.data;
        }));
    }

    getGradeById(id) {
        this.isCallingApi = true;
        this.allSubscribers.push(this.gradeService.getGradeById(id).subscribe(res => {
            this.isCallingApi = false;
            this.gradeModel = _.clone(res.data);
            if (res.data.school)
                this.gradeModel.schoolid = _.clone(res.data).school.id;
        }, err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err);
        }));
    }

    manageGrade() {
        this.isCallingApi = true;
        this.allSubscribers.push(this.gradeService.manageGrade(this.gradeModel).subscribe((res: any) => {
            this.isCallingApi = false;
            this.router.navigate(['/grades/grade-list']);
        }, err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err);
        }));
    }

    getGradeCourseById(id) {
        this.isGradeCourseLoading = true;
        this.allSubscribers.push(this.gradeService.getGradeCourse(this.filterCourseModel, id).subscribe(res => {
            this.isGradeCourseLoading = false;
            this.gradeCourseList = res.data;
            this.filterCourseModel.totalCount = res.totalcount;
        }, err => {
            this.isGradeCourseLoading = false;
            this.utilService.showErrorCall(err);
        }));
    }

    openAddCourseModal() {
        this.assignCourseDialog.openModal();
    }

    openDeleteConfirmation(course, index) {
        course.index = index;
        this.listCommonDialog.open(1, DELETE_TITLE, DELETE_MESSAGE, course);
    }

    addCourseToGrade(model) {
        this.allSubscribers.push(this.gradeService.addGradeCourse({
            "gradeid": this.gradeModel.id,
            "courseid": model.courseid
        }).subscribe(res => {
            this.assignCourseDialog.closeModal();
            this.utilService.showErrorSuccess('', "Course added successfully.");
            this.preInitData();
        }, err => {
            this.utilService.showErrorCall(err);
        }));
    }

    deleteCourseFromGrade(course) {
        this.allSubscribers.push(this.gradeService.deleteCourseFromGrade(course.id).subscribe(res => {
            this.gradeCourseList = _.remove(this.gradeCourseList, (o: any) => {
                return !(o.id == course.id);
            });
        }, err => {
            this.utilService.showErrorCall(err);
        }));
    }
    getSchool(filterObj) {
        this.allSubscribers.push(this.schoolService.getSchool(filterObj).subscribe(res => {
            this.SchoolList = res.data;
        }, err => {
            this.utilService.showErrorCall(err);
        }));
    }

    ngOnDestroy() {
        this.allSubscribers.map(value => value.unsubscribe());
    }

}
