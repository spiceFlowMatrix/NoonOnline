import { Component, OnInit } from '@angular/core';
import { UtilService } from '../../../shared';
import { PrivacyService } from '../../../shared/services/privacy.services';
@Component({
    selector: 'app-privacy-form',
    templateUrl: './privacy-form.component.html',
    styleUrls: ['./privacy-form.component.scss']
})
export class PrivacyFormComponent implements OnInit {
    private allSubscribers: Array<any> = [];
    isCallingApi: boolean = false;
    privacy: any = ``;
    // timeout_interval: number = 5;


    constructor(
        public privacyService: PrivacyService,
        public utilService: UtilService) {

    }
    ngOnInit() {
        this.getPrivacy();
    }

    getPrivacy() {
        this.isCallingApi = true;
        this.allSubscribers.push(this.privacyService.getPrivacy().subscribe((res)=> {
            this.isCallingApi = false;
            this.privacy = res.data.terms;
        },err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err);
        }))
    }

    managePolicy() {
        let modal = {
            terms: this.privacy
        }
        this.isCallingApi = true;
        this.allSubscribers.push(this.privacyService.managePrivacy(modal).subscribe((res)=> {
            this.isCallingApi = false;
        },err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err);
        }))
    }

    ngOnDestroy() {
        this.allSubscribers.map(value => value.unsubscribe());
    }
}