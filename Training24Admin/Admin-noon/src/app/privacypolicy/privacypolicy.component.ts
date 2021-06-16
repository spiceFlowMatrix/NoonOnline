import { Component, OnInit } from '@angular/core';
import { PrivacyService } from '../shared/services/privacy.services';
import { UtilService } from '../shared';

@Component({
  selector: 'app-privacypolicy',
  templateUrl: './privacypolicy.component.html',
  styleUrls: ['./privacypolicy.component.scss']
})
export class PrivacyPolicyComponent implements OnInit {
  privacy: any;
  private allSubscribers: Array<any> = [];
  isCallingApi: boolean = false;
  constructor(public privacyService:PrivacyService,public utilService:UtilService) { }

  ngOnInit() {
    this.getPolicy();
  }

  getPolicy() {

    this.isCallingApi = true;
    this.allSubscribers.push(this.privacyService.getPrivacy().subscribe((res) => {
      this.isCallingApi = false;
      this.privacy = res.data.terms;
      let refer = document.getElementById('privacy');
      refer.innerHTML = this.privacy;
    }, err => {
      this.isCallingApi = false;
      this.utilService.showErrorCall(err);
    }))
  }

}
