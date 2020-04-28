import { Injectable } from '@angular/core';
import { CommonAPIService } from './common.service';
import { Observable, of, throwError } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import * as _ from 'lodash'
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
        if (!filterObj || (filterObj && !filterObj.search)) {
            return of([]);
        }
        return this.commonApiService.get('v1/Files/GetFileList', filterObj).pipe(
            map(response => {
                return response.data;
            }),
            catchError(error => {
                return throwError(error);
            })
        );
    }

    getPdfSigned(data) {
        let formData = new FormData();
        Object.keys(data).forEach(key => {
            if (['id'].indexOf(key) < 0)
                formData.append(key, data[key]);
        });
       return this.commonApiService.postWithFormData('v1/Files/UploadLessonPdf', formData);
    }

    getImageSigned(data) {
        let formData = new FormData();
        Object.keys(data).forEach(key => {
            if (['id'].indexOf(key) < 0)
                formData.append(key, data[key]);
        });
       return this.commonApiService.postWithFormData('v1/Files/UploadLessonPdf', formData);
    }

    getVideoSigned(data) {
        let formData = new FormData();
        Object.keys(data).forEach(key => {
            if (['id'].indexOf(key) < 0)
                formData.append(key, data[key]);
        });
       return this.commonApiService.postWithFormData('v1/Files/UploadLessonPdf', formData);
    }

    putFileOnBucket(url, file) {
        let formData = new FormData();
        formData.append('file', file);
        return this.commonApiService.putWithFormDataUrl(url, formData);
    }

    searchGetFileListNew(filterObj?: any) {
        if (!filterObj || (filterObj && !filterObj.search)) {
            return of([]);
        }
        return this.commonApiService.get('v1/Files/GetFileListNew', filterObj).pipe(
            map(response => {
                return response.data;
            }),
            catchError(error => {
                return throwError(error);
            })
        );;
    }

    multipleFile(data: any) {
        let formData = new FormData();
        for (let i = 0; i < data.file.length; i++) {
            formData.append('file' + i, data.file[i]);
        }
        formData.append('fileTypeId', JSON.stringify(data.fileTypeId));
        formData.append('description', JSON.stringify(data.description));
        formData.append('duration', JSON.stringify(data.duration));
        formData.append('totalpages', JSON.stringify(data.totalpages));

        // Object.keys(data).forEach(key => {
        //     if (['id'].indexOf(key) < 0)
        //         formData.append(key, data[key]);
        // });
        return this.commonApiService.postWithProgress('v1/Files/UploadMultiple', formData);
    }

    manageFile(data: any, id?: string) {
        let formData = new FormData();
        Object.keys(data).forEach(key => {
            if (['id'].indexOf(key) < 0)
                formData.append(key, data[key]);
        });
        if (id)
            return this.commonApiService.putWithProgress('v1/Files/' + id, formData);
        else
            return this.commonApiService.postWithProgress('v1/Files', formData);
    }

    deleteFile(id) {
        return this.commonApiService.delete('v1/Files/' + id);
    }

    uploadCoverImage(data) {
        return this.commonApiService.postWithFormData('v1/Library/AddBookCover', data);
    }
}