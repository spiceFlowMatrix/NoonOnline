import { Injectable } from '@angular/core';
import { CommonAPIService } from './common.service';

@Injectable()
export class DeviceService {
    constructor(
        private commonApiService: CommonAPIService,
    ) {

    }

    getDevice(filerModal) {
        return this.commonApiService.get('v1/device/GetAllUserDeviceList?pagenumber=' + filerModal.page + '&perpagerecord=' + filerModal.pageRecord + '&userId=' + filerModal.userId)
    }

    getUserDevice(id) {
        return this.commonApiService.get('v1/device/GetUserDeviceListByUserId?userId=' + id)
    }
    toggleDeviceStatus(id) {
        return this.commonApiService.putStatus('v1/Device/ChaneDeviceStatus/' + id);
    }
    extensionRequest() {
        return this.commonApiService.get('v1/DeviceQuotas/getAllDeviceQuotaExtensionRequest?pagenumber=' + 1 + '&perpagerecord=' + 10 + '&search=' + '' + '&userId=' + 0 + '&todate=' + '' + '&fromdate=' + '')
    }
    AcceptRejectRequest(id, status) {
        return this.commonApiService.putStatus('v1/DeviceQuotas/?extensionRequestId=' + id + '&IsAccepted=' + status)
    }
}