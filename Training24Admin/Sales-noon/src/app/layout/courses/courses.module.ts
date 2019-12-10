import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgbAccordionModule } from '@ng-bootstrap/ng-bootstrap';
import { CoursesRoutingModule } from './courses-routing.module';
import { CoursesComponent } from './courses.component';

@NgModule({
  imports: [
    CommonModule,
    NgbAccordionModule.forRoot(),
    CoursesRoutingModule
  ],
  declarations: [CoursesComponent]
})
export class CoursesModule { }
