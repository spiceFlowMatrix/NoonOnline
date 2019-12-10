import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { UtilService, FileService, FileTypeLists } from '../../../shared';
import * as _ from 'lodash';
import { HttpClient, HttpEvent, HttpEventType } from '@angular/common/http';

@Component({
    selector: 'app-add-files',
    templateUrl: './add-files.component.html',
    styleUrls: ['./add-files.component.scss']
})
export class AddFileComponent implements OnInit {
    private allSubscribers: Array<any> = [];
    @ViewChild('manageFilesForm') manageFilesForm: NgForm;
    @ViewChild('fileUploadStatusDialog') fileUploadStatusDialog: any;
    public fileModel: any = {};
    public fileTypeList: any = [];
    isEditView: boolean = false;
    isCallingApi: boolean = false;
    isCallingFileApi: boolean = false;
    file: any = null;
    uploadedPercentage: number;

    constructor(
        private activatedRoute: ActivatedRoute,
        public utilService: UtilService,
        public fileService: FileService,
        public router: Router
    ) {
        this.fileTypeList = FileTypeLists;

        this.allSubscribers.push(this.activatedRoute.params.subscribe((params: Params) => {
            if (params['id']) {
                this.isEditView = true;
                this.fileModel.id = params['id'];
                this.getFileById(this.fileModel.id);
            } else {
                this.isEditView = false;
            }
        }));
    }

    ngOnInit() {
    }

    fileSelected(event) {
        if (event.target.files && event.target.files.length > 0) {
            let reader: any = new FileReader();
            switch (this.fileModel.fileTypeId) {
                case "1":
                case 1:
                    reader.readAsBinaryString(event.target.files[0]);
                    reader.onloadend = () => {
                        var count = reader.result.match(/\/Type[\s]*\/Page[^s]/g).length;
                        this.fileModel.totalpages = count;
                    }
                    break;
                case "2":
                case 2:
                    let fileInput: any = document.getElementById('myVideo');
                    fileInput.src = URL.createObjectURL(event.target.files[0]);
                    fileInput.onloadeddata = (e) => {
                        this.fileModel.duration = fileInput.duration;
                    };
                    break;
            }
            this.file = event.target.files[0];
        }
    }

    getFileById(id) {
        this.isCallingApi = true;
        this.allSubscribers.push(this.fileService.getFileById(id).subscribe(res => {
            this.isCallingApi = false;
            this.fileModel = _.clone(res.data);
        }, err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err);
        }));
    }

    manageFile() {
        // if (!this.isEditView && !this.file) {
        //     return this.utilService.showErrorToast("Required", "Please add file.");
        // }

        // if (this.file) {
        //     this.fileModel.file = this.file;
        // }
        // this.isCallingFileApi = true;
        // this.allSubscribers.push(this.fileService.manageFile(this.fileModel).subscribe((res: any) => {
        //     this.isCallingFileApi = false;
        //     this.router.navigate(['/files/files-list']);
        // }, err => {
        //     this.isCallingFileApi = false;
        //     this.utilService.showErrorCall(err);
        // }));

        if (!this.isEditView && !this.file) {
            return this.utilService.showErrorToast("Required", "Please add file.");
        }
        if (this.file) {
            this.fileModel.file = this.file;
        }
        this.isCallingApi = true;
        this.allSubscribers.push(this.fileService.manageFile(this.fileModel).subscribe((res: any) => {
            switch (res.type) {
                case HttpEventType.Sent:
                    this.uploadedPercentage = 0;
                    this.fileUploadStatusDialog.openModal();
                    this.utilService.showInfoToast("Notification", "Your file upaloding started.");
                    break;
                case HttpEventType.Response:
                    this.isCallingFileApi = false;
                    this.fileUploadStatusDialog.closeModal();
                    this.router.navigate(['/files/files-list']);
                    this.utilService.showInfoToast("", "File uploaded successfully.");
                    break;
                case 1: {
                    if (Math.round(this.uploadedPercentage) !== Math.round(event['loaded'] / event['total'] * 100)) {
                        this.uploadedPercentage = event['loaded'] / event['total'] * 100;
                        // console.log(Math.round(this.uploadedPercentage));
                    }
                    break;
                }
            }
        }, err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err);
        }));
    }

    ngOnDestroy() {
        this.allSubscribers.map(value => value.unsubscribe());
    }

}
