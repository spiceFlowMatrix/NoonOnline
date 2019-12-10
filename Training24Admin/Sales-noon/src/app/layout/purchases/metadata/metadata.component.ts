import {
    Component,
    OnInit,
    Input,
    Output,
    EventEmitter,
    ViewChild,
    SimpleChanges,
    ChangeDetectorRef
} from "@angular/core";
import {
    AgentService,
    CourseService,
    UtilService,
    Categories,
    DataService
} from "../../../shared";
import { NgbCalendar } from "@ng-bootstrap/ng-bootstrap";
import { NgbDate } from "@ng-bootstrap/ng-bootstrap/datepicker/ngb-date";
import * as _ from "lodash";
import * as moment from "moment";
import { AdditionalService } from "../../../shared/services/additional.services";
import { PackageService } from "../../../shared/services/package.services";

@Component({
    selector: "noon-metadata",
    templateUrl: "./metadata.component.html",
    styleUrls: ["./../purchases.component.scss"]
})
export class MetadataComponent implements OnInit {
    @ViewChild("managePurchaseForm") managePurchaseForm: any;

    @Input("data")
    metadata: any = {};
    dropDownConfig: any = {
        displayKey: "subject", //if objects array passed which key to be displayed defaults to description
        search: true, //true/false for the search functionlity defaults to false,
        height: "auto", //height of the list so that if there are more no of items it can show a scroll defaults to auto. With auto height scroll will never appear
        placeholder: "Select" // text to be displayed when no item is selected defaults to Select,
    };
    courseList: any = [];
    discountPackages: any = [];
    agents: any = [];
    subscriptionCategories: any = [];
    allSubscribers: any = [];
    isCallingApi: boolean;
    serviceList: any;
    filterModel: any;
    packageList: any;
    userRoles: any;
    isSalesAgent: boolean = false;
    isAdmin: boolean = false;
    serviceids: any = [];

    @Output() subscriptionChangeEvent: EventEmitter<any> = new EventEmitter();
    @Output() courseChangeEvent: EventEmitter<any> = new EventEmitter();
    @Output() metaDataEvent: EventEmitter<any> = new EventEmitter();
    @Output() discountPackageChangeEvent: EventEmitter<
        any
    > = new EventEmitter();
    @Output() additionalServiceChangeEvent: EventEmitter<
        any
    > = new EventEmitter();
    @Output() packageChangeEvent: EventEmitter<any> = new EventEmitter();
    service: any;
    constructor(
        public courseService: CourseService,
        public agentService: AgentService,
        public dataService: DataService,
        public utilService: UtilService,
        public additionalService: AdditionalService,
        public packageService: PackageService
    ) {
        this.subscriptionCategories = Categories;
    }

    ngOnInit() {
        this.preInitData();
        this.checkForAgent();
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
        let tempDate: any;
        if (this.metadata.discountpackageid === 0) {
            this.metadata.discountpackageid = undefined;
        }
        if (this.metadata.enrollmentfromdate) {
            tempDate = _.clone(moment(this.metadata.enrollmentfromdate));
            this.metadata.enrollmentfromdate = {
                year: tempDate.get("year"),
                month: tempDate.get("month") + 1,
                day: tempDate.get("date")
            };
        } else {
            this.metadata.noofmonths = 0;
        }
        if (this.metadata.enrollmenttodate) {
            tempDate = _.clone(moment(this.metadata.enrollmenttodate));
            this.metadata.enrollmenttodate = {
                year: tempDate.get("year"),
                month: tempDate.get("month") + 1,
                day: tempDate.get("date")
            };
        }
        if (this.metadata.serviceids) {
            this.serviceids = [];
            for(let i = 0;i<this.serviceList.length;i++){
                this.serviceList[i].value=false;
            }
            let val = this.metadata.serviceids;
            for(let j = 0;j<val.length;j++){
                let index = _.findIndex(this.serviceList, function(o: any) {
                    return o.id == val[j];
                });
                if (index != -1) {
                    this.serviceList[index].value = true;
                    this.additionalServiceChangeEvent.emit(this.serviceList[index]);
                    // this.onServiceChange(this.serviceList[index],null)
                }
            }
            this.serviceids = this.metadata.serviceids
        } else {
            this.serviceids = [];
            if(this.serviceList)
            this.serviceList.forEach(element => {
                element.value = false
            });
        }
        if(this.metadata.packageid){
            let index = _.findIndex(this.packageList, (o: any) => {
                return o.id == this.metadata.packageid;
            });
            this.packageChangeEvent.emit(this.packageList[index]);
        }
    
    }

    preInitData() {
        this.filterModel = {
            pagenumber: 1,
            perpagerecord: 10,
            totalCount: 0
        };
        this.getCourses();
        this.getDiscountPackages();
        this.getAgents();
        this.getService();
        this.getPackages();
    }

    ngAfterViewInit() {}

    onCourseSelection($event) {
        this.courseChangeEvent.emit($event);
    }
    onPackageChange(event) {
        if (event) {
            let id = this.metadata.packageid;
                let index = _.findIndex(this.packageList, (o: any) => {
                    return o.id == id;
                });
                if (index != -1) {
                    this.packageChangeEvent.emit(this.packageList[index]);
                }
                else {
                    this.packageChangeEvent.emit(null);
                }
        }
    }

    onServiceChange(service, i) {
        this.metadata.serviceids = [];
        this.additionalServiceChangeEvent.emit(service);
        if (service.value) {
            this.serviceids.push(service.id);
        } else {
            this.serviceids.splice(this.serviceids.indexOf(service.id), 1);
        }
        this.metadata.serviceids = this.serviceids;
        // this.metadata.serviceids = _.uniq(this.serviceids);
        
    }

    getService() {
        this.allSubscribers.push(
            this.additionalService
                .getServices(this.filterModel)
                .subscribe(res => {
                    this.serviceList = res.data;
                    for (
                        let index = 0;
                        index < this.serviceList.length;
                        index++
                    ) {
                        this.serviceList[index]["value"] = false;
                    }
                })
        );
    }

    getPackages() {
        this.filterModel.perpagerecord = 1000;
        this.isCallingApi = true;
        this.allSubscribers.push(
            this.packageService.getPackages(this.filterModel).subscribe(
                res => {
                    this.packageList = res.data;
                    this.isCallingApi = false;
                },
                err => {
                    this.isCallingApi = false;
                    this.utilService.showErrorCall(err);
                }
            )
        );
    }

    getAgents() {
        this.allSubscribers.push(
            this.agentService.getSalesAgents({}).subscribe(
                res => {
                    this.isCallingApi = false;
                    this.agents = res.data;
                    this.dataService.setFilterData("agents", res.data);
                },
                err => {
                    this.isCallingApi = false;
                    this.utilService.showErrorCall(err);
                }
            )
        );
    }

    getDiscountPackages() {
        this.allSubscribers.push(
            this.courseService.getDiscounts({}).subscribe(
                res => {
                    this.isCallingApi = false;
                    this.discountPackages = res.data;
                    this.utilService.discountPackages = this.discountPackages;
                    this.dataService.setFilterData(
                        "discountPackages",
                        res.data
                    );
                },
                err => {
                    this.isCallingApi = false;
                    this.utilService.showErrorCall(err);
                }
            )
        );
    }

    getCourses() {
        this.isCallingApi = true;
        this.allSubscribers.push(
            this.courseService.getCourseDefinitions({}).subscribe(
                res => {
                    this.courseList = res.data;
                    this.dataService.setFilterData("courseList", res.data);
                },
                err => {
                    this.utilService.showErrorCall(err);
                }
            )
        );
    }

    subscriptionChange() {
        this.subscriptionChangeEvent.emit({
            categoryId: this.metadata.subscriptiontypeid
        });
    }

    calculateCourseDuration(event?: any) {
        if (this.metadata.enrollmentfromdate && this.metadata.noofmonths) {
            let tempDate = moment(
                new Date(
                    this.metadata.enrollmentfromdate.year,
                    this.metadata.enrollmentfromdate.month - 1,
                    this.metadata.enrollmentfromdate.day 
                )
            );
            tempDate = tempDate.add(
                parseInt(this.metadata.noofmonths) * 28,
                "days"
            );
            tempDate = tempDate.subtract(1, "day");
            this.metadata.enrollmenttodate = {
                year: tempDate.get("year"),
                month: tempDate.get("month") + 1,
                day: tempDate.get("date") 
            };
            
            this.discountPackageChange();
        }
    }

    onDateSelection(date: any) {
        date = _.clone(moment(date));
        if (
            !this.metadata.enrollmentfromdate &&
            !this.metadata.enrollmenttodate
        ) {
            this.metadata.enrollmentfromdate = date;
        } else if (
            this.metadata.enrollmentfromdate &&
            !this.metadata.enrollmenttodate &&
            date.isAfter(this.metadata.enrollmentfromdate)
        ) {
            this.metadata.enrollmenttodate = date;
        } else {
            this.metadata.enrollmenttodate = null;
            this.metadata.enrollmentfromdate = date;
        }
    }

    isHovered(date) {
        date = _.clone(moment(date));
        return (
            this.metadata.enrollmentfromdate &&
            !this.metadata.enrollmenttodate &&
            this.metadata.hoveredDate &&
            date.isAfter(this.metadata.enrollmentfromdate) &&
            date.isBefore(this.metadata.hoveredDate)
        );
    }

    isInside(date) {
        date = _.clone(moment(date));
        return (
            date.isAfter(this.metadata.enrollmentfromdate) &&
            date.isBefore(this.metadata.enrollmenttodate)
        );
    }

    isRange(date) {
        date = _.clone(moment(date));
        return (
            date.isSame(this.metadata.enrollmentfromdate) ||
            date.isSame(this.metadata.enrollmenttodate) ||
            this.isInside(date) ||
            this.isHovered(date)
        );
    }

    managePurchase() {
        if(this.metadata.discountpackageid == 'undefined')
        delete this.metadata.discountpackageid;
        if(this.metadata.packageid == 'undefined')
        delete this.metadata.packageid; 
        this.metaDataEvent.emit(this.metadata);
    }

    discountPackageChange() {
        this.discountPackageChangeEvent.emit({ data: this.metadata });
    }

    ngOnDestroy() {
        this.allSubscribers.map(value => value.unsubscribe());
    }
}
