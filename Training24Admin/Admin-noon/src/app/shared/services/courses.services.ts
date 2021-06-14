import { Injectable } from '@angular/core';
import { CommonAPIService } from './common.service';
import { DataService } from './data.services';
import { UtilService } from './utils.services';
@Injectable()
export class CourseService {
    courseList
    constructor(
        private commonApiService: CommonAPIService,
        private dataService: DataService,
        private utilService: UtilService
    ) {

    }

    getCourses(filterObj) {
        return this.commonApiService.get('v1/Course', filterObj);
    }

    getMiniCourseslist(filterObj?: any) {
        return this.commonApiService.get('v1/Course/GetCourseList', filterObj);
    }

    getCourseList() {
        return new Promise((resolve, reject) => {
            if (this.dataService.getCourseList() && this.dataService.getCourseList().length > 0) {
                resolve(this.dataService.getCourseList());
            } else {
                this.getMiniCourseslist({}).subscribe(res => {
                    this.dataService.coursesList = res.data;
                    resolve(this.dataService.coursesList)
                }, err => {
                    this.utilService.showErrorCall(err);
                    reject(err);
                });
            }
        });
    }

getCourseDefination(subname?: string ) {
    if(subname == undefined) {
    return this.commonApiService.get('v1/CourseDefination/GetSubject' )
    }
    else {
    return this.commonApiService.get('v1/CourseDefination/GetSubject?search=' + subname)
    }
}

    getCourseById(id?: string) {
        return this.commonApiService.get('v1/Course' + (id ? '/' + id : ''));
    }

    manageCourse(data: any, id?: any) {
        let formData = new FormData();
        Object.keys(data).forEach(key => {
            if (['id', 'image'].indexOf(key) < 0)
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
        if (dataObj.id)
            return this.commonApiService.put('v1/UserCourse/' + dataObj.id, dataObj);
        else
            return this.commonApiService.post('v1/UserCourse', dataObj);
    }

    deleteCourseFromUser(id) {
        return this.commonApiService.delete('v1/UserCourse/' + id);
    }

    coursePreview(id) {
        return this.commonApiService.get('v1/Course/CoursePriview/' + id);
    }

    courseUsersPreview(id) {
        return this.commonApiService.get('v1/Users/getAssignedPersonDetails?search=' + id + '&pagenumber=1&perpagerecord=1');
    }

    ChapterOrderChange(data) {
        return this.commonApiService.post('v1/Chapter/ChapterOrderChange', data);
    }

    lessonOrderChange(data) {
        return this.commonApiService.post('v1/Lesson/LessonOrderChange', data);
    }

    getCourseCardSigned(data) {
        let formData = new FormData();
        Object.keys(data).forEach(key => {
            if (['id'].indexOf(key) < 0)
                formData.append(key, data[key]);
        });
       return this.commonApiService.postWithFormData('v1/Files/UploadCourseCardImage', formData);
    }
}