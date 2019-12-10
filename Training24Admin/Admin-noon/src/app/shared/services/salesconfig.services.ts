import { Injectable } from '@angular/core';
import { CommonAPIService } from './common.service';

@Injectable()
export class SalesConfigService {
    constructor(private commonapiservice: CommonAPIService) {

    }
    loginErp() {
        return this.commonapiservice.erppost('Account/Login');
    }

    setCredential(data) {
        this.commonapiservice.setErpCredentials(data);
    }

    getCredentials() {
        return this.commonapiservice.getWithoutHeader('v1/Users/GetErpDetails');
    }

    getAccountList(token) {
        return this.commonapiservice.getErpAccounts('Account/GetAllInputLevelAccountCode', token);
    }
    setAccountsData(data) {
        return this.commonapiservice.putWithoutHeader('v1/ErpAccounts/AddUpdateErpAccounts', data);
    }
    getAccountsData() {
        return this.commonapiservice.getWithoutHeader('v1/ErpAccounts/GetAccountsPub');
    }

}