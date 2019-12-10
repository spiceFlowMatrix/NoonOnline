import { Component, OnInit, ViewChild } from "@angular/core";
import * as _ from "lodash";
import { UtilService } from "../../../shared";
import { AdditionalService } from "../../../shared/services/additionalservice.services";
import { DELETE_TITLE, DELETE_MESSAGE } from '../../../shared';

@Component({
    selector: "app-additional-list",
    templateUrl: "./additional-list.component.html",
    styleUrls: ["./additional-list.component.scss"]
})
export class AdditionalListComponent implements OnInit {
    filterModel: any = {};
    private allSubscribers: Array<any> = [];
    isAdditionalLoading: boolean = false;
    serviceList: any;
    @ViewChild('listdialog') listCommonDialog: any;

    constructor(
        public additionalService: AdditionalService,
        public utilService: UtilService,
    ) { }
    ngOnInit() {
        this.filterModel = {
            pagenumber: 1,
            perpagerecord: 10,
            totalCount: 0
        }
        this.getServices(this.filterModel);
    }
    getServices(filterObj) {
        this.isAdditionalLoading = true;
        this.allSubscribers.push(this.additionalService.getServices(filterObj).subscribe(res => {
            this.isAdditionalLoading = false;
            this.serviceList = res.data;
            this.filterModel.totalCount = res.totalcount;
        }, err => {
            this.isAdditionalLoading = false;
            this.utilService.showErrorCall(err);
        }));
    }
    
    openDeleteConfirmation(service, index) {
        service.index = index;
        this.listCommonDialog.open(1, DELETE_TITLE, DELETE_MESSAGE, service);
    }
    
    search($event) {
        this.filterModel.pagenumber = 1;
        this.filterModel.search = $event.value;
        if (!this.filterModel.search)
            delete this.filterModel.search;
        this.getServices(this.filterModel);
    }
    onPageChange(event) {
        this.filterModel.pagenumber = event;
        this.getServices(this.filterModel);
    }

    deleteServices(data) {
        this.allSubscribers.push(this.additionalService.deleteService(data.id).subscribe(res => {
            this.serviceList = _.remove(this.serviceList, (o: any) => {
                return !(o.id == data.id);
            });
        }, err => {
            this.utilService.showErrorCall(err);
        }));
    }
    
    ngOnDestroy() {
        this.allSubscribers.map(value => value.unsubscribe());
    }
}
