import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import {
    DELETE_TITLE,
    DELETE_MESSAGE,
    UtilService,
    QuizService,
    QuestionsService,
    DataService
} from '../../../shared';
import * as _ from 'lodash';
import { KatexOptions } from 'ng-katex';

@Component({
    selector: 'app-add-quizs',
    templateUrl: './add-quizs.component.html',
    styleUrls: ['./add-quizs.component.scss']
})
export class AddQuizsComponent implements OnInit {
    private allSubscribers: Array<any> = [];
    // equation: string = '\\sum_{i=1}^n(x_i^2 - \\overline{x}^2)';
    // equation: string = '\\sum_{i=1}^nx_i';
    // paragraph: string = "You can write text, that contains expressions like this: $x ^ 2 + 5$ inside them. As you probably know.You also can write expressions in display mode as follows: $$\\sum_{i=1}^n(x_i^2 - \\overline{x}^2)$$.In first case you will need to use \\$expression\\$ and in the second one \\$\\$expression\\$\\$.To scape the \\$ symbol it's mandatory to write as follows: \\\\$";
    paragraph: string = "";
    options: KatexOptions = {
        displayMode: true,
      };
    @ViewChild('manageQuizForm') manageQuizForm: NgForm;
    public quizModel: any = {};
    isEditView: boolean = false;
    isQuestionEditView: boolean = false;
    isCallingApi: boolean = false;
    id: any;
    // @ViewChild('assignQuestionsDialog') assignQuestionsDialog: any;
    @ViewChild('listdialog') listCommonDialog: any;
    @ViewChild('questionsUploadDialog') questionsUploadDialog: any;
    quizQuestionList: any = [];
    // questionsList: any = [];
    addQuizQuestionModel: any = {};
    filterQuestionModel: any = {};
    questionIds: any = [];
    quizid : any;
    questionUploadingIndex: number = 0;
    isQuestionUploading: boolean;
    modalRef: any = null;
    formData: any = null;
    uploadQuizsubScriber: any = null;

    constructor(
        private activatedRoute: ActivatedRoute,
        public utilService: UtilService,
        public quizService: QuizService,
        public questionsService: QuestionsService,
        public dataService: DataService,
        public modalService: NgbModal,
        public router: Router
    ) {
        this.allSubscribers.push(this.activatedRoute.params.subscribe((params: Params) => {
            if (params['id']) {
                this.isEditView = true;
                this.isQuestionEditView = false;
                this.quizModel.id = params['id'];
                this.getQuizById(this.quizModel.id);
                // setTimeout(() => {
                this.preInitData();
                // }, 1000);
            } else {
                this.isQuestionEditView = false;
                this.quizModel.code = this.utilService.getRandomCode('QZ');
                this.isEditView = false;
            }
        }));

        this.allSubscribers.push(this.activatedRoute.queryParams.subscribe((params: Params) => {
            if (params['chapterid'] && params['chaptername'] && params['type'] && params['courseid']) {
                this.quizModel.chaptername = params['chaptername'];
                this.quizModel.chapterId = params['chapterid'];
                this.quizModel.type = params['type'] ? params['type'] : '';
                this.quizModel.courseid = params['courseid'] ? params['courseid'] : '';
                params['itemorder'] ? this.quizModel.itemOrder = (parseInt(params['itemorder']) + 1) : '';
            }
        }));
    }

    ngOnInit() {
        setTimeout(() => {
            window.scrollTo(0, 0); // how far to scroll on each step
        }, 500);
        this.utilService.allowOnlyNumber("pass_mark", true);
        this.utilService.allowOnlyNumber("number");
        this.utilService.allowOnlyNumber("timeout");

    }

    preInitData() {
        this.addQuestion();
    }

    addQuestion() {
        this.quizQuestionList.push({
            answers: [],
            files: [],
            images: [],
            questiontypeid: 2,
            questiontext: '',
            explanation: '',
            isNew: true
        });
    }

    addAnswer(qId) {
        this.quizQuestionList[qId].answers.push({
            answer: '',
            files: [],
            images: [],
            extratext: '',
            iscorrect: false,
            isNew: true
        });
        this.quizQuestionList[qId].isNew = true;
    }

    updatedItem(event, type, qId, aId) {
        this.quizQuestionList[qId].isNew = true;
        switch (type) {
            // case 'question':
            case 'answer':
                this.quizQuestionList[qId].answers[aId].isNew = true;
                break;
        }
    }

    // getQuestions() {
    //     this.allSubscribers.push(this.questionsService.getQuestions().subscribe(res => {
    //         this.questionsList = res.data;
    //     }));
    // }

    getQuizById(id) {
        this.isCallingApi = true;
        this.allSubscribers.push(this.quizService.getQuizById(id).subscribe(res => {
            this.id = res.data.id;
            this.isCallingApi = false;
            Object.keys(res.data).forEach(key => {
                this.quizModel[key] = res.data[key];
            })
            this.quizid = id;
            this.getQuizQuestionById(id);
        }, err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err);
        }));
    }

    manageQuiz() {
        this.isCallingApi = true;
        this.allSubscribers.push(this.quizService.manageQuiz(this.quizModel).subscribe((res: any) => {
            this.isCallingApi = false;
            // this.router.navigate(['quizs']);
            this.quizid = res.data.id;
            // console.log(res.data.id, '/courses/course-preview/' + this.quizModel.courseid + '/edit-quiz/' + res.data.id + window.location.search);
            // this.router.navigateByUrl('/courses/course-preview/' + this.quizModel.courseid + '/edit-quiz/' + res.data.id + window.location.search);
            this.dataService.refreshCourseData({
                operation: 'add',
                type: 'quiz',
                obj: res.data
            });
            // this.getQuizQuestionById(this.quizid);
            this.saveQuiz(0);
        }, err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err);
        }));
    }

    openFileSelecter(qId, aId, answer) {
        let id = 'card_' + qId;
        if (typeof aId == 'number')
            id = id + aId;
        this.utilService.openFileSelecter(id);
    }

    onPicSelected(event, type, qId, aId) {
        switch (type) {
            case 'question':
                // this.quizQuestionList[qId].files = _.concat(this.quizQuestionList[qId].files.event.target.files);
                if (!this.quizQuestionList[qId].files)
                    this.quizQuestionList[qId].files = [];
                if (!this.quizQuestionList[qId].images)
                    this.quizQuestionList[qId].images = [];

                if (event.target.files && event.target.files.length > 0) {
                    for (let index = 0; index < event.target.files.length; index++) {
                        this.getFileUrl(event.target.files[index]).then((res: any) => {
                            this.quizQuestionList[qId].files.push(res.file);
                            this.quizQuestionList[qId].images.push(res.body);
                        });
                    }
                }
                this.quizQuestionList[qId].isNew = true;
                break;

            case 'answer':
                if (!this.quizQuestionList[qId].answers[aId].files)
                    this.quizQuestionList[qId].answers[aId].files = [];
                if (!this.quizQuestionList[qId].answers[aId].images)
                    this.quizQuestionList[qId].answers[aId].images = [];

                if (event.target.files && event.target.files.length > 0) {
                    for (let index = 0; index < event.target.files.length; index++) {
                        this.getFileUrl(event.target.files[index]).then((res: any) => {
                            this.quizQuestionList[qId].answers[aId].files.push(res.file);
                            this.quizQuestionList[qId].answers[aId].images.push(res.body);
                        });
                    }
                }
                this.quizQuestionList[qId].isNew = true;
                this.quizQuestionList[qId].answers[aId].isNew = true;
                break;
        }
    }

    deleteQuizPart(type, qId, aId, fileIndex) {
        let model = {
            "quizid": this.quizModel.id,
            "questionid": 0,
            "answerid": 0,
            "fileid": 0,
            "recordtodelete": 0
        }
        let found = true;
        model.questionid = this.quizQuestionList[qId].id;

        switch (type) {
            case 'question':
                model.recordtodelete = 2;
                if (!model.questionid)
                    found = false;
                break;
            case 'answer':
                model.answerid = this.quizQuestionList[qId].answers[aId].id;
                model.recordtodelete = 3;
                if (!model.answerid)
                    found = false;
                break;
            case 'questionfile':
                found = typeof this.quizQuestionList[qId].images[fileIndex].fileid == 'number' ? true : false;
                model.fileid = this.quizQuestionList[qId].images[fileIndex].fileid;
                model.recordtodelete = 4;
                break;
            case 'answerfile':
                found = typeof this.quizQuestionList[qId].answers[aId].images[fileIndex].fileid == 'number' ? true : false;
                model.answerid = this.quizQuestionList[qId].answers[aId].id;
                model.fileid = this.quizQuestionList[qId].answers[aId].images[fileIndex].fileid;
                model.recordtodelete = 5;
                break;
            default:
                break;
        }

        if (!found) {
            this.removeFromObj(type, qId, aId, fileIndex);
            return;
        }
        this.isCallingApi = true;
        this.allSubscribers.push(this.quizService.deleteQuizDetail(model).subscribe(res => {
            this.isCallingApi = false;
            this.removeFromObj(type, qId, aId, fileIndex);
        }, err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err);
        }));
    }

    removeFromObj(type, qId, aId, fileIndex) {
        switch (type) {
            case 'question':
                this.quizQuestionList.splice(qId, 1);
                break;
            case 'answer':
                this.quizQuestionList[qId].answers.splice(aId, 1);
                break;
            case 'questionfile':
                if (this.quizQuestionList[qId].files && this.quizQuestionList[qId].files[fileIndex])
                    this.quizQuestionList[qId].files.splice(fileIndex, 1);
                if (this.quizQuestionList[qId].images && this.quizQuestionList[qId].images[fileIndex])
                    this.quizQuestionList[qId].images.splice(fileIndex, 1);
                break;

            case 'answerfile':
                if (this.quizQuestionList[qId].answers[aId].files && this.quizQuestionList[qId].answers[aId].files[fileIndex])
                    this.quizQuestionList[qId].answers[aId].files.splice(fileIndex, 1);
                if (this.quizQuestionList[qId].answers[aId].images && this.quizQuestionList[qId].answers[aId].images[fileIndex])
                    this.quizQuestionList[qId].answers[aId].images.splice(fileIndex, 1);
                break;
        }
    }

    getFileUrl(fileObj) {
        return new Promise((resolve, reject) => {
            let f = fileObj;
            var reader = new FileReader();
            reader.onload = (e: any) => {
                resolve({ body: e.target.result, file: f });
            }
            reader.readAsDataURL(fileObj);
        });
    }

    saveQuiz(index) {
        if (!this.isQuestionUploading) {
            this.modalRef = this.modalService.open(this.questionsUploadDialog, { backdrop: 'static' });
            this.isQuestionUploading = true;
        }

        if (this.isQuestionUploading && this.questionUploadingIndex == this.quizQuestionList.length) {
            this.isQuestionUploading = false;
            this.modalRef.dismiss();
        }

        if (this.uploadQuizsubScriber) {
            this.uploadQuizsubScriber.unsubscribe();
        }
        this.questionUploadingIndex = index;

        this.uploadQuestion(this.questionUploadingIndex).then(res => {
            index++;
            this.moveToNextUpload(index);
        }).catch(err => {
            console.log(index, err);
            index++;
            this.moveToNextUpload(index);
        });
    }

    moveToNextUpload(index) {
        if (index < this.quizQuestionList.length) {
            this.saveQuiz(index);
        } else {
            this.getQuizQuestionById(this.quizid);
            this.isQuestionUploading = false;
            this.modalRef.dismiss();
        }
    }

    uploadQuestion(index) {
        return new Promise((resolve, reject) => {
            if (this.quizQuestionList[index].isNew) {
                console.log(this.quizQuestionList[index]);
                this.formData = null;
                this.formData = new FormData();
                this.formData.append('quizid', this.quizid);
                this.formData.append('noanswer', this.quizQuestionList[index].answers.length);
                this.formData.append('questions[0].id', this.quizQuestionList[index].id ? this.quizQuestionList[index].id : '0');
                this.formData.append('questions[0].questiontypeid', this.quizQuestionList[index].questiontypeid);
                this.formData.append('questions[0].questiontype', 2);
                this.formData.append('questions[0].questiontext', this.quizQuestionList[index].questiontext);
                this.formData.append('questions[0].explanation', this.quizQuestionList[index].explanation);
                this.formData.append('questions[0].ismultianswer', true);
                for (let fIndex = 0; fIndex < this.quizQuestionList[index].images.length; fIndex++) {
                    if (this.quizQuestionList[index].files && !this.quizQuestionList[index].images[fIndex].id)
                        this.formData.append('questions[0].files', this.quizQuestionList[index].files[fIndex]);
                }
                for (let aIndex = 0; aIndex < this.quizQuestionList[index].answers.length; aIndex++) {
                    if (this.quizQuestionList[index].answers[aIndex].isNew) {
                        this.formData.append('questions[0].answers[' + aIndex + '].id', this.quizQuestionList[index].answers[aIndex].id ? this.quizQuestionList[index].answers[aIndex].id : '0');
                        this.formData.append('questions[0].answers[' + aIndex + '].answer', this.quizQuestionList[index].answers[aIndex].answer);
                        this.formData.append('questions[0].answers[' + aIndex + '].extraText', this.quizQuestionList[index].answers[aIndex].extratext);
                        this.formData.append('questions[0].answers[' + aIndex + '].isCorrect', this.quizQuestionList[index].answers[aIndex].iscorrect);
                        for (let fIndex = 0; fIndex < this.quizQuestionList[index].answers[aIndex].images.length; fIndex++) {
                            if (this.quizQuestionList[index].answers[aIndex].files && !this.quizQuestionList[index].answers[aIndex].images[fIndex].id)
                                this.formData.append('questions[0].answers[' + aIndex + '].files', this.quizQuestionList[index].answers[aIndex].files[fIndex]);
                        }
                    }
                }
                this.uploadQuizsubScriber = this.quizService.uploadQuizQuestion(this.formData).subscribe(res => {
                    this.quizQuestionList[index].isNew = false;
                    resolve(res);
                }, err => {
                    reject(err);
                });
            } else {
                resolve({});
            }
        })
    }

    openAddQuestionModal() {
        this.isQuestionEditView = true;
        this.getQuizQuestionById(this.id);
        // this.getQuestions();
    }

    getQuizQuestionById(quizId) {
        this.isCallingApi = true;
        this.allSubscribers.push(this.quizService.getQuizDetail(quizId).subscribe(res => {
            this.isCallingApi = false;
            this.quizQuestionList = [];
            this.quizQuestionList = res.data.questions;
            // this.quizQuestionList = res.data.questions ? res.data.questions : [];
            // console.log(this.quizQuestionList);

            // for (let index = 0; index < this.quizQuestionList.length; index++) {
            //     this.questionIds.push(this.quizQuestionList[index]);
            //     this.questionIds = _.unionBy(this.questionIds, 'id');
            // }
            this.addQuestion();
        }, err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err);
        }));
        // this.allSubscribers.push(this.quizService.getQuizQuestions(this.filterQuestionModel, id).subscribe(res => {
        //     this.isCallingApi = false;
        //     this.quizQuestionList = res.data;
        //     for (let index = 0; index < this.quizQuestionList.length; index++) {
        //         this.questionIds.push(this.quizQuestionList[index]);
        //         this.questionIds = _.unionBy(this.questionIds, 'id');
        //     }
        //     // this.filterQuestionModel.totalCount = res.totalcount;
        // }, err => {
        //     this.isCallingApi = false;
        //     this.utilService.showErrorCall(err);
        // }));
    }

    // addQuestionId(questionId) {
    //     this.questionIds.push(_.find(this.questionsList, { id: parseInt(questionId) }));
    //     this.questionIds = _.unionBy(this.questionIds, 'id');
    //     this.addQuizQuestionModel.questionId = undefined;
    // }

    // addQuizQuestion() {
    //     let model: any = {
    //         quizId: this.quizModel.id,
    //         lstQuestionId: []
    //     };
    //     for (let index = 0; index < this.questionIds.length; index++) {
    //         model.lstQuestionId.push(this.questionIds[index].id);
    //     }

    //     this.allSubscribers.push(this.quizService.addQuizQuestion(model).subscribe(res => {
    //         this.modalRef.dismiss();
    //         this.utilService.showErrorSuccess('', "Course added successfully.");
    //         this.preInitData();
    //     }, err => {
    //         this.utilService.showErrorCall(err);
    //     }));
    // }

    // onPageChange(event) {
    //     this.filterQuestionModel.pagenumber = event;
    //     this.getQuizQuestionById(this.quizModel.id);
    // }

    openDeleteConfirmation(question, index) {
        question.index = index;
        this.listCommonDialog.open(1, DELETE_TITLE, DELETE_MESSAGE, question);
    }

    deleteQuestionFromQuiz(question) {
        this.allSubscribers.push(this.quizService.deleteQuizQuestion(question.id).subscribe(res => {
            this.quizQuestionList = _.remove(this.quizQuestionList, (o: any) => {
                return !(o.id == question.id);
            });
        }, err => {
            this.utilService.showErrorCall(err);
        }));
    }

    ngOnDestroy() {
        this.allSubscribers.map(value => value.unsubscribe());
    }

}



// removeFile(type, imageIndex, qId, aId) {
    //     switch (type) {
    //         case 'question':
    //             if (this.quizQuestionList[qId].files && this.quizQuestionList[qId].files[imageIndex])
    //                 this.quizQuestionList[qId].files.splice(imageIndex, 1);
    //             if (this.quizQuestionList[qId].images && this.quizQuestionList[qId].images[imageIndex])
    //                 this.quizQuestionList[qId].images.splice(imageIndex, 1);
    //             break;

    //         case 'answer':
    //             console.log(type, imageIndex, qId, aId, this.quizQuestionList[qId].answers);
    //             if (this.quizQuestionList[qId].answers[aId].files && this.quizQuestionList[qId].answers[aId].files[imageIndex])
    //                 this.quizQuestionList[qId].answers[aId].files.splice(imageIndex, 1);
    //             if (this.quizQuestionList[qId].answers[aId].images && this.quizQuestionList[qId].answers[aId].images[imageIndex])
    //                 this.quizQuestionList[qId].answers[aId].images.splice(imageIndex, 1);
    //             break;

    //         default:
    //             break;
    //     }
    //     // this.deleteQuizPart(type + 'file', qId, aId, imageIndex);
    // }
