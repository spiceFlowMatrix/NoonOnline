import { Component, OnInit, ViewChild } from "@angular/core";
import { NgForm } from "@angular/forms";
import { Router, ActivatedRoute, Params } from "@angular/router";
import * as _ from "lodash";
import { NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { Observable, Subject, of, merge } from "rxjs";
import {
    catchError,
    debounceTime,
    distinctUntilChanged,
    map,
    tap,
    switchMap
} from "rxjs/operators";
import { HttpEvent, HttpEventType } from "@angular/common/http";
import { AdditionalService } from "../../../shared/services/additionalservice.services";
import { UtilService } from "../../../shared";

@Component({
    selector: "app-add-additional",
    templateUrl: "./add-additional.component.html",
    styleUrls: ["./add-additional.component.scss"]
})
export class AddAdditionalComponent implements OnInit {
    private allSubscribers: Array<any> = [];
    public serviceModel: any = {};
    updateModel: any ={}; 
    id: any;
    isEditView: boolean = false;
    isCallingApi: boolean = false;

    constructor(
        public additionalServices: AdditionalService,
        public router: Router,
        public activatedRoute: ActivatedRoute,
        public utilService: UtilService
    ) {
        this.allSubscribers.push(
            this.activatedRoute.params.subscribe((params: Params) => {
                if (params["id"]) {
                    this.isEditView = true;
                    this.id = params["id"];
                    this.getServicesById(this.id);
                } else {
                    this.isEditView = false;
                }
            })
        );
    }

    ngOnInit() {}
   
   
    getServicesById(id) {
        this.isCallingApi = true;
        this.allSubscribers.push(this.additionalServices.getServicesById(id).subscribe(res => {
            this.isCallingApi = false;
            this.serviceModel = _.clone(res.data);
        }, err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err);
        }));
    }
   
   
    manageService() {
        this.isCallingApi = true;
        this.serviceModel.key = "Edit";
        this.allSubscribers.push(this.additionalServices.manageService(this.serviceModel).subscribe(
                (res: any) => {
                    this.isCallingApi = false;
                    this.router.navigate(["/additional_service/additional-list"]);
                },
                err => {
                    this.isCallingApi = false;
                    this.utilService.showErrorCall(err);
                }
            )
        );
    }

    ngOnDestroy() {
        this.allSubscribers.map(value => value.unsubscribe());
    }
}
