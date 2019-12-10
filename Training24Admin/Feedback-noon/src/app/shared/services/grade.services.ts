import { Injectable } from '@angular/core';
import { CommonAPIService } from './common.service';
import { UtilService } from './utils.services';
import * as _ from 'lodash';

@Injectable()
export class GradeService {

    constructor(
        private commonApiService: CommonAPIService,
        private utilService: UtilService,
    ) {

    }

    getGradeById(id?: string) {
        return this.commonApiService.get('v1/Grade' + (id ? '/' + id : ''));
    }

    getGrades(filterObj?: any) {
        return this.commonApiService.get('v1/Grade', filterObj);
    }

    getGradeList(filterObj?: any) {
        return new Promise((resolve, reject) => {
            if (this.utilService.getGrades() && this.utilService.getGrades().length > 0) {
                resolve(this.utilService.getGrades());
            } else {
                this.getGrades(filterObj).subscribe(res => {
                    this.utilService.setGrades(res.data);
                    resolve(res.data);
                }, err => {
                    this.utilService.showErrorCall(err);
                    reject(err);
                });
            }
        });
    }

    manageGrade(data: any) {
        if (data.id)
            return this.commonApiService.put('v1/Grade/' + data.id, data);
        else
            return this.commonApiService.post('v1/Grade', data);
    }

    deleteGrade(id) {
        return this.commonApiService.delete('v1/Grade/' + id);
    }

    getGradeCourse(filterObj, id?: string) {
        return this.commonApiService.get('v1/Course/GetCourseByGradeId' + (id ? '/' + id : ''), filterObj);
    }

    getLessions(filterObj, id?: string) {
        return this.commonApiService.get('v1/Lesson/GetLessonByCourseId' + (id ? '/' + id : ''), filterObj);
    }
    getChapters(filterObj, id?: string) {
        return this.commonApiService.get('v1/Chapter/GetChapterByCourseId' + (id ? '/' + id : ''), filterObj);
    }

    addGradeCourse(dataObj) {
        return this.commonApiService.post('v1/Course/CourseGrade', dataObj);
    }

    deleteCourseFromGrade(id) {
        return this.commonApiService.delete('v1/Course/CourseGrade/' + id);
    }
}