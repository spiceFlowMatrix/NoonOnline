import { Component, OnInit, ViewChild } from '@angular/core';
import * as _ from 'lodash';
import { DELETE_TITLE, DELETE_MESSAGE, UtilService, FileService, FileTypeLists } from '../../../shared';

@Component({
    selector: 'app-files-list',
    templateUrl: './files-list.component.html',
    styleUrls: ['./files-list.component.scss']
})
export class FilesListComponent implements OnInit {
    private allSubscribers: Array<any> = [];
    @ViewChild('listdialog') listCommonDialog: any;
    filterModel: any = {};
    isFilesLoading: boolean;
    filesList: Array<any> = [];
    public fileTypeList: any = [];

    constructor(
        public utilService: UtilService,
        public filesService: FileService
    ) {
        this.fileTypeList = FileTypeLists;
    }


    ngOnInit() {
        this.filterModel = {
            pagenumber: 1,
            perpagerecord: 10,
            totalCount: 0
        }
        this.getFiles(this.filterModel);
    }

    filterByType(filetype) {
        this.filterModel.filetype = filetype;
        this.filterModel.pagenumber = 1;
        this.getFiles(_.clone(this.filterModel));
    }

    openDeleteConfirmation(bundle, index) {
        bundle.index = index;
        this.listCommonDialog.open(1, DELETE_TITLE, DELETE_MESSAGE, bundle);
    }

    search($event) {
        this.filterModel.pagenumber = 1;
        this.filterModel.search = $event.value;
        if (!this.filterModel.search)
            delete this.filterModel.search;
        this.getFiles(this.filterModel);
    }

    onPageChange(event) {
        this.filterModel.pagenumber = event;
        this.getFiles(this.filterModel);
    }

    getFiles(filterObj) {
        if (!filterObj.filetype || (filterObj.filetype && filterObj.filetype == 'undefined')) {
            delete filterObj.filetype;
        }
        this.isFilesLoading = true;
        this.allSubscribers.push(this.filesService.getFiles(filterObj).subscribe(res => {
            this.isFilesLoading = false;
            this.filesList = res.data;
            this.filterModel.totalCount = res.totalcount;
        }, err => {
            this.isFilesLoading = false;
            this.utilService.showErrorCall(err);
        }));
    }

    deleteFile(data) {
        this.allSubscribers.push(this.filesService.deleteFile(data.id).subscribe(res => {
            this.filesList = _.remove(this.filesList, (o) => {
                return !(o.id == data.id);
            });
        }, err => {
            this.utilService.showErrorCall(err);
        }));
    }

    ngOnDestroy() {
        this.allSubscribers.map(value => value.unsubscribe());
    }
}
