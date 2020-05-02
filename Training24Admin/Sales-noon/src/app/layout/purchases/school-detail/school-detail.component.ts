import {
    Component,
    OnInit,
    ViewChild,
    Input,
    SimpleChanges
} from "@angular/core";
import { UtilService, PurchasesService, FileService } from "./../../../shared";
import * as _ from "lodash";
import { Observable, Subject, of, merge } from "rxjs";
import {
    catchError,
    debounceTime,
    distinctUntilChanged,
    map,
    tap,
    switchMap
} from "rxjs/operators";
import { HttpEventType } from "@angular/common/http";

@Component({
    selector: "noon-school-detail",
    templateUrl: "./school-detail.component.html",
    styleUrls: ["./../purchases.component.scss"]
})
export class SchoolDetailComponent implements OnInit {
    allSubscribers: Array<any> = [];
    focus$ = new Subject<string>();
    selectedAccount: any = {};
    public fileModal: any = {};
    @ViewChild("manageSchoolDetailForm") manageSchoolDetailForm: any;

    @Input("data")
    public schoolDetailModel: any = {
        registernumber: this.utilService.getRandomCode("RGN")
    };
    isCallingApi: boolean;

    constructor(
        public utilService: UtilService,
        public purchasesService: PurchasesService,
        public fileService: FileService
    ) { }

    ngOnInit() {
        this.utilService.allowOnlyNumber("numberofstudentmale");
        this.utilService.allowOnlyNumber("numberofstudentfemale");
        this.utilService.allowOnlyNumber("numberofteachermale");
        this.utilService.allowOnlyNumber("numberofteacherfemale");
        this.utilService.allowOnlyNumber("numberofstaffmale");
        this.utilService.allowOnlyNumber("numberofstafffemale");
        this.utilService.allowOnlyNumber("computers");
        this.utilService.allowOnlyNumber("monitors");
        this.utilService.allowOnlyNumber("routers");
        this.utilService.allowOnlyNumber("dongles");
    }

    ngAfterViewInit() {
        setTimeout(() => {
            this.purchasesService.getDetailInList(true).subscribe(res => {
                this.purchasesService.schoolData = res.data;
            });
        }, 1000);
    }

    ngOnChanges(changes: SimpleChanges) {
        if (!this.schoolDetailModel) {
            this.schoolDetailModel = {
                registernumber: this.utilService.getRandomCode("RGN")
            };
        }
    }

    focusOnAccountSelection() {
        setTimeout(() => {
            document.getElementById("scholldetailInput").focus();
        }, 500);
    }

    formatter = (x: { registerNumber: string }) => x.registerNumber;
    rformatter = (result: any) => result.registerNumber;
    searchUser = (text$: Observable<any>) => {
        const debouncedText$ = text$.pipe(
            debounceTime(400),
            distinctUntilChanged()
        );
        const inputFocus$ = this.focus$;
        return merge(debouncedText$, inputFocus$).pipe(
            tap(() => (this.isCallingApi = true)),
            switchMap(term => {
                return this.purchasesService
                    .searchUsers(
                        {
                            search: term
                        },
                        true
                    )
                    .pipe(
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

    accountSelected($event) {
        this.selectedAccount = _.clone($event.item);
        this.getSchoolDetail($event.item);
    }

    reset() {
        this.selectedAccount = {};
        this.schoolDetailModel = {};
        this.schoolDetailModel.registernumber = this.utilService.getRandomCode("RGN");
    }

    getSchoolDetail(obj) {
        this.isCallingApi = true;
        this.allSubscribers.push(
            this.purchasesService.getSchoolDetailById(obj.id).subscribe(
                res => {
                    this.schoolDetailModel = res.data;
                    if (this.selectedAccount) {
                        this.schoolDetailModel.account = _.clone(
                            this.selectedAccount
                        );
                    }
                    this.isCallingApi = false;
                },
                err => {
                    this.isCallingApi = false;
                    this.utilService.showErrorCall(err);
                }
            )
        );
    }

    onFileSelected($event, key,newKey) {
        this.schoolDetailModel[key] = $event.target.files;
        if (!this.schoolDetailModel[newKey])
        this.schoolDetailModel[newKey] = [];
        for (let i = 0; i < $event.target.files.length; i++) {
            this.fileModal = {};
            this.isCallingApi = true;
            let modal = {
                fileTypeId: 1,
                contentType: $event.target.files[i].type,
                fileName: $event.target.files[i].name
            }
            let reader: any = new FileReader();
            reader.readAsBinaryString($event.target.files[i]);
            reader.onloadend = () => {
                var count = reader.result.match(/\/Type[\s]*\/Page[^s]/g).length;
                this.fileModal.totalpages = count.toString();
                this.fileModal.fileTypeId = "1";
            }
            this.allSubscribers.push(this.purchasesService.getSchoolDocumentFileSigned(modal).subscribe((res) => {
                this.fileModal.filename = res.data.filename;
                this.schoolDetailModel[newKey].push(res.data.filename);
                let signUrl = res.data.signedurl;
                this.fileService.putFileOnBucket(signUrl, $event.target.files[i]).subscribe((res: any) => {
                    switch (res.type) {
                        case HttpEventType.Response:
                            this.isCallingApi = false;
                    }
                }, err => {
                    this.isCallingApi = false;
                    this.utilService.showErrorCall(err);
                })
            }))
        }
    }

    public getDetail() {
        return this.schoolDetailModel;
    }

    ngOnDestroy() {
        this.allSubscribers.map(value => value.unsubscribe());
    }
}
