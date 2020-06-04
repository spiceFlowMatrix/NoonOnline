import { Component, OnInit } from '@angular/core';
import { UtilService } from '../../../shared';
import { CsvService } from '../../../shared/services/csv.services';
import { AppUpdateService } from '../../../shared/services/app-update.services';
@Component({
    selector: 'app-app-update-form',
    templateUrl: './app-update-form.component.html',
    styleUrls: ['./app-update-form.component.scss']
})
export class AppUpdateFormComponent implements OnInit {
    private allSubscribers: Array<any> = [];
    isCallingApi: boolean = false;
    // timeout_interval: number = 5;
    public updateModel: any = {};
    constructor(
        public appUpdateService: AppUpdateService,
        public utilService: UtilService) {
    }
    ngOnInit() {
        this.getAppDetail();
    }
    getAppDetail() {
        this.isCallingApi = true;
        this.appUpdateService.getAppDetail().subscribe((res) => {
            console.log(res);
            this.updateModel = res.data;
            this.isCallingApi = false;
        })
    }
    manageAppUpdate() {
        this.isCallingApi = true;
        this.allSubscribers.push(this.appUpdateService.putAppDetail(this.updateModel).subscribe((res) => {
            this.isCallingApi = false;
            console.log(res);
        }, err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err);
        }))
    }



    ngOnDestroy() {
        this.allSubscribers.map(value => value.unsubscribe());
    }
}