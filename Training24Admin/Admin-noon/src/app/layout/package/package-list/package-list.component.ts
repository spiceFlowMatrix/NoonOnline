import { Component, OnInit, ViewChild } from '@angular/core';
import * as _ from 'lodash';
import { PackageService } from '../../../shared/services/package.service';
import { UtilService, DELETE_TITLE, DELETE_MESSAGE } from '../../../shared';

@Component({
    selector: 'app-package-list',
    templateUrl: './package-list.component.html',
    styleUrls: ['./package-list.component.scss']
})
export class PackageListComponent implements OnInit {
    private allSubscribers: Array<any> = [];
    @ViewChild('listdialog') listCommonDialog: any;
    filterModel: any = {};
    packageList: any;
    isPackageLoading: boolean = false;
    constructor(public packageService: PackageService,
        public utilService: UtilService) {

    }

    ngOnInit() {
        this.filterModel = {
            pagenumber: 1,
            perpagerecord: 10,
            totalCount: 0
        }
        this.getPackages(this.filterModel);
    }

getPackages(filter) {
    this.isPackageLoading = true;
    this.allSubscribers.push(this.packageService.getPackages(filter).subscribe((res: any) => {
        this.packageList = res.data;
        this.isPackageLoading = false;
        this.filterModel.totalCount = res.totalcount;
    }, err => {
        this.isPackageLoading = false;
        this.utilService.showErrorCall(err);
    }));
}

    deletePackage(data) {
        this.allSubscribers.push(this.packageService.deletePackage(data.id).subscribe(res => {
            this.packageList = _.remove(this.packageList, (o:any) => {
                return !(o.id == data.id);
            });
        }, err => {
            this.utilService.showErrorCall(err);
        }));
    }

    openDeleteConfirmation(service, index) {
        service.index = index;
        this.listCommonDialog.open(1, DELETE_TITLE, DELETE_MESSAGE, service);
    }

    onPageChange(event) {
        this.filterModel.pagenumber = event;
        this.getPackages(this.filterModel);
    }

    search($event) {
        this.filterModel.pagenumber = 1;
        this.filterModel.search = $event.value;
        if (!this.filterModel.search)
            delete this.filterModel.search;
        this.getPackages(this.filterModel);
    }
  

    ngOnDestroy() {
        this.allSubscribers.map(value => value.unsubscribe());
    }

}