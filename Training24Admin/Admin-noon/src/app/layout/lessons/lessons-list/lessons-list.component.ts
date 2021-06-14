import { Component, OnInit, ViewChild } from '@angular/core';
import * as _ from 'lodash';
import { DELETE_TITLE, DELETE_MESSAGE, UtilService, LessonService } from '../../../shared';

@Component({
    selector: 'app-lessons-list',
    templateUrl: './lessons-list.component.html',
    styleUrls: ['./lessons-list.component.scss']
})
export class LessonsListComponent implements OnInit {
    private allSubscribers: Array<any> = [];
    @ViewChild('listdialog') listCommonDialog: any;
    filterModel: any = {};
    isLessonsLoading: boolean;
    lessonsList: Array<any> = [];
    constructor(
        public utilService: UtilService,
        public lessonService: LessonService
    ) { }


    ngOnInit() {
        this.filterModel = {
            pagenumber: 1,
            perpagerecord: 10,
            totalCount: 0
        }
        this.getLessons(this.filterModel);
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
        this.getLessons(this.filterModel);
    }

    onPageChange(event) {
        this.filterModel.pagenumber = event;
        this.getLessons(this.filterModel);
    }

    getLessons(filterObj) {
        this.isLessonsLoading = true;
        this.allSubscribers.push(this.lessonService.getLessons(filterObj).subscribe(res => {
            this.isLessonsLoading = false;
            this.lessonsList = res.data;
            this.filterModel.totalCount = res.totalcount;
        }, err => {
            this.isLessonsLoading = false;
            this.utilService.showErrorCall(err);
        }));
    }

    deleteLesson(data) {
        this.allSubscribers.push(this.lessonService.deleteLesson(data.id).subscribe(res => {
            this.lessonsList = _.remove(this.lessonsList, (o) => {
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
