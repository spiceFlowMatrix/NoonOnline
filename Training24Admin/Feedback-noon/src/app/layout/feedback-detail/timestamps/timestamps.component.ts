import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'noon-timestamps',
  templateUrl: './timestamps.component.html',
  styleUrls: ['./../feedback-detail.component.scss']
})
export class TimestampsComponent implements OnInit {
  @Input('data') feedbackDetails: any = {};
  constructor() { }

  ngOnInit() {

  }

  ngAfterViewInit() {
  }

}
