import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { UtilService, AgentService, FileService } from '../../../shared';
import * as _ from 'lodash';
import * as moment from 'moment';
import { NgbCalendar, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { HttpEventType } from '@angular/common/http';
@Component({
    selector: 'app-add-deposit',
    templateUrl: './add-deposit.component.html',
    styleUrls: ['./add-deposit.component.scss']
})

export class AddDepositComponent implements OnInit {
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
        public fileService:FileService,
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
            this.getAgents();
        }, 500);
        this.agentDepositModel.depositdate = this.calendar.getToday();
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
                'year': tempDate.get('year'), 'month': tempDate.get('month'), 'day': tempDate.get('date')
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
            this.router.navigate(['/agent-finances/agent-finances-list']);
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
          if(!this.agentDepositModel.filename) {
                    this.agentDepositModel.filename = []
                }
        for (let index = 0; index < $event.target.files.length; index++) {
            this.isCallingApi = true;
            let modal = {
                fileTypeId: 1,
                contentType: $event.target.files[index].type,
                fileName: $event.target.files[index].name
            }
            let fileModal = {
                totalpages: "",
                fileTypeId: "",
                filename: ""
            }
            let reader: any = new FileReader();
            reader.readAsBinaryString($event.target.files[index]);
            reader.onloadend = () => {
                var count = reader.result.match(/\/Type[\s]*\/Page[^s]/g).length;
                fileModal.totalpages = count.toString();
                fileModal.fileTypeId = "1";
            }
            this.allSubscribers.push(this.agentService.getSupportDocumentFileSigned(modal).subscribe((res) => {
                fileModal.filename = res.data.filename;
                this.agentDepositModel.filename.push(res.data.filename);
                this.agentDepositModel.documentid.push($event.target.files[index]);
                let signUrl = res.data.signedurl;
                this.fileService.putFileOnBucket(signUrl, $event.target.files[index]).subscribe((res: any) => {
                    switch (res.type) {
                        case HttpEventType.Response:
                            this.isCallingApi = false;

                    }
                }, err => {
                    this.isCallingApi = false;
                    this.utilService.showErrorCall(err);
                })
            }))
        }
    }

    ngOnDestroy() {
        this.allSubscribers.map(value => value.unsubscribe());
    }
}
