import { Component, OnInit } from '@angular/core';
import { TimeoutService } from '../../../shared/services/timeout.services';
import { UtilService } from '../../../shared';
@Component({
    selector: 'app-timeout-form',
    templateUrl: './timeout-form.component.html',
    styleUrls: ['./timeout-form.component.scss']
})
export class TimeOutFormComponent implements OnInit {
    private allSubscribers: Array<any> = [];
    isCallingApi: boolean = false;
    // timeout_interval: number = 5;
    public intervalModel: any = {};

    constructor(
        public timeService: TimeoutService,
        public utilService: UtilService) {
        this.getInterval();
    }
    ngOnInit() {
        var inputBox = document.getElementById("timeout");
        var invalidChars = [
            "-",
            "+",
            "e",
        ];
        inputBox.addEventListener("keydown", function (e) {
            if (invalidChars.includes(e.key)) {
                e.preventDefault();
            }
        });

    }

    getInterval() {
        this.isCallingApi = true;
        this.allSubscribers.push(this.timeService.getTimeout().subscribe(res => {
            this.isCallingApi = false;
            this.intervalModel = res.data;
        }, err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err);
        }))
    }

    manageTimeout() {
        this.isCallingApi = true;
        this.intervalModel.key = "Edit";
        this.allSubscribers.push(this.timeService.manageTimeout(this.intervalModel).subscribe(res => {
            this.isCallingApi = false;
        }, err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err);
        }));
    }

    ngOnDestroy() {
        this.allSubscribers.map(value => value.unsubscribe());
    }
}