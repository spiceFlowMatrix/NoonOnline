import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { UtilService, CourseService, UsersService, GradeService } from '../../../shared';
import * as _ from 'lodash';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
    selector: 'app-add-courses',
    templateUrl: './add-courses.component.html',
    styleUrls: ['./add-courses.component.scss']
})
export class AddCoursesComponent implements OnInit {
    private allSubscribers: Array<any> = [];
    @ViewChild('manageCourseForm') manageCourseForm: NgForm;
    public courseModel: any = {};
    public courseImageFile: any = null;
    // public teachersList: any = [];
    public gradeList: any = [];
    isEditView: boolean = false;
    isCallingApi: boolean = false;
    isGradesLoading: boolean = false;

    // @ViewChild('assignCourseDialog') assignCourseDialog: any;
    // public filterCourseModel: any = {};
    // public assignCourseModal: any = null;
    // public addCourseToUserModel: any = {};
    // public courseUserList: any = [];
    // public usersList: any = [];

    constructor(
        private activatedRoute: ActivatedRoute,
        public utilService: UtilService,
        public courseService: CourseService,
        public gradeService: GradeService,
        public usersService: UsersService,
        public modalService: NgbModal,
        public router: Router
    ) {
        this.allSubscribers.push(this.activatedRoute.params.subscribe((params: Params) => {
            if (params['id']) {
                this.isEditView = true;
                this.courseModel.id = params['id'];
                this.getCourseById(this.courseModel.id);
            } else {
                this.isEditView = false;
                this.courseModel.code = this.utilService.getRandomCode('CS');
            }
        }));

        this.getGrades();
    }

    ngOnInit() {
        this.utilService.allowOnlyNumber('c_pass_mark', true);
    }

    // getTeachers() {
    //     this.allSubscribers.push(this.usersService.getUsers({ roleid: 3 }).subscribe(res => {
    //         this.teachersList = res.data;
    //     }));
    // }
    getCourseById(id) {
        this.isCallingApi = true;
        this.allSubscribers.push(this.courseService.getCourseById(id).subscribe(res => {
            this.isCallingApi = false;
            this.courseModel = _.clone(res.data);
            if (!this.courseModel.code)
                this.courseModel.code = this.utilService.getRandomCode('CS');
        }, err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err);
        }));
    }

    getGrades() {
        this.isGradesLoading = true;
        this.allSubscribers.push(this.gradeService.getGrades().subscribe(res => {
            this.isGradesLoading = false;
            this.gradeList = res.data;         
        }, err => {
            this.isGradesLoading = false;
            this.utilService.showErrorCall(err);
        }));
    }

    manageCourse() {
        // if (!this.isEditView && !this.courseImageFile) {
        //     return this.utilService.showErrorToast("Required", "Please add course card.");
        // }
        this.courseModel.istrial = !this.courseModel.istrial ? false : true;
        if (this.courseImageFile) {
            this.courseModel.file = this.courseImageFile;
            // formData.append('file', this.courseImageFile);
        }
        this.isCallingApi = true;
        this.allSubscribers.push(this.courseService.manageCourse(this.courseModel, this.courseModel.id).subscribe((res: any) => {
            this.isCallingApi = false;
            this.router.navigate(['courses']);
        }, err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err);
        }));
    }

    openFileSelecter(id) {
        this.utilService.openFileSelecter(id);
    }

    onPicSelected(event) {
        if (event.target.files && event.target.files.length > 0) {
            this.courseImageFile = event.target.files[0];
            var reader = new FileReader();
            reader.onload = (e: any) => {
                this.courseModel.image = e.target.result;
            }
            reader.readAsDataURL(event.target.files[0]);
        }
    }

    ngOnDestroy() {
        this.allSubscribers.map(value => value.unsubscribe());
    }
}
