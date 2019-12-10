import { Component, OnInit } from '@angular/core';
import { UtilService } from '../../../shared';
import { CsvService } from '../../../shared/services/csv.services';
@Component({
    selector: 'app-csvinput-form',
    templateUrl: './csvinput-form.component.html',
    styleUrls: ['./csvinput-form.component.scss']
})
export class CsvInputFormComponent implements OnInit {
    private allSubscribers: Array<any> = [];
    isCallingApi: boolean = false;
    // timeout_interval: number = 5;
    public csvModel: any = {};
    file: any;
    constructor(
        public csvService: CsvService,
        public utilService: UtilService) {
    }
    ngOnInit() {
    }

    manageFile() {
        this.isCallingApi = true;
        this.allSubscribers.push(this.csvService.manageCsvFiles(this.file).subscribe((res) => {
            this.isCallingApi = false;
            console.log(res);
        }, err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err);
        }))
    }

    manageAppTrackFile() {
        this.isCallingApi = true;
        this.allSubscribers.push(this.csvService.manageAppTrackCsv(this.file).subscribe((res) => {
            this.isCallingApi = false;
            console.log(res);
        }, err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err);
        }))
    }
    
    fileSelected(event) {
        let re = /(?:\.([^.]+))?$/;
        let ext = "";
        ext = re.exec(event.target.files[0].name)[1];
        if (ext == "csv") {
            this.file = event.target.files[0];
        }
        else {
            this.utilService.showErrorToast("Please Select CSV File!")
        }
    }
    fileAppSelected(event) {
        let re = /(?:\.([^.]+))?$/;
        let ext = "";
        ext = re.exec(event.target.files[0].name)[1];
        if (ext == "csv") {
            this.file = event.target.files[0];
        }
        else {
            this.utilService.showErrorToast("Please Select CSV File!")
        } 
    }

    ngOnDestroy() {
        this.allSubscribers.map(value => value.unsubscribe());
    }
}