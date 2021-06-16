import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { UtilService, CourseService, UsersService, GradeService, FileService } from '../../../shared';
import * as _ from 'lodash';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { HttpEventType } from '@angular/common/http';

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
    public fileModal: any = {};
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
        public fileService:FileService,
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
    uploadCourseCover(event) {
        this.fileModal = {};
        this.isCallingApi = true;
        let modal = {
            fileTypeId: 3,
            contentType: event.target.files[0].type,
            fileName: event.target.files[0].name
        }
        this.courseImageFile = event.target.files[0];
        var reader = new FileReader();
        reader.onload = (e: any) => {
            // this.courseModel.image = e.target.result;
        }
        reader.readAsDataURL(event.target.files[0]);
        this.allSubscribers.push(this.courseService.getCourseCardSigned(modal).subscribe((res) => {
            this.fileModal.filename = res.data.filename;
            this.courseModel.filename = res.data.filename;
            let signUrl = res.data.signedurl;
            this.fileService.putFileOnBucket(signUrl, event.target.files[0]).subscribe((res: any) => {
                switch (res.type) {
                    case HttpEventType.Sent:
                        // this.uploadedPercentage = 0;
                        // this.fileUploadStatusDialog.openModal();
                        this.utilService.showInfoToast("Notification", "Your file upaloding started.");
                        break;
                    case HttpEventType.Response:
                        this.isCallingApi = false;
                        this.utilService.showInfoToast("", "File uploaded successfully.");
                        console.log(res);
                        // this.file = event.target.files[i];
                        // this.isCallingApi = true;
                        // this.allSubscribers.push(this.fileService.SaveFileMetaData(this.fileModal).subscribe((res: any) => {
                        //     this.isCallingApi = false;
                        //     // this.courseModel.coverimage = res.data.id;
                        // }, err => {
                        //     this.isCallingApi = false;
                        //     this.utilService.showErrorCall(err);
                        // }));
                        break;
                    case 1: {
                        // if (
                        //     Math.round(this.uploadedPercentage) !==
                        //     Math.round(
                        //         (event["loaded"] / event["total"]) * 100
                        //     )
                        // ) {
                        //     this.uploadedPercentage =
                        //         (event["loaded"] / event["total"]) * 100;
                        //     console.log(
                        //         Math.round(this.uploadedPercentage)
                        //     );
                        // }
                        break;
                    }
                }
            }, err => {
                this.isCallingApi = false;
                this.utilService.showErrorCall(err);
            })
        }))
    }

    ngOnDestroy() {
        this.allSubscribers.map(value => value.unsubscribe());
    }
}
