import { Injectable } from '@angular/core';
import { CommonAPIService } from './common.service';
@Injectable()
export class CourseService {
    courseList
    constructor(
        private commonApiService: CommonAPIService) {

    }

    getCourses(filterObj) {
        return this.commonApiService.get('v1/Course', filterObj);
    }

    getCourseDefinitions(filterObj) {
        return this.commonApiService.get('v1/CourseDefination', filterObj);
    }

    getCourseDefinitionById(id?: string) {
        return this.commonApiService.get('v1/CourseDefination' + (id ? '/' + id : ''));
    }

    manageCourseDefinition(data: any, id?: any) {
        if (id) {
            return this.commonApiService.put('v1/CourseDefination/' + id, data);
        } else
            return this.commonApiService.post('v1/CourseDefination', data);
    }

    deleteCourseDefinition(id) {
        return this.commonApiService.delete('v1/CourseDefination/' + id);
    }

    getDiscountById(id?: string) {
        return this.commonApiService.get('v1/Discount' + (id ? '/' + id : ''));
    }

    getDiscounts(filterObj?: any) {
        return this.commonApiService.get('v1/Discount', filterObj);
    }

    manageDiscount(data: any) {
        return this.commonApiService.post('v1/Discount', data);
    }

    deleteDiscount(id) {
        return this.commonApiService.delete('v1/Discount/' + id);
    }

}