import { Component, OnInit, ViewChild } from '@angular/core';
import * as _ from 'lodash';
import { DELETE_TITLE, DELETE_MESSAGE, UtilService, QuestionsService } from '../../../shared';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { NgForm } from '@angular/forms';

@Component({
    selector: 'app-questions-list',
    templateUrl: './questions-list.component.html',
    styleUrls: ['./questions-list.component.scss']
})
export class QuestionListComponent implements OnInit {
    private allSubscribers: Array<any> = [];
    @ViewChild('listdialog') listCommonDialog: any;
    filterModel: any = {};
    questionAnswers: any = [];
    tempQues: any = {};
    isQuestionsLoading: boolean = false;

    @ViewChild('assignAnswerDialog') assignAnswerDialog: any;
    @ViewChild('assignAnswerForm') assignAnswerForm: NgForm;
    questionsList: Array<any> = [];
    isLoadingAddAns: boolean = false;
    public assignAnswer: any = null;

    constructor(
        public utilService: UtilService,
        public questionsService: QuestionsService,
        private modalService: NgbModal,
    ) { }


    ngOnInit() {
        this.filterModel = {
            pagenumber: 1,
            perpagerecord: 10,
            totalCount: 0
        }
        this.getQuestions(this.filterModel);
    }

    addAnswer(notValidate: boolean = true) {
        if (notValidate && !this.validateAnswers()) {
            return false;
        }
        this.questionAnswers.push({
            answer: '',
            extraText: "",
            isCorrect: (this.tempQues && !this.tempQues.isMultiAnswer) ? true : false,
        });
    }

    validateAnswers() {
        let findEmptyField: boolean = false;
        for (let index = 0; index < this.questionAnswers.length; index++) {
            if (!this.questionAnswers[index].answer) {
                findEmptyField = true;
                this.questionAnswers[index].notFilled = true;
            }
        }

        if (findEmptyField)
            return false;

        return true;
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
        this.getQuestions(this.filterModel);
    }

    onPageChange(event) {
        this.filterModel.pagenumber = event;
        this.getQuestions(this.filterModel);
    }

    getQuestions(filterObj) {
        this.isQuestionsLoading = true;
        this.allSubscribers.push(this.questionsService.getQuestions(filterObj).subscribe(res => {
            this.isQuestionsLoading = false;
            this.questionsList = res.data;
            this.filterModel.totalCount = res.totalcount;
        }, err => {
            this.isQuestionsLoading = false;
            this.utilService.showErrorCall(err);
        }));
    }

    openAssignAnswerModal(question) {
        this.tempQues = question;
        this.getQuestionAnswer();
        this.addAnswer();
        this.assignAnswer = this.modalService.open(this.assignAnswerDialog, { backdrop: 'static', size: 'lg' });
    }

    getQuestionAnswer() {
        this.isLoadingAddAns = true;
        this.allSubscribers.push(this.questionsService.getQuestionAnswer(this.tempQues.id).subscribe(res => {
            this.isLoadingAddAns = false;
            if (res.data && res.data.length > 0) {
                this.questionAnswers = res.data;
                this.addAnswer(false);
            } else {
                this.questionAnswers = [];
                this.addAnswer();
            }
        }, err => {
            this.isLoadingAddAns = false;
            this.utilService.showErrorCall(err);
        }));
    }

    addAnswersToQuestion() {
        if (this.questionAnswers.length == 1 && !this.questionAnswers[0].answer) {
            this.questionAnswers[0].notFilled = true;
            return false;
        }
        let model = {
            "id": this.tempQues.id,
            "lstAnswers": []
        };

        model.lstAnswers = _.clone(this.questionAnswers);
        for (let index = 0; index < model.lstAnswers.length; index++) {
            if (!model.lstAnswers[index].answer) {
                model.lstAnswers.splice(index, 1);
            }
        }

        let answerIndex = _.findIndex(model.lstAnswers, (o) => {
            if (o.isCorrect)
                return true
        });

        if (this.tempQues.isMultiAnswer && answerIndex < 0) {
            this.utilService.showErrorWarning("Validation", "Please select atleast one answer");
            return;
        }
        this.isLoadingAddAns = true;
        this.allSubscribers.push(this.questionsService.addQuestionAnswer(model).subscribe(res => {
            this.isLoadingAddAns = false;
            this.assignAnswer.dismiss();
        }, err => {
            this.isLoadingAddAns = false;
            this.utilService.showErrorCall(err);
        }));

    }

    deleteQuestion(data) {
        this.allSubscribers.push(this.questionsService.deleteQuestions(data.id).subscribe(res => {
            this.questionsList = _.remove(this.questionsList, (o) => {
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
