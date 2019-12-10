import {
    ViewChild,
    Component,
    OnInit,
    Input,
    SimpleChanges
} from "@angular/core";
import {
    UtilService,
    UsersService,
    PurchasesService,
    ProvisionsList
} from "./../../../shared";
import { Observable, Subject, of, merge } from "rxjs";
import {
    catchError,
    debounceTime,
    distinctUntilChanged,
    map,
    tap,
    switchMap
} from "rxjs/operators";
import * as moment from "moment";
import * as _ from "lodash";

@Component({
    selector: "noon-individual-detail",
    templateUrl: "./individual-detail.component.html",
    styleUrls: ["./../purchases.component.scss"]
})
export class IndividualDetailComponent implements OnInit {
    public allSubscribers: any = [];
    @ViewChild("manageIndividualDetailForm") manageIndividualDetailForm: any;
    focus$ = new Subject<string>();
    selectedAccount: any = {};
    minDate: any = {
        year: 1970,
        month: 1,
        day: 1
    };
    @Input("data")
    public individualDetails: any = [];
    accountDetail: any = {};
    isCallingApi: boolean;
    provisions: any = [];
    parenttazrika: any;
    studenttazrika: any;
    previousmarksheets: any;
    marksheetDownload: any;
    studentDownload: any;
    parentDownload: any;
    userRoles: any;
    isSalesAgent: boolean = false;
    isAdmin: boolean = false;
    dob: any;

    constructor(
        public utilService: UtilService,
        public usersService: UsersService,
        public purchasesService: PurchasesService
    ) {}

    ngOnInit() {
        this.addNewDetail();
        this.checkForAgent();
        setTimeout(() => {
            this.purchasesService.getDetailInList().subscribe(res => {
                this.purchasesService.individualData = res.data;
            });
        }, 1000);
    }
    checkForAgent() {
        this.userRoles = this.utilService.getRole();
        this.isSalesAgent = _.find(this.userRoles, (o:any)=>{
            return (o) == 'sales_agent';
        }) ? true : false;   
        this.isAdmin = _.find(this.userRoles, (o:any)=>{
            return (o) == 'sales_admin';
        }) ? true : false;   
        
    }
    ngOnChanges(changes: SimpleChanges) {
        this.studenttazrika = null;
        this.parenttazrika = null;
        this.previousmarksheets = null;
        this.studentDownload = null;
        this.parentDownload = null;
        this.marksheetDownload = null;
        this.dob = null;
        for (let index = 0; index < this.individualDetails.length; index++) {
            if (
                this.individualDetails[index].id &&
                this.individualDetails[index].provinceid &&
                this.individualDetails[index].cityid
            ) {
                this.individualDetails[index]["cities"] = [];
                Object.keys(
                    ProvisionsList[this.individualDetails[index].provinceid]
                ).forEach(key => {
                    this.individualDetails[index]["cities"].push(key);
                });

                if(this.individualDetails[index].dateofbirth){
                    let tempDate = _.clone(moment(this.individualDetails[index].dateofbirth));
                    this.dob= {
                        year: tempDate.get("year"),
                        month: tempDate.get("month")+1,
                        day: tempDate.get("date")-1
                    };
                    
                }

                if(this.individualDetails[index].studenttazkira)
                for (let i = 0; i < this.individualDetails[index].studenttazkira.length; i++) {
                 this.studenttazrika = this.individualDetails[index].studenttazkira[i].name;
                 this.studentDownload = this.individualDetails[index].studenttazkira[i].documenturl;
                }

                if(this.individualDetails[index].parenttazrika)
                for (let i = 0; i < this.individualDetails[index].parenttazrika.length; i++) {
                    this.parenttazrika = this.individualDetails[index].parenttazrika[i].name;
                    this.parentDownload = this.individualDetails[index].parenttazrika[i].documenturl;
                }
             
                if(this.individualDetails[index].previousmarksheets)
                for (let i = 0; i < this.individualDetails[index].previousmarksheets.length; i++) {
                 this.previousmarksheets = this.individualDetails[index].previousmarksheets[i].name;
                 this.marksheetDownload = this.individualDetails[index].previousmarksheets[i].documenturl;
                 
                }
            }
        }
    }

    ngAfterViewInit() {
        // this.utilService.allowOnlyNumber("Phone");
        Object.keys(ProvisionsList).forEach(key => {
            this.provisions.push(key);
        });
    }

    provinceChanged(index) {
        this.individualDetails[index]["cities"] = [];
        Object.keys(
            ProvisionsList[this.individualDetails[index].provinceid]
        ).forEach(key => {
            this.individualDetails[index]["cities"].push(key);
        });
        if (this.individualDetails[index]["cities"].length > 0) {
            this.individualDetails[index].cityid = this.individualDetails[
                index
            ]["cities"][0];
        }
    }

    formatter = (x: { studentCode: string }) => x.studentCode;
    rformatter = (result: any) => result.studentCode;
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
                    .searchUsers({
                        search: term
                    })
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

    focusOnAccountSelection() {
        setTimeout(() => {
            document.getElementById("individualdetailInput").focus();
        }, 500);
    }

    addNewDetail() {
        if (this.individualDetails.length < 1)
            this.individualDetails.push({
                studentcode: this.utilService.getRandomCode("SC")
            });
    }

    accountSelected($event, index) {
        this.selectedAccount = _.clone($event.item);
        this.getIndividualDetail($event.item, index);
    }

    reset() {
        this.selectedAccount = {};
        this.individualDetails = [];
        this.individualDetails.push({
            studentcode: this.utilService.getRandomCode("SC")
        });
    }

    getIndividualDetail(obj, index) {
        this.isCallingApi = true;
        this.allSubscribers.push(
            this.purchasesService.getIndividualDetailById(obj.id).subscribe(
                res => {
                    
                    this.individualDetails[index] = res.data;
                    if(this.individualDetails[index].studenttazkira)
                    for (let index = 0; index < this.individualDetails.length; index++) {
                    for (let i = 0; i < this.individualDetails[index].studenttazkira.length; i++) {
                     this.studenttazrika = this.individualDetails[index].studenttazkira[i].name || 'file';                     
                     this.studentDownload = this.individualDetails[index].studenttazkira[i].documenturl;
                    }
    
                    if(this.individualDetails[index].parenttazrika)
                    for (let i = 0; i < this.individualDetails[index].parenttazrika.length; i++) {
                        this.parenttazrika = this.individualDetails[index].parenttazrika[i].name || 'file';
                        this.parentDownload = this.individualDetails[index].parenttazrika[i].documenturl;
                    }
                 
                    if(this.individualDetails[index].previousmarksheets)
                    for (let i = 0; i < this.individualDetails[index].previousmarksheets.length; i++) {
                     this.previousmarksheets = this.individualDetails[index].previousmarksheets[i].name || 'file';
                     this.marksheetDownload = this.individualDetails[index].previousmarksheets[i].documenturl;
                     
                    }
                }
                    if (this.selectedAccount) {
                        this.individualDetails[index].account = _.clone(
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

    // onFileSelected($event, key, index) {
    //     console.log($event);
    
    //     this.individualDetails[index][key] = $event.target.files;
    // }
    onStudentFileSelected($event,index){
        this.studentDownload = null;
        this.studenttazrika = $event.target.files[0].name;
        this.individualDetails[index]['studenttazrika'] = $event.target.files;
    }
    onMarksheetFileSelected($event,index){
        this.marksheetDownload = null;
        this.previousmarksheets = $event.target.files[0].name;
        this.individualDetails[index]['previousmarksheets'] = $event.target.files;
    }

    onParentFileSelected($event,index){
        this.parentDownload = null;
        this.parenttazrika = $event.target.files[0].name;
        this.individualDetails[index]['parenttazrika'] = $event.target.files;
    }

    downloadStudentFiles() {
        window.open(this.studentDownload);
    }
    
    downloadParentFiles() {
        window.open(this.parentDownload);
    }

    downloadMarksheetFiles() {
        window.open(this.marksheetDownload);
    }

    public getDetail() {
        // console.log(this.individualDetails.dateofbirth);
        
        // this.individualDetails.dateofbirth = 
        // typeof this.individualDetails.dateofbirth == "string"
        //     ? this.individualDetails.dateofbirth
        //     : new Date(
        //           this.individualDetails.dateofbirth["year"],
        //           this.individualDetails.dateofbirth["month"],
        //           this.individualDetails.dateofbirth["day"]
        //       ).toISOString();
        
        return this.individualDetails;
    }
    dateSelect(event) {
        this.individualDetails[0].dateofbirth = 
        typeof event == "string"
            ? event
            : new Date(
                  event["year"],
                  event["month"]-1,
                  event["day"]+1
              ).toISOString();
    }

    ngOnDestroy() {
        this.allSubscribers.map(value => value.unsubscribe());
    }
}
