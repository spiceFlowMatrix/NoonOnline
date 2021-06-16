import { Component, ViewChild, OnInit, Input, OnChanges, SimpleChanges } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { FileTypeLists } from '../../../shared';
@Component({
    selector: 'noon-upload-status',
    templateUrl: './upload-status.component.html',
})
export class UploadStatusDialogComponent implements OnInit, OnChanges {
    @ViewChild('fileUploadStatusDialog') fileUploadStatusDialog: any;
    public modalRef: any = null;

    @Input('data')
    uploadedPercentage: any;

    constructor(
        public modelService: NgbModal
    ) { }

    ngOnChanges(changes: SimpleChanges) {
    }

    ngOnInit() {
    }

    openModal() {
        this.modalRef = this.modelService.open(this.fileUploadStatusDialog, { backdrop: 'static', size: 'lg' });
    }

    closeModal() {
        this.modalRef.dismiss();
        this.modalRef = null;
    }
}
