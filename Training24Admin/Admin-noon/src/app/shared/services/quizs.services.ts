import { Injectable } from '@angular/core';
import { CommonAPIService } from './common.service';

@Injectable()
export class QuizService {
    constructor(
        private commonApiService: CommonAPIService,
    ) {

    }

    getQuizs(filterObj?: any) {
        return this.commonApiService.get('v1/Quiz', filterObj);
    }

    getQuizById(id?: string) {
        return this.commonApiService.get('v1/Quiz' + (id ? '/' + id : ''));
    }

    manageQuiz(data: any) {
        if (data.id) {
            delete data.itemorder;
            return this.commonApiService.put('v1/Quiz/' + data.id, data);
        }
        else
            return this.commonApiService.post('v1/Quiz', data);
    }

    deleteQuiz(id) {
        return this.commonApiService.delete('v1/Quiz/' + id);
    }

    getQuizPreview(id: string) {
        return this.commonApiService.get('v1/Quiz/Quizpriview/' + id);
    }

    getQuizDetail(id: string) {
        return this.commonApiService.get('v1/Questions/getQuizDetail/' + id);
    }

    getQuizQuestions(filterObj, id?: string) {
        return this.commonApiService.get('v1/Questions/GetQuestionsByQuiz' + (id ? '/' + id : ''));
    }

    addQuizQuestion(dataObj) {
        return this.commonApiService.post('v1/Quiz/AddQuizQuestions', dataObj);
    }

    deleteQuizDetail(dataObj) {
        return this.commonApiService.post('v1/Questions/DeleteQuizDetail', dataObj);
    }

    deleteQuizQuestion(id) {
        return this.commonApiService.delete('v1/QuizQuestions/' + id);
    }

    uploadQuizQuestion(data) {
        return this.commonApiService.postWithFormData('v1/Questions/UploadQuizDetail', data);
    }

    getQuizQuestionImageSigned(data) {
        let formData = new FormData();
        Object.keys(data).forEach(key => {
            if (['id'].indexOf(key) < 0)
                formData.append(key, data[key]);
        });
       return this.commonApiService.postWithFormData('v1/Files/UploadQuizImage', formData);
    }

    getQuizAnswerImageSigned(data) {
        let formData = new FormData();
        Object.keys(data).forEach(key => {
            if (['id'].indexOf(key) < 0)
                formData.append(key, data[key]);
        });
       return this.commonApiService.postWithFormData('v1/Files/UploadQuizAnswerImage', formData);
    }
}