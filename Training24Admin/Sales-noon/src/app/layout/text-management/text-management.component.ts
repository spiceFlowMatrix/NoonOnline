import { Component, OnInit } from '@angular/core';
import { UsersService, UtilService } from './../../shared';
@Component({
    selector: 'app-text-management',
    templateUrl: './text-management.component.html',
    styleUrls: ['./text-management.component.scss']
})
export class TextManagementPageComponent implements OnInit {
    managementInfo: any = {};
    isCallingApi: boolean = false;

    constructor(
        public usersService: UsersService,
        public utilService: UtilService
    ) { }

    ngOnInit() {
        this.getManagementInfo();
    }

    getManagementInfo() {
        this.isCallingApi = true;
        this.usersService.getManagementInfo().subscribe(res => {
            this.isCallingApi = false;
            if (res.data)
                this.managementInfo = res.data;
        }, err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err);
        });
    }

    saveInfo() {
        this.isCallingApi = true;
        this.usersService.setManagementInfo(this.managementInfo).subscribe(res => {
            this.isCallingApi = false;
            this.utilService.showErrorSuccess("", "Info saved successfully.")
        }, err => {
            this.isCallingApi = false;
            this.utilService.showErrorCall(err);
        });
    }
}
