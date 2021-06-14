import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { UtilService, AgentService } from '../../../shared';
import * as _ from 'lodash';

@Component({
    selector: 'app-add-agent-category',
    templateUrl: './add-agent-category.component.html',
    styleUrls: ['./add-agent-category.component.scss']
})
export class AddCategoryComponent implements OnInit {
    private allSubscribers: Array<any> = [];
    @ViewChild('manageAgentCategoryForm') manageAgentCategoryForm: NgForm;
    public agentCategoryModel: any = {};    
    isEditView: boolean = false;
    isCallingApi: boolean = false;

    constructor(
        private activatedRoute: ActivatedRoute,
        public utilService: UtilService,
        public agentService: AgentService,
        public router: Router
    ) {
        this.allSubscribers.push(this.activatedRoute.params.subscribe((params: Params) => {
            if (params['id']) {
                this.isEditView = true;
                this.agentCategoryModel.id = params['id'];
                this.getQuestionTypeById(this.agentCategoryModel.id);
            } else {
                this.isEditView = false;
            }
        }));
    }

    ngOnInit() {
        this.utilService.allowOnlyNumber("commission");
    }

    getQuestionTypeById(id) {
        this.isCallingApi = true;
        this.allSubscribers.push(this.agentService.getAgentCategoryById(id).subscribe(res => {
            this.isCallingApi = false;
            this.agentCategoryModel = _.clone(res.data);
        }, err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err);
        }));
    }

    manageQuestionType() {
        this.isCallingApi = true;
        this.allSubscribers.push(this.agentService.manageAgentCategory(this.agentCategoryModel).subscribe((res: any) => {
            this.isCallingApi = false;
            this.router.navigate(['/agents/agents-list']);
        }, err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err);
        }));
    }

    ngOnDestroy() {
        this.allSubscribers.map(value => value.unsubscribe());
    }

}
