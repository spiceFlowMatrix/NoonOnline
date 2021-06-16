import { Component, OnInit, ViewChild } from '@angular/core';
import * as _ from 'lodash';
import { DELETE_TITLE, DELETE_MESSAGE, UtilService, AssignmentService } from '../../../shared';

@Component({
    selector: 'app-assignment-list',
    templateUrl: './assignment-list.component.html',
    styleUrls: ['./assignment-list.component.scss']
})
export class AssignmentsComponent implements OnInit {
    private allSubscribers: Array<any> = [];
    @ViewChild('listdialog') listCommonDialog: any;
    filterModel: any = {};
    isAssignmentsLoading: boolean;
    assignmentsList: Array<any> = [];

    constructor(
        public utilService: UtilService,
        public assignmentService: AssignmentService
    ) { }


    ngOnInit() {
        this.filterModel = {
            pagenumber: 1,
            perpagerecord: 10,
            totalCount: 0
        }
        this.getAssignments(this.filterModel);
    }

    search($event) {
        this.filterModel.pagenumber = 1;
        this.filterModel.search = $event.value;
        if (!this.filterModel.search)
            delete this.filterModel.search;
        this.getAssignments(this.filterModel);
    }


    openDeleteConfirmation(bundle, index) {
        bundle.index = index;
        this.listCommonDialog.open(1, DELETE_TITLE, DELETE_MESSAGE, bundle);
    }

    onPageChange(event) {
        this.filterModel.pagenumber = event;
        this.getAssignments(this.filterModel);
    }

    getAssignments(filterObj) {
        this.isAssignmentsLoading = true;
        this.allSubscribers.push(this.assignmentService.getAssignments(filterObj).subscribe(res => {
            this.isAssignmentsLoading = false;
            this.assignmentsList = res.data;
            this.filterModel.totalCount = res.totalcount;
        }, err => {
            this.isAssignmentsLoading = false;
            this.utilService.showErrorCall(err);
        }));
    }

    deleteAssignment(data) {
        this.allSubscribers.push(this.assignmentService.deleteAssignment(data.id).subscribe(res => {
            this.assignmentsList = _.remove(this.assignmentsList, (o) => {
                return !(o.id == data.id);
            });
        }, err => {
            this.utilService.showErrorCall(err);
        }));
    }

    ngOnDestroy() {
        this.allSubscribers.map(value => value.unsubscribe());
    }
}
