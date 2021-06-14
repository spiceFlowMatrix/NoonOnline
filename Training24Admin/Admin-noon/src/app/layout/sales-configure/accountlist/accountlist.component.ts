import { Component, OnInit, ViewChild } from "@angular/core";
import { UtilService } from "../../../shared";
import { SalesConfigService } from "../../../shared/services/salesconfig.services";
@Component({
    selector: "app-accountlist",
    templateUrl: "./accountlist.component.html",
    styleUrls: ["./accountlist.component.scss"]
})
export class AccountListComponent implements OnInit {
    AccountList: any = [];
    allSubscribers: Array<any> = [];
    isCallingApi: boolean = false;
    erpToken: any;
    taxexpense: '';
    taxpayable: '';
    asset: '';
    comission: '';
    income: ''



    constructor(
        public utilService: UtilService,
        public salesConfigService: SalesConfigService
    ) {
    }
    ngOnInit() {
        this.getErpCredentials();

    }

    getErpCredentials() {
        this.isCallingApi = true;
        this.allSubscribers.push(this.salesConfigService.getCredentials().subscribe((res) => {
            this.salesConfigService.setCredential(res.data);
            this.isCallingApi = false;
            this.loginInErp();
        }, err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err);
        }))
    }

    loginInErp() {
        this.isCallingApi = true;
        this.allSubscribers.push(this.salesConfigService.loginErp().subscribe((res) => {
            this.erpToken = res.data.Token;
            this.isCallingApi = false;
            this.getAccountList();
        }, err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err);
        }))
    }

    getAccountList() {
        this.isCallingApi = true;
        this.allSubscribers.push(this.salesConfigService.getAccountList(this.erpToken).subscribe((res) => {
            this.AccountList = res.data.AccountDetailList;
            this.isCallingApi = false;
            this.getAccountData();
        }, err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err);
        }))
    }

    getAccountData() {
        this.isCallingApi = true;
        this.allSubscribers.push(this.salesConfigService.getAccountsData().subscribe((res) => {
            for (let i = 0; i < res.data.length; i++) {
                if (res.data[i].type == 1) {
                    this.income = res.data[i].accountcode;
                }
                if (res.data[i].type == 2) {
                    this.comission = res.data[i].accountcode;
                }
                if (res.data[i].type == 3) {
                    this.asset = res.data[i].accountcode;
                }
                if (res.data[i].type == 4) {
                    this.taxpayable = res.data[i].accountcode;
                }
                if (res.data[i].type == 5) {
                    this.taxexpense = res.data[i].accountcode;
                }
            }
            this.isCallingApi = false;
        }, err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err);
        }))
    }

    manageAccount() {
        this.isCallingApi = true;
        let modal = [
            {
                type: 1,
                accountcode: this.income
            },
            {
                type: 2,
                accountcode: this.comission
            },
            {
                type: 3,
                accountcode: this.asset
            },
            {
                type: 4,
                accountcode: this.taxpayable
            },
            {
                type: 5,
                accountcode: this.taxexpense
            }
        ]
        this.allSubscribers.push(this.salesConfigService.setAccountsData(modal).subscribe((res) => {
            this.isCallingApi = false;
        }, err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err)
        }))

    }

    ngOnDestroy() {
        this.allSubscribers.map(value => value.unsubscribe());
    }
}
