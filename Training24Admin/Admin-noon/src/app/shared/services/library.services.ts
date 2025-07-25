import { Injectable } from '@angular/core';
import { CommonAPIService } from './common.service';

@Injectable()
export class LibraryService {
    constructor(
        private commonApiService: CommonAPIService,
    ) {

    }

    getBooks(filter) {
        return this.commonApiService.get('v1/Library/GetBooks', filter);
    }

    getBookById(id) {
        return this.commonApiService.get('v1/Library/GetBookById'+  (id ? '/' + id : ''));
    }

    manageBooks(data) {
        if (data.id)
                return this.commonApiService.put('v1/Library/UpdateBook' , data);
            else
                return this.commonApiService.post('v1/Library/AddBook', data);
    }
    deleteBooks(id) {
        return this.commonApiService.delete('v1/Library/DeleteBook/' + id);
    }
    publishBook(id) {
        return this.commonApiService.put('v1/Library/PublishBook/' + id);
    }
    getLibraryPdfSigned(data) {
        let formData = new FormData();
        Object.keys(data).forEach(key => {
            if (['id'].indexOf(key) < 0)
                formData.append(key, data[key]);
        });
       return this.commonApiService.postWithFormData('v1/Files/LibraryBookPdf', formData);
    }
    getLibraryCardSigned(data) {
        let formData = new FormData();
        Object.keys(data).forEach(key => {
            if (['id'].indexOf(key) < 0)
                formData.append(key, data[key]);
        });
       return this.commonApiService.postWithFormData('v1/Files/LibraryBookCardImage', formData);
    }
}