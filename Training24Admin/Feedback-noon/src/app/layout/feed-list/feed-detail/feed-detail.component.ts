import { ViewChild, Component, OnInit } from '@angular/core';
import { FeedbackService, DataService, UtilService, DELETE_MESSAGE, DELETE_TITLE, UsersService } from '../../../shared';
import { Router, ActivatedRoute, Params } from '@angular/router';
import * as _ from 'lodash';
@Component({
    selector: 'app-feed-detail',
    templateUrl: './feed-detail.component.html',
    styleUrls: ['./feed-detail.component.scss'],
})
export class FeedDetailComponent implements OnInit {
    public allSubscribers: Array<any> = [];
    isCallingApi: boolean;
    feedbackDetail: any = {};
    currUserRoles: any = [];
    users: any;
    userid: any;
    constructor(
        public dataService: DataService,
        public feedbackService: FeedbackService,
        public router: Router,
        public utilService: UtilService,
        public usersService: UsersService,
        public activatedRoute: ActivatedRoute
    ) {
        this.activatedRoute.params.subscribe(params => {
            if (params.id) {
                this.getFeedbackById(params.id);
            }
        });
    }

    ngOnInit() {
        this.getUserRoleWise();
    }

    getUserRoleWise() {
        this.isCallingApi = true;
        this.allSubscribers.push(this.usersService.getUsers({
            roleid: '1,6,7,8,9,10,11,12,14,15,16'
        }).subscribe(res => {
            this.isCallingApi = false;
            this.users = res.data;
        }, err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err);
        }));
    }

    assignUsers(event) {
        this.isCallingApi = true;
        this.allSubscribers.push(this.feedbackService.assignUser(this.feedbackDetail.id, this.userid).subscribe((res) => {
            this.isCallingApi = false;
        }, err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err);
        }))
    }

    moveToStage(type) {
        let id = []
        id.push(this.feedbackDetail.id);
        switch (type) {
            case 'progressfromQueue':
                this.moveToProgress(id);
                break;
            case 'complete':
                this.moveToCompleted(id);
                break;
            case 'archivefromQueue':
                this.moveToArchived(id);
                break;
            case 'archivefromProcess':
                this.moveToArchived(id);
                break;
            case 'progressfromArchive':
                this.moveToProgress(id);
                break;
        }
    }

    moveToProgress(ids) {
        this.isCallingApi = true;
        this.allSubscribers.push(this.feedbackService.moveToProgress(ids).subscribe((res) => {
            this.isCallingApi = false;
            this.router.navigate(['feed-list']);
        }, err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err);
        }))
    }
    moveToCompleted(ids) {
        this.isCallingApi = true;
        this.allSubscribers.push(this.feedbackService.moveToCompleted(ids).subscribe((res) => {
            this.isCallingApi = false;
            this.router.navigate(['feed-list']);
        }, err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err);
        }))
    }
    moveToArchived(ids) {
        this.isCallingApi = true;
        this.allSubscribers.push(this.feedbackService.moveInArchived(ids).subscribe((res) => {
            this.isCallingApi = false;
            this.router.navigate(['feed-list']);
        }, err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err);
        }))
    }

    back() {
        this.router.navigate(['feed-list']);
    }
    download(url) {
        window.open(url);
    }

    getFeedbackById(id) {
        this.isCallingApi = true;
        this.allSubscribers.push(this.feedbackService.getFeedbackById(id).subscribe(res => {
            this.isCallingApi = false;
            this.feedbackDetail = res.data;
            this.userid = res.data.assign.toString();
        }, err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err);
        }));
    }

    ngOnDestroy() {
        this.allSubscribers.map(value => value.unsubscribe());
    }
}
