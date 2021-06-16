import { Component, OnInit, ViewChild } from '@angular/core';
import * as _ from 'lodash';
import * as moment from 'moment';
import { UtilService, FeedbackService, UsersService, DataService, GradeService } from '../../shared';
import { Router } from '@angular/router';
import { debounceTime } from 'rxjs/operators';

@Component({
  selector: 'app-feedback-list',
  templateUrl: './feedback-list.component.html',
  styleUrls: ['./feedback-list.component.scss']
})
export class FeedbackListComponent implements OnInit {
  @ViewChild("filterPopOver") filterPopOver: any;
  @ViewChild("popContent") popContent: any;
  @ViewChild("feedbackFilterForm") feedbackFilterForm: any;
  feedbackModel: any = {};
  // feedbackList: Array<any> = [];
  feedbackList: any = {};
  gradeList: Array<any> = [];
  gradeCourseList: Array<any> = [];
  isCallingApi: boolean;
  isDetailCalled: boolean;
  isUsersLoading: boolean;
  users: any = {};
  filterDate: any = '';
  selectedFeedback: any = {};
  public allSubscribers: Array<any> = [];
  public filterFormSubscriber: any;

  constructor(
    public feedbackService: FeedbackService,
    public usersService: UsersService,
    public utilService: UtilService,
    public gradeService: GradeService,
    public dataService: DataService,
    public router: Router
  ) {
    this.dataService.setSideMenu(true);
    this.dataService.feedbackDetail$.subscribe(res => {
      this.openDetailView(res, true);
    });

    this.dataService.managerUpdate$.subscribe(res => {
      this.updateListByManagerUpdate(res);
    });

    this.dataService.status$.subscribe(res => {
      this.updateListByStatusUpdate(res);
    });
  }

  ngOnInit() {
    this.resetfilter();
    this.getUserRoleWise();
    this.getGrades();
  }

  ngAfterViewInit() {
    this.toggleScroll(false);
  }

  toggleScroll(flag) {
    let element: HTMLBodyElement = document.getElementsByTagName("body")[0];
    if (flag) {
      element.classList.remove('hide-overflow');
    } else {
      element.classList.add('hide-overflow');
    }
  }

  resetfilter() {
    this.feedbackModel.fltroptiongrade = 2;
    this.feedbackModel.fltroptionenddate = 2;
    this.feedbackModel.isfilterneeded = false;
    this.isCallingApi =true;
    setTimeout(() => {
      this.getFeedbackList(this.feedbackModel);
    }, 500);
  }

  initForm() {
    if (this.filterFormSubscriber) {
      this.filterFormSubscriber.unsubscribe();
    }
    this.filterFormSubscriber = null;
    setTimeout(() => {
      this.filterFormSubscriber = this.feedbackFilterForm.valueChanges.pipe(debounceTime(1000));
      this.filterFormSubscriber.subscribe(val => {
        this.applyfeedbackFilter(val);
      });
    }, 1000);
  }

  applyfeedbackFilter(val) {
    let model: any = {};
    model.fltroptionenddate = this.feedbackModel.fltroptionenddate;
    model.fltroptiongrade = this.feedbackModel.fltroptiongrade;
    model.isfilterneeded = true;
    if (this.feedbackModel.grade) {
      let obj = _.find(this.gradeList, obj => {
        return obj.id == parseInt('' + this.feedbackModel.grade)
      });
      if (obj) {
        model.grade = obj.name;
      }
    }
    model.courseids = [];
    if (this.feedbackModel.selectedCourses && Array.isArray(this.feedbackModel.selectedCourses))
      for (let index = 0; index < this.feedbackModel.selectedCourses.length; index++) {
        model.courseids.push(this.feedbackModel.selectedCourses[index].courseid);
      }

    if (model.courseids.length > 0) {
      model.courseids = _.clone(model.courseids).toString()
    } else {
      delete model.courseids;
    }
    if (this.feedbackModel['enddate'])
      model.enddate = new Date(this.feedbackModel['enddate']['year'], this.feedbackModel['enddate']['month'] - 1, this.feedbackModel['enddate']['day']).toISOString();

    this.getFeedbackList(model);
  }

  gradeChange(gradeid) {
    this.feedbackModel.selectedCourses = [];
    this.isCallingApi = true;
    this.allSubscribers.push(this.gradeService.getGradeCourse({}, this.feedbackModel.grade).subscribe(res => {
      this.isCallingApi = false;
      this.gradeCourseList = res.data;
    }, err => {
      this.isCallingApi = false;
      this.utilService.showErrorCall(err);
    }));
  }

  courseAddedToFilter(courseId: string) {
    let tempObj: any = _.find(this.gradeCourseList, (obj) => {
      return obj.courseid == parseInt(courseId);
    });
    if (tempObj) {
      let fIndex = _.findIndex(this.feedbackModel.selectedCourses, { courseid: courseId });
      if (fIndex < 0) {
        this.feedbackModel.selectedCourses.push(_.clone(tempObj));
      }
    }
  }

  getGrades() {
    this.isCallingApi = true;
    this.gradeService.getGradeList({}).then((res: any) => {
      this.isCallingApi = false;
      this.gradeList = res;
    }).catch(err => {
      this.isCallingApi = false;
    });
    // this.allSubscribers.push(this.gradeService.getGrades({}).subscribe(res => {
    //   this.isCallingApi = false;
    //   this.gradeList = res.data;
    // }, err => {
    //   this.isCallingApi = false;
    //   this.utilService.showErrorCall(err);
    // }));
  }

  getFeedbackList(feedbackModel?: any) {
    this.isCallingApi = true;
    this.allSubscribers.push(this.feedbackService.getFeedbacks(feedbackModel).subscribe(res => {
      this.isCallingApi = false;
      this.feedbackList = _.clone(res.data);
      this.openDetailView(this.selectedFeedback, true);
    }, err => {
      this.isCallingApi = false;
      this.utilService.showErrorCall(err);
    }));
  }

  updateListByStatusUpdate(res) {
    let isFounded = false;
    Object.keys(this.feedbackList).forEach(key => {
      if (!isFounded) {
        for (let index = 0; index < this.feedbackList[key].length; index++) {
          if (this.feedbackList[key][index].id == res.id) {
            if (res.status == 1) {
              this.feedbackList['completedfeedback'].push(_.clone(this.feedbackList[key][index]));
            }
            if (res.status == 2) {
              this.feedbackList['uploadedfeedback'].push(_.clone(this.feedbackList[key][index]));
            }
            this.feedbackList[key].splice(index, 1);
            isFounded = true;
            break;
          }
        }
      }
    });
  }

  getUserRoleWise() {
    this.isUsersLoading = true;
    this.allSubscribers.push(this.usersService.getUsers({
      roleid: '6,7,8,9,10,11,12,14,15,16'
    }).subscribe(res => {
      this.isUsersLoading = false;
      this.users = res.data;
      this.dataService.setUsers(res.data);
    }, err => {
      this.isUsersLoading = false;
      this.utilService.showErrorCall(err);
    }));
  }

  callAddTask() {
    this.feedbackList.push(1);
  }

  openDetailView(object, usingSubscriber?: boolean, key?: string) {
    this.selectedFeedback = object;
    let found: boolean = false;
    if (key) {
      for (let index = 0; index < this.feedbackList[key].length; index++) {
        this.feedbackList[key][index].selected = false;
        if (object.id == this.feedbackList[key][index].id) {
          found = true;
          this.feedbackList[key][index].selected = true;
        }
      }
    } else {
      Object.keys(this.feedbackList).forEach(key => {
        this.openDetailView(object, usingSubscriber, key);
      });
      // for (let index = 0; index < this.feedbackList.length; index++) {
      //   this.feedbackList[index].selected = false;
      //   if (object.id == this.feedbackList[index].id) {
      //     found = true;
      //     this.feedbackList[index].selected = true;
      //   }
      // }
    }
    if (found) {
      if (!usingSubscriber)
        this.router.navigate(['/feedback-list/detail' + (object.id ? '/' + object.id : '')]);
      this.isDetailCalled = true;
    }
  }

  updateListByManagerUpdate(res) {

    let isFounded = false;
    Object.keys(this.feedbackList).forEach(key => {
      if (!isFounded) {
        for (let index = 0; index < this.feedbackList[key].length; index++) {
          if (this.feedbackList[key][index].id == res.id) {
            switch (res.type) {
              case 1:
              case '1':
                this.feedbackList[key][index].editingManager = res.user;
                break;
              case 2:
              case '2':
                this.feedbackList[key][index].graphicsManager = res.user;
                break;
              case 3:
              case '3':
                this.feedbackList[key][index].filmingManager = res.user;
                break;
              default:
                break;
            }
            isFounded = true;
            break;
          }
        }
      }
    });

    // let fIndex = _.findIndex(this.feedbackList, { id: res.id });
    // if (fIndex > -1) {
    //   switch (res.type) {
    //     case 1:
    //     case '1':
    //       this.feedbackList[fIndex].editingManager = res.user;
    //       break;
    //     case 2:
    //     case '2':
    //       this.feedbackList[fIndex].graphicsManager = res.user;
    //       break;
    //     case 3:
    //     case '3':
    //       this.feedbackList[fIndex].filmingManager = res.user;
    //       break;
    //     default:
    //       break;
    //   }
    // }
  }

  filterDateSelected($event) {
    this.filterDate = new Date($event.year, $event.month, $event.day);
  }

  closeFilterPopOver() {
    this.filterPopOver.close();
  }

  ngOnDestroy() {
    this.toggleScroll(true);
    this.allSubscribers.map(value => value.unsubscribe());
  }
}
