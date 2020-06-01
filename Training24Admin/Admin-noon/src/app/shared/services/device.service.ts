import { Injectable } from '@angular/core';
import { CommonAPIService } from './common.service';

@Injectable()
export class DeviceService {
    constructor(
        private commonApiService: CommonAPIService,
    ) {

    }

    getDevice() {
        return this.commonApiService.get('v1/device/GetAllUserDeviceList?pagenumber=' + 1 + '&perpagerecord=' + 10 + '&userId=' + 39)
    }

}