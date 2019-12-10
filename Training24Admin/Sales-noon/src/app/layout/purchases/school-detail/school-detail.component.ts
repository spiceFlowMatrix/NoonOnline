import {
    Component,
    OnInit,
    ViewChild,
    Input,
    SimpleChanges
} from "@angular/core";
import { UtilService, PurchasesService } from "./../../../shared";
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

@Component({
    selector: "noon-school-detail",
    templateUrl: "./school-detail.component.html",
    styleUrls: ["./../purchases.component.scss"]
})
export class SchoolDetailComponent implements OnInit {
    allSubscribers: Array<any> = [];
    focus$ = new Subject<string>();
    selectedAccount: any = {};

    @ViewChild("manageSchoolDetailForm") manageSchoolDetailForm: any;

    @Input("data")
    public schoolDetailModel: any = {
        registernumber: this.utilService.getRandomCode("RGN")
    };
    isCallingApi: boolean;

    constructor(
        public utilService: UtilService,
        public purchasesService: PurchasesService
    ) {}

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

    onFileSelected($event, key) {
        this.schoolDetailModel[key] = $event.target.files;
    }

    public getDetail() {
        return this.schoolDetailModel;
    }

    ngOnDestroy() {
        this.allSubscribers.map(value => value.unsubscribe());
    }
}
