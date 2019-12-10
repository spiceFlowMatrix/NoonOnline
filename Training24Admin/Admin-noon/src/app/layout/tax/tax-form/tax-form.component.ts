import { Component, OnInit } from "@angular/core";
import { UtilService } from "../../../shared";
import { TaxService } from "../../../shared/services/tax.services";
@Component({
    selector: "app-tax-form",
    templateUrl: "./tax-form.component.html",
    styleUrls: ["./tax-form.component.scss"]
})
export class TaxFormComponent implements OnInit {
    private allSubscribers: Array<any> = [];
    isCallingApi: boolean = false;
    // timeout_interval: number = 5;
    public taxModel: any = {};

    constructor(
        public taxService: TaxService,
        public utilService: UtilService
    ) {
        this.getTax();
    }
    ngOnInit() {
        var inputBox = document.getElementById("tax");
        var invalidChars = ["-", "+", "e"];
        inputBox.addEventListener("keydown", function(e) {
            if (invalidChars.includes(e.key)) {
                e.preventDefault();
            }
        });
    }

    getTax() {
        this.isCallingApi = true;
        this.allSubscribers.push(
            this.taxService.getTax().subscribe(
                res => {
                    this.isCallingApi = false;
                    this.taxModel = res.data;
                },
                err => {
                    this.isCallingApi = false;
                    this.utilService.showErrorCall(err);
                }
            )
        );
    }

    manageTimeout() {
        this.isCallingApi = true;
        this.taxModel.key = "Edit";
        this.allSubscribers.push(
            this.taxService.manageTax(this.taxModel).subscribe(
                res => {
                    this.isCallingApi = false;
                },
                err => {
                    this.isCallingApi = false;
                    this.utilService.showErrorCall(err);
                }
            )
        );
    }

    ngOnDestroy() {
        this.allSubscribers.map(value => value.unsubscribe());
    }
}
