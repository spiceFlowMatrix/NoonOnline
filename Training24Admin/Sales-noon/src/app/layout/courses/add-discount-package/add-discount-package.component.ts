import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { UtilService, CourseService } from '../../../shared';
import * as _ from 'lodash';

@Component({
    selector: 'app-add-discount-package',
    templateUrl: './add-discount-package.component.html',
    styleUrls: ['./add-discount-package.component.scss']
})
export class AddDiscountPackageComponent implements OnInit {
    private allSubscribers: Array<any> = [];
    @ViewChild('manageDiscountPackageForm') manageDiscountPackageForm: NgForm;
    public discountPackage: any = {};
    isEditView: boolean = false;
    isCallingApi: boolean = false;

    constructor(
        private activatedRoute: ActivatedRoute,
        public utilService: UtilService,
        public courseService: CourseService,
        public router: Router
    ) {
        this.allSubscribers.push(this.activatedRoute.params.subscribe((params: Params) => {
            if (params['id']) {
                this.isEditView = true;
                this.discountPackage.id = params['id'];
                this.getQuestionTypeById(this.discountPackage.id);
            } else {
                this.isEditView = false;
            }
        }));
    }

    ngOnInit() {
        this.utilService.allowOnlyNumber("commission");
    }

    getQuestionTypeById(id) {
        this.isCallingApi = true;
        this.allSubscribers.push(this.courseService.getDiscountById(id).subscribe(res => {
            this.isCallingApi = false;
            this.discountPackage = _.clone(res.data);
        }, err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err);
        }));
    }

    manageDiscountPackage() {
        this.isCallingApi = true;
        this.allSubscribers.push(this.courseService.manageDiscount(this.discountPackage).subscribe((res: any) => {
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
