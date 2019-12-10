import { Component, OnInit, ViewChild } from '@angular/core';
import * as _ from 'lodash';
import { LibraryService } from '../../../shared/services/library.services';
import { UtilService, DELETE_TITLE, DELETE_MESSAGE, GradeService } from '../../../shared';

@Component({
    selector: 'app-book-list',
    templateUrl: './book-list.component.html',
    styleUrls: ['./book-list.component.scss']
})
export class BookListComponent implements OnInit {
    filterModel: any = {}
    private allSubscribers: Array<any> = [];
    @ViewChild('listdialog') listCommonDialog: any;
    isCallingApi: boolean = false;
    isBooksLoading: boolean = false;
    gradeList: any;
    booksList: any;

    constructor(public libraryService: LibraryService,
        public utilService: UtilService,
        public gradeService: GradeService) {}

ngOnInit() {
    this.filterModel = {
        pagenumber: 1,
        perpagerecord: 10,
        totalCount: 0
    }
    this.getBooks(this.filterModel);
    this.getGrades();
}

filterByRole(gradeId) {
        this.filterModel.gradeid = gradeId;
        this.filterModel.pagenumber = 1;
        this.getBooks(_.clone(this.filterModel));
}

getGrades() {
    this.isCallingApi = true;
    this.allSubscribers.push(this.gradeService.getGrades().subscribe(res => {
        this.isCallingApi = false;
        this.gradeList = res.data;
    }, err => {
        this.isCallingApi = false;
        this.utilService.showErrorCall(err);
    }));
}

getBooks(filter) {
    this.isBooksLoading = true;
    if (!filter.bygrade || (filter.bygrade && filter.bygrade == 'undefined')) {
        delete filter.bygrade;
    }
    this.allSubscribers.push(this.libraryService.getBooks(filter).subscribe((res: any) => {
        this.booksList = res.data;
        this.isBooksLoading = false;
        this.filterModel.totalCount = res.totalcount;
    }, err => {
        this.isBooksLoading = false;
        this.utilService.showErrorCall(err);
    }));
}

deleteBook(data) {
    this.allSubscribers.push(this.libraryService.deleteBooks(data.id).subscribe(res => {
        this.booksList = _.remove(this.booksList, (o:any) => {
            return !(o.id == data.id);
        });
    }, err => {
        this.utilService.showErrorCall(err);
    }));
}

onPageChange(event) {
    this.filterModel.pagenumber = event;
    this.getBooks(this.filterModel);
}
openDeleteConfirmation(service, index) {
    service.index = index;
    this.listCommonDialog.open(1, DELETE_TITLE, DELETE_MESSAGE, service);
}

search($event) {
    this.filterModel.pagenumber = 1;
    this.filterModel.search = $event.value;
    if (!this.filterModel.search)
        delete this.filterModel.search;
    this.getBooks(this.filterModel);
}
ngOnDestroy() {
    this.allSubscribers.map(value => value.unsubscribe());
}
}