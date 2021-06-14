import { Component, OnInit, ViewChild } from '@angular/core';
import * as _ from 'lodash';
import { DELETE_TITLE, DELETE_MESSAGE, UtilService, AgentService } from '../../../shared';

@Component({
    selector: 'app-agents-list',
    templateUrl: './agents-list.component.html',
    styleUrls: ['./agents-list.component.scss']
})
export class AgentListComponent implements OnInit {
    @ViewChild('listdialog') listCommonDialog: any;
    panels: string[] = ['sales_agents_panel', 'agents_category_panel'];
    aglfilterModel: any = {};
    agclfilterModel: any = {};
    isCallingApi: boolean;
    agentList: Array<any> = [];
    allSubscribers: Array<any> = [];
    agentCategoryList: Array<any> = [];
    constructor(
        public utilService: UtilService,
        public agentService: AgentService
    ) { }

    ngOnInit() {
        this.aglfilterModel = {
            pagenumber: 1,
            perpagerecord: 10,
            totalCount: 0
        }
        this.agclfilterModel = _.clone(this.aglfilterModel);
        this.getAgent(this.aglfilterModel);
        this.getAgentCategory(this.agclfilterModel);
    }

    openDeleteConfirmation(agent, index) {
        agent.index = index;
        this.listCommonDialog.open(1, DELETE_TITLE, DELETE_MESSAGE, agent);
    }

    search($event) {
        this.aglfilterModel.search = $event.value;
        if (!this.aglfilterModel.search)
            delete this.aglfilterModel.search;
        this.getAgent(this.aglfilterModel);
    }

    onPageChange(event, type) {
        if (type == 'agl') {
            this.aglfilterModel.pagenumber = event;
            this.getAgent(this.aglfilterModel);
        } else if (type = 'agcl') {
            this.agclfilterModel.pagenumber = event;
            this.getAgentCategory(this.agclfilterModel);
        }
    }

    getAgent(filterObj) {
        this.isCallingApi = true;
        this.allSubscribers.push(this.agentService.getSalesAgents(filterObj).subscribe(res => {
            this.isCallingApi = false;
            this.agentList = res.data;
            this.aglfilterModel.totalCount = res.totalcount;
        }, err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err);
        }));
    }

    deleteAgent(data) {
        this.allSubscribers.push(this.agentService.deleteSalesAgent(data.salesAgentId).subscribe(res => {
            this.agentList = _.remove(this.agentList, (o) => {
                return !(o.salesAgentId == data.salesAgentId);
            });
        }, err => {
            this.utilService.showErrorCall(err);
        }));
    }

    getAgentCategory(filterObj) {
        this.isCallingApi = true;
        this.allSubscribers.push(this.agentService.getAgentCategory(filterObj).subscribe(res => {
            this.isCallingApi = false;
            this.agentCategoryList = res.data;
            this.agclfilterModel.totalCount = res.totalcount;
        }, err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err);
        }));
    }
    ngOnDestroy() {
        this.allSubscribers.map(value => value.unsubscribe());
    }

}
