import { Injectable } from '@angular/core';
import { CommonAPIService } from './common.service';

@Injectable()
export class LessonService {
    constructor(
        private commonApiService: CommonAPIService,
    ) {

    }

    deleteLessonAssignmentFile(id) {
        return this.commonApiService.delete('v1/Lesson/DeleteAssignmentFile/' + id);
    }

    getLessonById(id?: string) {
        return this.commonApiService.get('v1/Lesson' + (id ? '/' + id : ''));
    }

    getLessons(filterObj?: any) {
        return this.commonApiService.get('v1/Lesson', filterObj);
    }

    manageLesson(data: any) {
        // let formData = new FormData();
        // if (data.id) {
        //     delete data.itemorder;
        // }
        // Object.keys(data).forEach(key => {
        //     if (['id'].indexOf(key) < 0) {
        // if (key == 'file') {
        //     for (let index = 0; index < data['file'].length; index++) {
        //         formData.append('fileid', data['file'][index].id);
        //     }
        // } else
        // formData.append(key, data[key]);
        //     }
        // });
        return this.commonApiService.post('v1/Lesson', data);
    }
    updateLesson(data) {
        return this.commonApiService.put('v1/Lesson', data);
    }

    importFiles(fileType) {
        let url = '';
        switch (fileType.id) {
            case "1":
            case 1:
                url = 'ImportPdf';
                break;
            case "2":
            case 2:
                url = 'ImportVideos';
                break;
            default:
                break;
        }
        return this.commonApiService.get('v1/Files/' + url);
    }

    deleteLesson(id) {
        return this.commonApiService.delete('v1/Lesson/' + id);
    }

    getLessonPdfSigned(data) {
        let formData = new FormData();
        Object.keys(data).forEach(key => {
            if (['id'].indexOf(key) < 0)
                formData.append(key, data[key]);
        });
       return this.commonApiService.postWithFormData('v1/Files/UploadLessonPdf', formData);
    }

    getLessonAssignmentFileSigned(data) {
        let formData = new FormData();
        Object.keys(data).forEach(key => {
            if (['id'].indexOf(key) < 0)
                formData.append(key, data[key]);
        });
       return this.commonApiService.postWithFormData('v1/Files/UploadLessonAssignment', formData);
    }

    getLesssonVideoSigned(data) {
        let formData = new FormData();
        Object.keys(data).forEach(key => {
            if (['id'].indexOf(key) < 0)
                formData.append(key, data[key]);
        });
       return this.commonApiService.postWithFormData('v1/Files/UploadLessonVideo', formData);
    }
}