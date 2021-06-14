import { Injectable } from '@angular/core';
import { CommonAPIService } from './common.service';
import { UtilService } from './utils.services';
import * as _ from 'lodash';

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
        if (data.id)
            return this.commonApiService.put('v1/Quiz/' + data.id, data);
        else
            return this.commonApiService.post('v1/Quiz', data);
    }

    deleteQuiz(id) {
        return this.commonApiService.delete('v1/Quiz/' + id);
    }

    getQuizQuestions(filterObj, id?: string) {
        return this.commonApiService.get('v1/Questions/GetQuestionsByQuiz' + (id ? '/' + id : ''));
    }

    addQuizQuestion(dataObj) {
        return this.commonApiService.post('v1/Quiz/AddQuizQuestions', dataObj);
    }

    deleteQuizQuestion(id) {
        return this.commonApiService.delete('v1/QuizQuestions/' + id);
    }
}