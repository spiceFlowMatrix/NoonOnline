import { Injectable } from '@angular/core';
import { CommonAPIService } from './common.service';

@Injectable()
export class PrivacyService {
    constructor(private commonapiservice: CommonAPIService) {

    }
    managePrivacy(data) {
        return this.commonapiservice.post('v1/Users/UpdateTerms',data);
    }
    getPrivacy() {
        return this.commonapiservice.get('v1/Users/GetTerms');
    }
}