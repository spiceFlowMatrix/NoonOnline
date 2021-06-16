
import { Injectable } from '@angular/core';
import { CommonAPIService } from './common.service';

@Injectable()
export class SalesConfigService {
    constructor(private commonapiservice: CommonAPIService) {
    }
    loginErp() {
        return this.commonapiservice.erppostLogin('Account/Login');
    }
    setCredential(data) {
        this.commonapiservice.setErpCredentials(data);
    }

    getCredentials() {
        return this.commonapiservice.getWithoutHeader('v1/Users/GetErpDetails');
    }
    // getAccountList(token) {
    //     return this.commonapiservice.getErpAccounts('ChartOfAccount/GetAllAccountsByParentId', token);
    // }
    addVoucher(data, token) {
        return this.commonapiservice.postErp('VoucherTransaction/AddVoucherDetail', token, data);
    }
    getJournal(token) {
        return this.commonapiservice.geterp('Code/GetAllJournalDetail', token);
    }
    getOffice(token) {
        return this.commonapiservice.geterp('Code/GetAllOfficeDetails', token);
    }
    getFinanceYear(token) {
        return this.commonapiservice.geterp('Code/GetAllFinancialYearDetail', token);
    }
    getCurrency(token) {
        return this.commonapiservice.geterp('Code/GetAllCurrency', token);
    }
    addTransaction(data, token) {
        return this.commonapiservice.postErp('VoucherTransaction/AddEditTransactionList', token, data);
    }
    getAccountsData() {
        return this.commonapiservice.getWithoutHeader('v1/ErpAccounts/GetAccountsPub');
    }



}
