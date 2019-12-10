import { Component, OnInit, Input, OnChanges, SimpleChanges } from '@angular/core';

@Component({
  selector: 'noon-metadata',
  templateUrl: './metadata.component.html',
  styleUrls: ['./../feedback-detail.component.scss']
})
export class MetadataComponent implements OnInit, OnChanges {

  @Input('data') feedbackDetails: any = {};
  constructor() { }

  ngOnInit() {

  }

  ngAfterViewInit() {
  }

  ngOnChanges(changes: SimpleChanges) {  
    
  }

}
