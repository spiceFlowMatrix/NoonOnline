import { Injectable } from '@angular/core';
import { CommonAPIService } from './common.service';

@Injectable()
export class CsvService {
    constructor(private commonapiservice: CommonAPIService) {

    }
    manageCsvFiles(file) {
        let formData = new FormData();
        formData.append('file',file)
        return this.commonapiservice.postWithFormData('v1/Upload/ImportLessonCSV', formData);
    }
    manageAppTrackCsv(file) {
        let formData = new FormData();
        formData.append('file',file)
        return this.commonapiservice.postWithFormData('v1/ProgessSync/ImportAppTimeCSV', formData);
    }
}