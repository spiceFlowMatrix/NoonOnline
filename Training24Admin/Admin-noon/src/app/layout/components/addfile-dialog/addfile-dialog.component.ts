import { Component, ViewChild, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { FileTypeLists } from './../../../shared';
@Component({
    selector: 'noon-addfile-dialog',
    templateUrl: './addfile-dialog.component.html',
    styleUrls: ['./addfile-dialog.component.scss']
})
export class AddFileDialogComponent implements OnInit {
    @ViewChild('addFileDialog') addFileDialog: any;
    @ViewChild('manageFilesForm') manageFilesForm: any;
    public fileModel: any;
    public fileTypeList:any;
    public modalRef: any = {};
    @Input() pdfOnly;
    @Output() addFileEvent: EventEmitter<any> = new EventEmitter();
    // @Input('coursesList')
    // coursesList: any = [];
    // @Input('title')
    // title: string = '';

    constructor(
        public modelService: NgbModal
    ) { }

    ngOnInit() {
        this.fileModel = {};
        if(this.pdfOnly){
            this.fileModel.fileTypeId=1;
        }
        this.fileTypeList = FileTypeLists;
    }

    openModal() {
        this.modalRef = this.modelService.open(this.addFileDialog, { backdrop: 'static', size: 'lg' });
    }

    manageFile() {
        this.addFileEvent.emit(this.fileModel);
    }

    fileSelected(event) {
        if (event.target.files && event.target.files.length > 0) {
            this.fileModel.file = event.target.files[0];
        }
    }

    closeModal() {
        this.modalRef.dismiss();
        this.modalRef = null;
    }
}
