import { Injectable } from '@angular/core';
import { CommonAPIService } from './common.service';

@Injectable()
export class BundleService {
    constructor(
        private commonApiService: CommonAPIService,
    ) {

    }

    getBundleById(id?: string) {
        return this.commonApiService.get('v1/Bundle' + (id ? '/' + id : ''));
    }

    getBundles(filterObj) {
        return this.commonApiService.get('v1/Bundle', filterObj);
    }

    manageBundle(data: any) {
        if (data.id)
            return this.commonApiService.put('v1/Bundle/' + data.id, data);
        else
            return this.commonApiService.post('v1/Bundle', data);
    }

    deleteBundle(id) {
        return this.commonApiService.delete('v1/Bundle/' + id);
    }

    getBundleCourse(filterObj, id?: string) {
        return this.commonApiService.get('v1/Bundle/GetCourseByBundleId' + (id ? '/' + id : ''), filterObj);
    }

    addBundleCourse(dataObj) {
        return this.commonApiService.post('v1/Bundle/AddBundleCourse', dataObj);
    }

    deleteCourseFromBundle(id) {
        return this.commonApiService.delete('v1/Bundle/DeleteBundleCourse/' + id);
    }
}