import { Injectable } from '@angular/core';
import { CommonAPIService } from './common.service';
import { UtilService } from './utils.services';
import * as _ from 'lodash';
import { Observable, of, throwError } from 'rxjs';
import { map, catchError } from 'rxjs/operators';

@Injectable()
export class UsersService {
    public users: any = [];

    constructor(
        private commonApiService: CommonAPIService,
        private utilService: UtilService
    ) {

    }

    setUsers(users) {
        this.users = users;
    }

    getUserList(): any[] {
        return this.users;
    }


    getRoleList() {
        return this.commonApiService.get('v1/Roles');
    }
    getTempData() {
        return this.commonApiService.tempGet('v1/Roles');
    }

    searchUsers(filterObj?: any, issearch?: boolean) {
        // if (issearch && (!filterObj || (filterObj && !filterObj.search))) {
        //     return of(this.users);
        // }
        return this.commonApiService.get('v1/Users/GetUserList', filterObj).pipe(
            map(response => {
                return response.data;
            }),
            catchError(error => {
                return throwError(error);
            })
        );;
    }

    getUsers(filterObj) {
        return this.commonApiService.get('v1/Users/GetUserList');
    }
    getParents(id) {
        return this.commonApiService.get('v1/PurchaseDetails/GetParentByStudentId/' + id);
    }

    getUserById(id?: string) {
        return this.commonApiService.get('v1/Users' + (id ? '/' + id : ''));
    }

    manageUser(data: any) {
        if (data.id) {
            return this.commonApiService.put('v1/Users/' + data.id, data);
        } else
            return this.commonApiService.post('v1/Users', data);
    }

    updateProfile(data: any) {
        return this.commonApiService.put('v1/Profile', data);
    }

    changeProfilePic(data: any) {
        return this.commonApiService.putWithFormData('v1/Upload/UploadProfilePicture', data);
    }

    changePassword(data: any) {
        return this.commonApiService.put('v1/Password', data);
    }

    deleteUser(id) {
        return this.commonApiService.delete('v1/Users/' + id);
    }

    checkUserName(name) {
        return this.commonApiService.get('v1/Profile?Username=' + name);
    }

    getRoles(isAdminRoleignored) {
        return new Promise((resolve, reject) => {
            if (this.utilService.getRoles() && this.utilService.getRoles().length > 0) {
                resolve(this.filterRoleList(this.utilService.getRoles(), isAdminRoleignored));
            } else {
                this.getRoleList().subscribe(res => {
                    resolve(this.filterRoleList(res.data, isAdminRoleignored))
                }, err => {
                    this.utilService.showErrorCall(err);
                    reject(err);
                });
            }
        });
    }

    filterRoleList(roleList, isAdminRoleignored) {
        if (isAdminRoleignored)
            return _.clone(_.remove(roleList, (o: any) => {
                if (o.id == 1)
                    return false;
                return true;
            }));
        return roleList;
    }

    getManagementInfo() {
        return this.commonApiService.get('v1/Users/GetManagementInfo');
    }

    setManagementInfo(data: any) {
        return this.commonApiService.post('v1/Users/ManageManagementInfo', data);
    }
}