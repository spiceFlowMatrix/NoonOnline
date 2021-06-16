import { Component, OnInit, ViewChild } from '@angular/core';
import * as _ from 'lodash';
import { DELETE_TITLE, DELETE_MESSAGE, UtilService,SchoolService } from '../../../shared';

@Component({
    selector: 'app-school-list',
    templateUrl: './school-list.component.html',
    styleUrls: ['./school-list.component.scss']
})
export class SchoolListComponent implements OnInit {
    @ViewChild('listdialog') listCommonDialog: any;
    filterModel: any = {};
    isSchoolLoading: boolean;
    SchoolList: Array<any> = [];
    allSubscribers: Array<any> = [];
    constructor(
        public utilService: UtilService,
        public schoolService: SchoolService
    ) { }


    ngOnInit() {
        this.filterModel = {
            pagenumber: 1,
            perpagerecord: 10,
            totalCount: 0
        }
        this.getSchool(this.filterModel);
    }

    openDeleteConfirmation(bundle, index) {
        bundle.index = index;
        this.listCommonDialog.open(1, DELETE_TITLE, DELETE_MESSAGE, bundle);
    }

    search($event) {
        this.filterModel.pagenumber = 1;
        this.filterModel.search = $event.value;
        if (!this.filterModel.search)
            delete this.filterModel.search;
        this.getSchool(this.filterModel);
    }

    onPageChange(event) {
        this.filterModel.pagenumber = event;
        this.getSchool(this.filterModel);
    }

    getSchool(filterObj) {
        this.isSchoolLoading = true;
        this.allSubscribers.push(this.schoolService.getSchool(filterObj).subscribe(res => {
            this.isSchoolLoading = false;
            this.SchoolList = res.data;
            this.filterModel.totalCount = res.totalcount;
        }, err => {
            this.isSchoolLoading = false;
            this.utilService.showErrorCall(err);
        }));
    }

    deleteSchool(data) {
        this.allSubscribers.push(this.schoolService.deleteSchool(data.id).subscribe(res => {
            this.SchoolList = _.remove(this.SchoolList, (o) => {
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
