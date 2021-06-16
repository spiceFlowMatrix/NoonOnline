import { Injectable } from '@angular/core';
import { CommonAPIService } from './common.service';

@Injectable()
export class TimeoutService {
    constructor(private commonapiservice: CommonAPIService) {

    }
    manageTimeout(data) {
        return this.commonapiservice.put('v1/TimeInterval/UpdateInterval', data);
    }
    getTimeout() {
        return this.commonapiservice.get('v1/TimeInterval/GetInterval')
    }
    managePrivacy(data) {
        return this.commonapiservice.post('v1/Users/UpdateTerms',data);
    }
    getPrivacy() {
        return this.commonapiservice.get('v1/Users/GetTerms');
    }
}