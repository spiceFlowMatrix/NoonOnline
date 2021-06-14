import { Component, OnInit,Input } from '@angular/core';

@Component({
  selector: 'noon-informer-detail',
  templateUrl: './informer-detail.component.html',
  styleUrls: ['./../feedback-detail.component.scss']
})
export class InformerDetailComponent implements OnInit {

  @Input('data') feedbackDetails: any = {};

  constructor() { }

  ngOnInit() {

  }

  ngAfterViewInit() {
  }

}
