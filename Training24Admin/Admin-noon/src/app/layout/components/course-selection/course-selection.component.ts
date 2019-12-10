import { Component, ViewChild, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
    selector: 'noon-course-selection',
    templateUrl: './course-selection.component.html',
    styleUrls: ['./course-selection.component.scss']
})
export class CourseSelectionComponent implements OnInit {
    @ViewChild('assignCourseDialog') assignCourseDialog: any;
    @ViewChild('assignCourseForm') assignCourseForm: any;
    public addCourseToBundleModel: any;
    public modalRef: any = {};

    @Output() addCourse: EventEmitter<any> = new EventEmitter();
    @Input('coursesList')
    coursesList: any = [];
    @Input('title')
    title: string = '';
    
    constructor(
        public modelService: NgbModal
    ) { }

    ngOnInit() {
        this.addCourseToBundleModel = {};
    }

    openModal() {
        this.modalRef = this.modelService.open(this.assignCourseDialog, { backdrop: 'static' });
    }

    addCourseAny() {
        this.addCourse.emit(this.addCourseToBundleModel);
    }

    closeModal() {
        this.modalRef.dismiss();
        this.modalRef = null;
    }
}
