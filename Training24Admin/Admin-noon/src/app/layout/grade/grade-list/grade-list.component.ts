import { Component, OnInit, ViewChild } from '@angular/core';
import * as _ from 'lodash';
import { DELETE_TITLE, DELETE_MESSAGE, UtilService, GradeService } from '../../../shared';

@Component({
    selector: 'app-grade-list',
    templateUrl: './grade-list.component.html',
    styleUrls: ['./grade-list.component.scss']
})
export class GradeListComponent implements OnInit {
    @ViewChild('listdialog') listCommonDialog: any;
    filterModel: any = {};
    isGradesLoading: boolean;
    gradeList: Array<any> = [];
    allSubscribers: Array<any> = [];
    constructor(
        public utilService: UtilService,
        public gradeService: GradeService
    ) { }


    ngOnInit() {
        this.filterModel = {
            pagenumber: 1,
            perpagerecord: 10,
            totalCount: 0
        }
        this.getGrades(this.filterModel);
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
        this.getGrades(this.filterModel);
    }

    onPageChange(event) {
        this.filterModel.pagenumber = event;
        this.getGrades(this.filterModel);
    }

    getGrades(filterObj) {
        this.isGradesLoading = true;
        this.allSubscribers.push(this.gradeService.getGrades(filterObj).subscribe(res => {
            this.isGradesLoading = false;
            this.gradeList = res.data;
            this.filterModel.totalCount = res.totalcount;
        }, err => {
            this.isGradesLoading = false;
            this.utilService.showErrorCall(err);
        }));
    }

    deleteGrade(data) {
        this.allSubscribers.push(this.gradeService.deleteGrade(data.id).subscribe(res => {
            this.gradeList = _.remove(this.gradeList, (o) => {
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
