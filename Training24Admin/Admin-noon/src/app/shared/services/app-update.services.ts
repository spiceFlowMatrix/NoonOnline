import { Injectable } from '@angular/core';
import { CommonAPIService } from './common.service';

@Injectable()
export class AppUpdateService {
    constructor(private commonapiservice: CommonAPIService) {

    }
    getAppDetail() {
        return this.commonapiservice.get('v1/AppVersion');
    }
    putAppDetail(modal) {
        return this.commonapiservice.put('v1/AppVersion',modal)
    }
}