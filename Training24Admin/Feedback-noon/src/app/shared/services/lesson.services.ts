import { Injectable } from '@angular/core';
import { CommonAPIService } from './common.service';
import { UtilService } from './utils.services';
import * as _ from 'lodash';

@Injectable()
export class LessonService {
    constructor(
        private commonApiService: CommonAPIService,
    ) {

    }

    getLessonById(id?: string) {
        return this.commonApiService.get('v1/Lesson' + (id ? '/' + id : ''));
    }

    getLessons(filterObj?: any) {
        return this.commonApiService.get('v1/Lesson', filterObj);
    }

    manageLesson(data: any) {
        let formData = new FormData();
        Object.keys(data).forEach(key => {
            if (['id'].indexOf(key) < 0) {
                if (key == 'file') {
                    for (let index = 0; index < data['file'].length; index++) {
                        formData.append('fileid', data['file'][index].id);
                    }
                } else
                    formData.append(key, data[key]);
            }
        });
        if (data.id)
            return this.commonApiService.putWithFormData('v1/Lesson/' + data.id, formData);
        else
            return this.commonApiService.postWithFormData('v1/Lesson', formData);
    }

    deleteLesson(id) {
        return this.commonApiService.delete('v1/Lesson/' + id);
    }
}