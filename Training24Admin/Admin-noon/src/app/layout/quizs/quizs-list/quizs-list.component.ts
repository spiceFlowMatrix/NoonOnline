import { Component, OnInit, ViewChild } from '@angular/core';
import * as _ from 'lodash';
import { DELETE_TITLE, DELETE_MESSAGE, UtilService, QuizService } from '../../../shared';

@Component({
    selector: 'app-quizs-list',
    templateUrl: './quizs-list.component.html',
    styleUrls: ['./quizs-list.component.scss']
})
export class QuizsListComponent implements OnInit {
    private allSubscribers: Array<any> = [];
    @ViewChild('listdialog') listCommonDialog: any;
    @ViewChild('previewDialog') previewDialog: any;
    filterModel: any = {};
    quizList: Array<any> = [];
    isQuizsLoading: boolean;
    quizPreviewObj: any = null;

    @ViewChild('quizPreviewDialog') quizPreviewDialog: any;
    constructor(
        public utilService: UtilService,
        public quizService: QuizService,
        
    ) { }


    ngOnInit() {
        this.isQuizsLoading = true;
        this.filterModel = {
            pagenumber: 1,
            perpagerecord: 10,
            totalCount: 0
        }
        // setTimeout(() => {
        this.getQuizs(this.filterModel);
        // }, 1000);
    }

    openDeleteConfirmation(quiz, index) {
        quiz.index = index;
        this.listCommonDialog.open(1, DELETE_TITLE, DELETE_MESSAGE, quiz);
    }

    search($event) {
        this.filterModel.pagenumber = 1;
        this.filterModel.search = $event.value;
        if (!this.filterModel.search)
            delete this.filterModel.search;
        this.getQuizs(this.filterModel);
    }


    onPageChange(event) {
        this.filterModel.pagenumber = event;
        this.getQuizs(this.filterModel);
    }

    getQuizs(filterModelObj) {
        this.isQuizsLoading = true;
        this.allSubscribers.push(this.quizService.getQuizs(filterModelObj).subscribe(res => {
            this.isQuizsLoading = false;
            this.quizList = res.data;
            this.filterModel.totalCount = res.totalcount;
        }, err => {
            this.isQuizsLoading = false;
            this.utilService.showErrorCall(err);
        }));
    }

    deleteQuiz(data) {
        this.allSubscribers.push(this.quizService.deleteQuiz(data.id).subscribe(res => {
            this.quizList = _.remove(this.quizList, (o) => {
                return !(o.id == data.id);
            });
        }, err => {
            this.utilService.showErrorCall(err);
        }));
    }

    loadQuizPreview(quizId) {
        this.quizService.getQuizPreview(quizId).subscribe(res => {
            this.quizPreviewObj = res.data;
            this.previewDialog.open();
        });
    }

    ngOnDestroy() {
        this.allSubscribers.map(value => value.unsubscribe());
    }
}
