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
    toggleDeviceStatus(id, userId) {
        return this.commonApiService.putStatus('v1/Device/ChangeuserDeviceStatus/' + userId + '/' + id);
    }
    extensionRequest(filerModal) {
        return this.commonApiService.get('v1/DeviceQuotas/getAllDeviceQuotaExtensionRequest?pagenumber=' + filerModal.pagenumber + '&perpagerecord=' + filerModal.perpagerecord + '&search=' + filerModal.search + '&userId=' + filerModal.userId + '&todate=' + filerModal.todate + '&fromdate=' + filerModal.fromdate)
    }
    AcceptRejectRequest(id, status) {
        return this.commonApiService.putStatus('v1/DeviceQuotas/?extensionRequestId=' + id + '&IsAccepted=' + status)
    }
}