import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { UtilService, GradeService, CourseService } from '../../../shared';
import * as _ from 'lodash';

@Component({
    selector: 'app-add-course-definition',
    templateUrl: './add-course-definition.component.html',
    styleUrls: ['./add-course-definition.component.scss']
})
export class AddCourseDefinitionComponent implements OnInit {
    private allSubscribers: Array<any> = [];
    @ViewChild('manageCoursedefinitionForm') manageCoursedefinitionForm: NgForm;
    public courseDefinitionModel: any = {};
    public gradeList: any = [];
    public coursesList: any = [];
    isEditView: boolean = false;
    isCallingApi: boolean = false;

    constructor(
        private activatedRoute: ActivatedRoute,
        public utilService: UtilService,
        public gradeService: GradeService,
        public courseService: CourseService,
        public router: Router
    ) {
        this.allSubscribers.push(this.activatedRoute.params.subscribe((params: Params) => {
            if (params['id']) {
                this.isEditView = true;
                this.courseDefinitionModel.id = params['id'];
                this.getQuestionTypeById(this.courseDefinitionModel.id);
            } else {
                this.isEditView = false;
            }
        }));
    }

    getGrades() {
        this.isCallingApi = true;
        this.allSubscribers.push(this.gradeService.getGrades({}).subscribe(res => {
            this.isCallingApi = false;
            this.gradeList = res.data;
        }, err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err);
        }));
    }

    ngOnInit() {
        this.utilService.allowOnlyNumber("phone_num");
        this.utilService.allowOnlyNumber("basePrice");
        this.getGrades();
    }

    gradeChange(gradeId) {
        this.isCallingApi = true;
        this.allSubscribers.push(this.gradeService.getGradeCourse({}, gradeId).subscribe(res => {
          this.isCallingApi = false;
          this.coursesList = res.data;
        }, err => {
          this.isCallingApi = false;
          this.utilService.showErrorCall(err);
        }));
      }
    

    getQuestionTypeById(id) {
        this.isCallingApi = true;
        this.allSubscribers.push(this.courseService.getCourseDefinitionById(id).subscribe(res => {
            this.isCallingApi = false;
            this.courseDefinitionModel = _.clone(res.data);
            this.gradeChange(this.courseDefinitionModel.gradeId);
        }, err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err);
        }));
    }

    manageCourseDefinition() {
        this.isCallingApi = true;
        this.allSubscribers.push(this.courseService.manageCourseDefinition(this.courseDefinitionModel).subscribe((res: any) => {
            this.isCallingApi = false;
            this.router.navigate(['/courses/courses-list']);
        }, err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err);
        }));
    }

    ngOnDestroy() {
        this.allSubscribers.map(value => value.unsubscribe());
    }

}
