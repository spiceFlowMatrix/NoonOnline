import { Component, OnInit, ViewChild } from '@angular/core';
import * as _ from 'lodash';
import * as moment from 'moment';
import { UtilService, FeedbackService, UsersService, DataService, GradeService } from '../../../shared';
import { Router } from '@angular/router';
import { SelectionModel } from '@angular/cdk/collections';

const QUEUE_DATA: any = [];
@Component({
  selector: 'app-lists',
  templateUrl: './lists.component.html',
  styleUrls: ['./lists.component.scss']
})
export class ListsComponent implements OnInit {
  filterModel: any = {};
  isCallingApi: boolean;
  tabIndex: any = 0;
  selectedFeedback: any = {};
  totalArchived: any;
  sub_date: any;
  start_date: any;
  archive_date: any;
  totalCompleted: any;
  totalQueue: any;
  totalProgress: any;
  categoryList: any;
  feedbackUser: any;
  public allSubscribers: Array<any> = [];
  queueDisplayedColumns: string[] = ['selectQueue', 'description', 'category', 'submitted_date', 'submitted_by'];
  progressDisplayedColumns: string[] = ['selectProgress', 'description', 'category', 'submitted_date', 'start_date', 'submitted_by'];
  completeDisplayedColumns: string[] = ['selectCompleted', 'description', 'category', 'submitted_date', 'start_date', 'completed_date', 'submitted_by'];
  archiveDisplayedColumns: string[] = ['selectArchived', 'description', 'category', 'submitted_date', 'archived_date', 'submitted_by'];
  queueData: any;
  progressData: any;
  completeData: any;
  archiveData: any;
  catSelected: any;
  userSelected: any;
  queueSelection = new SelectionModel<any>(true, []);
  progressSelection = new SelectionModel<any>(true, []);
  completeSelection = new SelectionModel<any>(true, []);
  archiveSelection = new SelectionModel<any>(true, []);

  constructor(
    public feedbackService: FeedbackService,
    public usersService: UsersService,
    public utilService: UtilService,
    public gradeService: GradeService,
    public dataService: DataService,
    public router: Router
  ) {
    this.filterModel = {
      "pagenumber": 1,
      "perpagerecord": 10,
      "totalCount": 0,
      "userid": 0,
      "categoryid": 0,
      "begin": "",
      "end": "",
      "startdate": "",
      "completeddate": "",
      "archiveddate": ""
    }
    this.dataService.setSideMenu(false);
  }
  change(event) {
    this.filterModel.begin = moment(event.begin).format('YYYY-MM-DD');
    this.filterModel.end = moment(event.end).format('YYYY-MM-DD');
  }
  changeStart(event) {
    this.filterModel.startdate = moment(event).format('YYYY-MM-DD');
  }
  changeArchive(event) {
    this.filterModel.archiveddate = moment(event).format('YYYY-MM-DD');
  }

  applyFilter(type) {
    console.log(this.start_date);
    
    if (this.start_date == null)
      this.filterModel.startdate = "";
    if (this.sub_date == null) {
      this.filterModel.begin = "";
      this.filterModel.end = "";
    }
    if(this.archive_date == null)
    this.filterModel.archiveddate = "";
    switch (type) {
      case 'queue':
        this.getQueueList();
        break
      case 'progress':
        this.getProgressList();
        break
      case 'complete':
        this.getCompleteList();
        break
      case 'archive':
        this.getArchiveList();
        break
    }

  }

  isAllQueueSelected() {
    const numSelected = this.queueSelection.selected.length;
    if (this.queueData) {
      const numRows = this.queueData.length;
      return numSelected === numRows;
    }
  }

  masterQueueToggle() {
    this.isAllQueueSelected() ?
      this.queueSelection.clear() :
      this.queueData.forEach(row => this.queueSelection.select(row));
  }

  checkboxQueueLabel(row?: any): string {
    if (!row) {
      return `${this.isAllQueueSelected() ? 'select' : 'deselect'} all`;
    }
    return `${this.queueSelection.isSelected(row) ? 'deselect' : 'select'} row ${row.category + 1}`;
  }

  isAllProgressSelected() {
    const numSelected = this.progressSelection.selected.length;
    const numRows = this.progressData.length;
    return numSelected === numRows;
  }

  masterProgressToggle() {
    this.isAllProgressSelected() ?
      this.progressSelection.clear() :
      this.progressData.forEach(row => this.progressSelection.select(row));
  }

  checkboxProgressLabel(row?: any): string {
    if (!row) {
      return `${this.isAllProgressSelected() ? 'select' : 'deselect'} all`;
    }
    return `${this.progressSelection.isSelected(row) ? 'deselect' : 'select'} row ${row.category + 1}`;
  }

  isAllCompleteSelected() {
    const numSelected = this.completeSelection.selected.length;
    const numRows = this.completeData.length;
    return numSelected === numRows;
  }

  masterCompleteToggle() {
    this.isAllCompleteSelected() ?
      this.completeSelection.clear() :
      this.completeData.forEach(row => this.completeSelection.select(row));
  }

  checkboxCompleteLabel(row?: any): string {
    if (!row) {
      return `${this.isAllCompleteSelected() ? 'select' : 'deselect'} all`;
    }
    return `${this.completeSelection.isSelected(row) ? 'deselect' : 'select'} row ${row.category + 1}`;
  }

  isAllArchiveSelected() {
    const numSelected = this.archiveSelection.selected.length;
    const numRows = this.archiveData.length;
    return numSelected === numRows;
  }

  masterArchiveToggle() {
    this.isAllCompleteSelected() ?
      this.archiveSelection.clear() :
      this.archiveData.forEach(row => this.archiveSelection.select(row));
  }

  checkboxArchiveLabel(row?: any): string {
    if (!row) {
      return `${this.isAllArchiveSelected() ? 'select' : 'deselect'} all`;
    }
    return `${this.archiveSelection.isSelected(row) ? 'deselect' : 'select'} row ${row.category + 1}`;
  }

  openDetail(row) {
    this.router.navigate(['feeddetail/' + row.id]);
  }

  moveToStage(type) {
    switch (type) {
      case 'progressfromQueue':
        let pids = [];
        if (this.queueSelection.selected.length != 0) {
          this.queueSelection.selected.forEach(element => {
            pids.push(element.id);
          })
          this.moveToProgress(pids)
        }
        break;
      case 'complete':
        let cids = [];
        if (this.progressSelection.selected.length != 0) {
          this.progressSelection.selected.forEach(element => {
            cids.push(element.id);
          })
          this.moveToCompleted(cids)
        }
        break;
      case 'archivefromQueue':
        let aids = [];
        if (this.queueSelection.selected.length != 0) {
          this.queueSelection.selected.forEach(element => {
            aids.push(element.id);
          })
          this.moveToArchived(aids);
        }
        break
      case 'archivefromProcess':
        let apids = [];
        if (this.progressSelection.selected.length != 0) {
          this.progressSelection.selected.forEach(element => {
            apids.push(element.id);
          })
          this.moveToArchived(apids)
        }
        break;
      case 'progressfromArchive':
        let paids = [];
        if (this.archiveSelection.selected.length != 0) {
          this.archiveSelection.selected.forEach(element => {
            paids.push(element.id);
          })
          this.moveToProgress(paids)
        }
        break;
    }
  }

  ngOnInit() {
    this.getQueueList();
    this.getCategoryList();
  }

  ngAfterViewInit() {

  }
  getCategoryList() {
    this.isCallingApi = true;
    this.allSubscribers.push(this.feedbackService.getCategory().subscribe((res) => {
      this.categoryList = res.data;
      this.isCallingApi = false;
      this.getFeedBackList();
    }, err => {
      this.isCallingApi = false;
      this.utilService.showErrorCall(err);
    }))
  }
  getFeedBackList() {
    this.isCallingApi = true;
    this.allSubscribers.push(this.feedbackService.getFeedUser().subscribe((res) => {
      this.feedbackUser = res.data;
      this.isCallingApi = false;
    }, err => {
      this.isCallingApi = false;
      this.utilService.showErrorCall(err);
    }));
  }

  moveToProgress(ids) {
    this.isCallingApi = true;
    this.allSubscribers.push(this.feedbackService.moveToProgress(ids).subscribe((res) => {
      this.isCallingApi = false;
      this.filterModel.pagenumber = 1;
      this.filterModel.perpagerecord = 10;
      this.getQueueList();
      this.getArchiveList();
    }, err => {
      this.isCallingApi = false;
      this.utilService.showErrorCall(err);
    }))
  }
  moveToCompleted(ids) {
    this.isCallingApi = true;
    this.allSubscribers.push(this.feedbackService.moveToCompleted(ids).subscribe((res) => {
      this.isCallingApi = false;
      this.getProgressList();
    }, err => {
      this.isCallingApi = false;
      this.utilService.showErrorCall(err);
    }))
  }
  moveToArchived(ids) {
    this.isCallingApi = true;
    this.allSubscribers.push(this.feedbackService.moveInArchived(ids).subscribe((res) => {
      this.isCallingApi = false;
      this.getQueueList();
      this.getProgressList();
    }, err => {
      this.isCallingApi = false;
      this.utilService.showErrorCall(err);
    }))
  }
  getQueueList() {
    this.isCallingApi = true;
    this.allSubscribers.push(this.feedbackService.getQueueList(this.filterModel).subscribe((res) => {
      this.isCallingApi = false;
      this.queueData = res.data;
      this.totalQueue = res.totalcount;
    }, err => {
      this.isCallingApi = false;
      this.utilService.showErrorCall(err);
    }))
  }
  getProgressList() {
    this.isCallingApi = true;
    this.allSubscribers.push(this.feedbackService.getProgressList(this.filterModel).subscribe((res) => {
      this.isCallingApi = false;
      this.totalProgress = res.totalcount;
      this.progressData = res.data;
    }, err => {
      this.isCallingApi = false;
      this.utilService.showErrorCall(err);
    }))
  }
  getCompleteList() {
    this.isCallingApi = true;
    this.allSubscribers.push(this.feedbackService.getCompleteList(this.filterModel).subscribe((res) => {
      this.isCallingApi = false;
      this.completeData = res.data;
      this.totalCompleted = res.totalcount;
    }, err => {
      this.isCallingApi = false;
      this.utilService.showErrorCall(err);
    }))
  }
  getArchiveList() {
    this.isCallingApi = true;
    this.allSubscribers.push(this.feedbackService.getArchiveList(this.filterModel).subscribe((res) => {
      this.isCallingApi = false;
      this.archiveData = res.data;
      this.totalArchived = res.totalcount;
    }, err => {
      this.isCallingApi = false;
      this.utilService.showErrorCall(err);
    }))
  }
  tabChange(event) {
    this.filterModel = {
      "pagenumber": 1,
      "perpagerecord": 10,
      "totalCount": 0,
      "userid": 0,
      "categoryid": 0,
      "begin": "",
      "end": "",
      "startdate": "",
      "completeddate": "",
      "archiveddate": ""
    }
    this.sub_date = "";
    this.archive_date = "";
    this.start_date = "";
    this.tabIndex = event.index;
    switch (event.index) {
      case 0:
        this.getQueueList();
        break;
      case 1:
        this.getProgressList();
        break;
      case 2:
        this.getCompleteList();
        break;
      case 3:
        this.getArchiveList();
        break;
    }
  }

  progressPageChange(event) {
    this.filterModel.pagenumber = event.pageIndex + 1;
    this.getProgressList();
  }
  queuePageChange(event) {
    this.filterModel.pagenumber = event.pageIndex + 1;
    this.getQueueList();
  }
  completePageChange(event) {
    this.filterModel.pagenumber = event.pageIndex + 1;
    this.getCompleteList();
  }
  archivePageChange(event) {
    this.filterModel.pagenumber = event.pageIndex + 1;
    this.getArchiveList();
  }

  ngOnDestroy() {
    this.allSubscribers.map(value => value.unsubscribe());
  }
}
