import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { UtilService, QuestionTypeService } from '../../../shared';
import * as _ from 'lodash';

@Component({
    selector: 'app-add-question-type',
    templateUrl: './add-question-type.component.html',
    styleUrls: ['./add-question-type.component.scss']
})
export class AddQuestionTypeComponent implements OnInit {
    private allSubscribers: Array<any> = [];
    @ViewChild('manageQuestionTypeForm') manageQuestionTypeForm: NgForm;
    public questionTypeModel: any = {};
    isEditView: boolean = false;
    isCallingApi: boolean = false;

    constructor(
        private activatedRoute: ActivatedRoute,
        public utilService: UtilService,
        public questionTypeService: QuestionTypeService,
        public router: Router
    ) {
        this.allSubscribers.push(this.activatedRoute.params.subscribe((params: Params) => {
            if (params['id']) {
                this.isEditView = true;
                this.questionTypeModel.id = params['id'];
                this.getQuestionTypeById(this.questionTypeModel.id);
            } else {
                this.questionTypeModel.code = this.utilService.getRandomCode('QT');
                this.isEditView = false;
            }
        }));
    }

    ngOnInit() { }

    getQuestionTypeById(id) {
        this.isCallingApi = true;
        this.allSubscribers.push(this.questionTypeService.getQuestionTypeById(id).subscribe(res => {
            this.isCallingApi = false;
            this.questionTypeModel = _.clone(res.data);
        }, err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err);
        }));
    }

    manageQuestionType() {
        this.isCallingApi = true;
        this.allSubscribers.push(this.questionTypeService.manageQuestionType(this.questionTypeModel).subscribe((res: any) => {
            this.isCallingApi = false;
            this.router.navigate(['/question-types/question-type-list']);
        }, err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err);
        }));
    }

    ngOnDestroy() {
        this.allSubscribers.map(value => value.unsubscribe());
    }
}
