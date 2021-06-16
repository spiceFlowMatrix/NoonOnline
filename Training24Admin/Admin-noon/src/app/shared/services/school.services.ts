import { Injectable } from '@angular/core';
import { CommonAPIService } from './common.service';

@Injectable()
export class SchoolService {
    constructor(
        private commonApiService: CommonAPIService,
    ) {

    }

    getSchoolById(id?: string) {
        return this.commonApiService.get('v1/School' + (id ? '/' + id : ''));
    }

    getSchool(filterObj?: any) {
        return this.commonApiService.get('v1/School', filterObj);
    }

    manageSchool(data: any) {
        if (data.id)
            return this.commonApiService.put('v1/School/' + data.id, data);
        else
            return this.commonApiService.post('v1/School', data);
    }

    deleteSchool(id) {
        return this.commonApiService.delete('v1/School/' + id);
    }
}