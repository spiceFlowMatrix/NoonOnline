import { Injectable } from '@angular/core';
import { CommonAPIService } from './common.service';

@Injectable()
export class GradeService {
    constructor(
        private commonApiService: CommonAPIService,
    ) {

    }

    getGradeById(id?: string) {
        return this.commonApiService.get('v1/Grade' + (id ? '/' + id : ''));
    }

    getGrades(filterObj?: any) {
        return this.commonApiService.get('v1/Grade', filterObj);
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

    addGradeCourse(dataObj) {
        return this.commonApiService.post('v1/Course/CourseGrade', dataObj);
    }

    deleteCourseFromGrade(id) {
        return this.commonApiService.delete('v1/Course/CourseGrade/' + id);
    }
}