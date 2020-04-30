import { Injectable } from '@angular/core';
import { CommonAPIService } from './common.service';
import * as _ from 'lodash';
import { Observable, of, throwError } from 'rxjs';
import { map, catchError } from 'rxjs/operators';

@Injectable()
export class PurchasesService {
    public schoolData: any = [];
    public individualData: any = [];

    constructor(
        private commonApiService: CommonAPIService,
    ) {

    }

    searchUsers(filterObj?: any, isSchoolDetail?: boolean) {
        if (!filterObj || (filterObj && !filterObj.search)) {
            if (isSchoolDetail && this.schoolData.length > 0) {
                return of(this.schoolData);
            }
            if (!isSchoolDetail && this.individualData.length > 0) {
                return of(this.individualData);
            }
        }
        let url = isSchoolDetail ? 'GetSchoolList' : 'GetIndividualList'
        return this.commonApiService.get('v1/PurchaseDetails/' + url, filterObj).pipe(
            map(response => {
                return response.data;
            }),
            catchError(error => {
                return throwError(error);
            })
        );
    }

    getTax() {
        return this.commonApiService.get('v1/SalesTax/GetTaxPub');
    }

    getDetailInList(isSchoolDetail?: boolean) {
        let url = isSchoolDetail ? 'GetSchoolList' : 'GetIndividualList'
        return this.commonApiService.get('v1/PurchaseDetails/' + url);
    }

    getPurchaseDetail(id?: string) {
        return this.commonApiService.get('v1/PurchaseDetails/GetPurchaseDetail?id=' + id);
        // return this.commonApiService.get('v1/PurchaseDetails/GetPurchaseDetail?id=51');
    }

    getPurchaseList(data: any) {
        Object.keys(data).forEach(key => {
            if (key != 'isfilterneeded' && (!data[key] || data[key] == "undefined")) {
                delete data[key];
            }
            if (key == 'enrollmentfromdate' || key == 'enrollmenttodate') {
                data[key] = (new Date(data[key]['year'], data[key]['month'] - 1, data[key]['day'])).toISOString();
            }
        });
        return this.commonApiService.post('v1/PurchaseDetails/GetPurchaseList', data);
    }

    getSummary(data) {
        Object.keys(data).forEach(key => {
            if (key != 'isfilterneeded' && (!data[key] || data[key] == "undefined")) {
                delete data[key];
            }
            if (key == 'enrollmentfromdate' || key == 'enrollmenttodate') {
                data[key] = (new Date(data[key]['year'], data[key]['month'] - 1, data[key]['day'])).toISOString();
            }
        });
        return this.commonApiService.post('v1/SubscriptionMetada/PurchaseSummary', data);
    }

    getSubscriptionMetadaById(id?: string) {
        return this.commonApiService.get('v1/SubscriptionMetada' + (id ? '/' + id : ''));
    }

    getSubscriptionMetada(filterObj?: any) {
        return this.commonApiService.get('v1/SubscriptionMetada', filterObj);
    }

    manageSubscriptionMetada(data: any, id?: string) {
        return this.commonApiService.post('v1/SubscriptionMetada', data);
    }

    manageUserSubscriptions(data: any, id?: string) {
        return this.commonApiService.post('v1/Subscription', data);
    }

    generateSignableReceipt(data: any) {
        return this.commonApiService.post('v1/PurchaseDetails/GetgenerateSignableReceipt', data);
    }

    generateSignedReceipt(data: any) {
        let formData = new FormData();
        Object.keys(data).forEach(key => {
            formData.append(key, data[key]);
        });
        return this.commonApiService.postWithFormData('v1/PurchaseDetails/UploadSignedReceipt', formData);
    }

    deleteSubscriptionMetada(id) {
        return this.commonApiService.delete('v1/SubscriptionMetada/' + id);
    }

    AddSchoolDetails(data: any, id?: string) {
        let formData = new FormData();
        Object.keys(data).forEach(key => {
            if (data[key]) {
                if (key == 'schoollicense' || key == 'registerationpaper') {
                    for (let index = 0; index < data[key].length; index++) {
                        if (!data[key][index]['id'])
                            formData.append(key, data[key][index]);
                    }
                } else {
                    formData.append(key, data[key]);
                }
            }
        });
        if (id) {
            return this.commonApiService.putWithFormData('v1/PurchaseDetails/UpdateSchoolDetails/' + id, formData);
        }
        return this.commonApiService.postWithFormData('v1/PurchaseDetails/AddSchoolDetails', formData);
    }

    AddIndividualDetails(data: any, id?: string) {
        let formData = new FormData();
        Object.keys(data).forEach(key => {
            if (data[key]) {
                if (key == 'studenttazrika' || key == 'parenttazrika' || key == 'previousmarksheets') {
                    for (let index = 0; index < data[key].length; index++) {
                        if (!data[key][index]['id'])
                            formData.append(key, data[key][index]);
                    }
                } else {
                    formData.append(key, data[key]);
                }
            }
        });
        if (id) {
            return this.commonApiService.putWithFormData('v1/PurchaseDetails/UpdateIndividualDetails/' + id, formData);
        }
        return this.commonApiService.postWithFormData('v1/PurchaseDetails/AddIndividualDetails', formData);
    }
    saveParent(data) {
        return this.commonApiService.post('v1/Subscription/SaveParent',data);
    }
    getIndividualDetailById(id) {
        return this.commonApiService.get('v1/PurchaseDetails/GetIndividualDetailById?id=' + id);
    }

    getSchoolDetailById(id) {
        return this.commonApiService.get('v1/PurchaseDetails/GetSchoolDetailById?id=' + id);
    }
    getAgentDetail() {
        return this.commonApiService.get('v1/SalesAgent/GetAgentComission');
    }
    removeParent(studentId, parentId) {
        return this.commonApiService.delete('v1/PurchaseDetails/RemoveParentUser?studentid=' + studentId + '&parentid=' + parentId);
    }
    getReceiptFileSigned(data) {
        let formData = new FormData();
        Object.keys(data).forEach(key => {
            if (['id'].indexOf(key) < 0)
                formData.append(key, data[key]);
        });
       return this.commonApiService.postWithFormData('v1/Files/SalesPurchaseReceipt', formData);
    }
    getIndividualDocumentFileSigned(data) {
        let formData = new FormData();
        Object.keys(data).forEach(key => {
            if (['id'].indexOf(key) < 0)
                formData.append(key, data[key]);
        });
       return this.commonApiService.postWithFormData('v1/Files/SalesPurchaseIndividualDetailsDocument', formData);
    }
    getSchoolDocumentFileSigned(data) {
        let formData = new FormData();
        Object.keys(data).forEach(key => {
            if (['id'].indexOf(key) < 0)
                formData.append(key, data[key]);
        });
       return this.commonApiService.postWithFormData('v1/Files/SalesPurchaseSchoolDetailsDocument', formData);
    }
}