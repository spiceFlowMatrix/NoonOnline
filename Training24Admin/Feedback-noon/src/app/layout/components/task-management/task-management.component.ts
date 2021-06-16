import { ViewChild, Component, OnInit, Input, Output, EventEmitter, OnChanges, SimpleChanges } from '@angular/core';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { Observable, Subject, merge } from 'rxjs';
import { debounceTime, distinctUntilChanged, filter, map } from 'rxjs/operators';
import { DataService } from './../../../shared';
import { FeedbackService, UtilService, UrlPattern } from './../../../shared';
import * as moment from 'moment';
import * as _ from 'lodash';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
    selector: 'noon-task-management',
    templateUrl: './task-management.component.html',
})
export class TaskManagementComponent implements OnInit, OnChanges {
    @ViewChild('assignTasksForm') assignTasksForm: any;
    @ViewChild('assignTasksDialog') assignTasksDialog: any;

    @Input('data')
    data: any = {};

    @Input('currUserRoles')
    currUserRoles: any = {};

    assignTaskModel: any = {};
    isAllowToTaskManage: boolean;
    isAllowToManagerSelection: boolean;
    isAllowToStaffAction: boolean;
    isManager: boolean;
    manager: any = {
        filming: false,
        graphics: false,
        editing: false,
    }
    model: any;
    allSubscribers: any = [];
    tempStaffType: any;
    typeheadData: any = [];
    focus$ = new Subject<string>();
    click$ = new Subject<string>();
    formatter = (x: { username: string }) => x.username;
    rformatter = (result: any) => result.username.toUpperCase();
    @Output() selectRoleEvent: EventEmitter<any> = new EventEmitter();
    @Output() removeStaffEvent: EventEmitter<any> = new EventEmitter();
    isCallingApi: boolean;
    modalRef: any = null;
    urlPattern: any = '';

    constructor(
        public dataService: DataService,
        public feedbackService: FeedbackService,
        public utilService: UtilService,
        public activatedRoute: ActivatedRoute,
        public modelService: NgbModal
    ) {

    }

    ngOnChanges(changes: SimpleChanges) {
        this.isManager = false;
        if (_.find(this.currUserRoles, (roleid) => {
            return roleid == 'edit_team_leader' || roleid == 'shooting_team_leader' || roleid == 'graphics_team_leader';
        }))
            this.isManager = true;

        this.manager = {
            filming: false,
            graphics: false,
            editing: false,
        };
        if (this.currUserRoles.indexOf('shooting_team_leader') > -1)
            this.manager.filming = true;
        if (this.currUserRoles.indexOf('graphics_team_leader') > -1)
            this.manager.graphics = true;
        if (this.currUserRoles.indexOf('edit_team_leader') > -1)
            this.manager.editing = true;


        let temp = _.find(this.currUserRoles, (roleid) => {
            return roleid == 'admin' || roleid == 'coordinator' || roleid == 'edit_team_leader' || roleid == 'graphics_team_leader' || roleid == 'shooting_team_leader';
        });

        if (temp) {
            this.isAllowToTaskManage = false;
            if (_.find(this.currUserRoles, (roleid) => {
                return roleid == 'admin' || roleid == 'coordinator';
            }))
                this.isAllowToTaskManage = true;

            if (this.isAllowToTaskManage || this.manager.graphics)
                if (!this.data.graphicstask || (this.data.graphicstask && this.data.graphicstask.length < 1)) {
                    this.data.graphicstask = [{}];
                } else {
                    this.data.graphicstask = _.concat({}, this.data.graphicstask);
                }
            if (this.isAllowToTaskManage || this.manager.filming)
                if (!this.data.filmingtask || (this.data.filmingtask && this.data.filmingtask.length < 1)) {
                    this.data.filmingtask = [{}];
                } else {
                    this.data.filmingtask = _.concat({}, this.data.filmingtask);
                }
            if (this.isAllowToTaskManage || this.manager.editing)
                if (!this.data.editingtask || (this.data.editingtask && this.data.editingtask.length < 1)) {
                    this.data.editingtask = [{}];
                } else {
                    this.data.editingtask = _.concat({}, this.data.editingtask);
                }
        }

        this.isAllowToManagerSelection = false;
        if (_.find(this.currUserRoles, (roleid) => {
            return roleid == 'admin' || roleid == 'aafmanager' || roleid == 'coordinator';
        })) {
            this.isAllowToManagerSelection = true;
        }

        this.isAllowToStaffAction = false;
        if (this.currUserRoles.indexOf('admin') > -1 || this.currUserRoles.indexOf('aafmanager') > -1)
            this.isAllowToStaffAction = true;
    }

    ngOnInit() {
        this.urlPattern = UrlPattern;
    }

    openSelectedtionPopOver(key, type?: any) {
        if (key)
            this.tempStaffType = type;
        this.typeheadData = this.dataService.getUsers()[key];
    }

    callAddTask(event: any, type: any, taskCollectionKey: string) {
        if (event.data.description) {
            let model = {
                "feedbackid": this.data.id,
                "description": event.data.description,
                "filelink": event.data.filelink ? event.data.filelink : '',
                "type": type
            }
            if (event.data.id)
                model['id'] = event.data.id;
            this.isCallingApi = true;
            this.allSubscribers.push(this.feedbackService.addTasks(model).subscribe(res => {
                this.isCallingApi = false;
                if (this.modalRef) {
                    this.modalRef.dismiss();
                    this.modalRef = null;
                }
                if (!model['id']) {
                    this.data[taskCollectionKey].push(res.data);
                    this.data[taskCollectionKey][0] = {};
                } else {
                    let fIndex = _.findIndex(this.data[taskCollectionKey], { id: model['id'] });
                    if (fIndex > -1) {
                        this.data[taskCollectionKey][fIndex] = res.data;
                    }
                }
                // this.data[taskCollectionKey] = _.concat({}, this.data[taskCollectionKey]);
                this.utilService.focusInput(taskCollectionKey + "0");
            }, err => {
                this.isCallingApi = false;
                this.utilService.showErrorCall(err);
            }));
        }
    }

    openAddTaskModal($event, index) {
        if ((this.isAllowToTaskManage || this.manager.graphics) && $event.target.nodeName == 'INPUT') {
            this.assignTaskModel = _.clone(this.data.graphicstask[index]);
            this.assignTaskModel.index = index;
            this.modalRef = this.modelService.open(this.assignTasksDialog, {
                size: 'lg',
                backdrop: 'static',
                centered: true
            });
        }
    }


    addGraphicsTasks() {
        this.callAddTask({ data: this.assignTaskModel }, 2, "graphicstask");
    }

    search = (text$: Observable<string>) => {
        const debouncedText$ = text$.pipe(debounceTime(200), distinctUntilChanged());
        const inputFocus$ = this.focus$;
        return merge(debouncedText$, inputFocus$).pipe(
            map(term => (term === '' ? this.typeheadData
                : this.typeheadData.filter(v =>
                    v.username.toLowerCase().indexOf(term.toLowerCase()) > -1))));
    }

    selectItem($event, type) {
        $event.item.ismanager = false;
        $event.item.type = this.tempStaffType;
        this.selectRoleEvent.emit({ data: $event.item });
    }

    publishRoleChangeEvent($event) {
        $event.data.ismanager = true;
        this.selectRoleEvent.emit($event);
    }

    removeStaff(staffObj, type) {
        staffObj = _.clone(staffObj)
        staffObj.type = type;
        this.removeStaffEvent.emit({ data: staffObj });
    }

    actionOnTask(event: any, type: any, taskCollectionKey: string, index) {
        let isActionable: boolean = false;
        let action: string = "";
        if (_.find(this.currUserRoles, (roleid) => {
            return roleid == 'edit_team_leader' || roleid == 'shooting_team_leader' || roleid == 'graphics_team_leader';
        })) {
            action = 'complete';
            isActionable = true;
        } else if (_.find(this.currUserRoles, (roleid) => {
            return roleid == 'quality_assurance';
        })) {
            action = 'approved';
            isActionable = true;
        }

        if (isActionable) {
            this.isCallingApi = true;
            this.allSubscribers.push(this.feedbackService.updateTaskStatusPerson(event.data, event.data.id).subscribe(res => {
                this.isCallingApi = false;
                if (res.data)
                    this.data[taskCollectionKey][index] = res.data;
            }, err => {
                this.isCallingApi = false;
                this.utilService.showErrorCall(err);
            }));
        }
    }

    ngOnDestroy() {
        this.allSubscribers.map(value => value.unsubscribe());
    }

}
