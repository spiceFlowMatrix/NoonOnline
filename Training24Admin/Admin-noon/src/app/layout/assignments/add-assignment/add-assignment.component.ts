import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router, ActivatedRoute, Params } from '@angular/router';
import {
    UtilService,
    AssignmentService,
    FileService,
    UsersService,
    ChapterService,
    DELETE_TITLE,
    DELETE_MESSAGE,
    DataService
} from '../../../shared';
import * as _ from 'lodash';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Observable, Subject, of, merge } from 'rxjs';
import { catchError, debounceTime, distinctUntilChanged, map, tap, switchMap } from 'rxjs/operators';
import { HttpEvent, HttpEventType } from '@angular/common/http';
import { FileHandle } from '../../../shared/modules/directives';

@Component({
    selector: 'app-add-assignment',
    templateUrl: './add-assignment.component.html',
    styleUrls: ['./add-assignment.component.scss']
})
export class AddAssignmentComponent implements OnInit {
    private allSubscribers: Array<any> = [];
    assignmentDropedfiles: FileHandle[] = [];
    @ViewChild('manageAssignmentForm') manageAssignmentForm: NgForm;
    @ViewChild('fileDialog') fileDialog: any;
    @ViewChild('assignWorkDialog') assignWorkDialog: any;
    @ViewChild('fileUploadStatusDialog') fileUploadStatusDialog: any;
    @ViewChild('listdialog') listCommonDialog: any;
    @ViewChild('addallFileDialog') addallFileDialog: any;

    public assignmentModel: any = {};
    public addAssignmentToUserModel: any = {};
    isEditView: boolean = false;
    isCallingApi: boolean = false;
    chapterList: any = [];
    studentsList: any = [];
    studentByAssignmentList = [];
    fileList: any = [];
    uploadedPercentage: number;
    flag: boolean = false;
    addallFileModalRef: any = [];
    uploadModalRef: any = null;
    public assignModal: any = {};
    assignmentfiles: any = [];
    tempfiles: any = [];
    filesid: any = [];
    showAssignmentProgress: boolean = false;
    mode = "buffer";
    signUrl: any;
    constructor(
        public modalService: NgbModal,
        private activatedRoute: ActivatedRoute,
        public utilService: UtilService,
        public usersService: UsersService,
        public assignmentService: AssignmentService,
        public chapterService: ChapterService,
        public fileService: FileService,
        public dataService: DataService,
        public router: Router
    ) {
        this.allSubscribers.push(this.activatedRoute.params.subscribe((params: Params) => {
            if (params['assignid']) {
                this.isEditView = true;
                this.assignmentModel.id = params['assignid'];
                this.getAssignmentById(this.assignmentModel.id);
            } else {
                this.assignmentModel.code = this.utilService.getRandomCode('AS');
                this.isEditView = false;
            }
        }));
        this.allSubscribers.push(this.activatedRoute.queryParams.subscribe((params: Params) => {
            if (params['chapterid'] && params['chaptername'] && params['courseid']) {
                this.assignmentModel.chaptername = params['chaptername'];
                this.assignmentModel.chapterId = params['chapterid'];
                // this.lessonModel.type = params['type'] ? params['type'] : '';
                this.assignmentModel.courseid = params['courseid'] ? params['courseid'] : '';
            }
            // else {
            //     this.router.navigateByUrl('/courses')
            // }
        }));
    }

    ngOnInit() {
        // this.getChapters();
        // setTimeout(() => {
        //     this.getStudentByAssignmentList();
        // }, 1000);
    }

    formatter = (x: { name: string }) => x.name;
    search = (text$: Observable<any>) => {
        const debouncedText$ = text$.pipe(debounceTime(300), distinctUntilChanged());
        return merge(debouncedText$).pipe(
            tap(() => this.isCallingApi = true),
            switchMap(term => {
                let filterObj = {
                    search: term
                }
                return this.fileService.searchFiles(filterObj).pipe(
                    tap(() => this.isCallingApi = false),
                    catchError(() => {
                        this.isCallingApi = false;
                        return of([]);
                    }))
            }),
            tap(() => this.isCallingApi = false)
        )
    }

    openFileModal() {
        this.fileDialog.openModal();
    }
    openAllFileModal() {
        document.getElementById('assignfile').click();

        // this.flag = true;
        // this.addallFileModalRef = this.modalService.open(this.addallFileDialog, { backdrop: "static" });
    }

    assignmentFileDrop(files: FileHandle[]): void {
        this.assignModal = {};
        let assignfiles = [];
        if (!this.assignmentModel.assignmentfiles)
            this.assignmentModel.assignmentfiles = [];
        // let files = []
        let tempFiles = [];
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
                this.assignmentService.getChapterAssignmentFileSigned(modal).subscribe(async (res) => {
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
                this.assignmentService.getChapterAssignmentFileSigned(modal).subscribe(async (res) => {
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
                this.assignmentService.getChapterAssignmentFileSigned(modal).subscribe(async (res) => {
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
                this.assignmentService.getChapterAssignmentFileSigned(modal).subscribe(async (res) => {
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
                this.assignmentService.getChapterAssignmentFileSigned(modal).subscribe(async (res) => {
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
                this.assignmentService.getChapterAssignmentFileSigned(modal).subscribe(async (res) => {
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
    manageAssignFile() {
        this.isCallingApi = true;
        this.allSubscribers.push(this.fileService.multipleFile(this.assignModal).subscribe((res: any) => {
            switch (res.type) {
                case HttpEventType.Sent:
                    this.uploadedPercentage = 0;
                    // this.uploadModalRef = this.modalService.open(this.fileUploadDialog, {
                    //     backdrop: 'static',
                    //     size: 'lg'
                    // });
                    // this.fileUploadStatusDialog.openModal();
                    this.showAssignmentProgress = true;
                    this.utilService.showInfoToast("Notification", "Your file upaloding started.");
                    break;
                case HttpEventType.Response:
                    // this.uploadModalRef.dismiss();
                    this.isCallingApi = false;
                    this.showAssignmentProgress = false;
                    this.mode = "buffer";
                    // this.fileUploadStatusDialog.closeModal();
                    // if (this.flag) {
                    //     this.addallFileModalRef.dismiss();
                    //     this.flag = false;
                    // }
                    for (let i = 0; i < res.body.data.length; i++) {
                        this.filesid.push(res.body.data[i].id);
                        this.assignmentfiles.push(res.body.data[i]);
                        this.tempfiles.push(res.body.data[i]);
                        // this.assignmentModel.filename.push(res.body.data[i]);
                    }
                    this.assignmentModel.assignmentfiles = this.assignmentfiles;
                    this.utilService.showInfoToast("", "File uploaded successfully.");
                    break;
                case 1: {
                    if (Math.round(this.uploadedPercentage) !== Math.round(event['loaded'] / event['total'] * 100)) {
                        this.uploadedPercentage = event['loaded'] / event['total'] * 100;
                        // console.log(Math.round(this.uploadedPercentage));
                        if (this.uploadedPercentage == 100) {
                            this.mode = "indeterminate";
                        }
                    }
                    break;
                }
            }
        }, err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err);
        }));
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
                    // if (i === event.target.files.length - 1) {
                    this.showAssignmentProgress = false;
                    // this.isCallingApi = false;
                    // this.isCallingApi = true;
                    // }
                    // setTimeout(() => {
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
    assignmentfileSelected(event) {
        this.assignModal = {};
        if (!this.assignmentModel.assignmentfiles)
            this.assignmentModel.assignmentfiles = [];
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
                this.assignmentService.getChapterAssignmentFileSigned(modal).subscribe(async (res) => {
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
                this.assignmentService.getChapterAssignmentFileSigned(modal).subscribe(async (res) => {
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
                this.assignmentService.getChapterAssignmentFileSigned(modal).subscribe(async (res) => {
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
                this.assignmentService.getChapterAssignmentFileSigned(modal).subscribe(async (res) => {
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
                this.assignmentService.getChapterAssignmentFileSigned(modal).subscribe(async (res) => {
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
                this.assignmentService.getChapterAssignmentFileSigned(modal).subscribe(async (res) => {
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
            this.assignmentModel.assignmentfiles.push(fileModal);
            this.filesid.push(res.data.id);
            this.assignmentfiles.push(res.data);
            this.tempfiles.push(res.data);
            this.utilService.showInfoToast("", "File uploaded successfully.");
            // }
        }, err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err);
        }));
    }

    getChapters() {
        this.allSubscribers.push(this.chapterService.getChapters().subscribe(res => {
            this.chapterList = res.data;
        }));
    }

    getAssignmentById(id) {
        this.isCallingApi = true;
        this.allSubscribers.push(this.assignmentService.getAssignmentById(id).subscribe(res => {
            this.isCallingApi = false;
            this.assignmentModel = res.data;
            this.assignmentfiles = [];
            this.filesid = []
            this.tempfiles = this.assignmentModel.assignmentfiles;
            if (this.assignmentModel.assignmentfiles && this.assignmentModel.assignmentfiles.length > 0)
                // this.assignmentModel.filename = this.assignmentModel.assignmentfiles[0].files;
                for (let i = 0; i < this.assignmentModel.assignmentfiles.length; i++) {
                    this.assignmentfiles.push(this.assignmentModel.assignmentfiles[i].files);
                    this.filesid.push(this.assignmentModel.assignmentfiles[i].files.id)
                }
            this.assignmentModel.assignmentfiles = this.assignmentfiles;
            // this.assignmentModel.fileid = this.filesid;
            // this.assignmentModel.fileId = this.assignmentModel.assignmentfiles[0].files.id;
            if (this.assignmentModel.chapter && this.assignmentModel.chapter.id)
                this.assignmentModel.chapterid = this.assignmentModel.chapter.id;

        }, err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err);
        }));
    }

    manageAssignment() {
        this.isCallingApi = true;
        // this.assignmentModel.fileid = [this.assignmentModel.filename.id];
        this.assignmentModel.key = "Edit";
        this.allSubscribers.push(this.assignmentService.manageAssignment(this.assignmentModel).subscribe((res: any) => {
            this.isCallingApi = false;

            if (!this.isEditView) {
                this.router.navigateByUrl('/courses/course-preview/' + this.assignmentModel.courseid + '/edit-assignment/' + res.data.id + window.location.search);
            }
            else {
                this.router.navigateByUrl('/courses/course-preview/' + res.data.chapter.courseid + '/edit-assignment/' + this.assignmentModel.id + window.location.search);
            }
            this.dataService.refreshCourseData({
                operation: 'add',
                type: 'chapter',
                obj: res.data
            });
            // this.router.navigate(['/assignments/assignment-list']);
        }, err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err);
        }));
    }

    DeleteFile(files, i) {
        if (this.tempfiles[i].id) {
            this.isCallingApi = true;
            this.assignmentService.deleteAssignmentFile(this.tempfiles[i].id).subscribe((res) => {
                this.isCallingApi = false;
                this.tempfiles.splice(i, 1);
                this.assignmentfiles.splice(i, 1);
                this.assignmentModel.assignmentfiles = this.assignmentfiles;
            })
        }
        else {
            this.tempfiles.splice(i, 1);
            this.assignmentfiles.splice(i, 1);
            this.assignmentModel.assignmentfiles = this.assignmentfiles;
        }
    }

    manageFile($event) {
        this.isCallingApi = true;
        this.allSubscribers.push(this.fileService.manageFile($event).subscribe((res: any) => {
            switch (res.type) {
                case HttpEventType.Sent:
                    this.fileUploadStatusDialog.openModal();
                    this.uploadedPercentage = 0;
                    this.utilService.showInfoToast("Notification", "Your file upaloding started.");
                    break;
                case HttpEventType.Response:
                    this.isCallingApi = false;
                    this.fileDialog.closeModal();
                    this.fileUploadStatusDialog.closeModal();
                    this.assignmentModel.fileid = res.body.data.id;
                    this.assignmentModel.filename = res.body.data;
                    this.utilService.showInfoToast("", "File uploaded successfully.");
                    break;
                case 1: {
                    if (Math.round(this.uploadedPercentage) !== Math.round(event['loaded'] / event['total'] * 100)) {
                        this.uploadedPercentage = event['loaded'] / event['total'] * 100;
                        console.log(Math.round(this.uploadedPercentage));
                    }
                    break;
                }
            }

        }, err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err);
        }));
    }

    getStudentByAssignmentList() {
        this.allSubscribers.push(this.assignmentService.getAssignmentsByStudentId(this.assignmentModel.id).subscribe(res => {
            this.studentByAssignmentList = res.data;
        }));
    }

    openDeleteConfirmation(studentAssign, index) {
        studentAssign.index = index;
        this.listCommonDialog.open(1, DELETE_TITLE, DELETE_MESSAGE, studentAssign);
    }

    deleteAssignmentFromStudent(data) {
        this.allSubscribers.push(this.assignmentService.deleteAssignmentFromStudent(data.assignmentStudentId).subscribe(res => {
            this.studentByAssignmentList = _.remove(this.studentByAssignmentList, (o) => {
                return !(o.assignmentStudentId == data.assignmentStudentId);
            });
        }, err => {
            this.utilService.showErrorCall(err);
        }));
    }

    openAddAssignmentUserModal(obj) {
        this.addAssignmentToUserModel = {};
        if (obj) {
            this.addAssignmentToUserModel.studentid = obj.id;
        }
        if (this.studentsList.length == 0)
            this.isCallingApi = true;
        this.uploadModalRef = this.modalService.open(this.assignWorkDialog, { backdrop: 'static', size: 'lg' });
        this.getStudents();
    }

    getStudents() {

        if (this.utilService.getRoles().indexOf('admin') < 0) {
            this.allSubscribers.push(this.usersService.getUsers({ roleid: 4 }).subscribe(res => {
                this.isCallingApi = false;
                if (res.data)
                    this.studentsList = res.data;
            }, err => {
                this.isCallingApi = false;
                this.utilService.showErrorCall(err);
            }));
        } else {
            this.allSubscribers.push(this.assignmentService.getAssignedPersonDetails({}).subscribe(res => {
                this.isCallingApi = false;
                if (res.data) {
                    this.studentsList = [];
                    for (let index = 0; index < res.data.length; index++) {
                        if (res.data[index].userDetails) {
                            for (let stuIndex = 0; stuIndex < res.data[index].userDetails.length; stuIndex++) {
                                this.studentsList.push(res.data[index].userDetails[stuIndex]);
                            }
                        }
                    }
                }
            }, err => {
                this.isCallingApi = false;
                this.utilService.showErrorCall(err);
            }));
        }
    }

    addAssignMentToUser() {
        let model = {
            "assignmentid": this.assignmentModel.id,
            "studentid": this.addAssignmentToUserModel.studentid,
        }
        this.isCallingApi = true;
        this.allSubscribers.push(this.assignmentService.addAssignmentStudent(model).subscribe(res => {
            this.isCallingApi = false;
            this.uploadModalRef.dismiss();
            this.getStudentByAssignmentList();
        }, err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err);
        }));
    }

    ngOnDestroy() {
        this.allSubscribers.map(value => value.unsubscribe());
    }
}
