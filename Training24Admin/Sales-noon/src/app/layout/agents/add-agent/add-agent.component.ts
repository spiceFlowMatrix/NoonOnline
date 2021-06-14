import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { UtilService, AgentService, EmailPattern, CurrencyLists } from '../../../shared';
import * as _ from 'lodash';

@Component({
    selector: 'app-add-agent',
    templateUrl: './add-agent.component.html',
    styleUrls: ['./add-agent.component.scss']
})
export class AddAgentComponent implements OnInit {
    private allSubscribers: Array<any> = [];
    @ViewChild('manageAgentForm') manageAgentForm: NgForm;
    public agentModel: any = {};
    public categorylist: any = [];
    public currencyLists: any = [];
    isEditView: boolean = false;
    isCallingApi: boolean = false;
    emailPattern: string = '';

    constructor(
        private activatedRoute: ActivatedRoute,
        public utilService: UtilService,
        public agentService: AgentService,
        public router: Router
    ) {
        this.agentModel.roles = [{roleid: 17}];
        this.agentModel.addedfrom = "sales";
        this.agentModel.isActive = true;
        this.allSubscribers.push(this.activatedRoute.params.subscribe((params: Params) => {
            if (params['id']) {
                this.isEditView = true;
                this.agentModel.salesAgentId = params['id'];
                this.getSalesAgentById(this.agentModel.salesAgentId);
            } else {
                this.isEditView = false;
            }
        }));
    }

    ngOnInit() {
        this.currencyLists = CurrencyLists;
        this.emailPattern = EmailPattern;
        this.utilService.allowOnlyNumber("phone");
        this.utilService.allowOnlyNumber("agreedMonthlyDeposit");
        this.getAgentCategory();
    }

    getAgentCategory() {
        this.allSubscribers.push(this.agentService.getAgentCategory({}).subscribe(res => {
            this.categorylist = res.data;
        }, err => {
            this.utilService.showErrorCall(err);
        }));
    }

    getSalesAgentById(id) {
        this.isCallingApi = true;
        this.allSubscribers.push(this.agentService.getSalesAgentById(id).subscribe(res => {
            this.isCallingApi = false;
            this.agentModel = _.clone(res.data);
            this.agentModel.roles = [{roleid: 17}];
        }, err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err);
        }));
    }

    manageAgents() {
        this.isCallingApi = true;
        this.allSubscribers.push(this.agentService.manageSalesAgent(this.agentModel).subscribe((res: any) => {
            this.isCallingApi = false;
            this.router.navigate(['/agents/agents-list']);
        }, err => {
            this.isCallingApi = false;
            if(err.error.Password[0] === "PasswordIsRequired") {
                this.utilService.showPasswordToast();
            }else {
                this.utilService.showErrorCall(err);
            }
        }));
    }

    ngOnDestroy() {
        this.allSubscribers.map(value => value.unsubscribe());
    }

}
