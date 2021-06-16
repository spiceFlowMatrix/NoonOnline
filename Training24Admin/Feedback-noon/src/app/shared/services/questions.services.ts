import { Injectable } from '@angular/core';
import { CommonAPIService } from './common.service';
import { UtilService } from './utils.services';
import * as _ from 'lodash';

@Injectable()
export class QuestionsService {
    constructor(
        private commonApiService: CommonAPIService,
    ) {

    }

    getQuestionsById(id?: string) {
        return this.commonApiService.get('v1/Questions' + (id ? '/' + id : ''));
    }

    getQuestions(filterObj?: any) {
        return this.commonApiService.get('v1/Questions', filterObj);
    }

    manageQuestions(data: any) {
        if (data.id)
            return this.commonApiService.put('v1/Questions/' + data.id, data);
        else
            return this.commonApiService.post('v1/Questions', data);
    }

    deleteQuestions(id) {
        return this.commonApiService.delete('v1/Questions/' + id);
    }

    addQuestionAnswer(data) {
        return this.commonApiService.post('v1/Questions/AddQuestionAnswer', data);
    }

    getQuestionAnswer(id) {
        return this.commonApiService.get('v1/QuestionAnswer/GetAnswersByQuestion/' + id);
    }
}