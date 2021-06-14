import { ViewChild, Component, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { UtilService, UsersService } from '../../shared';

@Component({
    selector: 'app-dashboard',
    templateUrl: './dashboard.component.html',
    styleUrls: ['./dashboard.component.scss'],
})
export class DashboardComponent implements OnInit {
    @ViewChild('manageDefaultsForm') manageDefaultsForm: any;
    @ViewChild('manageDefaultsDialog') manageDefaultsDialog: any;

    public modalRef: any = null;
    public isCallingApi: boolean;
    public defaultsModel: any = {};
    public allSubscribers: any = [];

    constructor(
        public utilService: UtilService,
        public usersService: UsersService,
        public modalService: NgbModal
    ) { }

    ngOnInit() { }

    openDefaultsModal() {
        this.modalRef = this.modalService.open(this.manageDefaultsDialog, {
            backdrop: 'static',
            size: 'lg'
        });
        this.defaultsModel = {};
        this.getDefaultValue();
    }

    getDefaultValue() {
        this.isCallingApi = true;
        this.allSubscribers.push(this.usersService.getDefaults().subscribe(res => {
            this.isCallingApi = false;
            this.defaultsModel = res.data;
        }, err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err);
        }));
    }

    manageDefaults() {
        this.isCallingApi = true;
        this.allSubscribers.push(this.usersService.manageDefaults(this.defaultsModel).subscribe(res => {
            this.isCallingApi = false;
            this.modalRef.dismiss();
        }, err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err);
        }));
    }
}
