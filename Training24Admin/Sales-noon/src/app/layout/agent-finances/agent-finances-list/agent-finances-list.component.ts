import { Component, OnInit, ViewChild } from '@angular/core';
import * as _ from 'lodash';
import { DELETE_TITLE, DELETE_MESSAGE, UtilService, AgentService } from '../../../shared';
import { HttpClient } from '@angular/common/http';

@Component({
    selector: 'app-agent-finances-list',
    templateUrl: './agent-finances-list.component.html',
    styleUrls: ['./agent-finances-list.component.scss']
})
export class AgentFinancesListComponent implements OnInit {
    @ViewChild('listdialog') listCommonDialog: any;
    panels: string[] = ['agent_account_desposits', 'agent_account_summary'];
    agffilterModel: any = {};
    agclfilterModel: any = {};
    isCallingApi: boolean;
    agentDepositList: Array<any> = [];
    agentSummeryList: Array<any> = [];
    allSubscribers: Array<any> = [];

    constructor(
        public utilService: UtilService,
        public agentService: AgentService,
        public http: HttpClient
    ) { }

    ngOnInit() {
        this.agffilterModel = {
            pagenumber: 1,
            perpagerecord: 10,
            totalCount: 0
        }
        this.agclfilterModel = _.clone(this.agffilterModel);
        this.getAgent(this.agffilterModel);
        setTimeout(() => {
            this.getAgentSummeryList();
        }, 800);
    }

    openDeleteConfirmation(agent, index) {
        agent.index = index;
        this.listCommonDialog.open(1, DELETE_TITLE, DELETE_MESSAGE, agent);
    }

    search($event) {
        this.agffilterModel.search = $event.value;
        if (!this.agffilterModel.search)
            delete this.agffilterModel.search;
        this.getAgent(this.agffilterModel);
    }

    onPageChange(event, type) {
        // if (type == 'agf') {
        this.agffilterModel.pagenumber = event;
        this.getAgent(this.agffilterModel);
        // } 
    }

    getAgent(filterObj) {
        this.isCallingApi = true;
        this.allSubscribers.push(this.agentService.getAgentDeposit(filterObj).subscribe(res => {
            this.isCallingApi = false;
            this.agentDepositList = res.data;
            this.agffilterModel.totalCount = res.totalcount;
        }, err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err);
        }));
    }

    updateSatus(index, key) {
        let model = _.clone(this.agentDepositList[index]);
        model = this.toggleDepositStatus(model, key);
        this.isCallingApi = true;
        this.allSubscribers.push(this.agentService.manageSalesAgentDeposit(model).subscribe(res => {
            this.isCallingApi = false;
            this.agentDepositList[index] = this.toggleDepositStatus(model, key);
            this.getAgentSummeryList();
        }, err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err);
        }));
    }

    toggleDepositStatus(model, key) {
        if (key == 'isrevoke') {
            model.isrevoke = true;
            model.isconfirm = false;
        } else {
            model.isrevoke = false;
            model.isconfirm = true;
        }
        return model;
    }

    downloadFiles(documentid) {
        for (let index = 0; index <= documentid.length; index++) {
           this.utilService.downloadFile(documentid[index].documenturl);
        }
    }

    getAgentSummeryList() {
        this.isCallingApi = true
        this.allSubscribers.push(this.agentService.getAccountSummary().subscribe(res => {
            this.isCallingApi = false;
            this.agentSummeryList = res.data;
        }, err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err);
        }));

    }

    ngOnDestroy() {
        this.allSubscribers.map(value => value.unsubscribe());
    }

}
