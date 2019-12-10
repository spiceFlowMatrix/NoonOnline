import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import {
  NgbPopoverModule,
  NgbModalModule,
  NgbTypeaheadModule,
  NgbTabsetModule,
  NgbDatepickerModule,
  NgbAccordionModule,
  NgbDatepickerConfig,
  NgbDateParserFormatter
} from '@ng-bootstrap/ng-bootstrap';
import { PurchasesRoutingModule } from './purchases-routing.module';
import { PurchasesComponent } from './purchases.component';
import { MetadataComponent } from './metadata/metadata.component';
import { IndividualDetailComponent } from './individual-detail/individual-detail.component';
import { SchoolDetailComponent } from './school-detail/school-detail.component';
import { DirectivesModule, UsersService, CourseService, AgentService, PurchasesService, NgbDateFRParserFormatter } from '../../shared';
import { SelectDropDownModule } from 'ngx-select-dropdown'
import { InfiniteScrollModule } from 'ngx-infinite-scroll';
import { AdditionalService } from '../../shared/services/additional.services';
import { PackageService } from '../../shared/services/package.services';
import { ReactiveFormsModule } from '@angular/forms';
import { SalesConfigService } from '../../shared/services/salesconfig.services';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    SelectDropDownModule,
    ReactiveFormsModule,
    DirectivesModule,
    NgbPopoverModule.forRoot(),
    NgbTypeaheadModule.forRoot(),
    NgbDatepickerModule.forRoot(),
    NgbModalModule.forRoot(),
    NgbTabsetModule.forRoot(),
    NgbAccordionModule.forRoot(),
    InfiniteScrollModule,
    PurchasesRoutingModule
  ],
  declarations: [
    PurchasesComponent,
    MetadataComponent,
    SchoolDetailComponent,
    IndividualDetailComponent,
  ],
  providers: [
    UsersService,
    AdditionalService,
    PackageService,
    CourseService,
    AgentService,
    SalesConfigService,
    PurchasesService,
    { provide: NgbDateParserFormatter, useClass: NgbDateFRParserFormatter }
  ]
})
export class PurchasesModule { }
