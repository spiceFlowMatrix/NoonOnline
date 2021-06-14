import { Injectable } from '@angular/core';
import { CommonAPIService } from './common.service';
import { UtilService } from './utils.services';
import * as _ from 'lodash';

@Injectable()
export class QuestionTypeService {
    constructor(
        private commonApiService: CommonAPIService,
    ) {

    }

    getQuestionTypeById(id?: string) {
        return this.commonApiService.get('v1/QuestionType' + (id ? '/' + id : ''));
    }

    getQuestionTypes(filterObj?: any) {
        return this.commonApiService.get('v1/QuestionType', filterObj);
    }

    manageQuestionType(data: any) {
        if (data.id)
            return this.commonApiService.put('v1/QuestionType/' + data.id, data);
        else
            return this.commonApiService.post('v1/QuestionType', data);
    }

    deleteQuestionType(id) {
        return this.commonApiService.delete('v1/QuestionType/' + id);
    }
}