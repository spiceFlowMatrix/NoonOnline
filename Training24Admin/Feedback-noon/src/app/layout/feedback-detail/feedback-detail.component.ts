import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { FeedbackService, UtilService } from '../../shared';

@Component({
  selector: 'app-feedback-detail',
  templateUrl: './feedback-detail.component.html',
  styleUrls: ['./feedback-detail.component.scss']
})
export class FeedbackDetailComponent implements OnInit {
  allSubscribers: Array<any> = [];
  feedbackDetails: any = {};
  constructor(
    public route: ActivatedRoute,
    public modalService: NgbModal,
    public feedbackService: FeedbackService,
    public utilService: UtilService
  ) {

    this.allSubscribers.push(this.route.params.subscribe(params => {
      if (params['id']) {
        this.feedbackDetails['id'] = params['id'];
        this.getFeedbackDetail();
      }
    }));
  }

  ngOnInit() {

  }

  ngAfterViewInit() {
  }

  getFeedbackDetail() {
    this.allSubscribers.push(
      this.feedbackService.getFeedbackById(this.feedbackDetails.id).subscribe(res => {
        this.feedbackDetails = res.data;
      }, err => {
        this.utilService.showErrorCall(err);
      })
    );
  }

  ngOnDestroy() {
    this.allSubscribers.map(value => value.unsubscribe());
  }
}
