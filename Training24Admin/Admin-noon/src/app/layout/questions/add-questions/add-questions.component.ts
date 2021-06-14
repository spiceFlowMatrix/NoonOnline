import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { UtilService, QuestionsService, QuestionTypeService, FileService } from '../../../shared';
import * as _ from 'lodash';

@Component({
    selector: 'app-add-questions',
    templateUrl: './add-questions.component.html',
    styleUrls: ['./add-questions.component.scss']
})
export class AddQuestionComponent implements OnInit {
    private allSubscribers: Array<any> = [];
    @ViewChild('manageQuestionTypeForm') manageQuestionTypeForm: NgForm;
    public questionModel: any = {
    };
    public questionTypeList: any = [];
    public fileList: any = [];
    isEditView: boolean = false;
    isCallingApi: boolean = false;

    constructor(
        private activatedRoute: ActivatedRoute,
        public utilService: UtilService,
        public questionsService: QuestionsService,
        public questionTypeService: QuestionTypeService,
        public fileService: FileService,
        public router: Router
    ) {
        this.questionModel.isMultiAnswer = false;
        this.allSubscribers.push(this.activatedRoute.params.subscribe((params: Params) => {
            if (params['id']) {
                this.isEditView = true;
                this.questionModel.id = params['id'];
                this.getQuestionById(this.questionModel.id);
                this.preInitData();
            } else {
                this.isEditView = false;
            }
        }));
    }

    ngOnInit() {
        this.allSubscribers.push(this.questionTypeService.getQuestionTypes().subscribe(res => {
            this.questionTypeList = res.data;
        }));
    }

    preInitData() {
        this.getFiles();
    }

    getFiles() {
        this.allSubscribers.push(this.fileService.getFiles().subscribe(res => {
            this.fileList = res.data;
        }));
    }


    getQuestionById(id) {
        this.isCallingApi = true;
        this.allSubscribers.push(this.questionsService.getQuestionsById(id).subscribe(res => {
            this.isCallingApi = false;
            this.questionModel = _.clone(res.data);
            this.questionModel.files = [];
            for (let index = 0; index < _.clone(res.data).files.length; index++) {
                this.questionModel.files.push({ fileid: _.clone(res.data).files[index].files.id });
            }
            this.questionModel.files.push({ fileid: '' });
        }, err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err);
        }));
    }

    manageQuestionType() {
        this.isCallingApi = true;
        for (let index = 0; index < this.questionModel.files.length; index++) {
            if (!this.questionModel.files[index].fileid)
                this.questionModel.files.splice(index, 1);
        }
        this.allSubscribers.push(this.questionsService.manageQuestions(this.questionModel).subscribe((res: any) => {
            this.isCallingApi = false;
            this.router.navigate(['/questions/question-list']);
        }, err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err);
        }));
    }

    ngOnDestroy() {
        this.allSubscribers.map(value => value.unsubscribe());
    }

}
