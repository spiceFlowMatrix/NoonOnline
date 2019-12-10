import { Component, OnInit, ViewChild } from '@angular/core';
import * as _ from 'lodash';
import { UtilService, FeedbackService, UsersService, DataService, GradeService } from '../../shared';
import { Router } from '@angular/router';


@Component({
  selector: 'app-feed-list',
  templateUrl: './feed-list.component.html',
  styleUrls: ['./feed-list.component.scss']
})
export class FeedListComponent implements OnInit {
 
  constructor(
    public feedbackService: FeedbackService,
    public usersService: UsersService,
    public utilService: UtilService,
    public gradeService: GradeService,
    public dataService: DataService,
    public router: Router
  ) {
    this.dataService.setSideMenu(false);
  }
  ngOnInit() {
  }
  ngAfterViewInit() {
  }
}
