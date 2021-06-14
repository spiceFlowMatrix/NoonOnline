import { Injectable } from '@angular/core';
import { CommonAPIService } from './common.service';
import * as _ from 'lodash';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
@Injectable()
export class FeedbackService {
    constructor(
        private commonApiService: CommonAPIService,
        public httpClient: HttpClient
    ) {

    }

    getFeedbackById(id?: string) {
        return this.commonApiService.get('v1/TaskFeedBack/GetTaskFeedBackDetailsById?Id=' + id);
    }

    getFeedbacks(filterObj?: any) {
        // return this.commonApiService.get('v1/Feedback/GetAllFeedBack', filterObj);
        return this.commonApiService.post('v1/Feedback/GetAllFeedBackTest', filterObj);
    }

    addContactDetail(data: any) {
        return this.commonApiService.post('v1/Feedback/AddContact', data);
    }

    addFeedback(data: any) {
        return this.commonApiService.post('v1/Feedback/AddFeedback', data);
    }

    uploadFeedBackTime(data: any) {
        return this.commonApiService.postWithFormData('v1/Feedback/AddFeedbackTime', data);
    }

    manageFeedback(data: any) {
        if (data.id)
            return this.commonApiService.putWithFormData('Feedback/' + data.id, data);
        else
            return this.commonApiService.postWithFormData('Feedback/', data);
    }

    deleteFeedback(id) {
        return this.commonApiService.delete('v1/Feedback/' + id);
    }

    getFeedbackCourse(filterObj, id?: string) {
        return this.commonApiService.get('v1/Course/GetCourseByFeedbackId' + (id ? '/' + id : ''), filterObj);
    }

    getLessions(filterObj, id?: string) {
        return this.commonApiService.get('v1/Lesson/getLessonByChapterId' + (id ? '/' + id : ''), filterObj);
    }

    addFeedbackCourse(dataObj) {
        return this.commonApiService.post('v1/Course/CourseFeedback', dataObj);
    }

    deleteCourseFromFeedback(id) {
        return this.commonApiService.delete('v1/Course/CourseFeedback/' + id);
    }

    assignPerson(dataObj) {
        return this.commonApiService.post('v1/FeedBackStaff', dataObj);
    }

    updateTaskStatusPerson(dataObj, id) {
        return this.commonApiService.post('v1/FeedBackTask/ChangeTaskStatus/' + id, dataObj);
    }

    removeStaff(dataObj) {
        return this.commonApiService.post('v1/FeedBackStaff/RemoveStaff', dataObj);
    }

    addTasks(dataObj) {
        if (dataObj.id)
            return this.commonApiService.put('v1/FeedBackTask', dataObj, dataObj.id);
        else
            return this.commonApiService.post('v1/FeedBackTask', dataObj);
    }

    changeFeedbackStatus(dataObj) {
        return this.commonApiService.post('v1/Feedback/FeedbackStatusChanged', dataObj);
    }

    getImage(image) {
        return this.commonApiService.getImage(image);
    }
    getaudio(audio) {
        return this.commonApiService.getAudio(audio);
    }

    getQueueList(filterObj) {
        return this.commonApiService.post('v1/TaskFeedBack/GetInQueueList', filterObj)
    }
    getProgressList(filterObj) {
        return this.commonApiService.post('v1/TaskFeedBack/GetInProcessList', filterObj)
    }
    getCompleteList(filterObj) {
        return this.commonApiService.post('v1/TaskFeedBack/GetCompletedList', filterObj)
    }
    getArchiveList(filterObj) {
        return this.commonApiService.post('v1/TaskFeedBack/GetArchivedList', filterObj)
    }
    moveToProgress(ids) {
        return this.commonApiService.post('v1/TaskFeedBack/MoveInProgress', ids);
    }
    assignUser(fid, uid) {
        return this.commonApiService.get('v1/TaskFeedBack/AssignUserTask?taskid=' + fid + '&userid=' + uid);
    }
    getCategory() {
        return this.commonApiService.get('v1/TaskFeedBack/GetFeedbackCategoryList');
    }
    getFeedUser() {
        return this.commonApiService.get('v1/TaskFeedBack/GetFeedbackUsersList');
    }
    moveToCompleted(ids) {
        return this.commonApiService.post('v1/TaskFeedBack/MoveInCompleted', ids);
    }
    moveInArchived(ids) {
        return this.commonApiService.post('v1/TaskFeedBack/MoveInArchived', ids);
    }



}