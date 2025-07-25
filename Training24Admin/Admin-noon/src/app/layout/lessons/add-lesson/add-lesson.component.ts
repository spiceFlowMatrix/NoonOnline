import { Component, OnInit, ViewChild, ChangeDetectorRef } from '@angular/core';
import { NgForm, FormControl, FormGroup, Validators } from '@angular/forms';
import { Observable, Subject, of, merge } from 'rxjs';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { catchError, debounceTime, distinctUntilChanged, map, tap, switchMap, startWith, finalize } from 'rxjs/operators';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { UtilService, LessonService, FileService, ChapterService, DataService, FileTypeLists } from '../../../shared';
import * as _ from 'lodash';
import { HttpClient, HttpEvent, HttpEventType } from '@angular/common/http';
import { FileHandle } from '../../../shared/modules/directives';
import { THROW_IF_NOT_FOUND } from '@angular/core/src/di/injector';
import { stringify } from '@angular/core/src/render3/util';

@Component({
    selector: 'app-add-lesson',
    templateUrl: './add-lesson.component.html',
    styleUrls: ['./add-lesson.component.scss']
})
export class AddLessonComponent implements OnInit {
    private allSubscribers: Array<any> = [];
    @ViewChild('manageLessonForm') manageLessonForm: NgForm;
    // @ViewChild('addFileDialog') addFileDialog: any;
    // @ViewChild('addallFileDialog') addallFileDialog: any;
    @ViewChild('fileUploadDialog') fileUploadDialog: any;
    @ViewChild('fileUploadStatusDialog') fileUploadStatusDialog: any;
    @ViewChild('manageFilesForm') manageFilesForm: NgForm;
    focus$ = new Subject<string>();
    public lessonModel: any = {};
    public fileModel: any = {};
    isEditView: boolean = false;
    isCallingApi: boolean = false;
    fileList: any = [];
    fileTypeList: any = [];
    file: any = null;
    addFileModalRef: any = [];
    addallFileModalRef: any = [];
    uploadModalRef: any = [];
    videofilename: any;
    pdffilename: any;
    files: any = [];
    mode = 'buffer';
    tempfiles: any = [];
    flag: boolean = false;
    uploadedPercentage: any;
    public assignModal: any = {};
    videoDropedfiles: any = [];
    pdfDropedfiles: any = [];
    assignmentDropedfiles: any = [];
    assignmentfiles: any = [];
    assigncode: any;
    assignmentName: any;
    assignmentDesc: any;
    assignFilesId: any = [];
    showPdfProgress: boolean = false;
    showVideoProgress: boolean = false;
    showAssignmentProgress: boolean = false;
    assignmentid: any;
    // chapterList: any = [];
    config = {
        displayKey: "name",
        search: true
    }
    mypdfControl = new FormControl();
    myvideoControl = new FormControl();
    pdfFilesList: any = [];
    videoFilesList: any = [];
    filterVideo: any;
    signUrl: any;
    constructor(
        private activatedRoute: ActivatedRoute,
        public utilService: UtilService,
        public lessonService: LessonService,
        public fileService: FileService,
        public dataService: DataService,
        public chapterService: ChapterService,
        public modalService: NgbModal,
        public changeDetect: ChangeDetectorRef,
        public router: Router
    ) {

        this.allSubscribers.push(this.activatedRoute.params.subscribe((params: Params) => {
            if (params['id']) {
                this.isEditView = true;
                this.lessonModel.id = params['id'];
                setTimeout(() => {
                    this.getLessonById(this.lessonModel.id);
                }, 500);

            } else {
                this.assigncode = this.utilService.getRandomCode('AS');
                this.lessonModel.code = this.utilService.getRandomCode('LS');
                this.isEditView = false;
            }
        }));
        this.allSubscribers.push(this.activatedRoute.queryParams.subscribe((params: Params) => {
            if (params['chapterid'] && params['chaptername'] && params['courseid']) {
                this.lessonModel.chaptername = params['chaptername'];
                this.lessonModel.chapterId = params['chapterid'];
                // this.lessonModel.type = params['type'] ? params['type'] : '';
                this.lessonModel.courseid = params['courseid'] ? params['courseid'] : '';
                // this.getFiles();
            } else {
                this.router.navigateByUrl('/courses')
            }
        }));
    }

    ngOnInit() {
        let filterObj: any;

        this.myvideoControl.valueChanges
            .pipe(
                debounceTime(500),
                tap(() => {
                    this.videoFilesList = [];
                    this.filterVideo = {
                        fileType: 2,
                        search: this.myvideoControl.value,
                        pagenumber: 1,
                        perpagerecord: 150
                    }
                }),
                switchMap(value => this.fileService.searchGetFileListNew(this.filterVideo)
                    .pipe(
                        finalize(() => {
                        }))))
            .subscribe(data => {
                this.videoFilesList = data;
            });
        this.mypdfControl.valueChanges
            .pipe(
                debounceTime(500),
                tap(() => {
                    this.pdfFilesList = [];
                    filterObj = {
                        fileType: 1,
                        search: this.mypdfControl.value,
                        pagenumber: 1,
                        perpagerecord: 150
                    }
                }),
                switchMap(value => this.fileService.searchGetFileListNew(filterObj)
                    .pipe(
                        finalize(() => {
                        }),
                    )
                )
            )
            .subscribe(data => {
                this.pdfFilesList = data;
            });

        setTimeout(() => {
            window.scrollTo(0, 0); // how far to scroll on each step
        }, 500);
        this.fileTypeList = FileTypeLists;
        this.assigncode = this.utilService.getRandomCode('AS');
    }
    displayFnPdf(pdfFilesList): string | undefined {
        return pdfFilesList ? pdfFilesList.name : undefined;
    }
    displayFnVideo(videoFilesList): string | undefined {
        return videoFilesList ? videoFilesList.name : undefined;
    }


    // private _filter(name: string) {
    //     const filterValue = name.toLowerCase();

    //     return this.options.filter(option => option.name.toLowerCase().indexOf(filterValue) === 0);
    // }

    videoFilesDropped(files: FileHandle[]): void {
        this.fileModel = {};
        if (files[0].file.type != "video/mp4" && files[0].file.type != "video/webm") {
            this.utilService.showErrorToast("Please Select video File!")
        }
        if (files.length != 1) {
            this.utilService.showErrorToast("Please Select One File!")
        }
        if (files.length == 1 && (files[0].file.type == "video/mp4" || files[0].file.type == "video/webm")) {
            this.videoDropedfiles = files[0].file;
            let fileInput: any = document.getElementById('mynAllVideo');
            fileInput.src = URL.createObjectURL(files[0].file);
            fileInput.onloadeddata = (e) => {
                this.fileModel.duration = fileInput.duration;
                this.fileModel.fileTypeId = 2;
            };
            let modal = {
                fileTypeId: 2,
                contentType: files[0].file.type,
                fileName: files[0].file.name
            }
            // this.file = files[0].file;
            // setTimeout(() => {
            //     this.manageFile();
            // }, 500);
            this.isCallingApi = true;
            this.lessonService.getLesssonVideoSigned(modal).subscribe((res) => {
                console.log(res);
                this.fileModel.filename = res.data.filename;
                let signUrl = res.data.signedurl;
                this.fileService.putFileOnBucket(signUrl, files[0].file).subscribe((res: any) => {
                    switch (res.type) {
                        case HttpEventType.Sent:
                            this.uploadedPercentage = 0;
                            this.showVideoProgress = true;
                            this.utilService.showInfoToast("Notification", "Your file upaloding started.");
                            break;
                        case HttpEventType.Response:
                            this.mode = "buffer";
                            this.showVideoProgress = false;
                            this.isCallingApi = false;
                            this.utilService.showInfoToast("", "File uploaded successfully.");
                            console.log(res);
                            if (files[0].file) {
                                this.file = files[0].file;
                                setTimeout(() => {
                                    this.manageFile();
                                }, 500);
                            }
                            break;
                        case 1: {
                            if (Math.round(this.uploadedPercentage) !== Math.round(event['loaded'] / event['total'] * 100)) {
                                this.uploadedPercentage = event['loaded'] / event['total'] * 100;
                                if (this.uploadedPercentage == 100) {
                                    this.mode = "indeterminate";
                                }
                            }
                            break;
                        }
                    }
                }, err => {
                    this.isCallingApi = false;
                    this.showPdfProgress = false;
                    this.showVideoProgress = false;
                    this.utilService.showErrorCall(err);
                })
            })
        }
    }


    pdfFilesDropped(files: FileHandle[]): void {
        this.fileModel = {};
        if (files[0].file.type != "application/pdf") {
            this.utilService.showErrorToast("Please Select Pdf File!")
        }
        if (files.length != 1) {
            this.utilService.showErrorToast("Please Select One File!")
        }
        if (files.length == 1 && files[0].file.type == "application/pdf") {
            this.pdfDropedfiles = files[0].file;
            let reader: any = new FileReader();
            reader.readAsBinaryString(files[0].file);
            reader.onloadend = () => {
                var count = reader.result.match(/\/Type[\s]*\/Page[^s]/g).length;
                this.fileModel.totalpages = count;
                this.fileModel.fileTypeId = 1;

            }
            this.file = files[0].file;
            // setTimeout(() => {
            //     this.manageFile();
            // }, 500);
            let modal = {
                fileTypeId: 1,
                contentType: files[0].file.type,
                fileName: files[0].file.name
            }
            this.isCallingApi = true;
            this.lessonService.getLesssonVideoSigned(modal).subscribe((res) => {
                console.log(res);
                this.fileModel.filename = res.data.filename;
                let signUrl = res.data.signedurl;
                this.fileService.putFileOnBucket(signUrl, files[0].file).subscribe((res: any) => {
                    switch (res.type) {
                        case HttpEventType.Sent:
                            this.uploadedPercentage = 0;
                            this.showVideoProgress = true;
                            this.utilService.showInfoToast("Notification", "Your file upaloding started.");
                            break;
                        case HttpEventType.Response:
                            this.mode = "buffer";
                            this.showVideoProgress = false;
                            this.isCallingApi = false;
                            this.utilService.showInfoToast("", "File uploaded successfully.");
                            console.log(res);
                            if (files[0].file) {
                                this.file = files[0].file;
                                setTimeout(() => {
                                    this.manageFile();
                                }, 500);
                            }
                            break;
                        case 1: {
                            if (Math.round(this.uploadedPercentage) !== Math.round(event['loaded'] / event['total'] * 100)) {
                                this.uploadedPercentage = event['loaded'] / event['total'] * 100;
                                if (this.uploadedPercentage == 100) {
                                    this.mode = "indeterminate";
                                }
                            }
                            break;
                        }
                    }
                }, err => {
                    this.isCallingApi = false;
                    this.showPdfProgress = false;
                    this.showVideoProgress = false;
                    this.utilService.showErrorCall(err);
                })
            })
        }
    }
    async putFileOnBucket(element, type, page?: any, duration?: any, id?: any, name?: any) {

        this.fileService.putFileOnBucket(this.signUrl, element).subscribe((res: any) => {
            switch (res.type) {
                case HttpEventType.Sent:
                    this.uploadedPercentage = 0;
                    this.showAssignmentProgress = true;
                    this.utilService.showInfoToast("Notification", "Your file upaloding started.");
                    break;
                case HttpEventType.Response:
                    this.mode = "buffer";
                    this.showAssignmentProgress = false;
                    console.log(this.assignModal);
                    switch (type) {
                        case "pdf":
                            let pdfmodal = {
                                fileTypeId: id,
                                filename: name,
                                totalpages: page
                            }
                            this.SaveFileMetaData(pdfmodal);
                            break;
                        case "video":
                            let videomodal = {
                                fileTypeId: id,
                                filename: name,
                                duration: duration
                            }
                            this.SaveFileMetaData(videomodal);
                            break;
                        case "image":
                            let imagemodal = {
                                fileTypeId: id,
                                filename: name,
                            }
                            this.SaveFileMetaData(imagemodal);
                            break;
                        case "csv":
                            let csvmodal = {
                                fileTypeId: id,
                                filename: name,
                            }
                            this.SaveFileMetaData(csvmodal);
                            break;
                        case "docs":
                            let docsmodal = {
                                fileTypeId: id,
                                filename: name,
                            }
                            this.SaveFileMetaData(docsmodal);
                            break;
                        case "ppt":
                            let pptmodal = {
                                fileTypeId: id,
                                filename: name,
                            }
                            this.SaveFileMetaData(pptmodal);
                            break;
                        default:
                            break;

                    }
                    break;
                case 1: {
                    if (Math.round(this.uploadedPercentage) !== Math.round(event['loaded'] / event['total'] * 100)) {
                        this.uploadedPercentage = event['loaded'] / event['total'] * 100;
                        if (this.uploadedPercentage == 100) {
                            this.mode = "indeterminate";
                        }
                    }
                    console.log(res.type);
                    break;
                }
            }
        }, err => {
            this.isCallingApi = false;
            this.showAssignmentProgress = false;
            this.utilService.showErrorCall(err);
        })
    }
    SaveFileMetaData(modal) {
        this.allSubscribers.push(this.fileService.SaveFileMetaData(modal).subscribe((res: any) => {
            this.isCallingApi = false;
            // for (let i = 0; i < res.body.data.length; i++) {
            let fileModal = {
                filetypeid: res.data.filetypeid,
                filetypename: res.data.filetypename,
                id: res.data.id,
                name: res.data.name
            }
            this.isCallingApi = false;
            this.assignFilesId.push(res.data.id);
            this.assignmentfiles.push(res.data);
            this.tempfiles.push(res.data);
            this.utilService.showInfoToast("", "File uploaded successfully.");
            // }
        }, err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err);
        }));
    }
    assignmentFileDrop(files: FileHandle[]): void {
        this.assignModal = {};
        let temp: any = [];
        let tempFiles = [];
        let assignfiles = [];
        let re = /(?:\.([^.]+))?$/;
        for (let i = 0; i < files.length; i++) {
            tempFiles.push(files[i].file)
        }
        tempFiles.forEach(element => {
            let ext = "";
            this.signUrl = "";
            this.assignModal.filename = "";
            ext = re.exec(element.name)[1];
            assignfiles.push(element);
            if (element.type === "application/pdf") {
                this.assignModal = {};
                let reader: any = new FileReader();
                reader.readAsBinaryString(element);
                reader.onloadend = () => {
                    var count = reader.result.match(/\/Type[\s]*\/Page[^s]/g).length;
                    this.assignModal.totalpages = count.toString();
                    this.assignModal.fileTypeId = "1";
                }
                let modal = {
                    fileTypeId: 1,
                    contentType: element.type,
                    fileName: element.name
                }
                this.isCallingApi = true;
                this.signUrl = "";
                this.lessonService.getLessonAssignmentFileSigned(modal).subscribe(async (res) => {
                    this.assignModal.filename = res.data.filename;
                    this.signUrl = res.data.signedurl;
                    if (this.signUrl) {
                        await this.putFileOnBucket(element, "pdf", this.assignModal.totalpages, "", "1", res.data.filename);
                    }
                });
            }
            else if (element.type === "video/mp4" || element.type === "video/webm") {
                this.assignModal = {};
                this.signUrl = "";
                let fileInput: any = document.getElementById('mynAllVideo');
                fileInput.src = URL.createObjectURL(element);
                fileInput.onloadeddata = (e) => {
                    this.assignModal.duration = fileInput.duration.toString();
                    this.assignModal.fileTypeId = "2";
                };
                let modal = {
                    fileTypeId: 2,
                    contentType: element.type,
                    fileName: element.name
                }
                this.lessonService.getLessonAssignmentFileSigned(modal).subscribe(async (res) => {
                    this.assignModal.filename = res.data.filename;
                    this.signUrl = res.data.signedurl;
                    if (this.signUrl) {
                        await this.putFileOnBucket(element, "video", "", this.assignModal.duration, "2", res.data.filename);
                    }
                });
            }
            else if (element.type === "image/png" || element.type === "image/jpeg") {
                this.assignModal = {};
                this.signUrl = "";
                this.assignModal.fileTypeId = "3"
                let modal = {
                    fileTypeId: 3,
                    contentType: element.type,
                    fileName: element.name
                }
                this.lessonService.getLessonAssignmentFileSigned(modal).subscribe(async (res) => {
                    this.assignModal.filename = res.data.filename;
                    this.signUrl = res.data.signedurl;
                    if (this.signUrl) {
                        await this.putFileOnBucket(element, "image", "", "", "3", res.data.filename);
                    }
                });
            }
            else if (ext === "xlsx" || ext === "xls" || ext === "csv" || ext == "dbf" || ext === "dif" || ext === "ods" || ext === "prn" || ext === "slk" || ext === "xla" || ext === "xlam" || ext === "xlsb" || ext === "xlsm" || ext === "xlt" || ext === "xltm" || ext === "xltx" || ext === "xlw") {
                this.assignModal = {};
                this.assignModal.fileTypeId = "6";
                let modal = {
                    fileTypeId: 6,
                    contentType: element.type,
                    fileName: element.name
                }
                this.lessonService.getLessonAssignmentFileSigned(modal).subscribe(async (res) => {
                    this.assignModal.filename = res.data.filename;
                    this.signUrl = res.data.signedurl;
                    if (this.signUrl) {
                        await this.putFileOnBucket(element, "csv", "", "", "6", res.data.filename);
                    }
                });
            }
            else if (ext === "docs" || ext === "docx" || ext === "doc" || ext === "docm" || ext === "dot" || ext === "dotm" || ext === "dotx" || ext === "odt" || ext === "rtf" || ext === "txt" || ext === "wps" || ext === "xml" || ext === "xps") {
                this.assignModal = {};
                this.assignModal.fileTypeId = "7";
                let modal = {
                    fileTypeId: 7,
                    contentType: element.type,
                    fileName: element.name
                }
                this.lessonService.getLessonAssignmentFileSigned(modal).subscribe(async (res) => {
                    this.assignModal.filename = res.data.filename;
                    this.signUrl = res.data.signedurl;
                    if (this.signUrl) {
                        await this.putFileOnBucket(element, "docs", "", "", "7", res.data.filename);
                    }
                });
            }
            else if (ext === "pptx" || ext === "ppt" || ext === "pptm" || ext === "potx" || ext === "potm" || ext === "odp") {
                this.assignModal = {};
                this.assignModal.fileTypeId = "8";
                let modal = {
                    fileTypeId: 8,
                    contentType: element.type,
                    fileName: element.name
                }
                this.lessonService.getLessonAssignmentFileSigned(modal).subscribe(async (res) => {
                    this.assignModal.filename = res.data.filename;
                    this.signUrl = res.data.signedurl;
                    if (this.signUrl) {
                        await this.putFileOnBucket(element, "ppt", "", "", "8", res.data.filename);
                    }
                });
            }
            else {
                this.utilService.showErrorToast("wrong File Extension")
            }
        });
    }

    assignmentfileSelected(event) {
        this.assignModal = {};
        let files = []
        let tempFiles = [];
        let re = /(?:\.([^.]+))?$/;
        for (let i = 0; i < event.target.files.length; i++) {
            tempFiles.push(event.target.files[i])
        }
        tempFiles.forEach(element => {
            let ext = "";
            this.signUrl = "";
            this.assignModal.filename = "";
            ext = re.exec(element.name)[1];
            files.push(element);
            if (element.type === "application/pdf") {
                this.assignModal = {};
                let reader: any = new FileReader();
                reader.readAsBinaryString(element);
                reader.onloadend = () => {
                    var count = reader.result.match(/\/Type[\s]*\/Page[^s]/g).length;
                    this.assignModal.totalpages = count.toString();
                    this.assignModal.fileTypeId = "1";
                }
                let modal = {
                    fileTypeId: 1,
                    contentType: element.type,
                    fileName: element.name
                }
                this.isCallingApi = true;
                this.signUrl = "";
                this.lessonService.getLessonAssignmentFileSigned(modal).subscribe(async (res) => {
                    this.assignModal.filename = res.data.filename;
                    this.signUrl = res.data.signedurl;
                    if (this.signUrl) {
                        await this.putFileOnBucket(element, "pdf", this.assignModal.totalpages, "", "1", res.data.filename);
                    }
                });
            }
            else if (element.type === "video/mp4" || element.type === "video/webm") {
                this.assignModal = {};
                this.signUrl = "";
                let fileInput: any = document.getElementById('mynAllVideo');
                fileInput.src = URL.createObjectURL(element);
                fileInput.onloadeddata = (e) => {
                    this.assignModal.duration = fileInput.duration.toString();
                    this.assignModal.fileTypeId = "2";
                };
                let modal = {
                    fileTypeId: 2,
                    contentType: element.type,
                    fileName: element.name
                }
                this.lessonService.getLessonAssignmentFileSigned(modal).subscribe(async (res) => {
                    this.assignModal.filename = res.data.filename;
                    this.signUrl = res.data.signedurl;
                    if (this.signUrl) {
                        await this.putFileOnBucket(element, "video", "", this.assignModal.duration, "2", res.data.filename);
                    }
                });
            }
            else if (element.type === "image/png" || element.type === "image/jpeg") {
                this.assignModal = {};
                this.signUrl = "";
                this.assignModal.fileTypeId = "3"
                let modal = {
                    fileTypeId: 3,
                    contentType: element.type,
                    fileName: element.name
                }
                this.lessonService.getLessonAssignmentFileSigned(modal).subscribe(async (res) => {
                    this.assignModal.filename = res.data.filename;
                    this.signUrl = res.data.signedurl;
                    if (this.signUrl) {
                        await this.putFileOnBucket(element, "image", "", "", "3", res.data.filename);
                    }
                });
            }
            else if (ext === "xlsx" || ext === "xls" || ext === "csv" || ext == "dbf" || ext === "dif" || ext === "ods" || ext === "prn" || ext === "slk" || ext === "xla" || ext === "xlam" || ext === "xlsb" || ext === "xlsm" || ext === "xlt" || ext === "xltm" || ext === "xltx" || ext === "xlw") {
                this.assignModal = {};
                this.assignModal.fileTypeId = "6";
                let modal = {
                    fileTypeId: 6,
                    contentType: element.type,
                    fileName: element.name
                }
                this.lessonService.getLessonAssignmentFileSigned(modal).subscribe(async (res) => {
                    this.assignModal.filename = res.data.filename;
                    this.signUrl = res.data.signedurl;
                    if (this.signUrl) {
                        await this.putFileOnBucket(element, "csv", "", "", "6", res.data.filename);
                    }
                });
            }
            else if (ext === "docs" || ext === "docx" || ext === "doc" || ext === "docm" || ext === "dot" || ext === "dotm" || ext === "dotx" || ext === "odt" || ext === "rtf" || ext === "txt" || ext === "wps" || ext === "xml" || ext === "xps") {
                this.assignModal = {};
                this.assignModal.fileTypeId = "7";
                let modal = {
                    fileTypeId: 7,
                    contentType: element.type,
                    fileName: element.name
                }
                this.lessonService.getLessonAssignmentFileSigned(modal).subscribe(async (res) => {
                    this.assignModal.filename = res.data.filename;
                    this.signUrl = res.data.signedurl;
                    if (this.signUrl) {
                        await this.putFileOnBucket(element, "docs", "", "", "7", res.data.filename);
                    }
                });
            }
            else if (ext === "pptx" || ext === "ppt" || ext === "pptm" || ext === "potx" || ext === "potm" || ext === "odp") {
                this.assignModal = {};
                this.assignModal.fileTypeId = "8";
                let modal = {
                    fileTypeId: 8,
                    contentType: element.type,
                    fileName: element.name
                }
                this.lessonService.getLessonAssignmentFileSigned(modal).subscribe(async (res) => {
                    this.assignModal.filename = res.data.filename;
                    this.signUrl = res.data.signedurl;
                    if (this.signUrl) {
                        await this.putFileOnBucket(element, "ppt", "", "", "8", res.data.filename);
                    }
                });
            }
            else {
                this.utilService.showErrorToast("wrong File Extension")
            }
        });
    }


    assignmentFilesDropped(files: FileHandle[]): void {
        this.assignModal = {};
        let temp: any = [];
        for (let j = 0; j < files.length; j++) {
            temp.push(files[j].file)
        }
        if (temp && temp.length > 0) {
            let tempcount = new Array(temp.length).fill("");
            let tempduration = new Array(temp.length).fill("");
            let tempdescription = new Array(temp.length).fill("");
            let tempfiletypeId = new Array(temp.length).fill("");
            let assignfiles = []
            let re = /(?:\.([^.]+))?$/;
            for (let i = 0; i < temp.length; i++) {
                let ext = "";
                ext = re.exec(temp[i].name)[1];
                assignfiles.push(temp[i]);
                if (temp[i].type === "application/pdf") {
                    let reader: any = new FileReader();
                    reader.readAsBinaryString(temp[i]);
                    reader.onloadend = () => {
                        var count = reader.result.match(/\/Type[\s]*\/Page[^s]/g).length;
                        tempcount[i] = count.toString();
                        tempfiletypeId[i] = "1";
                    }
                }
                else if (temp[i].type === "video/mp4" || temp[i].type === "video/webm") {
                    let fileInput: any = document.getElementById('mynAllVideo');
                    fileInput.src = URL.createObjectURL(temp[i]);
                    fileInput.onloadeddata = (e) => {
                        tempduration[i] = fileInput.duration.toString();
                        tempfiletypeId[i] = "2";
                    };
                }
                else if (temp[i].type === "image/png" || temp[i].type === "image/jpeg") {
                    tempfiletypeId[i] = "3"
                }
                else if (ext === "xlsx" || ext === "xls" || ext === "csv" || ext == "dbf" || ext === "dif" || ext === "ods" || ext === "prn" || ext === "slk" || ext === "xla" || ext === "xlam" || ext === "xlsb" || ext === "xlsm" || ext === "xlt" || ext === "xltm" || ext === "xltx" || ext === "xlw") {
                    tempfiletypeId[i] = "6";
                }
                else if (ext === "docs" || ext === "docx" || ext === "doc" || ext === "docm" || ext === "dot" || ext === "dotm" || ext === "dotx" || ext === "odt" || ext === "rtf" || ext === "txt" || ext === "wps" || ext === "xml" || ext === "xps") {
                    tempfiletypeId[i] = "7";
                }
                else if (ext === "pptx" || ext === "ppt" || ext === "pptm" || ext === "potx" || ext === "potm" || ext === "odp") {
                    tempfiletypeId[i] = "8";
                }
                else {
                    this.utilService.showErrorToast("wrong File Extension")
                }
                let tempid = i + 1;
                if (temp.length <= tempid) {
                    setTimeout(() => {
                        this.assignModal.totalpages = tempcount;
                        this.assignModal.duration = tempduration;
                        this.assignModal.description = tempdescription;
                        this.assignModal.fileTypeId = tempfiletypeId;
                        this.assignModal.file = assignfiles;
                        this.manageAssignFile();
                    }, 500);
                }
            }


        }
    }


    // Videosearch = (text$: Observable<any>) => {
    //     const debouncedText$ = text$.pipe(debounceTime(300), distinctUntilChanged());
    //     // const inputFocus$ = this.focus$;
    //     // return merge(debouncedText$, inputFocus$).pipe(
    //     return merge(debouncedText$).pipe(
    //         tap(() => this.isCallingApi = true),
    //         switchMap(term => {
    //             // let typeObj = _.find(FileTypeLists, (o) => {
    //             //     return o.name.toLowerCase() == "video"
    //             //     // return o.name.toLowerCase() == this.lessonModel.type.toLowerCase()
    //             // });
    //             let filterObj = {
    //                 fileType: 2,
    //                 search: term,
    //                 pagenumber: 1,
    //                 perpagerecord: 150
    //             }
    //             return this.fileService.searchGetFileListNew(filterObj).pipe(
    //                 tap(() => this.isCallingApi = false),
    //                 catchError(() => {
    //                     this.isCallingApi = false;
    //                     return of([]);
    //                 }))
    //         }),
    //         tap(() => this.isCallingApi = false)
    //     )
    // }

    // pdfsearch = (text$: Observable<any>) => {
    //     const debouncedText$ = text$.pipe(debounceTime(300), distinctUntilChanged());
    //     // const inputFocus$ = this.focus$;
    //     // return merge(debouncedText$, inputFocus$).pipe(
    //     return merge(debouncedText$).pipe(
    //         tap(() => this.isCallingApi = true),
    //         switchMap(term => {
    //             // let typeObj = _.find(FileTypeLists, (o) => {
    //             //     // return o.name.toLowerCase() == this.lessonModel.type.toLowerCase()
    //             //     return o.name.toLowerCase() == "pdf"
    //             // });
    //             let filterObj = {
    //                 fileType: 1,
    //                 search: term,
    //                 pagenumber: 1,
    //                 perpagerecord: 150
    //             }
    //             return this.fileService.searchGetFileListNew(filterObj).pipe(
    //                 tap(() => this.isCallingApi = false),
    //                 catchError(() => {
    //                     this.isCallingApi = false;
    //                     return of([]);
    //                 }))
    //         }),
    //         tap(() => this.isCallingApi = false)
    //     )
    // }

    // formatterv = (x: { name: string }) => x.name;
    // formatterp = (x: { name: string }) => x.name;

    getFiles() {
        // let typeObj = _.find(FileTypeLists, (o) => {
        //     return o.name.toLowerCase() == this.lessonModel.type.toLowerCase()
        // });
        let filterObj = {
            // fileType: typeObj ? typeObj.id : '',
            search: '',
            pagenumber: 1,
            perpagerecord: 150
        }
        this.allSubscribers.push(this.fileService.getFiles(filterObj).subscribe(res => {
            this.fileList = res.data;
        }));
    }

    selectVideoFile() {
        let videofile = this.myvideoControl.value;
        if (this.files.length != 0) {
            for (let i = 0; i < this.files.length; i++) {
                if (this.files[i].filetypeid == videofile.filetypeid) {
                    this.files.splice(i, 1);
                    this.files.push(videofile);
                }
                else if (this.files.length == 1 && this.files[i].filetypeid != videofile.filetypeid) {
                    this.files.push(videofile);
                }
            }
        }
        else {
            this.files.push(videofile);
        }
    }
    selectpdfFile() {
        let pdffile = this.mypdfControl.value;
        if (this.files.length != 0) {
            for (let i = 0; i < this.files.length; i++) {
                if (this.files[i].filetypeid == pdffile.filetypeid) {
                    this.files.splice(i, 1);
                    this.files.push(pdffile);
                }
                else if (this.files.length == 1 && this.files[i].filetypeid != pdffile.filetypeid) {
                    this.files.push(pdffile);
                }
            }
        }
        else {
            this.files.push(pdffile);
        }
    }

    DeleteFile(i) {
        if (this.tempfiles[i].id) {
            this.isCallingApi = true;
            this.lessonService.deleteLessonAssignmentFile(this.tempfiles[i].id).subscribe((res) => {
                this.isCallingApi = false;
                this.tempfiles.splice(i, 1);
                this.assignmentfiles.splice(i, 1);
                this.assignFilesId.splice(i, 1);
            })
        }
        else {
            this.tempfiles.splice(i, 1);
            this.assignmentfiles.splice(i, 1);
            this.assignFilesId.splice(i, 1);
        }
    }

    getLessonById(id) {
        this.isCallingApi = true;
        this.assignmentfiles = [];
        this.pdffilename = "";
        this.videofilename = "";
        this.tempfiles = [];
        this.assignmentName = "";
        this.assignmentDesc = "";
        this.files = [];
        this.allSubscribers.push(this.lessonService.getLessonById(id).subscribe(res => {
            this.isCallingApi = false;
            Object.keys(res.data).forEach(key => {
                this.lessonModel[key] = res.data[key];
            });
            this.lessonModel.chapterId = this.lessonModel.chapter.id;
            if (res.data.assignment) {
                // this.assignmentfiles = res.data.assignment.assignmentfiles;
                this.assignmentid = res.data.assignment.id;
                this.assignmentName = res.data.assignment.name;
                this.assignmentDesc = res.data.assignment.description;
                this.assigncode = res.data.assignment.code;

            }
            if (res.data.assignment.assignmentfiles.length > 0) {

                this.tempfiles = res.data.assignment.assignmentfiles;
                let assignIds = [];
                for (let ind = 0; ind < res.data.assignment.assignmentfiles.length; ind++) {
                    this.assignmentfiles.push(res.data.assignment.assignmentfiles[ind].files)
                    assignIds.push(res.data.assignment.assignmentfiles[ind].files.id)
                }
                this.assignFilesId = assignIds;
            }
            if (res.data.lessonfiles && res.data.lessonfiles.length > 0) {
                for (let i = 0; i < res.data.lessonfiles.length; i++) {
                    if (res.data.lessonfiles[i].files.filetypeid == 1) {
                        this.mypdfControl.setValue(res.data.lessonfiles[i].files);
                        this.files.push(res.data.lessonfiles[i].files);
                    }
                    if (res.data.lessonfiles[i].files.filetypeid == 2) {
                        this.myvideoControl.setValue(res.data.lessonfiles[i].files);
                        this.files.push(res.data.lessonfiles[i].files);
                    }
                }
            }



        }, err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err);
        }));
    }

    updateLesson() {
        let update = {
            chapterId: this.lessonModel.chapterId,
            lesson: {
                id: this.lessonModel.id,
                name: this.lessonModel.name,
                code: this.lessonModel.code,
                files: this.files,
                assignment: {
                    id: this.assignmentid,
                    name: this.assignmentName,
                    description: this.assignmentDesc,
                    code: this.assigncode,
                    files: this.assignFilesId
                }
            }
        }
        this.isCallingApi = true;
        this.allSubscribers.push(this.lessonService.updateLesson(update).subscribe((res: any) => {
            this.isCallingApi = false;
            if (!this.isEditView)
                this.router.navigateByUrl('/courses/course-preview/' + this.lessonModel.courseid + '/edit-lesson/' + res.data.id + window.location.search);
            else
                this.router.navigateByUrl('/courses/course-preview/' + this.lessonModel.courseid + '/edit-lesson/' + this.lessonModel.id + window.location.search);
            this.dataService.refreshCourseData({
                operation: 'add',
                type: 'chapter',
                obj: res.data
            });
        }, err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err);
        }));
    }

    manageLession() {
        let modal = {
            chapterId: this.lessonModel.chapterId,
            lesson: {
                name: this.lessonModel.name,
                code: this.lessonModel.code,
                files: this.files,
                assignment: {
                    name: this.assignmentName,
                    description: this.assignmentDesc,
                    code: this.assigncode,
                    files: this.assignFilesId
                }
            }
        }
        this.isCallingApi = true;
        this.allSubscribers.push(this.lessonService.manageLesson(modal).subscribe((res: any) => {
            this.isCallingApi = false;
            if (!this.isEditView)
                this.router.navigateByUrl('/courses/course-preview/' + this.lessonModel.courseid + '/edit-lesson/' + res.data.id + window.location.search);
            else
                this.router.navigateByUrl('/courses/course-preview/' + this.lessonModel.courseid + '/edit-lesson/' + this.lessonModel.id + window.location.search);
            this.dataService.refreshCourseData({
                operation: 'add',
                type: 'chapter',
                obj: res.data
            });
        }, err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err);
        }));
    }

    selectFile(type) {
        if (type == 2) {
            document.getElementById("videofile").click();
        }
        if (type == 1) {
            document.getElementById('pdffile').click();
        }
    }

    openAllFileModal() {
        document.getElementById('assignfile').click();
    }


    fileSelected(event, type) {
        this.fileModel = {};
        if (event.target.files) {
            let modal = {
                fileTypeId: type,
                contentType: event.target.files[0].type,
                fileName: event.target.files[0].name
            }
            this.isCallingApi = true;
            switch (type) {
                case "1":
                case 1:
                    let reader: any = new FileReader();
                    reader.readAsBinaryString(event.target.files[0]);
                    reader.onloadend = () => {
                        var count = reader.result.match(/\/Type[\s]*\/Page[^s]/g).length;
                        this.fileModel.totalpages = count;
                        this.fileModel.fileTypeId = 1;
                    }
                    this.lessonService.getLessonPdfSigned(modal).subscribe((res) => {
                        console.log(res);
                        this.fileModel.filename = res.data.filename;
                        let signUrl = res.data.signedurl;
                        this.fileService.putFileOnBucket(signUrl, event.target.files[0]).subscribe((res: any) => {
                            switch (res.type) {
                                case HttpEventType.Sent:
                                    this.uploadedPercentage = 0;
                                    if (this.fileModel.fileTypeId == 1) {
                                        this.showPdfProgress = true

                                    } else if (this.fileModel.fileTypeId == 2) {
                                        this.showVideoProgress = true;
                                    }
                                    this.utilService.showInfoToast("Notification", "Your file upaloding started.");
                                    break;
                                case HttpEventType.Response:
                                    this.mode = "buffer";
                                    if (this.fileModel.fileTypeId == 1) {
                                        this.showPdfProgress = false

                                    } else if (this.fileModel.fileTypeId == 2) {
                                        this.showVideoProgress = false;
                                    }
                                    this.isCallingApi = false;
                                    this.utilService.showInfoToast("", "File uploaded successfully.");
                                    console.log(res);
                                    if (event.target.files && event.target.files.length > 0) {
                                        this.file = event.target.files[0];
                                        setTimeout(() => {
                                            this.manageFile();
                                        }, 500);
                                    }
                                    break;
                                case 1: {
                                    if (Math.round(this.uploadedPercentage) !== Math.round(event['loaded'] / event['total'] * 100)) {
                                        this.uploadedPercentage = event['loaded'] / event['total'] * 100;
                                        if (this.uploadedPercentage == 100) {
                                            this.mode = "indeterminate";
                                        }
                                    }
                                    break;
                                }
                            }
                        }, err => {
                            this.isCallingApi = false;
                            this.showPdfProgress = false;
                            this.showVideoProgress = false;
                            this.utilService.showErrorCall(err);
                        })
                    })
                    break;
                case "2":
                case 2:
                    let fileInput: any = document.getElementById('mynAllVideo');
                    fileInput.src = URL.createObjectURL(event.target.files[0]);
                    fileInput.onloadeddata = (e) => {
                        this.fileModel.duration = fileInput.duration;
                        this.fileModel.fileTypeId = 2;
                    };
                    this.lessonService.getLesssonVideoSigned(modal).subscribe((res) => {
                        console.log(res);
                        this.fileModel.filename = res.data.filename;
                        let signUrl = res.data.signedurl;
                        this.fileService.putFileOnBucket(signUrl, event.target.files[0]).subscribe((res: any) => {
                            switch (res.type) {
                                case HttpEventType.Sent:
                                    this.uploadedPercentage = 0;
                                    this.showVideoProgress = true;
                                    this.utilService.showInfoToast("Notification", "Your file upaloding started.");
                                    break;
                                case HttpEventType.Response:
                                    this.mode = "buffer";
                                    if (this.fileModel.fileTypeId == 1) {
                                        this.showPdfProgress = false

                                    } else if (this.fileModel.fileTypeId == 2) {
                                        this.showVideoProgress = false;
                                    }
                                    this.isCallingApi = false;
                                    this.utilService.showInfoToast("", "File uploaded successfully.");
                                    console.log(res);
                                    if (event.target.files && event.target.files.length > 0) {
                                        this.file = event.target.files[0];
                                        setTimeout(() => {
                                            this.manageFile();
                                        }, 500);
                                    }
                                    break;
                                case 1: {
                                    if (Math.round(this.uploadedPercentage) !== Math.round(event['loaded'] / event['total'] * 100)) {
                                        this.uploadedPercentage = event['loaded'] / event['total'] * 100;
                                        if (this.uploadedPercentage == 100) {
                                            this.mode = "indeterminate";
                                        }
                                    }
                                    break;
                                }
                            }
                        }, err => {
                            this.isCallingApi = false;
                            this.showPdfProgress = false;
                            this.showVideoProgress = false;
                            this.utilService.showErrorCall(err);
                        })
                    })
                    break;
            }

        }
        // if (event.target.files && event.target.files.length > 0) {
        //     let reader: any = new FileReader();
        //     switch (type) {
        //         case "1":
        //         case 1:
        //             reader.readAsBinaryString(event.target.files[0]);
        //             reader.onloadend = () => {
        //                 var count = reader.result.match(/\/Type[\s]*\/Page[^s]/g).length;
        //                 this.fileModel.totalpages = count;
        //                 this.fileModel.fileTypeId = 1;
        //             }
        //             break;
        //         case "2":
        //         case 2:
        //             let fileInput: any = document.getElementById('mynAllVideo');
        //             fileInput.src = URL.createObjectURL(event.target.files[0]);
        //             fileInput.onloadeddata = (e) => {
        //                 this.fileModel.duration = fileInput.duration;
        //                 this.fileModel.fileTypeId = 2;
        //             };
        //             break;
        //     }
        //     this.file = event.target.files[0];
        //     setTimeout(() => {
        //         this.manageFile();
        //     }, 500);
        // }
    }
    manageAssignFile() {
        this.isCallingApi = true;
        this.allSubscribers.push(this.fileService.SaveFileMetaData(this.assignModal).subscribe((res: any) => {
            this.isCallingApi = false;
            // for (let i = 0; i < res.body.data.length; i++) {
            this.assignFilesId.push(res.data.id);
            this.assignmentfiles.push(res.data);
            this.tempfiles.push(res.data);
            // }
        }, err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err);
        }));
    }

    manageFile() {
        if (!this.isEditView && !this.file) {
            return this.utilService.showErrorToast("Required", "Please add file.");
        }
        // if (this.file) {
        //     this.fileModel.file = this.file;
        // }
        this.isCallingApi = true;
        this.allSubscribers.push(this.fileService.SaveFileMetaData(this.fileModel).subscribe((res: any) => {
            console.log(res);
            this.isCallingApi = false;
            let modal = {
                filetypeid: res.data.filetypeid,
                filetypename: res.data.filetypename,
                id: res.data.id,
                name: res.data.name
            }
            if (this.files.length != 0) {
                for (let i = 0; i < this.files.length; i++) {
                    if (this.files[i].filetypeid == modal.filetypeid) {
                        this.files.splice(i, 1);
                        this.files.push(modal);
                        if (modal.filetypeid === 1) {
                            this.mypdfControl.setValue(modal);
                        } else {
                            this.myvideoControl.setValue(modal);
                        }
                    }
                    else if (this.files.length == 1 && this.files[i].filetypeid != modal.filetypeid) {
                        this.files.push(modal);
                        if (modal.filetypeid === 1) {
                            this.mypdfControl.setValue(modal);
                        } else {
                            this.myvideoControl.setValue(modal);
                        }
                    }
                }
            }
            else {
                this.files.push(modal);
                if (modal.filetypeid === 1) {
                    this.mypdfControl.setValue(modal);
                } else {
                    this.myvideoControl.setValue(modal);
                }
            }
            //         this.utilService.showInfoToast("", "File uploaded successfully.");
            //         break;
            //     case 1: {
            //         if (Math.round(this.uploadedPercentage) !== Math.round(event['loaded'] / event['total'] * 100)) {
            //             this.uploadedPercentage = event['loaded'] / event['total'] * 100;
            //             if (this.uploadedPercentage == 100) {
            //                 this.mode = "indeterminate";
            //             }
            //         }
            //         break;
            //     }
            // }
        }, err => {
            this.isCallingApi = false;
            this.showPdfProgress = false;
            this.showVideoProgress = false;
            // this.fileUploadStatusDialog.openModal();
            this.utilService.showErrorCall(err);
        }));
    }

    importFiles() {
        let typeObj = _.find(FileTypeLists, (o) => {
            return o.name.toLowerCase() == this.lessonModel.type.toLowerCase()
        })
        if (typeObj) {
            this.isCallingApi = true;
            this.allSubscribers.push(this.lessonService.importFiles(typeObj).subscribe(res => {
                this.isCallingApi = false;
                this.utilService.showErrorSuccess('', "File imported Successfully.")
            }, err => {
                this.isCallingApi = false;
                this.utilService.showErrorCall(err);
            }));
        }
    }

    selectionChanged($event) {
        this.lessonModel.file = _.unionBy($event.value, 'id');
    }

    ngOnDestroy() {
        this.allSubscribers.map(value => value.unsubscribe());
    }
}
