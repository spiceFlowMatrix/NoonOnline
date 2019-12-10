import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { UtilService,SchoolService} from '../../../shared';
import * as _ from 'lodash';

@Component({
    selector: 'app-add-school',
    templateUrl: './add-school.component.html',
    styleUrls: ['./add-school.component.scss']
})
export class AddSchoolComponent implements OnInit {
    private allSubscribers: Array<any> = [];
    @ViewChild('manageSchoolForm') manageSchoolForm: NgForm;
    public schoolModel: any = {};
    isEditView: boolean = false;
    isCallingApi: boolean = false;

    constructor(
        private activatedRoute: ActivatedRoute,
        public utilService: UtilService,
        public schoolService: SchoolService,
        public router: Router
    ) {
        this.allSubscribers.push(this.activatedRoute.params.subscribe((params: Params) => {
            if (params['id']) {
                this.isEditView = true;
                this.schoolModel.id = params['id'];
                this.getSchoolById(this.schoolModel.id);
            } else {
                this.schoolModel.code = this.utilService.getRandomCode('SCH');
                this.isEditView = false;
            }
        }));
    }

    ngOnInit() { }

    getSchoolById(id) {
        this.isCallingApi = true;
        this.allSubscribers.push(this.schoolService.getSchoolById(id).subscribe(res => {
            this.isCallingApi = false;
            this.schoolModel = _.clone(res.data);
        }, err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err);
        }));
    }

    manageSchool() {
        this.isCallingApi = true;
        this.allSubscribers.push(this.schoolService.manageSchool(this.schoolModel).subscribe((res: any) => {
            this.isCallingApi = false;
            this.router.navigate(['/school/school-list']);
        }, err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err);
        }));
    }

    ngOnDestroy() {
        this.allSubscribers.map(value => value.unsubscribe());
    }
}
