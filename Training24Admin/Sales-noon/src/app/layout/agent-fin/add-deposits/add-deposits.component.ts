import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { UtilService, AgentService, UsersService } from '../../../shared';
import * as _ from 'lodash';
import * as moment from 'moment';
import {NgbCalendar, NgbDateStruct} from '@ng-bootstrap/ng-bootstrap';
@Component({
    selector: 'app-add-deposits',
    templateUrl: './add-deposits.component.html',
    styleUrls: ['./add-deposits.component.scss']
})

export class AddDepositsComponent implements OnInit {
    private allSubscribers: Array<any> = [];
    @ViewChild('manageAgentDepositForm') manageAgentDepositForm: NgForm;
    public agentDepositModel: any = {};
    public model: any = {};
    public agents: any = [];
    isEditView: boolean = false;
    isCallingApi: boolean = false;

    constructor(
        private activatedRoute: ActivatedRoute,
        public utilService: UtilService,
        public agentService: AgentService,
        public router: Router,
        public userService: UsersService,
        private calendar: NgbCalendar
    ) {
        this.allSubscribers.push(this.activatedRoute.params.subscribe((params: Params) => {
            if (params['id']) {
                this.isEditView = true;
                this.agentDepositModel.id = params['id'];
                this.getDepositById(this.agentDepositModel.id);
            } else {
                this.isEditView = false;
            }
        }));
    }

    ngOnInit() {
        this.utilService.allowOnlyNumber('deposit_amount');
        setTimeout(() => {
            this.getAgentById();
        }, 500);
        this.agentDepositModel.depositdate =  this.calendar.getToday();              
    }

    getAgentById() {
        let id = this.utilService.getAccountDetails()['https://noon-online/uid'];
        this.isCallingApi = true
        this.allSubscribers.push(this.userService.getUserById(id).subscribe(res => {
            this.isCallingApi = false;
            this.agentDepositModel.salesagentId = res.data.salesagentId;
        }, err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err);
        }));

    }

    getAgents() {
        this.allSubscribers.push(this.agentService.getSalesAgents({}).subscribe(res => {
            this.isCallingApi = false;
            this.agents = res.data;
        }, err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err);
        }));
    }

    getDepositById(id) {
        this.isCallingApi = true;
        this.allSubscribers.push(this.agentService.getAgentDepositById(id).subscribe(res => {
            this.isCallingApi = false;
            this.agentDepositModel = _.clone(res.data);
            let tempDate = moment(this.agentDepositModel.depositdate);
            this.agentDepositModel.depositdate = {
                'year':tempDate.get('year'),'month':tempDate.get('month'), 'day':tempDate.get('date')
            }
        }, err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err);
        }));
    }

    manageAgentDeposit() {
        this.isCallingApi = true;
        this.allSubscribers.push(this.agentService.manageSalesAgentDeposit(this.agentDepositModel).subscribe((res: any) => {
            this.isCallingApi = false;
            this.router.navigate(['/agent-fin/agent-fin-list']);
        }, err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err);
        }));
    }

    openFileSelecter(id) {
        this.utilService.openFileSelecter(id);
    }

    onFileSelected($event) {
        if (!this.agentDepositModel.documentid)
            this.agentDepositModel.documentid = [];
        for (let index = 0; index < $event.target.files.length; index++) {
            this.agentDepositModel.documentid.push($event.target.files[index]);
        }             
    }

    ngOnDestroy() {
        this.allSubscribers.map(value => value.unsubscribe());
    }
}
