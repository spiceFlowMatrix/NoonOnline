import { Injectable } from '@angular/core';
import { CommonAPIService } from './common.service';
import { Observable, of, throwError } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
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
    searchFiles(filterObj?: any) {
        if (!filterObj) {
            return of([]);
        }
        return this.commonApiService.get('v1/Files', filterObj).pipe(
            map(response => {
                return response.data;
            }),
            catchError(error => {
                return throwError(error);
            })
        );;
    }

    putFileOnBucket(url, file) {
        return this.commonApiService.putWithFormDataUrl(url, file);
    }
    
    SaveFileMetaData(data) {
        let formData = new FormData();
        Object.keys(data).forEach(key => {
            if (['id'].indexOf(key) < 0)
                formData.append(key, data[key]);
        });
        return this.commonApiService.postWithFormData('v1/Files/SaveFileMetaData', formData);
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