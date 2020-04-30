import { Injectable } from '@angular/core';
import { CommonAPIService } from './common.service';

@Injectable()
export class ChapterService {
    constructor(
        private commonApiService: CommonAPIService,
    ) {

    }

    getChapters(filterObj?: object) {
        return this.commonApiService.get('v1/Chapter', filterObj);
    }

    getChapterById(id?: string) {
        return this.commonApiService.get('v1/Chapter' + (id ? '/' + id : ''));
    }

    manageChapter(data: any) {        
        if (data.id)
            return this.commonApiService.put('v1/Chapter/' + data.id, data);
        else
            return this.commonApiService.post('v1/Chapter', data);
    }

    deleteChapter(id) {
        return this.commonApiService.delete('v1/Chapter/' + id);
    }

    getChapterAssignmentFileSigned(data) {
        let formData = new FormData();
        Object.keys(data).forEach(key => {
            if (['id'].indexOf(key) < 0)
                formData.append(key, data[key]);
        });
       return this.commonApiService.postWithFormData('v1/Files/UploadChapterAssignment', formData);
    }
}