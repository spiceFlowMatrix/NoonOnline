import { Component, OnInit, ViewChild } from '@angular/core';
import * as _ from 'lodash';
import { DELETE_TITLE, DELETE_MESSAGE, UsersService, UtilService, DataService, AuthenticationService } from '../../../shared';
declare var $: any;
@Component({
    selector: 'app-users-list',
    templateUrl: './users-list.component.html',
    styleUrls: ['./users-list.component.scss']
})
export class UsersListComponent implements OnInit {
    private allSubscribers: Array<any> = [];
    @ViewChild('listdialog') listCommonDialog: any;
    usersList: Array<any> = [];
    userRoles: Array<any> = [];
    filterModel: any = {};
    isUserLoading: boolean = false;

    constructor(
        public usersService: UsersService,
        public dataService: DataService,
        public utilService: UtilService,
        public authservice:AuthenticationService
    ) { }

    ngOnInit() {
        this.isUserLoading = true;
        // setTimeout(() => {
        this.filterModel = {
            pagenumber: 1,
            perpagerecord: 10,
            totalCount: 0
        }
        this.getUsers(this.filterModel);
        // }, 1000);
        this.getRoles(false);

        $('textarea.mention1').mentiony({
            onDataRequest: (mode, keyword, onDataRequestCompleteCallback) => {

                var data = [
                    { id: 1, name: 'Nguyen Luat', 'avatar': 'http://cdn0.4dots.com/i/customavatars/avatar7112_1.gif', 'info': 'Vietnam', href: 'http://oxwall.dev/user/1234' },
                    { id: 2, name: 'Dinh Luat', 'avatar': 'http://cdn0.4dots.com/i/customavatars/avatar7112_1.gif', 'info': 'Vietnam', href: 'http://oxwall.dev/user/1234' },
                    { id: 3, name: 'Max Luat', 'avatar': 'http://cdn0.4dots.com/i/customavatars/avatar7112_1.gif', 'info': 'Vietnam', href: 'http://oxwall.dev/user/1234' },
                    { id: 4, name: 'John Neo', 'avatar': 'http://cdn0.4dots.com/i/customavatars/avatar7112_1.gif', 'info': 'Vietnam', href: 'http://oxwall.dev/user/1234' },
                    { id: 5, name: 'John Neo', 'avatar': 'http://cdn0.4dots.com/i/customavatars/avatar7112_1.gif', 'info': 'Vietnam', href: 'http://oxwall.dev/user/1234' },
                    { id: 6, name: 'John Neo', 'avatar': 'http://cdn0.4dots.com/i/customavatars/avatar7112_1.gif', 'info': 'Vietnam', href: 'http://oxwall.dev/user/1234' },
                    { id: 7, name: 'John Neo', 'avatar': 'http://cdn0.4dots.com/i/customavatars/avatar7112_1.gif', 'info': 'Vietnam', href: 'http://oxwall.dev/user/1234' },
                    { id: 8, name: 'John Neo', 'avatar': 'http://cdn0.4dots.com/i/customavatars/avatar7112_1.gif', 'info': 'Vietnam', href: 'http://oxwall.dev/user/1234' },
                    { id: 9, name: 'John Neo', 'avatar': 'http://cdn0.4dots.com/i/customavatars/avatar7112_1.gif', 'info': 'Vietnam', href: 'http://oxwall.dev/user/1234' },
                    { id: 10, name: 'John Neo', 'avatar': 'http://cdn0.4dots.com/i/customavatars/avatar7112_1.gif', 'info': 'Vietnam', href: 'http://oxwall.dev/user/1234' },
                    { id: 11, name: 'John Neo', 'avatar': 'http://cdn0.4dots.com/i/customavatars/avatar7112_1.gif', 'info': 'Vietnam', href: 'http://oxwall.dev/user/1234' },
                    { id: 12, name: 'John Neo', 'avatar': 'http://cdn0.4dots.com/i/customavatars/avatar7112_1.gif', 'info': 'Vietnam', href: 'http://oxwall.dev/user/1234' },
                    { id: 13, name: 'John Neo', 'avatar': 'http://cdn0.4dots.com/i/customavatars/avatar7112_1.gif', 'info': 'Vietnam', href: 'http://oxwall.dev/user/1234' },
                    { id: 14, name: 'John Neo', 'avatar': 'http://cdn0.4dots.com/i/customavatars/avatar7112_1.gif', 'info': 'Vietnam', href: 'http://oxwall.dev/user/1234' },
                    { id: 15, name: 'John Neo', 'avatar': 'http://cdn0.4dots.com/i/customavatars/avatar7112_1.gif', 'info': 'Vietnam', href: 'http://oxwall.dev/user/1234' },
                    { id: 16, name: 'John Neo', 'avatar': 'http://cdn0.4dots.com/i/customavatars/avatar7112_1.gif', 'info': 'Vietnam', href: 'http://oxwall.dev/user/1234' },
                    { id: 17, name: 'Neo Nguyen Dinh', 'avatar': 'http://cdn0.4dots.com/i/customavatars/avatar7112_1.gif', 'info': 'Vietnam', href: 'http://oxwall.dev/user/1234' },
                    { id: 18, name: 'Neo Nguyen Dinh Luat', 'avatar': 'http://cdn0.4dots.com/i/customavatars/avatar7112_1.gif', 'info': 'Vietnam', href: 'http://oxwall.dev/user/1234' }
                ];

                // data = jQuery.grep(data, function( item ) {
                //     return item.name.toLowerCase().indexOf(keyword.toLowerCase()) > -1;
                // });

                // Call this to populate mention.
                onDataRequestCompleteCallback.call(this, data);
            },
            timeOut: 0,
            debug: 1,
        });
    }

    sendMsg() {
        let users = [];
        let inputs = $(".mentiony-content").find("a.mentiony-link").toArray();
        let fullMsg: string = $(".mentiony-content").text();
        for (let index = 0; index < inputs.length; index++) {
            if ($(inputs[index]).attr('id') && $(inputs[index]).text()) {
                users.push($(inputs[index]).attr('id'));
                fullMsg = fullMsg.replace(new RegExp($(inputs[index]).text(), 'g'), "%$+" + $(inputs[index]).attr('id'));
            }
        }
    }

    search($event) {
        this.filterModel.pagenumber = 1;
        this.filterModel.search = _.clone($event.value);
        if (!this.filterModel.search) {
            delete this.filterModel.search;
        }
        this.getUsers(this.filterModel);
    }

    onPageChange(event) {
        this.filterModel.pagenumber = event;
        this.getUsers(this.filterModel);
    }

    filterByRole(roleId) {
        this.filterModel.roleid = roleId;
        this.filterModel.pagenumber = 1;
        this.getUsers(_.clone(this.filterModel));
    }

    getRoles(isAdminRoleignored) {
        this.usersService.getRoles(isAdminRoleignored).then((res: any) => {
            this.userRoles = res;
        });
    }

    openDeleteConfirmation(user, index) {
        user.index = index;
        this.listCommonDialog.open(1, DELETE_TITLE, DELETE_MESSAGE, user);
    }

    getUsers(filterObj?: any) {
        this.isUserLoading = true;
        if (!filterObj.roleid || (filterObj.roleid && filterObj.roleid == 'undefined')) {
            delete filterObj.roleid;
        }
        this.allSubscribers.push(this.usersService.getUsers(filterObj).subscribe(res => {
            this.isUserLoading = false;
            this.usersList = res.data;
            this.filterModel.totalCount = res.totalcount;
        }, err => {
            this.isUserLoading = false;
            this.utilService.showErrorCall(err);
        }));
    }

    deleteUser(data) {
        this.allSubscribers.push(this.usersService.deleteUser(data.id).subscribe(res => {
            this.usersList = _.remove(this.usersList, (o) => {
                return !(o.id == data.id);
            });
        }, err => {
            this.utilService.showErrorCall(err);
        }));
    }

    ngOnDestroy() {
        this.allSubscribers.map(value => value.unsubscribe());
    }
}
