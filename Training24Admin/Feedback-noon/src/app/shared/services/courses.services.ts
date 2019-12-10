import { Injectable } from '@angular/core';
import { CommonAPIService } from './common.service';
import * as _ from 'lodash';

@Injectable()
export class CourseService {
    constructor(
        private commonApiService: CommonAPIService,
    ) {

    }

    getCourses(filterObj) {
        return this.commonApiService.get('v1/Course', filterObj);
    }

    getCourseById(id?: string) {
        return this.commonApiService.get('v1/Course' + (id ? '/' + id : ''));
    }

    manageCourse(data: any, id?: any) {
        let formData = new FormData();
        Object.keys(data).forEach(key => {
            if (['id', 'profilepicurl'].indexOf(key) < 0)
                formData.append(key, data[key]);
        });
        if (id) {
            return this.commonApiService.putWithFormData('v1/Course/' + id, formData);
        } else
            return this.commonApiService.postWithFormData('v1/Course', formData);
    }

    deleteCourse(id) {
        return this.commonApiService.delete('v1/Course/' + id);
    }

    getUserCourse(filterObj, id?: string) {
        return this.commonApiService.get('v1/UserCourse/GetCoursesByUserId' + (id ? '/' + id : ''), filterObj);
    }

    addUserCourse(dataObj) {
        return this.commonApiService.post('v1/UserCourse', dataObj);
    }

    deleteCourseFromUser(id) {
        return this.commonApiService.delete('v1/UserCourse/' + id);
    }
}