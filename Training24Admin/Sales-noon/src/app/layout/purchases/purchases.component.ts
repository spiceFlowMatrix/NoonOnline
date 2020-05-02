import { Component, OnInit, ViewChild, ChangeDetectorRef } from "@angular/core";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import {
    UtilService,
    UsersService,
    PurchasesService,
    DataService,
    FileService
} from "../../shared";
import { Observable, of, merge, Subject } from "rxjs";
import {
    catchError,
    debounceTime,
    distinctUntilChanged,
    map,
    tap,
    switchMap
} from "rxjs/operators";
import { NgbDate, NgbCalendar } from "@ng-bootstrap/ng-bootstrap";
import { environment } from "./../../../environments/environment";

import * as _ from "lodash";
import * as moment from "moment";
import * as jsPDF from "jspdf";
import * as html2canvas from "html2canvas";
import { NgAnalyzeModulesHost } from "@angular/compiler";
import { SalesConfigService } from "../../shared/services/salesconfig.services";
import { formatDate } from "@angular/common";
import { locateHostElement } from "@angular/core/src/render3/instructions";
import { debug, log } from "util";
import { HttpEventType } from "@angular/common/http";
@Component({
    selector: "app-purchases",
    templateUrl: "./purchases.component.html",
    styleUrls: ["./purchases.component.scss"]
})
export class PurchasesComponent implements OnInit {
    @ViewChild("assignParentDialog") assignParentDialog: any;
    @ViewChild("assignPersonsDialog") assignPersonsDialog: any;
    @ViewChild("detailUploadDialog") detailUploadDialog: any;
    @ViewChild("manageUserForm") manageUserForm: any;
    @ViewChild("schoolDetail") schoolDetail: any;
    @ViewChild("individualDetail") individualDetail: any;
    @ViewChild("acc") acc: any;
    @ViewChild("popContent") popContent: any;
    @ViewChild("filterPopOver") filterPopOver: any;

    public filterFormSubscriber: any;
    public filterPopUpMetaData: any = {};
    public filterModel: any = {};
    public receiptTitle: string = "";
    letFileUpload: boolean = false;

    focus$ = new Subject<string>();
    parentfocus$ = new Subject<string>();
    modalRef: any;
    uploadDetailsubScriber: any;
    purchaseList: Array<any> = [];
    purchaseModel: any = {};
    public fileModal: any = {};
    purchaseSummary: any = {};
    discountPackage: any = {};
    userModel: any = {};
    parentModal: any = [{}];
    modelRef: any = null;
    search: string = "";
    isCallingApi: boolean;
    isListLoading: boolean;
    isUserNameExist: boolean;
    userRoles: any;
    parentsubscription: any = [];
    selectedStudents: any;
    isSalesAgent: boolean = false;
    isAdmin: boolean = false;
    isSalesAdmin: boolean = false;
    isUserNameChecking: boolean;
    isIndividualDetailUploading: boolean;
    parentalControl: boolean = false;
    allSubscribers: Array<any> = [];
    singedReportFile: any;
    isSelected: any;
    pdfdata: any = {};
    temparrray: any = [];
    managementInfo: any = {};
    individualUploadingIndex: number;
    taxValue: any;
    userid: any;
    agentCommission: any;
    erpToken: any;
    journalList: any;
    officeList: any;
    currencyList: any;
    financeDetail: any;
    salesAgentDrComission: any;
    totalCrIncome: any;
    agentCurrency: any;
    totalDrAsset: any;
    totalCrTax: any;
    totalDrTaxExp: any;
    salesAgentName: any;
    parentIndex: any;
    taxexpense: '';
    taxpayable: '';
    asset: '';
    comission: '';
    income: '';
    voucherNo: any;
    file: any;
    currencyDetail = {
        currencyId: 0,
        currencyCode: ''
    }
    tempModal: any = [];
    students: any;
    constructor(
        public utilService: UtilService,
        public purchasesService: PurchasesService,
        public dataService: DataService,
        public usersService: UsersService,
        public fileService: FileService,
        public modalService: NgbModal,
        public changeDetectorRef: ChangeDetectorRef,
        public salesConfigService: SalesConfigService
    ) {
        this.filterModel.isfilterneeded = false;
        this.receiptTitle = environment.receiptTitle;
        this.userid = localStorage.getItem('userid');
    }

    ngOnInit() {
        this.dataService.setSideMenu(true);
        this.resetInitdata();
        this.checkForAgent();
        this.getAccountData();
        // this.generatePdf();
    }

    getAccountData() {
        this.isCallingApi = true;
        this.allSubscribers.push(this.salesConfigService.getAccountsData().subscribe((res) => {
            for (let i = 0; i < res.data.length; i++) {
                if (res.data[i].type == 1) {
                    this.income = res.data[i].accountcode;
                }
                if (res.data[i].type == 2) {
                    this.comission = res.data[i].accountcode;
                }
                if (res.data[i].type == 3) {
                    this.asset = res.data[i].accountcode;
                }
                if (res.data[i].type == 4) {
                    this.taxpayable = res.data[i].accountcode;
                }
                if (res.data[i].type == 5) {
                    this.taxexpense = res.data[i].accountcode;
                }
            }
            this.isCallingApi = false;
            this.getTax();
        }, err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err);
        }))
    }

    getTax() {
        this.isCallingApi = true;
        this.allSubscribers.push(
            this.purchasesService.getTax().subscribe(
                res => {
                    this.taxValue = res.data;
                    this.isCallingApi = false;
                    this.getManagementInfo();
                },
                err => {
                    this.isCallingApi = false;
                    this.utilService.showErrorCall(err);
                }
            )
        );
    }

    checkForAgent() {
        this.userRoles = this.utilService.getRole();
        this.isSalesAgent = _.find(this.userRoles, (o: any) => {
            return o == "sales_agent";
        })
            ? true
            : false;
        this.isSalesAdmin = _.find(this.userRoles, (o: any) => {
            return o == "sales_admin";
        })
            ? true
            : false;
        this.isAdmin = _.find(this.userRoles, (o: any) => {
            return o == "admin";
        })
            ? true
            : false;
        if (this.isSalesAgent) {
            this.getAgentDetail();
        }
    }

    getAgentDetail() {
        this.isCallingApi = true;
        this.allSubscribers.push(this.purchasesService.getAgentDetail().subscribe((res) => {
            this.agentCommission = res.data.commission;
            this.agentCurrency = res.data.currency;
            this.isCallingApi = false;
        }, err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err)
        }))
    }



    getManagementInfo() {
        this.isCallingApi = true;
        this.usersService.getManagementInfo().subscribe(
            res => {
                this.isCallingApi = false;
                if (res.data) this.managementInfo = res.data;
            },
            err => {
                this.isCallingApi = false;
                this.utilService.showErrorCall(err);
            }
        );
    }

    initForm() {
        let data = _.clone(this.dataService.getFilterData());
        Object.keys(data).forEach(key => {
            this.filterPopUpMetaData[key] = data[key];
        });
    }

    generatePdf() {
        html2canvas(document.getElementById("pdf_elements")).then(canvas => {
            // var imgData = canvas.toDataURL("image/png");
            this.pdfdata = canvas.toDataURL("image/png");
            var doc = new jsPDF();
            doc.addImage(this.pdfdata, "JPEG", 0, 0);
            doc.save("receipt_" + new Date().getTime() + ".pdf");
        });
    }

    resetInitdata() {
        this.purchaseModel = {};
        this.purchaseModel.individualdetails = [
            {
                studentcode: this.utilService.getRandomCode("SC")
            }
        ];
        this.purchaseModel.schooldetails = {
            registernumber: this.utilService.getRandomCode("RGN")
        };
        this.purchaseModel.subscriptions = [];
        this.purchaseModel.courseSubscriptions = [];
        this.purchaseModel.metadatadetails = [];
        this.purchaseModel.servicedetails = [];
        this.purchaseModel.packagedetails = [];

        // this.purchaseModel = {
        //     subscriptions: [],
        //     courseSubscriptions: [],
        //     metadatadetails: {},
        //     individualdetails: [
        //         {
        //             studentcode: this.utilService.getRandomCode("SC")
        //         }
        //     ],
        //     schooldetails: {
        //         registernumber: this.utilService.getRandomCode("RGN")
        //     }
        // };
        this.isSelected = null;
        this.addTempSubscription();
    }

    ngAfterViewInit() {
        setTimeout(() => {
            this.getPurchasesList(this.filterModel);
            // this.getSummary(this.filterModel);
            this.usersService
                .getUsers({ roleid: 4, search: "" })
                .subscribe(res => {
                    this.usersService.setUsers(res.data);
                });
        }, 500);
    }

    // getSummary(data: any) {
    //     this.isListLoading = true;
    //     this.allSubscribers.push(
    //         this.purchasesService.getSummary(_.clone(data)).subscribe(
    //             res => {
    //                 this.isListLoading = false;
    //                 this.purchaseSummary = res.data;
    //                 this.getPurchasesList(this.filterModel);
    //             },
    //             err => {
    //                 this.isListLoading = false;
    //                 this.utilService.showErrorCall(err);
    //             }
    //         )
    //     );
    // }

    getPurchasesList(data) {
        this.isListLoading = true;
        this.allSubscribers.push(
            this.purchasesService.getPurchaseList(_.clone(data)).subscribe(
                res => {
                    this.isListLoading = false;
                    // this.purchaseList = res.data;
                    if (!data.isfilterneeded) {
                        this.purchaseList = this.purchaseList.concat(
                            this.purchaseList,
                            res.data
                        );
                        this.purchaseList = _.uniqBy(this.purchaseList, "id");
                    } else {
                        this.purchaseList = res.data;
                    }
                },
                err => {
                    this.isListLoading = false;
                    this.utilService.showErrorCall(err);
                }
            )
        );
    }

    getPurchaseDetail(i, purchase, isPdfcall?: boolean) {
        this.isCallingApi = true;
        if (i || i == 0) this.isSelected = i;
        this.pdfdata = {};
        this.allSubscribers.push(
            this.purchasesService.getPurchaseDetail(purchase.id).subscribe(
                res => {
                    if (res.data.metadatadetails.saleagentname) {
                        this.salesAgentName = res.data.metadatadetails.saleagentname;
                    }
                    let temp = [];
                    if (res.data.subscriptions && res.data.subscriptions.userids && res.data.subscriptions.userids.length != 0) {
                        for (let i = 0; i < res.data.subscriptions.userids.length; i++) {
                            temp.push(res.data.subscriptions.userids[i].username)
                        }
                        this.students = temp.join();
                    }

                    this.isCallingApi = false;
                    if (isPdfcall) {
                        this.pdfdata.studentname = [];
                        this.pdfdata.id = purchase.id;
                        if (
                            res.data.subscriptions &&
                            res.data.subscriptions.userids
                        ) {
                            for (
                                let index = 0;
                                index < res.data.subscriptions.userids.length;
                                index++
                            ) {
                                this.pdfdata.studentname.push(
                                    res.data.subscriptions.userids[index]
                                        .username
                                );

                                // this.purchaseModel.subscriptions.push({
                                //     userids:
                                //         res.data.subscriptions.userids[index]
                                // });
                            }
                        }
                        this.pdfdata.studentname = this.pdfdata.studentname.join(
                            ", "
                        );
                        this.pdfdata.grades = [];
                        if (res.data.metadatadetails.packageid) {
                            this.pdfdata.packagedesc = this.purchaseModel[
                                "packagedetails"
                            ].name;
                        }
                        if (res.data.metadatadetails.serviceids) {
                            // this.pdfdata.servicedetails = this.purchaseModel["servicedetails"];
                            let temp = [];
                            this.purchaseModel["servicedetails"].forEach(
                                element => {
                                    temp.push(element.name);
                                }
                            );
                            this.pdfdata.servicedetails = temp.join(",");
                            // this.pdfdata.servicedetails = temp
                        }
                        if (res.data.metadatadetails.courseids) {
                            for (
                                let index = 0;
                                index <
                                res.data.metadatadetails.courseids.length;
                                index++
                            ) {
                                this.pdfdata.grades.push(
                                    res.data.metadatadetails.courseids[index]
                                        .gradeName
                                );
                            }
                        }
                        this.pdfdata.grades = _.uniq(this.pdfdata.grades);
                        this.pdfdata.grades = this.pdfdata.grades.join(", ");

                        this.pdfdata.package =
                            res.data.metadatadetails.discountpackagename;
                        // this.pdfdata.packagedesc = 'N/A';
                        // this.pdfdata.tax = 'N/A';
                        this.pdfdata.start_date = moment(
                            res.data.metadatadetails.enrollmentfromdate
                        ).format("DD-MM-YYYY");
                        this.pdfdata.end_date = moment(
                            res.data.metadatadetails.enrollmenttodate
                        ).format("DD-MM-YYYY");
                        this.pdfdata.paid = "N/A";
                        this.pdfdata.sales_agent =
                            res.data.metadatadetails.saleagentname;
                        this.pdfdata.total = this.purchaseModel.final_price;
                        if (res.data.metadatadetails.subscriptiontypeid == 1) {
                            let detailInfoElem = document.getElementById(
                                "detailInfo"
                            );
                            if (detailInfoElem) {
                                detailInfoElem.innerHTML = this.managementInfo.individual_receipt_notes;
                            }
                        } else {
                            let detailInfoElem = document.getElementById(
                                "detailInfo"
                            );
                            if (detailInfoElem) {
                                detailInfoElem.innerHTML = this.managementInfo.school_receipt_notes;
                            }
                        }
                        let backgroundInfoElem = document.getElementById(
                            "backgroundInfo"
                        );
                        if (backgroundInfoElem) {
                            backgroundInfoElem.innerHTML = this.managementInfo.noon_background;
                        }
                        setTimeout(() => {
                            this.generatePdf();
                            setTimeout(() => {
                                this.pdfdata = {};
                            }, 500);
                        }, 500);
                    } else {
                        this.purchaseModel = _.clone(res.data);
                        this.purchaseModel.categoryId =
                            res.data.metadatadetails.subscriptiontypeid;
                        this.purchaseModel.subscriptions = [];
                        if (
                            !res.data.subscriptions ||
                            (res.data.subscriptions &&
                                (!res.data.subscriptions.userids ||
                                    (res.data.subscriptions.userids &&
                                        res.data.subscriptions.userids.length <
                                        1))) ||
                            (res.data.subscriptions &&
                                !res.data.subscriptions.id)
                        ) {
                            this.addTempSubscription();
                        } else {
                            for (
                                let index = 0;
                                index < res.data.subscriptions.userids.length;
                                index++
                            ) {
                                this.purchaseModel.subscriptions.push({
                                    userids:
                                        res.data.subscriptions.userids[index]
                                });
                            }
                        }
                        this.acc.toggle("subscriptions_panel");
                        this.purchaseModel.total_subscription = this.countValidSubscription().count;
                        this.calculateFinalPrice();
                    }
                },
                err => {
                    this.isCallingApi = false;
                    this.utilService.showErrorCall(err);
                }
            )
        );
    }

    formatter = (x: { username: string }) => x.username;
    rformatter = (result: any) => result.username;
    searchUser = (text$: Observable<any>) => {
        const debouncedText$ = text$.pipe(
            debounceTime(200),
            distinctUntilChanged()
        );
        const inputFocus$ = this.focus$;
        return merge(debouncedText$, inputFocus$).pipe(
            tap(() => (this.isCallingApi = true)),
            switchMap(term => {
                return this.usersService
                    .searchUsers(
                        {
                            search: term,
                            roleid: 4
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
    pformatter = (x: { username: string }) => x.username;
    parentformatter = (result: any) => result.username;
    searchParent = (text$: Observable<any>) => {
        const debouncedText$ = text$.pipe(
            debounceTime(200),
            distinctUntilChanged()
        );
        const inputParentFocus$ = this.parentfocus$;
        return merge(debouncedText$, inputParentFocus$).pipe(
            tap(() => (this.isCallingApi = true)),
            switchMap(term => {
                return this.usersService
                    .searchUsers(
                        {
                            search: term,
                            roleid: 18
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

    focusOnAccountSelection(index) {
        console.log(index);
        setTimeout(() => {
            document.getElementById("typeahead-focus_" + index).focus();
        }, 500);
    }
    focusOnParentAccountSelection(index) {
        console.log(index);
        setTimeout(() => {
            document.getElementById("typeahead-focus_parent_" + index).focus();
        }, 300);
    }

    focusOnParentDropDown(i) {
        console.log(i);
        setTimeout(() => {
            for (
                let index = 0;
                index < this.parentModal.length;
                index++
            ) {
                let element: HTMLElement = document.querySelector(
                    "#parentAccountSelection_" + index + " ngb-typeahead-window"
                );
                if (element)
                    if (i == index) {
                        element.style.display = "block";
                    } else {
                        element.style.display = "none";
                    }
            }
        }, 300);
    }
    focusOnDropDown(i) {
        setTimeout(() => {
            for (
                let index = 0;
                index < this.purchaseModel.subscriptions.length;
                index++
            ) {
                let element: HTMLElement = document.querySelector(
                    "#accountSelection_" + index + " ngb-typeahead-window"
                );
                if (element)
                    if (i == index) {
                        element.style.display = "block";
                    } else {
                        element.style.display = "none";
                    }
            }
        }, 300);
    }

    categoryChanged($event) {
        this.purchaseModel.categoryId = $event.categoryId;
    }

    discountPackageChangeEvent($event) {
        this.calculateFinalPrice();
        this.purchaseModel.metadatadetails = $event.data;
    }

    saveMetaDataEvent($event) {
        console.log($event);

        // let enrolldate=moment($event.enrollmentfromdate['year']+'-'+($event.enrollmentfromdate["month"])+'-'+$event.enrollmentfromdate["day"],"YYYY-MM-DD").format("YYYY-MM-DDThh:mm:ss:sss")+"Z";
        // let enddate=moment($event.enrollmenttodate['year']+'-'+($event.enrollmenttodate["month"])+'-'+$event.enrollmenttodate["day"],"YYYY-MM-DD").format("YYYY-MM-DDThh:mm:ss:sss")+"Z";
        let enrolldate = new Date();
        enrolldate.setFullYear($event.enrollmentfromdate["year"]);
        enrolldate.setMonth($event.enrollmentfromdate["month"] - 1);
        enrolldate.setDate($event.enrollmentfromdate["day"]);
        let enddate = new Date();
        enddate.setFullYear($event.enrollmenttodate["year"]);
        enddate.setMonth($event.enrollmenttodate["month"] - 1);
        enddate.setDate($event.enrollmenttodate["day"]);

        this.isCallingApi = true;
        let model = {
            id: this.purchaseModel.metadatadetails
                ? this.purchaseModel.metadatadetails.id
                : undefined,
            courseids: _.map($event.courseids, "id"),
            packageid: $event.packageid,
            serviceids: $event.serviceids,
            discountpackageid: $event.discountpackageid,
            // salesagentid: $event.salesagentid,
            enrollmentfromdate: enrolldate.toISOString(),
            //  enrolldate,
            // typeof $event.enrollmentfromdate == "string"
            // ? $event.enrollmentfromdate
            // : new Date(
            //       $event.enrollmentfromdate["year"],
            //       $event.enrollmentfromdate["month"] - 1,
            //       $event.enrollmentfromdate["day"]
            //   ).toISOString(),

            enrollmenttodate: enddate.toISOString(),
            // typeof $event.enrollmenttodate == "string"
            //     ? $event.enrollmenttodate
            //     : new Date(
            //           $event.enrollmenttodate["year"],
            //           $event.enrollmenttodate["month"] - 1,
            //           $event.enrollmenttodate["day"]
            //       ).toISOString(),
            subscriptiontypeid: $event.subscriptiontypeid,
            noofmonths: $event.noofmonths
        };

        //    let tmp= new Date(
        //         $event.enrollmentfromdate["year"],
        //         $event.enrollmentfromdate["month"] - 1,
        //         $event.enrollmentfromdate["day"]
        //     ).toISOString();

        this.allSubscribers.push(
            this.purchasesService.manageSubscriptionMetada(model).subscribe(
                res => {
                    // this.getPurchasesList(this.filterModel);
                    this.isCallingApi = false;
                    this.purchaseModel.metadatadetails = res.data;

                    this.acc.toggle("subscriptions_panel");
                    if ($event.subscriptiontypeid == 1) {
                        this.purchaseModel[
                            "individualDetail"
                        ] = this.individualDetail.getDetail();
                        for (
                            let index = 0;
                            index <
                            this.purchaseModel["individualDetail"].length;
                            index++
                        ) {
                            this.purchaseModel["individualDetail"][index][
                                "metadataid"
                            ] = this.purchaseModel.metadatadetails.id;
                        }
                        this.addIndividualDetail(0);
                    } else {
                        this.purchaseModel[
                            "schoolDetail"
                        ] = this.schoolDetail.getDetail();
                        this.purchaseModel["schoolDetail"][
                            "metadataid"
                        ] = this.purchaseModel.metadatadetails.id;
                        this.addSchoolDetail();
                    }
                },
                err => {
                    this.isCallingApi = false;
                    this.utilService.showErrorCall(err);
                }
            )
        );
    }

    addIndividualDetail(index) {
        if (!this.isIndividualDetailUploading) {
            this.modalRef = this.modalService.open(this.detailUploadDialog, {
                backdrop: "static"
            });
            this.isIndividualDetailUploading = true;
        }

        if (
            this.isIndividualDetailUploading &&
            this.individualUploadingIndex ==
            this.purchaseModel["individualDetail"].length
        ) {
            this.isIndividualDetailUploading = false;
            this.modalRef.dismiss();
        }

        if (this.uploadDetailsubScriber) {
            this.uploadDetailsubScriber.unsubscribe();
        }
        this.individualUploadingIndex = index;

        this.uploadIndidualDetail(this.individualUploadingIndex)
            .then(res => {
                index++;
                this.moveToNextUpload(index);
                this.getPurchasesList(this.filterModel);
                // this.getSummary(this.filterModel);
            })
            .catch(err => {
                console.log(index, err);
                index++;
                this.moveToNextUpload(index);
            });
    }

    moveToNextUpload(index) {
        if (index < this.purchaseModel["individualDetail"].length) {
            this.addIndividualDetail(index);
        } else {
            this.isIndividualDetailUploading = false;
            this.modalRef.dismiss();
        }
    }

    uploadIndidualDetail(index) {
        return new Promise((resolve, reject) => {
            this.uploadDetailsubScriber = this.purchasesService
                .AddIndividualDetails(
                    this.purchaseModel["individualDetail"][index],
                    this.purchaseModel["individualDetail"][index]["id"]
                )
                .subscribe(
                    res => {
                        this.purchaseModel["individualDetail"][index] =
                            res.data;
                        resolve(res);
                    },
                    err => {
                        reject(err);
                    }
                );
        });
    }

    addSchoolDetail() {
        this.isCallingApi = true;
        this.purchasesService
            .AddSchoolDetails(
                this.purchaseModel["schoolDetail"],
                this.purchaseModel["schoolDetail"]["id"]
            )
            .subscribe(
                res => {
                    this.isCallingApi = false;
                },
                err => {
                    this.isCallingApi = false;
                    this.utilService.showErrorCall(err);
                }
            );
    }

    beforePanelChange($event) {
        if (
            $event.panelId == "price_summery_panel" &&
            $event.nextState &&
            (this.purchaseModel.subscriptions.length < 1 ||
                (this.purchaseModel.subscriptions.length == 1 &&
                    (!this.purchaseModel.subscriptions[0] ||
                        (this.purchaseModel.subscriptions[0] &&
                            !this.purchaseModel.subscriptions[0].userids))))
        ) {
            this.utilService.showErrorToast(
                "",
                "Please add subscriptions first..."
            );
            $event.preventDefault();
        }
    }

    courseChangeEvent($event) {
        if ($event) this.purchaseModel.courses = $event;
        this.calculateCoursesPrice(this.purchaseModel.courses);
        this.calculateFinalPrice();
    }
    packageChangeEvent(event) {
        if (event) {
            let packageModel = {
                name: event.name,
                price: event.price
            };
            this.purchaseModel["packagedetails"] = packageModel;
            this.calculatePackagePrice();
            this.calculateFinalPrice();
        } else {
            this.purchaseModel["packagedetails"] = "";
            this.calculatePackagePrice();
            this.calculateFinalPrice();
        }
    }

    additionalServiceChangeEvent(service) {
        this.parentalControl = false;
        if (service.value) {
            let servicemodel = {
                name: service.name,
                price: service.price
            };

            if (this.purchaseModel["servicedetails"]) {
                let servicedetails = this.purchaseModel["servicedetails"];
                servicedetails.push(servicemodel);
                // this.purchaseModel["servicedetails"] = servicedetails;
                this.purchaseModel["servicedetails"] = _.uniqBy(
                    servicedetails,
                    "name"
                );
                for (let i = 0; i < this.purchaseModel["servicedetails"].length; i++) {
                    if (this.purchaseModel["servicedetails"][i].name == "Parental Control") {
                        this.parentalControl = true;
                    }
                }
            } else {
                this.purchaseModel["servicedetails"] = [servicemodel];
                for (let i = 0; i < this.purchaseModel["servicedetails"].length; i++) {
                    if (this.purchaseModel["servicedetails"][i].name == "Parental Control") {
                        this.parentalControl = true;
                    }
                }
            }
        } else {
            let servicedetails = this.purchaseModel["servicedetails"];
            let index = _.findIndex(servicedetails, {
                name: service.name
            });
            servicedetails.splice(index, 1);
            // this.purchaseModel["servicedetails"] = servicedetails;
            this.purchaseModel["servicedetails"] = _.uniqBy(
                servicedetails,
                "name"
            );
            for (let i = 0; i < this.purchaseModel["servicedetails"].length; i++) {
                if (this.purchaseModel["servicedetails"][i].name == "Parental Control") {
                    this.parentalControl = true;
                }
            }
        }
        this.calculateServicePrice();
        this.calculateFinalPrice();
    }

    calculatePackagePrice() {
        this.purchaseModel.package_price = 0;
        this.purchaseModel.package_price =
            this.purchaseModel.package_price +
            parseInt(this.purchaseModel["packagedetails"].price);
    }

    calculateServicePrice() {
        this.purchaseModel.service_price = 0;
        if (this.purchaseModel["servicedetails"]) {
            for (
                let index = 0;
                index < this.purchaseModel["servicedetails"].length;
                index++
            ) {
                this.purchaseModel.service_price =
                    this.purchaseModel.service_price +
                    parseInt(this.purchaseModel["servicedetails"][index].price);
            }
        }
    }
    calculateCoursesPrice(courses) {
        this.purchaseModel.final_price = 0;
        if (courses)
            for (let index = 0; index < courses.length; index++) {
                this.purchaseModel.final_price +=
                    parseInt(courses[index].basePrice) *
                    (this.purchaseModel.metadatadetails.noofmonths
                        ? this.purchaseModel.metadatadetails.noofmonths
                        : 0);
            }

        return this.purchaseModel.final_price;
    }

    addParentUser() {
        console.log(this.parentsubscription[0]);
        this.isCallingApi = true;
        this.allSubscribers.push(this.purchasesService.saveParent(this.parentsubscription[0]).subscribe((res) => {
            console.log(res);
            this.isCallingApi = false;
        }, err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err);
        }))
        this.temparrray = [];
        this.parentModal = [{ userids: '' }];
        this.modelRef.dismiss();
    }

    addTempSubscription() {
        for (
            let index = 0;
            index < this.purchaseModel.subscriptions.length;
            index++
        ) {
            if (!this.purchaseModel.subscriptions[index].userids) {
                this.utilService.showErrorToast(
                    "Required",
                    "Please fill emply fields."
                );
                return;
            }
        }
        this.purchaseModel.subscriptions.push({});
        this.purchaseModel.total_subscription = this.countValidSubscription().count;
    }

    removeTempSubscription(index) {
        this.purchaseModel.subscriptions.pop(index, 1);
        this.purchaseModel.total_subscription = this.countValidSubscription().count;
    }
    addParent(index) {
        this.temparrray = [];
        this.parentModal = [{ userids: '' }];
        this.parentsubscription = [];
        this.parentIndex = index;
        this.selectedStudents = this.purchaseModel.subscriptions[index];
        this.isCallingApi = true;
        if (this.purchaseModel.subscriptions[index].userids.id) {
            this.allSubscribers.push(this.usersService.getParents(this.purchaseModel.subscriptions[index].userids.id).subscribe((res) => {
                console.log(res);
                this.isCallingApi = false;
                if (res.data.length != 0) {
                    this.parentModal = [{ userids: '' }];
                    this.temparrray = [];
                    this.parentsubscription = [];
                    // this.parentModal = res.data;
                    for (let i = 0; i < res.data.length; i++) {
                        this.temparrray.push(res.data[i].id)
                        this.parentModal.unshift({ userids: res.data[i] });
                    }
                    let modal = {
                        studentids: this.purchaseModel.subscriptions[index].userids.id,
                        parentsids: this.temparrray
                    }
                    this.parentsubscription.push(modal);
                }
            }))
        }
        if (this.purchaseModel.subscriptions[index].userids.parents) {
            this.parentModal = [{ userids: '' }];
            this.temparrray = [];
            this.parentsubscription = [];
            // this.parentModal = this.purchaseModel.subscriptions[index].userids.parents;
            for (let i = 0; i < this.purchaseModel.subscriptions[index].userids.parents.length; i++) {
                this.parentModal.unshift({ userids: this.purchaseModel.subscriptions[index].userids.parents[i] });
                this.temparrray.push(this.purchaseModel.subscriptions[index].userids.parents[i].id)
            }
            let modal = {
                studentids: this.purchaseModel.subscriptions[index].userids.id,
                parentsids: this.temparrray
            }
            this.parentsubscription.push(modal);
        }
        if (this.tempModal.length != 0) {
            for (let i = 0; i < this.tempModal.length; i++) {
                if (this.tempModal[i] === this.selectedStudents.userids.id) {
                    // this.utilService.showErrorToast("Duplicate Selection")
                } else {
                    this.tempModal.push(this.selectedStudents.userids.id);
                    break;
                }
            }
        } else {
            this.tempModal.push(this.selectedStudents.userids.id);
        }
        this.modelRef = this.modalService.open(this.assignParentDialog, {
            backdrop: "static",
        });
    }

    parentAccountSelected(event) {
        if (this.temparrray.length != 0) {
            for (let x = 0; x < this.temparrray.length; x++) {
                if (this.temparrray[x] === event.item.id) {
                    this.utilService.showErrorToast("Duplicate Selection");
                } else {
                    this.temparrray.push(event.item.id)
                    _.uniq(this.temparrray);
                    break;
                }
            }
        } else {
            this.temparrray.push(event.item.id);
            _.uniq(this.temparrray);
        }
        for (let i = 0; i < this.tempModal.length; i++) {
            if (this.tempModal[i] === this.selectedStudents.userids.id) {
                if (this.parentsubscription[this.parentIndex]) {
                    // for (let j = 0; j < this.parentsubscription.length; j++) {
                    if (this.parentsubscription[this.parentIndex].studentids === this.selectedStudents.userids.id) {
                        this.parentsubscription[this.parentIndex].parentsids = this.temparrray;
                        // break;
                    }
                    //  else {
                    //     let modal = {
                    //         studentids: this.tempModal[i],
                    //         parentsids: this.temparrray
                    //     }
                    //     this.parentsubscription.push(modal);
                    // break;
                    // }
                    // }
                } else {
                    let modal = {
                        studentids: this.tempModal[i],
                        parentsids: this.temparrray
                    }
                    this.parentsubscription.push(modal);
                    break;
                }

            }
        }
        console.log(this.parentsubscription);

    }

    addMoreParent(i) {
        if (this.parentModal[this.parentModal.length - 1].userids != '') {
            console.log(this.parentModal);
            this.parentModal.push({ userids: '' });
            console.log(this.parentModal);

        } else {
            this.utilService.showErrorToast("Please Select Parents")
        }
    }

    removeParent(i) {
        this.temparrray.splice(i, 1);
        if (this.parentsubscription[this.parentIndex].parentsids[i]) {
            this.isCallingApi = true;
            this.allSubscribers.push(this.purchasesService.removeParent(this.selectedStudents.userids.id, this.parentsubscription[this.parentIndex].parentsids[i]).subscribe((res) => {
                this.isCallingApi = false;
                if (this.parentsubscription[this.parentIndex].parentsids.length != 0) {
                    // this.parentsubscription.splice(this.parentIndex, 1);
                    this.parentsubscription[this.parentIndex].parentsids.splice(i, 1)
                }
                this.parentModal.splice(i, 1);
            }, err => {
                this.isCallingApi = false;
                this.utilService.showErrorCall(err);
            }))
        }
        else {
            this.parentsubscription[this.parentIndex].parentsids.splice(i, 1);
            console.log(this.parentsubscription);
            this.parentModal.splice(i, 1);


        }
    }

    accountSelected($event) {
        for (
            let index = 0;
            index < this.purchaseModel.subscriptions.length;
            index++
        ) {
            if (
                typeof this.purchaseModel.subscriptions[index].userids ==
                "object" &&
                this.purchaseModel.subscriptions[index].userids.id ==
                $event.item.id
            ) {
                this.utilService.showErrorToast(
                    "Duplicate selection",
                    "You were already selected this account."
                );
                $event.preventDefault();
                return;
            }
        }
        setTimeout(() => {
            this.purchaseModel.total_subscription = this.countValidSubscription().count;
            this.calculateFinalPrice();
        }, 500);
    }

    countValidSubscription() {
        let count = 0;
        let uIds = [];
        if (this.purchaseModel.subscriptions) {
            for (
                let index = 0;
                index < this.purchaseModel.subscriptions.length;
                index++
            ) {
                if (
                    this.purchaseModel.subscriptions[index] &&
                    this.purchaseModel.subscriptions[index].userids
                ) {
                    count++;
                    uIds.push(
                        this.purchaseModel.subscriptions[index].userids.id
                    );
                }
            }
        }

        return { count: count, ids: uIds };
    }

    addUserModal() {
        this.userModel = {};
        this.userModel.roles = [{ roleid: "4" }];
        this.modelRef = this.modalService.open(this.assignPersonsDialog, {
            backdrop: "static",
            size: "lg"
        });
    }

    addParentUserModal() {
        this.userModel = {};
        this.userModel.roles = [{ roleid: "18" }];
        this.modelRef = this.modalService.open(this.assignPersonsDialog, {
            backdrop: "static",
            size: "lg"
        });
    }

    checkUsernameExist() {
        if (this.userModel.username.length < 1) return;
        this.isUserNameChecking = true;
        this.allSubscribers.push(
            this.usersService.checkUserName(this.userModel.username).subscribe(
                res => {
                    this.isUserNameChecking = false;
                    this.isUserNameExist = !res;
                },
                err => {
                    this.isUserNameChecking = false;
                    this.utilService.showErrorCall(err);
                }
            )
        );
    }

    addUser() {
        if (this.isUserNameExist) {
            this.utilService.showErrorToast(
                "Please correct it!",
                "Username already taken."
            );
            return false;
        }
        this.isCallingApi = true;
        this.userModel.addedfrom = "sales";
        this.allSubscribers.push(
            this.usersService.manageUser(this.userModel).subscribe(
                (res: any) => {
                    this.isCallingApi = false;
                    this.modelRef.dismiss();
                    if (this.userModel.roles[0].roleid == "4") {
                        this.purchaseModel.subscriptions.push({
                            userids: res.data
                        });
                    }
                    this.calculateFinalPrice();
                },
                err => {
                    this.isCallingApi = false;
                    this.utilService.showErrorCall(err);
                }
            )
        );
    }

    saveSubscriptions() {
        let validSub: any = this.countValidSubscription();
        if (
            !this.purchaseModel.metadatadetails ||
            (this.purchaseModel.metadatadetails &&
                !this.purchaseModel.metadatadetails.id)
        ) {
            this.utilService.showErrorToast(
                "Required",
                "Please insert meta data first."
            );
            return;
        }
        let model = {
            subscriptionMetadataId: this.purchaseModel.metadatadetails.id,
            userids: validSub.ids,
        };

        this.isCallingApi = true;
        this.allSubscribers.push(
            this.purchasesService.manageUserSubscriptions(model).subscribe(
                res => {
                    this.isCallingApi = false;
                    this.purchaseModel.total_subscription = validSub.count;
                    this.calculateFinalPrice();
                },
                err => {
                    this.isCallingApi = false;
                    this.utilService.showErrorCall(err);
                }
            )
        );
    }

    calculateFinalPrice() {
        this.purchaseModel.total_base_price = null;
        this.totalCrIncome = null;
        this.totalDrAsset = null;
        this.totalCrTax = null;
        this.totalDrTaxExp = null;
        let validSub: any = _.clone(this.countValidSubscription());
        this.purchaseModel.courseSubscriptions = _.clone(
            this.purchaseModel.metadatadetails.courseids
        );

        if (
            this.purchaseModel.courseSubscriptions &&
            this.purchaseModel.courseSubscriptions.length > 0
        ) {
            for (
                let index = 0;
                index < this.purchaseModel.courseSubscriptions.length;
                index++
            ) {
                this.purchaseModel.courseSubscriptions[index][
                    "calculatedPrice"
                ] = this.purchaseModel.courseSubscriptions[index]["basePrice"];
                this.purchaseModel.courseSubscriptions[index]["validSub"] =
                    validSub.count;
            }
            this.discountPackage = _.find(this.utilService.discountPackages, {
                id: parseInt(
                    this.purchaseModel.metadatadetails.discountpackageid
                )
            });
            let totalPrice =
                this.calculateCoursesPrice(
                    this.purchaseModel.metadatadetails.courseids
                ) * validSub.count;
            if (validSub.count > 0) {
                if (this.purchaseModel.service_price) {
                    totalPrice =
                        totalPrice +
                        this.purchaseModel.service_price *
                        (validSub.count *
                            this.purchaseModel.metadatadetails.noofmonths);
                }
                if (this.purchaseModel.package_price) {
                    totalPrice =
                        totalPrice +
                        this.purchaseModel.package_price *
                        (validSub.count *
                            this.purchaseModel.metadatadetails.noofmonths);
                }
                this.purchaseModel.total_base_price = _.clone(totalPrice);
            } else {
                if (this.purchaseModel.service_price) {
                    totalPrice =
                        totalPrice +
                        this.purchaseModel.service_price *
                        this.purchaseModel.metadatadetails.noofmonths;
                }
                if (this.purchaseModel.package_price) {
                    totalPrice =
                        totalPrice +
                        this.purchaseModel.package_price *
                        this.purchaseModel.metadatadetails.noofmonths;
                }
                this.purchaseModel.total_base_price = _.clone(totalPrice);
            }

            if (this.discountPackage) {
                let singleSub;
                singleSub = totalPrice / validSub.count;
                // if (
                //     this.discountPackage.freeSubscriptions &&
                //     this.discountPackage.freeSubscriptions != "null"
                // ) {
                //     validSub.count =
                //         validSub.count - this.discountPackage.freeSubscriptions;
                // }
                if (this.discountPackage.freeSubscriptions > 0) {
                    validSub.count =
                        validSub.count - this.discountPackage.freeSubscriptions;
                    if (validSub.count == 0) {
                        totalPrice =
                            (totalPrice *
                                (validSub.count *
                                    this.discountPackage.offTotalPrice)) /
                            100;
                        this.purchaseModel.total_amount = totalPrice;
                        if (this.taxValue.tax && this.agentCommission) {
                            let agentcomissionamount = (totalPrice * this.agentCommission) / 100;
                            this.salesAgentDrComission = agentcomissionamount - (agentcomissionamount * this.taxValue.tax) / 100;
                            this.totalCrIncome = totalPrice;
                            this.totalDrAsset = totalPrice - this.salesAgentDrComission;
                            this.totalCrTax = (totalPrice * this.taxValue.tax) / 100;
                            this.totalDrTaxExp = (totalPrice * this.taxValue.tax) / 100;
                        }
                    } else {
                        totalPrice = totalPrice - singleSub;
                        let eachDiscount =
                            (totalPrice *
                                (validSub.count *
                                    this.discountPackage.offSubscriptions)) /
                            100;
                        totalPrice = totalPrice - eachDiscount;
                        totalPrice =
                            totalPrice -
                            (totalPrice * this.discountPackage.offTotalPrice) /
                            100;
                        this.purchaseModel.total_amount = totalPrice;
                        if (this.taxValue.tax && this.agentCommission) {
                            let agentcomissionamount = (totalPrice * this.agentCommission) / 100;
                            this.salesAgentDrComission = agentcomissionamount - (agentcomissionamount * this.taxValue.tax) / 100;
                            this.totalCrIncome = totalPrice;
                            this.totalDrAsset = totalPrice - this.salesAgentDrComission;
                            this.totalCrTax = (totalPrice * this.taxValue.tax) / 100;
                            this.totalDrTaxExp = (totalPrice * this.taxValue.tax) / 100;
                        }
                    }
                } else {
                    if (validSub.count == 0) {
                        totalPrice =
                            (totalPrice *
                                (validSub.count *
                                    this.discountPackage.offTotalPrice)) /
                            100;
                        this.purchaseModel.total_amount = totalPrice;
                        if (this.taxValue.tax && this.agentCommission) {
                            let agentcomissionamount = (totalPrice * this.agentCommission) / 100;
                            this.salesAgentDrComission = agentcomissionamount - (agentcomissionamount * this.taxValue.tax) / 100;
                            this.totalCrIncome = totalPrice;
                            this.totalDrAsset = totalPrice - this.salesAgentDrComission;
                            this.totalCrTax = (totalPrice * this.taxValue.tax) / 100;
                            this.totalDrTaxExp = (totalPrice * this.taxValue.tax) / 100;
                        }
                    } else {
                        // totalPrice = totalPrice - singleSub;
                        let eachDiscount =
                            (totalPrice *
                                (validSub.count *
                                    this.discountPackage.offSubscriptions)) /
                            100;
                        totalPrice = totalPrice - eachDiscount;
                        totalPrice =
                            totalPrice -
                            (totalPrice * this.discountPackage.offTotalPrice) /
                            100;
                        this.purchaseModel.total_amount = totalPrice;
                        if (this.taxValue.tax && this.agentCommission) {
                            let agentcomissionamount = (totalPrice * this.agentCommission) / 100;
                            this.salesAgentDrComission = agentcomissionamount - (agentcomissionamount * this.taxValue.tax) / 100;
                            this.totalCrIncome = totalPrice;
                            this.totalDrAsset = totalPrice - this.salesAgentDrComission;
                            this.totalCrTax = (totalPrice * this.taxValue.tax) / 100;
                            this.totalDrTaxExp = (totalPrice * this.taxValue.tax) / 100;
                        }
                    }
                }
            } else {
                // let totalPrice = this.calculateCoursesPrice(this.purchaseModel.metadatadetails.courseids) * validSub.count;
                // this.purchaseModel.total_base_price = _.clone(totalPrice);
                // let tempTotal = this.calculateCoursesPrice(this.purchaseModel.metadatadetails.courseids) * validSub.count;
                // let eachDiscount = tempTotal * (validSub.count * this.discountPackage.offSubscriptions) / 100;
                // totalPrice = tempTotal - eachDiscount;
                // totalPrice = totalPrice - (totalPrice * this.discountPackage.offTotalPrice / 100);
                this.purchaseModel.total_amount = totalPrice;
                if (this.taxValue.tax && this.agentCommission) {
                    let agentcomissionamount = (totalPrice * this.agentCommission) / 100;
                    this.salesAgentDrComission = agentcomissionamount - (agentcomissionamount * this.taxValue.tax) / 100;
                    this.totalCrIncome = totalPrice;
                    this.totalDrAsset = totalPrice - this.salesAgentDrComission;
                    this.totalCrTax = (totalPrice * this.taxValue.tax) / 100;
                    this.totalDrTaxExp = (totalPrice * this.taxValue.tax) / 100;
                }
            }
        } else {
            this.discountPackage = _.find(this.utilService.discountPackages, {
                id: parseInt(
                    this.purchaseModel.metadatadetails.discountpackageid
                )
            });
            // let totalPrice =
            //     this.calculateCoursesPrice(
            //         this.purchaseModel.metadatadetails.courseids
            //     ) * validSub.count;

            let totalPrice = 0;
            if (validSub.count > 0) {
                if (this.purchaseModel.service_price) {
                    totalPrice =
                        totalPrice +
                        this.purchaseModel.service_price *
                        (validSub.count *
                            this.purchaseModel.metadatadetails.noofmonths);
                }
                if (this.purchaseModel.package_price) {
                    totalPrice =
                        totalPrice +
                        this.purchaseModel.package_price *
                        (validSub.count *
                            this.purchaseModel.metadatadetails.noofmonths);
                }
                this.purchaseModel.total_base_price = _.clone(totalPrice);
            } else {
                if (this.purchaseModel.service_price) {
                    totalPrice =
                        totalPrice +
                        this.purchaseModel.service_price *
                        this.purchaseModel.metadatadetails.noofmonths;
                }
                if (this.purchaseModel.package_price) {
                    totalPrice =
                        totalPrice +
                        this.purchaseModel.package_price *
                        this.purchaseModel.metadatadetails.noofmonths;
                }
                this.purchaseModel.total_base_price = _.clone(totalPrice);
            }

            if (this.discountPackage) {
                let singleSub;
                singleSub = totalPrice / validSub.count;
                // if (
                //     this.discountPackage.freeSubscriptions &&
                //     this.discountPackage.freeSubscriptions != "null"
                // ) {
                //     validSub.count =
                //         validSub.count - this.discountPackage.freeSubscriptions;
                // }

                if (this.discountPackage.freeSubscriptions > 0) {
                    validSub.count =
                        validSub.count - this.discountPackage.freeSubscriptions;
                    if (validSub.count == 0) {
                        totalPrice =
                            (totalPrice *
                                (validSub.count *
                                    this.discountPackage.offTotalPrice)) /
                            100;
                        this.purchaseModel.total_amount = totalPrice;
                        if (this.taxValue.tax && this.agentCommission) {
                            let agentcomissionamount = (totalPrice * this.agentCommission) / 100;
                            this.salesAgentDrComission = agentcomissionamount - (agentcomissionamount * this.taxValue.tax) / 100;
                            this.totalCrIncome = totalPrice;
                            this.totalDrAsset = totalPrice - this.salesAgentDrComission;
                            this.totalCrTax = (totalPrice * this.taxValue.tax) / 100;
                            this.totalDrTaxExp = (totalPrice * this.taxValue.tax) / 100;
                        }
                    } else {
                        totalPrice = totalPrice - singleSub;
                        let eachDiscount =
                            (totalPrice *
                                (validSub.count *
                                    this.discountPackage.offSubscriptions)) /
                            100;
                        totalPrice = totalPrice - eachDiscount;
                        totalPrice =
                            totalPrice -
                            (totalPrice * this.discountPackage.offTotalPrice) /
                            100;
                        this.purchaseModel.total_amount = totalPrice;
                        if (this.taxValue.tax && this.agentCommission) {
                            let agentcomissionamount = (totalPrice * this.agentCommission) / 100;
                            this.salesAgentDrComission = agentcomissionamount - (agentcomissionamount * this.taxValue.tax) / 100;
                            this.totalCrIncome = totalPrice;
                            this.totalDrAsset = totalPrice - this.salesAgentDrComission;
                            this.totalCrTax = (totalPrice * this.taxValue.tax) / 100;
                            this.totalDrTaxExp = (totalPrice * this.taxValue.tax) / 100;
                        }
                    }
                } else {
                    if (validSub.count == 0) {
                        totalPrice =
                            (totalPrice *
                                (validSub.count *
                                    this.discountPackage.offTotalPrice)) /
                            100;
                        this.purchaseModel.total_amount = totalPrice;
                        if (this.taxValue.tax && this.agentCommission) {
                            let agentcomissionamount = (totalPrice * this.agentCommission) / 100;
                            this.salesAgentDrComission = agentcomissionamount - (agentcomissionamount * this.taxValue.tax) / 100;
                            this.totalCrIncome = totalPrice;
                            this.totalDrAsset = totalPrice - this.salesAgentDrComission;
                            this.totalCrTax = (totalPrice * this.taxValue.tax) / 100;
                            this.totalDrTaxExp = (totalPrice * this.taxValue.tax) / 100;
                        }
                    } else {
                        let eachDiscount =
                            (totalPrice *
                                (validSub.count *
                                    this.discountPackage.offSubscriptions)) /
                            100;
                        totalPrice = totalPrice - eachDiscount;
                        totalPrice =
                            totalPrice -
                            (totalPrice * this.discountPackage.offTotalPrice) /
                            100;
                        this.purchaseModel.total_amount = totalPrice;
                        if (this.taxValue.tax && this.agentCommission) {
                            let agentcomissionamount = (totalPrice * this.agentCommission) / 100;
                            this.salesAgentDrComission = agentcomissionamount - (agentcomissionamount * this.taxValue.tax) / 100;
                            this.totalCrIncome = totalPrice;
                            this.totalDrAsset = totalPrice - this.salesAgentDrComission;
                            this.totalCrTax = (totalPrice * this.taxValue.tax) / 100;
                            this.totalDrTaxExp = (totalPrice * this.taxValue.tax) / 100;
                        }
                    }
                }
            } else {
                // let totalPrice = this.calculateCoursesPrice(this.purchaseModel.metadatadetails.courseids) * validSub.count;
                // this.purchaseModel.total_base_price = _.clone(totalPrice);
                // let tempTotal = this.calculateCoursesPrice(this.purchaseModel.metadatadetails.courseids) * validSub.count;
                // let eachDiscount = tempTotal * (validSub.count * this.discountPackage.offSubscriptions) / 100;
                // totalPrice = tempTotal - eachDiscount;
                // totalPrice = totalPrice - (totalPrice * this.discountPackage.offTotalPrice / 100);
                this.purchaseModel.total_amount = totalPrice;
                if (this.taxValue.tax && this.agentCommission) {
                    let agentcomissionamount = (totalPrice * this.agentCommission) / 100;
                    this.salesAgentDrComission = agentcomissionamount - (agentcomissionamount * this.taxValue.tax) / 100;
                    this.totalCrIncome = totalPrice;
                    this.totalDrAsset = totalPrice - this.salesAgentDrComission;
                    this.totalCrTax = (totalPrice * this.taxValue.tax) / 100;
                    this.totalDrTaxExp = (totalPrice * this.taxValue.tax) / 100;
                }
            }
        }
    }
    generateSignableReport() {
        let model: any = this.getReceiptModel();
        if (model) {
            this.isCallingApi = true;
            this.allSubscribers.push(
                this.purchasesService.generateSignableReceipt(model).subscribe(
                    res => {
                        // this.getPurchasesList();

                        this.isCallingApi = false;
                        // if (res.data && res.data.receipturl) {
                        if (res.data) {
                            let fIndex = _.findIndex(this.purchaseList, {
                                id: this.purchaseModel.metadatadetails.id
                            });
                            if (fIndex > -1) {
                                this.purchaseList[fIndex]["status"] =
                                    res.data.status;
                            }
                            this.getPurchaseDetail(
                                null,
                                { id: model.metadataid },
                                true
                            );
                            // this.utilService.downloadFile(res.data.receipturl);
                        }
                    },
                    err => {
                        this.isCallingApi = false;
                        this.utilService.showErrorCall(err);
                    }
                )
            );
        }
    }

    selectReportFile() {
        document.getElementById("signed_report").click();
    }

    reportFileSelected(event) {
        this.file = event;
        if (event.target && event.target.files) {
            let model = this.getUploadReceiptModel(event.target.files[0]);
            // if (!this.letFileUpload) {
            //     this.getErpCredentials();
            // }
            if (model) {
                // if (this.letFileUpload) {
                console.log(model)
                this.isCallingApi = true;
                this.allSubscribers.push(
                    this.purchasesService
                        .generateSignedReceipt(model)
                        .subscribe(
                            res => {
                                this.letFileUpload = false;
                                this.isCallingApi = false;
                                let fIndex = _.findIndex(this.purchaseList, {
                                    id: this.purchaseModel.metadatadetails.id
                                });
                                if (fIndex > -1) {
                                    this.purchaseList[fIndex]["status"] =
                                        res.data.status;
                                }
                                // this.addTransaction();
                            },
                            err => {
                                this.isCallingApi = false;
                                this.utilService.showErrorCall(err);
                            }
                        )
                );
                // }
            }
        }
    }

    getErpCredentials() {
        this.isCallingApi = true;
        this.allSubscribers.push(this.salesConfigService.getCredentials().subscribe((res) => {
            this.salesConfigService.setCredential(res.data);
            this.isCallingApi = false;
            this.loginErp();
        }, err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err);
        }))
    }

    loginErp() {
        this.isCallingApi = true;
        this.allSubscribers.push(this.salesConfigService.loginErp().subscribe((result) => {
            this.erpToken = result.data.Token;
            this.isCallingApi = false;
            this.getAllJournalDetail();
        }, err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err)
        }))
    }

    getAllJournalDetail() {
        this.isCallingApi = true;
        this.allSubscribers.push(this.salesConfigService.getJournal(this.erpToken).subscribe((res) => {
            this.journalList = res.data.JournalDetailList[0];
            this.isCallingApi = false;
            this.getCurrencyList();
        }, err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err)
        }))
    }

    getCurrencyList() {
        this.isCallingApi = true;
        this.allSubscribers.push(this.salesConfigService.getCurrency(this.erpToken).subscribe((res) => {
            this.currencyList = res.data.CurrencyList;
            for (let i = 0; i < this.currencyList.length; i++) {
                if (this.agentCurrency === this.currencyList[i].CurrencyCode) {
                    this.currencyDetail.currencyId = this.currencyList[i].CurrencyId;
                    this.currencyDetail.currencyCode = this.currencyList[i].CurrencyCode;
                }
                if (this.agentCurrency === this.currencyList[i].currencyCode) {
                    this.currencyDetail.currencyId = this.currencyList[i].CurrencyId;
                    this.currencyDetail.currencyCode = this.currencyList[i].CurrencyCode;
                }
            }
            this.isCallingApi = false;
            this.getAllFinancialYear();
        }, err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err)
        }))
    }

    getAllOfficeDetail() {
        this.isCallingApi = true;
        this.allSubscribers.push(this.salesConfigService.getOffice(this.erpToken).subscribe((res) => {
            this.officeList = res.data.OfficeDetailsList[0];
            this.isCallingApi = false;
            this.addVoucher();
        }, err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err)
        }))
    }

    getAllFinancialYear() {
        this.isCallingApi = true;
        this.allSubscribers.push(this.salesConfigService.getFinanceYear(this.erpToken).subscribe((res) => {
            this.financeDetail = res.data.FinancialYearDetailList[0];
            this.isCallingApi = false;
            this.getAllOfficeDetail();
        }, err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err)
        }))
    }

    addVoucher() {
        this.isCallingApi = true;
        let date = new Date();
        let modal = {
            CurrencyId: this.currencyDetail.currencyId,
            CurrencyCode: this.currencyDetail.currencyCode,
            VoucherDate: moment(date).format('YYYY-MM-DDThh:mm:ss'),
            Description: "Sales Purchase by" + this.salesAgentName + "for" + this.students,
            JournalCode: this.journalList.JournalCode,
            JournalName: this.journalList.JournalName,
            VoucherTypeId: 2,
            OfficeId: this.officeList.OfficeId,
            OfficeName: this.officeList.OfficeName,
            FinancialYearId: this.financeDetail.FinancialYearId,
            FinancialYearName: this.financeDetail.FinancialYearName,
            IsExchangeGainLossVoucher: true,
            TimeZoneOffset: date.getTimezoneOffset(),
            CreatedDate: moment(date).format('YYYY-MM-DDThh:mm:ss'),
            ModifiedDate: moment(date).format('YYYY-MM-DDThh:mm:ss'),
        }

        this.allSubscribers.push(this.salesConfigService.addVoucher(modal, this.erpToken).subscribe((res) => {
            if (res.StatusCode != 200) {
                this.isCallingApi = false;
                this.utilService.showErrorToast(res.Message);
            }
            else {
                this.isCallingApi = false;
                this.voucherNo = res.data.VoucherDetailEntity.VoucherNo;
                // document.getElementById("signed_report").click();
                this.letFileUpload = true;
                this.reportFileSelected(this.file);
            }
        }, err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err);
        }));
    }

    addTransaction() {
        this.isCallingApi = true;
        let modal = {
            "VoucherNo": this.voucherNo,
            "VoucherTransactions": [
                {
                    "TransactionId": 0,
                    "AccountNo": parseInt(this.income),
                    "Description": "Income",
                    "Credit": this.totalCrIncome,
                    // Credit: 1000,
                    "Debit": 0,
                    "VoucherNo": this.voucherNo,
                    "IsDeleted": false
                },
                {
                    "TransactionId": 0,
                    "AccountNo": parseInt(this.comission),
                    "Description": "Comission",
                    "Credit": 0,
                    // Debit: 144,
                    "Debit": this.salesAgentDrComission,
                    "VoucherNo": this.voucherNo,
                    "IsDeleted": false
                },
                {
                    "TransactionId": 0,
                    "AccountNo": parseInt(this.asset),
                    "Description": "Asset",
                    "Credit": 0,
                    // Debit: 856,
                    "Debit": this.totalDrAsset,
                    "VoucherNo": this.voucherNo,
                    "IsDeleted": false
                },
                {
                    "TransactionId": 0,
                    "AccountNo": parseInt(this.taxpayable),
                    "Description": "Tax Payable",
                    "Credit": this.totalCrTax,
                    // Credit: 40,
                    "Debit": 0,
                    "VoucherNo": this.voucherNo,
                    "IsDeleted": false
                },
                {
                    "TransactionId": 0,
                    "AccountNo": parseInt(this.taxexpense),
                    "Description": "Tax Expense",
                    "Credit": 0,
                    "Debit": this.totalDrTaxExp,
                    // Debit: 40,
                    "VoucherNo": this.voucherNo,
                    "IsDeleted": false
                }
            ]
        }
        this.allSubscribers.push(this.salesConfigService.addTransaction(modal, this.erpToken).subscribe((res) => {
            this.isCallingApi = false;
        }, err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err);
        }))
    }
    // generateSignedReport() {
    //   let model = this.getReceiptModel();
    //   this.isCallingApi = true;
    //   this.allSubscribers.push(this.purchasesService.generateSignedReceipt(model).subscribe(res => {
    //     this.isCallingApi = false;
    //     let fIndex = _.findIndex(this.purchaseList, { id: this.purchaseModel.metadatadetails.id })
    //     if (fIndex > -1) {
    //       this.purchaseList[fIndex]['status'] = res.data.status;
    //     }
    //   }, err => {
    //     this.isCallingApi = false;
    //     this.utilService.showErrorCall(err);
    //   }));
    // }

    getReceiptModel(signed_file?: any) {
        let validSub: any = this.countValidSubscription();
        if (
            !this.purchaseModel.metadatadetails ||
            (this.purchaseModel.metadatadetails &&
                !this.purchaseModel.metadatadetails.id) ||
            validSub.count == 0
        ) {
            this.utilService.showErrorToast(
                "Required",
                "Please save Metadata/Subscriptions."
            );
            return null;
        }

        return {
            metadataid: this.purchaseModel.metadatadetails.id,
            totalsubscriptions: validSub.count,
            totalbaseprice: this.purchaseModel.total_base_price,
            finalprice: this.purchaseModel.total_amount,
            files: signed_file
        };
    }
    getUploadReceiptModel(signed_file?: any) {
        this.fileModal = {};
        let validSub: any = this.countValidSubscription();
        if (
            !this.purchaseModel.metadatadetails ||
            (this.purchaseModel.metadatadetails &&
                !this.purchaseModel.metadatadetails.id) ||
            validSub.count == 0
        ) {
            this.utilService.showErrorToast(
                "Required",
                "Please save Metadata/Subscriptions."
            );
            return null;
        }
        this.isCallingApi = true;
        let modal = {
            fileTypeId: 1,
            contentType: signed_file.type,
            fileName: signed_file.name
        }
        let reader: any = new FileReader();
        reader.readAsBinaryString(signed_file);
        reader.onloadend = () => {
            var count = reader.result.match(/\/Type[\s]*\/Page[^s]/g).length;
            this.fileModal.totalpages = count.toString();
            this.fileModal.fileTypeId = "1";
        }
        this.allSubscribers.push(this.purchasesService.getReceiptFileSigned(modal).subscribe((res) => {
            this.fileModal.filename = res.data.filename;
            let signUrl = res.data.signedurl;
            this.fileService.putFileOnBucket(signUrl, signed_file).subscribe((res: any) => {
                switch (res.type) {
                    case HttpEventType.Response:
                        console.log("file uploaded");
                        this.isCallingApi = false;
                        let tempModel = {
                            metadataid: this.purchaseModel.metadatadetails.id,
                            totalsubscriptions: validSub.count,
                            totalbaseprice: this.purchaseModel.total_base_price,
                            finalprice: this.purchaseModel.total_amount,
                            filename: this.fileModal.filename
                        };
                       
                        this.uploadReceipt(tempModel);
                }
            }, err => {
                this.isCallingApi = false;
                this.utilService.showErrorCall(err);
            })
        }))
    }

    uploadReceipt(modal) {
        this.isCallingApi = true;
        console.log(modal);
        this.allSubscribers.push(
            this.purchasesService
                .generateSignedReceipt(modal)
                .subscribe(
                    res => {
                        this.letFileUpload = false;
                        this.isCallingApi = false;
                        let fIndex = _.findIndex(this.purchaseList, {
                            id: this.purchaseModel.metadatadetails.id
                        });
                        if (fIndex > -1) {
                            this.purchaseList[fIndex]["status"] =
                                res.data.status;
                        }
                        // this.addTransaction();
                    },
                    err => {
                        this.isCallingApi = false;
                        this.utilService.showErrorCall(err);
                    }
                )
        );
    }

    onScrollDown() {
        // this.getPurchasesList();
    }

    togglePicker() {
        this.filterPopUpMetaData.showsDatePicker = !this.filterPopUpMetaData
            .showsDatePicker;
        this.changeDetectorRef.detectChanges();
    }

    closeFilterPopOver() {
        this.filterPopUpMetaData.showsDatePicker = false;
        this.filterPopOver.close();
    }

    filterData() {
        this.isSelected = null;
        this.closeFilterPopOver();
        this.filterModel.isfilterneeded = true;
        // this.getPurchasesList(this.filterModel);
        this.getPurchasesList(this.filterModel);
        // this.getSummary(this.filterModel);
    }

    resetfilter() {
        this.isSelected = null;
        this.filterModel = {};
        this.filterModel.isfilterneeded = false;
        this.closeFilterPopOver();
        // this.getPurchasesList(this.filterModel);
        this.getPurchasesList(this.filterModel);

        // this.getSummary(this.filterModel);
    }

    isHovered(date: NgbDate) {
        return (
            this.filterModel.enrollmentfromdate &&
            !this.filterModel.enrollmenttodate &&
            this.filterModel.hoveredDate &&
            date.after(this.filterModel.enrollmentfromdate) &&
            date.before(this.filterModel.hoveredDate)
        );
    }

    isInside(date: NgbDate) {
        return (
            date.after(this.filterModel.enrollmentfromdate) &&
            date.before(this.filterModel.enrollmenttodate)
        );
    }

    isRange(date: NgbDate) {
        return (
            date.equals(this.filterModel.enrollmentfromdate) ||
            date.equals(this.filterModel.enrollmenttodate) ||
            this.isInside(date) ||
            this.isHovered(date)
        );
    }

    onDateSelection(date: NgbDate) {
        if (
            !this.filterModel.enrollmentfromdate &&
            !this.filterModel.enrollmenttodate
        ) {
            this.filterModel.enrollmentfromdate = date;
        } else if (
            this.filterModel.enrollmentfromdate &&
            !this.filterModel.enrollmenttodate &&
            date.after(this.filterModel.enrollmentfromdate)
        ) {
            this.filterModel.enrollmenttodate = date;
        } else {
            this.filterModel.enrollmenttodate = null;
            this.filterModel.enrollmentfromdate = date;
        }
    }

    ngOnDestroy() {
        this.dataService.setSideMenu(false);
        this.allSubscribers.map(value => value.unsubscribe());
    }
}
