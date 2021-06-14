import { ViewChild, Component, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'noon-graphics-status',
  templateUrl: './graphics-status.component.html',
  styleUrls: ['./../feedback-detail.component.scss']
})
export class GraphicsStatusComponent implements OnInit {
  @ViewChild('assignPersonsDialog') assignPersonsDialog: any;
  addPersonModel: any = {};
  @ViewChild('assignTasksDialog') assignTasksDialog: any;
  assignTaskModel: any = {};

  constructor(
    private modalService: NgbModal
  ) { }

  ngOnInit() {

  }

  ngAfterViewInit() {
  }

  assignPersonsModal() {
    this.modalService.open(this.assignPersonsDialog, { backdrop: 'static' });
  }

  assignTasksModal(){
    this.modalService.open(this.assignTasksDialog, { backdrop: 'static' });
  }

}
