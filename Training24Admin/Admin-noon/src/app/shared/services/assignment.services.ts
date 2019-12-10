import { Injectable } from '@angular/core';
import { CommonAPIService } from './common.service';

@Injectable()
export class AssignmentService {
    constructor(
        private commonApiService: CommonAPIService,
    ) {

    }

    deleteAssignmentFile(id) {
        return this.commonApiService.delete('v1/Assignment/DeleteAssignmentFile/'+ id);
    }

    getAssignmentById(id?: string) {
        return this.commonApiService.get('v1/Assignment' + (id ? '/' + id : ''));
    }

    getAssignments(filterObj?: any) {
        return this.commonApiService.get('v1/Assignment', filterObj);
    }

    addAssignmentStudent(data) {
        return this.commonApiService.post('v1/Assignment/AddAssignmentStudent', data);
    }

    getAssignmentsByStudentId(id) {
        return this.commonApiService.get('v1/Assignment/GetStudentsByAssignmentId/' + id);
    }

    getAssignedPersonDetails(filterObj?: any) {
        return this.commonApiService.get('v1/Users/getAssignedPersonDetails', filterObj);
    }

    manageAssignment(data: any) {
        // let formData = new FormData();
        // Object.keys(data).forEach(key => {
        //     if (['id'].indexOf(key) < 0) {
        //         // if (key == 'file') {
        //         //     for (let index = 0; index < data['file'].length; index++) {
        //         //         formData.append('fileid', data['file'][index].id);
        //         //     }
        //         // } else
        //         formData.append(key, data[key]);
        //     }
        // });
        if (data.id)
            return this.commonApiService.put('v1/Assignment', data);
        else
            return this.commonApiService.post('v1/Assignment', data);
    }

    deleteAssignment(id) {
        return this.commonApiService.delete('v1/Assignment/' + id);
    }

    deleteAssignmentFromStudent(id) {
        return this.commonApiService.delete('v1/Assignment/DeleteAssignmentStudent/' + id);
    }
}