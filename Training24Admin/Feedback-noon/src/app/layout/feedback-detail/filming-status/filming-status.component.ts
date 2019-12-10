import { ViewChild, Component, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
@Component({
  selector: 'noon-filming-status',
  templateUrl: './filming-status.component.html',
  styleUrls: ['./../feedback-detail.component.scss']
})
export class FilmingStatusComponent implements OnInit {
  @ViewChild('assignTasksDialog') assignTasksDialog: any;
  @ViewChild('assignPersonsDialog') assignPersonsDialog: any;
  addTaskModel: any = {};
  addPersonModel: any = {};
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

  assignTasksModal() {
    this.modalService.open(this.assignTasksDialog, { backdrop: 'static' });
  }

}
