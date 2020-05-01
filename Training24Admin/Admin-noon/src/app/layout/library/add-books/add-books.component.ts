import { Component, OnInit, ViewChild, ChangeDetectorRef } from "@angular/core";
import { NgForm } from "@angular/forms";
import { Router, ActivatedRoute, Params } from "@angular/router";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";

import * as _ from "lodash";
import {
    GradeService,
    UtilService,
    CourseService,
    FileService,
    FileTypeLists,
    DataService
} from "../../../shared";
import { LibraryService } from "../../../shared/services/library.services";
import {
    catchError,
    debounceTime,
    distinctUntilChanged,
    map,
    tap,
    switchMap
} from "rxjs/operators";
import { HttpEvent, HttpEventType } from "@angular/common/http";

import { Observable, merge, of } from "rxjs";

@Component({
    selector: "app-add-books",
    templateUrl: "./add-books.component.html",
    styleUrls: ["./add-books.component.scss"]
})
export class AddBooksComponent implements OnInit {
    @ViewChild("fileDialog") fileDialog: any;
    @ViewChild("fileUploadStatusDialog") fileUploadStatusDialog: any;
    public fileModal: any = {};
    private allSubscribers: Array<any> = [];
    public bookModel: any = {};
    isEditView: boolean = false;
    isCallingApi: boolean = false;
    isGradesLoading: boolean = false;
    gradeList: any;
    subjectList: any;
    bookFile: any;
    fileList: any;
    tempfile: any;
    bookImageFile: any;
    grades: any;
    uploadedPercentage: number;
    uploadModalRef: any = null;
    config = {
        displayKey: "name",
        search: true
    };
    dropDownConfig: any = {
        displayKey: "name", //if objects array passed which key to be displayed defaults to description
        search: false, //true/false for the search functionlity defaults to false,
        height: "auto", //height of the list so that if there are more no of items it can show a scroll defaults to auto. With auto height scroll will never appear
        placeholder: "Select" // text to be displayed when no item is selected defaults to Select,
    };

    constructor(
        public gradeService: GradeService,
        public utilService: UtilService,
        public libraryService: LibraryService,
        public activatedRoute: ActivatedRoute,
        public router: Router,
        public fileService: FileService,
        public courseService: CourseService,
        public dataService: DataService,
        public change: ChangeDetectorRef
    ) {
        this.allSubscribers.push(
            this.activatedRoute.params.subscribe((params: Params) => {
                if (params["id"]) {
                    this.isEditView = true;
                    this.bookModel.id = params["id"];
                    this.getBookById(this.bookModel.id);
                } else {
                    this.isEditView = false;
                }
            })
        );
    }

    ngOnInit() {
        this.getGrades();
        this.getFiles();
        this.getSubject();
    }

    search = (text$: Observable<any>) => {
        const debouncedText$ = text$.pipe(
            debounceTime(300),
            distinctUntilChanged()
        );
        // const inputFocus$ = this.focus$;
        // return merge(debouncedText$, inputFocus$).pipe(
        return merge(debouncedText$).pipe(
            tap(() => (this.isCallingApi = true)),
            switchMap(term => {
                let filterObj = {
                    fileType: 1,
                    search: term,
                    pagenumber: 1,
                    perpagerecord: 150
                };
                return this.fileService.searchFiles(filterObj).pipe(
                    tap(() => (this.isCallingApi = false)),
                    catchError(() => {
                        this.isCallingApi = false;
                        return of([]);
                    })
                );
            }),
            tap(() => (this.isCallingApi = false))
        );

    };

    formatter = (x: { name: string }) => x.name;

    getFiles() {
        let filterObj = {
            fileType: 1,
            search: "",
            pagenumber: 1,
            perpagerecord: 150
        };
        this.allSubscribers.push(
            this.fileService.getFiles(filterObj).subscribe(res => {
                this.fileList = res.data;
            })
        );
    }

    selectFile($event) {
        this.bookModel.fileid = $event.item.id;
    }

    onGradeSelection($event) {
        let grades = [];
        $event.value.forEach(element => {
            grades.push(element.id);
        });
        this.bookModel.grades = grades;
    }

    publishBook() {
        this.isCallingApi = true;
        this.allSubscribers.push(
            this.libraryService.publishBook(this.bookModel.id).subscribe(
                (res: any) => {
                    this.isCallingApi = false;
                    this.router.navigate(["/library/book-list"]);
                },
                err => {
                    this.isCallingApi = false;
                    this.utilService.showErrorCall(err);
                }
            )
        );
    }

    getSubject() {
        this.isCallingApi = true;
        this.allSubscribers.push(
            this.courseService.getCourseDefination().subscribe(
                (res: any) => {
                    this.subjectList = res.data;

                    this.isCallingApi = false;
                },
                err => {
                    this.isCallingApi = false;
                    this.utilService.showErrorCall(err);
                }
            )
        );
    }

    getBookById(id) {
        this.isCallingApi = true;
        this.allSubscribers.push(
            this.libraryService.getBookById(id).subscribe(
                (res: any) => {
                    this.isCallingApi = false;
                    this.bookModel = _.clone(res.data);
                    this.tempfile = this.bookModel.file;
                    this.grades = this.bookModel.grades;
                },
                err => {
                    this.isCallingApi = false;
                    this.utilService.showErrorCall(err);
                }
            )
        );
    }

    getGrades() {
        this.isCallingApi = true;
        this.allSubscribers.push(
            this.gradeService.getGrades().subscribe(
                res => {
                    this.isCallingApi = false;
                    this.gradeList = res.data;
                },
                err => {
                    this.isCallingApi = false;
                    this.utilService.showErrorCall(err);
                }
            )
        );
    }
    openFileModal() {
        this.fileDialog.openModal();
    }

    fileSelected(event) {
        if (event.target.files && event.target.files.length > 0) {
            this.bookFile = event.target.files[0];
        }
    }

    manageBook() {
        this.isCallingApi = true;
        this.bookModel.key = "Edit";

        if (this.isEditView) {
            let grad: string = this.bookModel.gradeid;
            let gradarr = grad.split(",");
            this.bookModel.grades = gradarr;
        }

        this.allSubscribers.push(
            this.libraryService.manageBooks(this.bookModel).subscribe(
                (res: any) => {
                    this.isCallingApi = false;
                    this.router.navigate(["/library/book-list"]);
                },
                err => {
                    this.isCallingApi = false;
                    this.utilService.showErrorCall(err);
                }
            )
        );
    }

    uploadBookPdf(event) {
        this.fileModal = {};
        this.isCallingApi = true;
        let modal = {
            fileTypeId: 1,
            contentType: event.target.files[0].type,
            fileName: event.target.files[0].name
        }
        let reader: any = new FileReader();
        reader.readAsBinaryString(event.target.files[0]);
        reader.onloadend = () => {
            var count = reader.result.match(/\/Type[\s]*\/Page[^s]/g).length;
            this.fileModal.totalpages = count.toString();
            this.fileModal.fileTypeId = "1";
        }
        this.allSubscribers.push(this.libraryService.getLibraryPdfSigned(modal).subscribe((res) => {
            this.fileModal.filename = res.data.filename;
            let signUrl = res.data.signedurl;
            this.fileService.putFileOnBucket(signUrl, event.target.files[0]).subscribe((res: any) => {
                switch (res.type) {
                    case HttpEventType.Sent:
                        this.uploadedPercentage = 0;
                        this.fileUploadStatusDialog.openModal();
                        this.utilService.showInfoToast("Notification", "Your file upaloding started.");
                        break;
                    case HttpEventType.Response:
                        this.fileDialog.closeModal();
                        this.fileUploadStatusDialog.closeModal();
                        this.isCallingApi = false;
                        this.utilService.showInfoToast("", "File uploaded successfully.");
                        console.log(res);
                        if (event.target.files[0]) {
                            // this.file = event.target.files[i];
                            setTimeout(() => {
                                this.isCallingApi = true;
                                this.allSubscribers.push(this.fileService.SaveFileMetaData(this.fileModal).subscribe((res: any) => {
                                    this.isCallingApi = false;
                                    // for (let i = 0; i < res.body.data.length; i++) {
                                    this.bookModel.fileid = res.data.id;
                                    this.tempfile = res.data;
                                    // }
                                }, err => {
                                    this.isCallingApi = false;
                                    this.utilService.showErrorCall(err);
                                }));
                            }, 200);
                        }
                        break;
                    case 1: {
                        if (
                            Math.round(this.uploadedPercentage) !==
                            Math.round(
                                (event["loaded"] / event["total"]) * 100
                            )
                        ) {
                            this.uploadedPercentage =
                                (event["loaded"] / event["total"]) * 100;
                            console.log(
                                Math.round(this.uploadedPercentage)
                            );
                        }
                        break;
                    }
                }
            }, err => {
                this.isCallingApi = false;
                this.fileDialog.closeModal();
                this.fileUploadStatusDialog.closeModal();
                this.utilService.showErrorCall(err);
            })
        }))
    }

    onPicSelected(event) {
        if (event.target.files && event.target.files.length > 0) {
            this.bookModel.coverimage = event.target.files[0];
            var reader = new FileReader();
            reader.onload = (e: any) => {
                this.bookModel.coverurl = e.target.result;
            }
            reader.readAsDataURL(event.target.files[0]);
            let formData = new FormData();
            formData.append('file', event.target.files[0]);
            formData.append('fileTypeId', '4');
            this.isCallingApi = true;
            this.fileService.uploadCoverImage(formData)
                .subscribe((data) => {
                    if (data.status == 'Success' && data.data) {
                        this.isCallingApi = false;
                        this.bookModel.coverimage = data.data.id;
                    }
                }, err => {
                    this.isCallingApi = false;
                    this.utilService.showErrorCall(err);
                });
        }
    }
    uploadBookCover(event) {
        this.fileModal = {};
        // if (event.target.files[0])
            this.bookModel.coverimage = event.target.files[0];
        this.isCallingApi = true;
        let modal = {
            fileTypeId: 3,
            contentType: event.target.files[0].type,
            fileName: event.target.files[0].name
        }
        var reader = new FileReader();
        reader.onload = (e: any) => {
            this.bookModel.coverurl = e.target.result;
        }
        reader.readAsDataURL(event.target.files[0]);
        this.allSubscribers.push(this.libraryService.getLibraryCardSigned(modal).subscribe((res) => {
            this.fileModal.filename = res.data.filename;
            let signUrl = res.data.signedurl;
            this.bookModel.coverimage =  res.data.filename;
            this.fileService.putFileOnBucket(signUrl, event.target.files[0]).subscribe((res: any) => {
                switch (res.type) {
                    case HttpEventType.Sent:
                        this.uploadedPercentage = 0;
                        // this.fileUploadStatusDialog.openModal();
                        this.utilService.showInfoToast("Notification", "Your file upaloding started.");
                        break;
                    case HttpEventType.Response:
                        this.isCallingApi = false;
                        this.utilService.showInfoToast("", "File uploaded successfully.");
                        console.log(res);
                            // this.file = event.target.files[i];
                            // this.isCallingApi = true;
                            // this.allSubscribers.push(this.fileService.SaveFileMetaData(this.fileModal).subscribe((res: any) => {
                            //     this.isCallingApi = false;
                            //     this.bookModel.coverimage = res.data.id;
                            // }, err => {
                            //     this.isCallingApi = false;
                            //     this.utilService.showErrorCall(err);
                            // }));
                        break;
                    case 1: {
                        if (
                            Math.round(this.uploadedPercentage) !==
                            Math.round(
                                (event["loaded"] / event["total"]) * 100
                            )
                        ) {
                            this.uploadedPercentage =
                                (event["loaded"] / event["total"]) * 100;
                            console.log(
                                Math.round(this.uploadedPercentage)
                            );
                        }
                        break;
                    }
                }
            }, err => {
                this.isCallingApi = false;
                this.utilService.showErrorCall(err);
            })
        }))
    }

    manageFile($event) {
        this.isCallingApi = true;
        this.allSubscribers.push(
            this.fileService.manageFile($event).subscribe(
                (res: any) => {
                    switch (res.type) {
                        case HttpEventType.Sent:
                            this.fileUploadStatusDialog.openModal();
                            this.uploadedPercentage = 0;
                            this.utilService.showInfoToast(
                                "Notification",
                                "Your file upaloding started."
                            );
                            break;
                        case HttpEventType.Response:
                            this.isCallingApi = false;
                            this.fileDialog.closeModal();
                            this.fileUploadStatusDialog.closeModal();
                            this.bookModel.fileid = res.body.data.id;
                            this.tempfile = res.body.data;
                            // this.bookModel.filename = res.body.data;
                            this.utilService.showInfoToast(
                                "",
                                "File uploaded successfully."
                            );
                            break;
                        case 1: {
                            if (
                                Math.round(this.uploadedPercentage) !==
                                Math.round(
                                    (event["loaded"] / event["total"]) * 100
                                )
                            ) {
                                this.uploadedPercentage =
                                    (event["loaded"] / event["total"]) * 100;
                                console.log(
                                    Math.round(this.uploadedPercentage)
                                );
                            }
                            break;
                        }
                    }
                },
                err => {
                    this.isCallingApi = false;
                    this.utilService.showErrorCall(err);
                }
            )
        );
    }

    openFileSelecter(id) {
        this.utilService.openFileSelecter(id);
    }



    ngOnDestroy() {
        this.allSubscribers.map(value => value.unsubscribe());
    }
}
