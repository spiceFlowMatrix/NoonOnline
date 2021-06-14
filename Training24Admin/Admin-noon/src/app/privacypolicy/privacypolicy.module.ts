import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PrivacyPolicyRoutingModule } from './privacypolicy-routing.module';
import { PrivacyPolicyComponent } from './privacypolicy.component';
import { SharedModule } from '../shared';
import { PrivacyService } from '../shared/services/privacy.services';

@NgModule({
  imports: [
    CommonModule,
    SharedModule.forRoot(),
    PrivacyPolicyRoutingModule
  ],
  declarations: [PrivacyPolicyComponent],
  providers: [PrivacyService]
})
export class PrivacyPolicyModule { }
