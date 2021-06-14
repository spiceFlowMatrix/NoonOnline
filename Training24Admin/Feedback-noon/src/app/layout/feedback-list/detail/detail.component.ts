import { ViewChild, Component, OnInit } from '@angular/core';
import { FeedbackService, DataService, UtilService, DELETE_MESSAGE, DELETE_TITLE } from './../../../shared';
import { Router, ActivatedRoute, Params } from '@angular/router';
import * as _ from 'lodash';
@Component({
    selector: 'app-feedback-detail',
    templateUrl: './detail.component.html',
    styleUrls: ['./detail.component.scss'],
})
export class FeedbackDetailComponent implements OnInit {
    @ViewChild('listdialog') commonDeleteDialog: any;

    // filmingTasks: any = [1, 2];
    // graphicsTasks: any = [
    //     { link: 'https://fontawesome.com/v4.7.0/icon/plus-circle' },
    //     { link: 'https://fontawesome.com/v4.7.0/icon/plus-circle' }
    // ];
    // filmngAssStaff: any = ['H', 'W', 'M'];
    public allSubscribers: Array<any> = [];

    tabs: any = [
        { icon: 'fa-upload', selected: false, status: 2, visible: true },
        { icon: 'fa-check', selected: false, status: 1, visible: true },
        { icon: 'fa-stack-exchange', isChangable: true, selected: false, visible: true },
        { icon: 'fa-comments', isChangable: true, selected: false, visible: true }
    ];
    isCallingApi: boolean;
    feedbackDetail: any = {};
    currUserRoles: any = [];
    isTabVisible: boolean;
    isRoleActionable: boolean;

    constructor(
        public dataService: DataService,
        public feedbackService: FeedbackService,
        public utilService: UtilService,
        public activatedRoute: ActivatedRoute
    ) {
        this.activatedRoute.params.subscribe(params => {
            this.dataService.setdetail(params);
            if (params.id) {
                setTimeout(() => {
                    this.isCallingApi = true;
                    this.getFeedbackById(params.id);
                }, 500);
            }
        });
    }

    ngOnInit() {
        this.currUserRoles = this.utilService.getRole();        
        
        let temp = _.find(this.currUserRoles, (roleid) => {
            return roleid == 'admin' || roleid == 'aafmanager';
        });

        this.isTabVisible = false;
        if (temp) {
            this.isTabVisible = true;
            if (this.currUserRoles.indexOf('aafmanager') > -1) {
                this.tabs[0].visible = false;
                this.tabs[1].visible = false;
            }
            if (this.currUserRoles.indexOf('coordinator') > -1) {
                this.tabs[0].visible = false;
            }
            if (this.currUserRoles.indexOf('feedback_edge_team') > -1) {
                this.tabs[1].visible = false;
            }
        }

        this.isRoleActionable = false;
        if (_.find(this.currUserRoles, (roleid) => {
            return roleid == 'admin' || roleid == 'aafmanager';
        })) {
            this.isRoleActionable = true;
        }

    }

    getFeedbackById(id) {
        this.isCallingApi = true;
        this.allSubscribers.push(this.feedbackService.getFeedbackById(id).subscribe(res => {
            this.isCallingApi = false;
            this.feedbackDetail = res.data;
            this.feedbackDetail['text'] = this.utilService.getTagName(res.data);
        }, err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err);
        }));
    }

    selectTab(id) {
        for (let index = 0; index < this.tabs.length; index++) {
            if (id != index)
                this.tabs[index].selected = false;
        }

        if (this.tabs[id].status) {
            this.updateStatusOfFeedback(this.tabs[id].status);
        }

        this.tabs[id].selected = !this.tabs[id].selected;
    }

    updateStatusOfFeedback(status) {
        this.isCallingApi = true;
        if (this.feedbackDetail.status != status)
            this.allSubscribers.push(this.feedbackService.changeFeedbackStatus({
                feedbackid: this.feedbackDetail.id,
                status: status
            }).subscribe(res => {
                this.isCallingApi = false;
                this.feedbackDetail.status = status;
                this.dataService.updateStatusChange(this.feedbackDetail);
            }, err => {
                this.isCallingApi = false;
                this.utilService.showErrorCall(err);
            }));
    }

    selectRoleEvent($event) {
        this.isCallingApi = true;
        let model = {
            "feedbackid": this.feedbackDetail.id,
            "userid": $event.data.id,
            "type": $event.data.type,
            "ismanager": $event.data.ismanager ? true : false
        }

        this.allSubscribers.push(this.feedbackService.assignPerson(model).subscribe(res => {
            this.isCallingApi = false;
            if (res.data) {
                if (res.data.isManager)
                    this.dataService.updateManagerdetail({ id: this.feedbackDetail.id, user: res.data.user, type: res.data.type });
                else {
                    this.updateStaff(res.data.user, res.data.type, true);
                }
            }
        }, err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err);
        }));
    }

    removeStaffEvent($event) {
        let data = _.clone($event.data);
        data.deleteType = 'staff';
        this.commonDeleteDialog.open(1, DELETE_TITLE, DELETE_MESSAGE, data);
    }

    deleteEvent($event) {
        // this.isCallingApi = true;
        if ($event.deleteType == 'staff') {
            console.log($event);
            let model = {
                "feedbackid": this.feedbackDetail.id,
                "userid": $event.id,
                "type": $event.type,
                "ismanager": false
            };
            this.allSubscribers.push(this.feedbackService.removeStaff(model).subscribe(res => {
                this.isCallingApi = false;
                if (res.data) {
                    console.log(res.data);
                    this.updateStaff($event, $event.type, false)
                }
            }, err => {
                this.isCallingApi = false;
                this.utilService.showErrorCall(err);
            }));

        }
    }

    updateStaff(user, type, isAdded: boolean) {
        var key = '';
        switch (type) {
            case 1:
            case '1':
                key = 'editingStaffs';
                break;
            case 2:
            case '2':
                key = 'graphicsStaffs';
                break;
            case 3:
            case '3':
                key = 'filmingStaffs';
                break;
            default:
                break;
        }

        if (!this.feedbackDetail[key])
            this.feedbackDetail[key] = [];

        if (isAdded) {
            this.feedbackDetail[key].push(user);
        } else {
            let fIndex = _.findIndex(this.feedbackDetail[key], { id: user.id });
            if (fIndex > -1) {
                this.feedbackDetail[key].splice(fIndex, 1);
            }
        }
    }

    ngOnDestroy() {
        this.allSubscribers.map(value => value.unsubscribe());
    }
}
