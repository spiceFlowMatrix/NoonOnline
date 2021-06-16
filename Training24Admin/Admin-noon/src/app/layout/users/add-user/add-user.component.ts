import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { NgbModal, NgbCalendar } from '@ng-bootstrap/ng-bootstrap';
import { NgbDate } from '@ng-bootstrap/ng-bootstrap/datepicker/ngb-date';
import {
    DELETE_TITLE,
    DELETE_MESSAGE,
    UtilService,
    UsersService,
    CourseService,
    SchoolService,
    GradeService,
    EmailPattern,
    PasswordPattern
} from '../../../shared';
import * as _ from 'lodash';
import * as moment from 'moment';
import { CurrencyLists } from '../../../shared/models/currency-list';

@Component({
    selector: 'app-add-user',
    templateUrl: './add-user.component.html',
    styleUrls: ['./add-user.component.scss']
})
export class AddUserComponent implements OnInit {
    private allSubscribers: Array<any> = [];
    @ViewChild('manageUserForm') manageUserForm: NgForm;
    public userModel: any = {};
    public currencyLists: any = [];
    public categorylist: any = [];
    isEditView: boolean;
    isUserNameExist: boolean;
    isUserNameChecking: boolean;
    isCallingApi: boolean;
    isCourseLoadingApi: boolean;
    isStudent: boolean;
    isAgent: boolean;
    isParent: boolean;
    emailPattern: string = EmailPattern;
    passwordPattern: string = PasswordPattern;
    userRoles: any = [];

    @ViewChild('assignCourseDialog') assignCourseDialog: any;
    @ViewChild('listdialog') listCommonDialog: any;
    userCourseList: any = [];
    addCourseToUserModel: any = [];
    tempCoursesModel: any = [];
    tempCoursesModelIndex: number = 0;
    courseAssignIndex: number = 0;
    coursesList: any = [];
    schoolsList: any = [];
    gradeList: any = [];
    tempGradeList: any = [];
    modalRef: any = [];
    public filterCourseModel: any = {};

    constructor(
        private activatedRoute: ActivatedRoute,
        public utilService: UtilService,
        public usersService: UsersService,
        public courseService: CourseService,
        public schoolService: SchoolService,
        public gradeService: GradeService,
        public modalService: NgbModal,
        public calendar: NgbCalendar,
        public router: Router
    ) {
        this.userModel.isactive = true;
        this.activatedRoute.params.subscribe((params: Params) => {
            if (params['id']) {
                this.isEditView = true;
                this.userModel.id = params['id'];
                this.getUserById(this.userModel.id);
                setTimeout(() => {
                    this.preInitData();
                }, 1000);
            } else {
                this.userModel.roles = [];
                this.userModel.roles.push({ roleid: '' });
                this.isEditView = false;
                this.userModel.is_skippable = true;
            }
        });
    }

    ngOnInit() {
        this.currencyLists = CurrencyLists;
        this.getRoles(false);
        this.getAllCourses();
        this.getAgentCategory();

    }
    checkForStudent() {
        this.isStudent = _.find(this.userModel.roles, (o: any) => {
            return (o.roleid + '') == '4';
        }) ? true : false;
        this.isAgent = _.find(this.userModel.roles, (o: any) => {
            return (o.roleid + '') == '17';
        }) ? true : false;
        this.isParent = _.find(this.userModel.roles, (o:any) => {
            return (o.roleid + '') == '18';
        }) ? true : false;
        // if(this.isAgent) {
        //    this.userModel.isactive = true;
        // } 
    }

    getAgentCategory() {
        this.allSubscribers.push(this.usersService.getAgentCategory({}).subscribe(res => {
            this.categorylist = res.data;
        }, err => {
            this.utilService.showErrorCall(err);
        }));
    }

    preInitData() {
        this.filterCourseModel = {
            pagenumber: 1,
            perpagerecord: 10,
            totalCount: 0
        }
        setTimeout(() => {
            this.getUserCourseById(this.userModel.id);
            this.getSchools();
            this.getGrades();
        }, 700);
    }

    getRoles(isAdminRoleIgnored) {
        this.usersService.getRoles(isAdminRoleIgnored).then((res: any) => {
            this.userRoles = res;
        });
    }

    getSchools() {
        this.schoolService.getSchool().subscribe((res: any) => {
            this.schoolsList = res.data;
        });
    }

    schoolChange() {
        this.tempGradeList = [];
        for (let index = 0; index < this.gradeList.length; index++) {
            if (this.gradeList[index].school && this.gradeList[index].school.id == this.addCourseToUserModel.schoolid) {
                this.tempGradeList.push(this.gradeList[index]);
            }
        }
    }

    getGrades() {
        this.gradeService.getGrades().subscribe((res: any) => {
            this.gradeList = res.data;
            this.tempGradeList = _.clone(this.gradeList);
        });
    }

    gradeChange() {
        if (this.addCourseToUserModel.gradeid)
            this.gradeService.getGradeCourse({}, this.addCourseToUserModel.gradeid).subscribe(res => {
                this.coursesList = res.data;
            }, err => {
                this.utilService.showErrorCall(err);
            });
    }

    getUserById(id) {
        this.isCallingApi = true;
        this.allSubscribers.push(this.usersService.getUserById(id).subscribe(res => {
            this.isCallingApi = false;
            this.userModel = _.clone(res.data);
            this.userModel.roles = [];
            for (let index = 0; index < _.clone(res.data).roles.length; index++) {
                this.userModel.roles.push({ roleid: _.clone(res.data).roles[index] });
            }
            this.userModel.roles.push({ roleid: '' });
            this.checkForStudent();
        }, err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err);
        }));
    }

    checkUsernameExist() {
        if (this.userModel.username.length < 1)
            return;
        this.isUserNameChecking = true;
        this.usersService.checkUserName(this.userModel.username).subscribe(res => {
            this.isUserNameChecking = false;
            this.isUserNameExist = !res;
        }, err => {
            this.isUserNameChecking = false;
            this.utilService.showErrorCall(err);
        });
    }

    manageUser() {
        if (this.isUserNameExist) {
            this.utilService.showErrorToast('Please correct it!', "Username already taken.");
            return false;
        }

        let userModel = _.clone(this.userModel);
        this.isCallingApi = true;
        for (let index = 0; index < userModel.roles.length; index++) {
            if (!userModel.roles[index].roleid)
                userModel.roles.splice(index, 1);
        }
        this.userModel.addedfrom = 'admin';

        this.allSubscribers.push(this.usersService.manageUser(this.userModel).subscribe((res: any) => {
            this.isCallingApi = false;
            this.router.navigate(['users-list']);
        }, err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err);
        }));
    }

    getAllCourses() {
        this.allSubscribers.push(this.courseService.getMiniCourseslist({}).subscribe(res => {
            this.coursesList = res.data;
        }));
    }

    getUserCourseById(id) {
        this.isCourseLoadingApi = true;
        this.allSubscribers.push(this.courseService.getUserCourse(this.filterCourseModel, id).subscribe(res => {
            this.isCourseLoadingApi = false;
            this.userCourseList = res.data;
            this.filterCourseModel.totalCount = res.totalcount;
        }, err => {
            this.isCourseLoadingApi = false;
            this.utilService.showErrorCall(err);
        }));
    }

    openAddCourseModal(model) {
        this.addCourseToUserModel = _.clone(model);

        if (model.id) {
            let date = moment(new Date(this.addCourseToUserModel.startDate));
            this.addCourseToUserModel.startdate = new NgbDate(date.year(), date.month() + 1, date.date());
            date = _.clone(moment(new Date(this.addCourseToUserModel.endDate)));
            this.addCourseToUserModel.enddate = new NgbDate(date.year(), date.month() + 1, date.date());
            this.gradeChange();
        }
        else {
            // this.addCourseToUserModel.startdate = this.calendar.getToday();
            // this.addCourseToUserModel.enddate = this.calendar.getToday();
            this.tempCoursesModel = [];
            this.tempCoursesModel.push(this.addCourseToUserModel);
            this.tempCoursesModelIndex = 0;
        }
        this.modalRef = this.modalService.open(this.assignCourseDialog, { backdrop: 'static', size: 'lg' });
    }

    addTempCourse() {
        this.tempCoursesModel[this.tempCoursesModelIndex] = _.clone(this.addCourseToUserModel);
        this.tempCoursesModel.push({});
        this.addCourseToUserModel = {};
        this.tempCoursesModelIndex = this.tempCoursesModel.length - 1;
    }

    moveToCourseSelection(event, index) {
        if (event)
            event.preventDefault();
        if (index != this.tempCoursesModelIndex) {
            if (this.tempCoursesModel[this.tempCoursesModelIndex])
                this.tempCoursesModel[this.tempCoursesModelIndex] = _.clone(this.addCourseToUserModel);
            this.addCourseToUserModel = _.clone(this.tempCoursesModel[index]);
            this.tempCoursesModelIndex = index;
            this.schoolChange();
            this.gradeChange()
        }
    }

    removeCourse(event, index, isFirst, isLast) {
        event.preventDefault();
        this.tempCoursesModel[this.tempCoursesModelIndex] = _.clone(this.addCourseToUserModel);
        if (isFirst || isLast) {
            this.moveToCourseSelection(null, 0);
        }
        this.tempCoursesModel.splice(index, 1);
    }

    addCourses() {
        this.tempCoursesModel[this.tempCoursesModelIndex] = _.clone(this.addCourseToUserModel);
        let foundInComplete = false;

        for (let index = 0; index < this.tempCoursesModel.length; index++) {
            if (this.userModel.roleName.indexOf('Teacher') > -1) {
                if (!(this.tempCoursesModel[index].schoolid &&
                    this.tempCoursesModel[index].gradeid &&
                    this.tempCoursesModel[index].courseid)) {
                    this.moveToCourseSelection(null, index);
                    foundInComplete = true;
                    this.utilService.showErrorWarning("Please fill detail or remove it.")
                    break;
                }
            }
            else {
                if (!(this.tempCoursesModel[index].schoolid &&
                    this.tempCoursesModel[index].gradeid &&
                    this.tempCoursesModel[index].courseid &&
                    this.tempCoursesModel[index].startdate &&
                    this.tempCoursesModel[index].enddate)) {
                    this.moveToCourseSelection(null, index);
                    foundInComplete = true;
                    this.utilService.showErrorWarning("Please fill detail or remove it.")
                    break;
                } else {
                    let offSet = -1 * (new Date().getTimezoneOffset());
                    if (!this.tempCoursesModel[index].fromDate && this.tempCoursesModel[index].startdate) {
                        this.tempCoursesModel[index]['fromDate'] = _.clone(moment(this.tempCoursesModel[index].startdate.toString()).add(offSet, 'minutes'));
                    }
                    if (!this.tempCoursesModel[index].toDate && this.tempCoursesModel[index].enddate) {
                        this.tempCoursesModel[index]['toDate'] = _.clone(moment(this.tempCoursesModel[index].enddate.toString()).add(offSet, 'minutes'));
                    }

                    if (this.tempCoursesModel[index].fromDate.isAfter(this.tempCoursesModel[index].toDate)) {
                        this.utilService.showErrorToast("From date is not grater than to date. Please correct it.");
                        this.moveToCourseSelection(null, index);
                        foundInComplete = true;
                        break;
                    }
                }
            }
        }

        if (!foundInComplete) {
            this.assignCourses(0);
        }
    }

    assignCourses(index) {
        if (index >= this.tempCoursesModel.length) {
            this.modalRef.dismiss();
            this.utilService.showErrorSuccess('', "Course added successfully.");
            this.preInitData();
            return;
        }

        let data: any = {};
        if (this.tempCoursesModel.length > 0) {
            data = _.clone(this.tempCoursesModel[index]);
        } else {
            data = _.clone(this.addCourseToUserModel);
        }
        this.addCourseToUser(data).then(res => {
            this.courseAssignIndex++;
            this.assignCourses(this.courseAssignIndex);
        }).catch(err => {
            this.modalRef.dismiss();
        });
    }

    addCourseToUser(data) {
        return new Promise((resolve, reject) => {
            let model: any = {
                "userid": this.userModel.id,
                "courseid": data.courseid
            };

            if (data.id)
                model.id = data.id;
            let offSet = -1 * (new Date().getTimezoneOffset());
            if (!data.fromDate) {
                data['fromDate'] = _.clone(moment(data.startdate).add(offSet, 'minutes'));
            }
            if (!data.toDate) {
                data['toDate'] = _.clone(moment(data.enddate).add(offSet, 'minutes'));
            }

            if (data.fromDate.isAfter(data.toDate)) {
                this.utilService.showErrorToast("From date is not grater than to date. Please correct it.");
                return;
            }

            if (this.userModel.roleName.indexOf('Student') > -1) {
                model.startdate = moment(data.fromDate.toString()).add(offSet, 'minutes').toISOString();
                model.enddate = moment(data.toDate.toString()).add(offSet, 'minutes').toISOString();
            }

            this.isCallingApi = true;
            setTimeout(() => {
                this.allSubscribers.push(this.courseService.addUserCourse(model).subscribe(res => {
                    // this.modalRef.dismiss();
                    this.isCallingApi = false;
                    // this.utilService.showErrorSuccess('', "Course added successfully.");
                    // this.preInitData();                    
                    resolve();
                }, err => {
                    this.isCallingApi = false;
                    // this.utilService.showErrorCall(err);                    
                    resolve();
                }));
            }, 100);
        });
    }

    // addCourseToUser() {
    //     let model: any = {
    //         "userid": this.userModel.id,
    //         "courseid": this.addCourseToUserModel.courseid
    //     };

    //     if (this.addCourseToUserModel.id)
    //         model.id = this.addCourseToUserModel.id;
    //     let offSet = -1 * (new Date().getTimezoneOffset());
    //     if (!this.addCourseToUserModel.fromDate) {
    //         this.addCourseToUserModel['fromDate'] = _.clone(moment(this.addCourseToUserModel.startdate).add(offSet, 'minutes'));
    //     }
    //     if (!this.addCourseToUserModel.toDate) {
    //         this.addCourseToUserModel['toDate'] = _.clone(moment(this.addCourseToUserModel.enddate).add(offSet, 'minutes'));
    //     }

    //     if (this.addCourseToUserModel.fromDate.isAfter(this.addCourseToUserModel.toDate)) {
    //         this.utilService.showErrorToast("From date is not grater than to date. Please correct it.");
    //         return;
    //     }
    //     if (this.userModel.roleName.indexOf('Student') > -1) {
    //         model.startdate = moment(this.addCourseToUserModel.fromDate.toString()).add(offSet, 'minutes').subtract(1, 'month').toISOString();
    //         model.enddate = moment(this.addCourseToUserModel.toDate.toString()).add(offSet, 'minutes').subtract(1, 'month').toISOString();
    //     }

    //     this.isCallingApi = true;
    //     setTimeout(() => {
    //         this.allSubscribers.push(this.courseService.addUserCourse(model).subscribe(res => {
    //             this.modalRef.dismiss();
    //             this.isCallingApi = false;
    //             this.utilService.showErrorSuccess('', "Course added successfully.");
    //             this.preInitData();
    //         }, err => {
    //             this.isCallingApi = false;
    //             this.utilService.showErrorCall(err);
    //         }));
    //     }, 100);
    // }


    onPageChange(event) {
        this.filterCourseModel.pagenumber = event;
        this.getUserCourseById(this.userModel.id);
    }

    openDeleteConfirmation(course, index) {
        course.index = index;
        this.listCommonDialog.open(1, DELETE_TITLE, DELETE_MESSAGE, course);
    }

    deleteCourseFromUser(course) {
        this.allSubscribers.push(this.courseService.deleteCourseFromUser(course.id).subscribe(res => {
            this.userCourseList = _.remove(this.userCourseList, (o: any) => {
                return !(o.id == course.id);
            });
        }, err => {
            this.utilService.showErrorCall(err);
        }));
    }

    dateChanged(date: any, flag) {
        this.addCourseToUserModel[flag] = moment(NgbDate.from(date).toString());
    }

    onDateSelection(date: any) {
        date = _.clone(moment(date));
        if (!this.addCourseToUserModel.fromDate && !this.addCourseToUserModel.toDate) {
            this.addCourseToUserModel.fromDate = date;
        } else if (this.addCourseToUserModel.fromDate && !this.addCourseToUserModel.toDate && date.isAfter(this.addCourseToUserModel.fromDate)) {
            this.addCourseToUserModel.toDate = date;
        } else {
            this.addCourseToUserModel.toDate = null;
            this.addCourseToUserModel.fromDate = date;
        }
    }

    isHovered(date) {
        date = _.clone(moment(date));
        return this.addCourseToUserModel.fromDate && !this.addCourseToUserModel.toDate && this.addCourseToUserModel.hoveredDate && date.isAfter(this.addCourseToUserModel.fromDate) && date.isBefore(this.addCourseToUserModel.hoveredDate);
    }

    isInside(date) {
        date = _.clone(moment(date));
        return date.isAfter(this.addCourseToUserModel.fromDate) && date.isBefore(this.addCourseToUserModel.toDate);
    }

    isRange(date) {
        date = _.clone(moment(date));
        return date.isSame(this.addCourseToUserModel.fromDate) || date.isSame(this.addCourseToUserModel.toDate) || this.isInside(date) || this.isHovered(date);
    }

    ngOnDestroy() {
        this.allSubscribers.map(value => value.unsubscribe());
    }
}
