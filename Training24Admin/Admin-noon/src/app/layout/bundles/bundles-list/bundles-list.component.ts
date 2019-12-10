import { Component, OnInit, ViewChild } from '@angular/core';
import * as _ from 'lodash';
import { DELETE_TITLE, DELETE_MESSAGE, UtilService, BundleService } from '../../../shared';

@Component({
    selector: 'app-bundles-list',
    templateUrl: './bundles-list.component.html',
    styleUrls: ['./bundles-list.component.scss']
})
export class BundlesListComponent implements OnInit {
    private allSubscribers: Array<any> = [];
    @ViewChild('listdialog') listCommonDialog: any;
    filterModel: any = {};
    isBundleLoading: boolean = false;
    bundleList: Array<any> = [];
    constructor(
        public utilService: UtilService,
        public bundleService: BundleService
    ) { }


    ngOnInit() {
        this.filterModel = {
            pagenumber: 1,
            perpagerecord: 10,
            totalCount: 0
        }
        this.getBundles(this.filterModel);
    }

    openDeleteConfirmation(bundle, index) {
        bundle.index = index;
        this.listCommonDialog.open(1, DELETE_TITLE, DELETE_MESSAGE, bundle);
    }

    search($event) {
        this.filterModel.pagenumber = 1;
        this.filterModel.search = $event.value;
        if (!this.filterModel.search)
            delete this.filterModel.search;
        this.getBundles(this.filterModel);
    }
    
    onPageChange(event) {
        this.filterModel.pagenumber = event;
        this.getBundles(this.filterModel);
    }

    getBundles(filterObj) {
        this.isBundleLoading = true;
        this.allSubscribers.push(this.bundleService.getBundles(filterObj).subscribe(res => {
            this.isBundleLoading = false;
            this.bundleList = res.data;
            this.filterModel.totalCount = res.totalcount;
        }, err => {
            this.isBundleLoading = false;
            this.utilService.showErrorCall(err);
        }));
    }

    deleteBundle(data) {
        this.allSubscribers.push(this.bundleService.deleteBundle(data.id).subscribe(res => {
            this.bundleList = _.remove(this.bundleList, (o) => {
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
