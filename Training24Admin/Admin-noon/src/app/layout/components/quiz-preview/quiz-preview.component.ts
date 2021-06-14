import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
    selector: 'app-quizpreview',
    templateUrl: './quiz-preview.component.html',
    // styleUrls: ['./quizpreview.component.scss']
})
export class QuizPreviewComponent implements OnInit {
    @ViewChild('quizPreviewDialog') quizPreviewDialog;

    @Input('data')
    quizPreviewObj: any = {};

    modalRef: any = null;
    constructor(
        private modalService: NgbModal
    ) { }

    ngOnInit() {

    }

    open() {
        this.modalRef = this.modalService.open(this.quizPreviewDialog, { backdrop: 'static', size: 'lg' });
    }
}
