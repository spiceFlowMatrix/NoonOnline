import { Injectable } from '@angular/core';
import { CommonAPIService } from './common.service';

@Injectable()
export class FileService {
    constructor(
        private commonApiService: CommonAPIService,
    ) {

    }

    getFileById(id?: string) {
        return this.commonApiService.get('v1/Files' + (id ? '/' + id : ''));
    }

    getFiles(filterObj?: any) {
        return this.commonApiService.get('v1/Files', filterObj);
    }

    manageFile(data: any, id?: string) {
        let formData = new FormData();
        Object.keys(data).forEach(key => {
            if (['id'].indexOf(key) < 0)
                formData.append(key, data[key]);
        });
        if (id)
            return this.commonApiService.putWithFormData('v1/Files/' + id, formData);
        else
            return this.commonApiService.postWithFormData('v1/Files', formData);
    }

    deleteFile(id) {
        return this.commonApiService.delete('v1/Files/' + id);
    }
}