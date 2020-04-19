import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { URLSearchParams } from '@angular/http';
import { Observable, of, throwError } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import * as _ from 'lodash';
import { environment } from '../../../environments/environment';

@Injectable()
export class CommonAPIService {
    erpurl: any;
    erpusername: any;
    erpPassword: any;
    apiEndpoint = "/api/"
    private _adminHeaders = new HttpHeaders({
        'Accept': 'application/json',
        'Content-Type': 'application/json'
    });

    public erpHeader = new HttpHeaders({
        Accept: "application/json",
        "Content-Type": "application/json"
    });

    constructor(private http: HttpClient) {

    }

    setErpCredentials(data) {
        this.erpPassword = data.pw;
        this.erpusername = data.un;
        this.erpurl = data.url;
    }
    public erppostLogin(url) {
        return this.http
            .post<any>(this.erpurl + url, { "UserName": this.erpusername, "Password": this.erpPassword })
            .pipe(
                map(response => response),
                catchError(error => {
                    return throwError(error);
                })
            );
    }
    public geterp(url,token) {
        this.erpHeader = new HttpHeaders({
            Authorization: "Bearer " + token,
        });
        return this.http
            .get<any>(this.erpurl + url, {
                headers: this.erpHeader
            })
            .pipe(
                map(response => response),
                catchError(error => {
                    return throwError(error);
                })
            );
    }

    getWithoutHeader(url) {
        return this.http
            .get<any>(
                this.apiEndpoint +
                url
            )
            .pipe(
                map(response => response),
                catchError(error => {
                    return throwError(error);
                })
            );
    }

    postErp(url,token,data) {
        this.erpHeader = new HttpHeaders({
            Authorization: "Bearer " + token,
        });
        return this.http
            .post<any>(this.erpurl + url, data, {
                headers: this.erpHeader
            })
            .pipe(
                map(response => response),
                catchError(error => {
                    return throwError(error);
                })
            );
    }

    public getAdminHeaders(): HttpHeaders {
        this._adminHeaders = new HttpHeaders(
            {
                'Authorization': 'Bearer ' + localStorage.getItem('access_token'),
                'id_token': 'Bearer ' + localStorage.getItem('id_token'),
            });
        return this._adminHeaders;
    }

    private getUrlSearchParams(object): URLSearchParams {
        let urlSearchParams = new URLSearchParams();
        for (let key in object) {
            if (object.hasOwnProperty(key)) {
                urlSearchParams.set(key, object[key])
            }
        }
        return urlSearchParams;
    }

    get(url: string, params?: any): Observable<any> {
        let urlSearchParams = new URLSearchParams();
        if (params) {
            delete params.totalCount;
            urlSearchParams.appendAll(this.getUrlSearchParams(params));
            if (params.pfields) {
                params.pfields.forEach(field => {
                    urlSearchParams.append('pfields', field);
                });
            }
            delete params.pfields;
        }

        return this.http.get<any>(this.apiEndpoint + url + (urlSearchParams.toString() ? "?" + urlSearchParams.toString() : ''), { headers: this.getAdminHeaders() })
            .pipe(
                map(response => response),
                catchError(error => {
                    return throwError(error);
                })
            );
    };

    tempGet(url: string, params?: any): Observable<any> {
        let urlSearchParams = new URLSearchParams();
        if (params) {
            urlSearchParams.appendAll(this.getUrlSearchParams(params));
            if (params.pfields) {
                params.pfields.forEach(field => {
                    urlSearchParams.append('pfields', field);
                });
            }
            delete params.pfields;
        }

        return this.http.get<any>('https://apis.goleagueinc.com/api/getJoinTeamData', { headers: this.getAdminHeaders() })
            .pipe(
                map(response => response),
                catchError(error => {
                    return throwError(error);
                })
            );
    };

    getById(url: string, id: any): Observable<any> {
        return this.http.get<any>(this.apiEndpoint + url + id, { headers: this.getAdminHeaders() })
            .pipe(
                map(response => response),
                catchError(error => {
                    return throwError(error);
                })
            );
    };

    post(url: string, data: any): Observable<any> {
        return this.http.post<any>(this.apiEndpoint + url, data, { headers: this.getAdminHeaders() })
            .pipe(
                map(response => response),
                catchError(error => {
                    return throwError(error);
                })
            );
    };

    postWithFormData(url: string, data?: FormData): Observable<any> {
        let userHeaders = _.clone(this._adminHeaders);
        delete userHeaders['Content-Type'];
        let hdrs = new HttpHeaders({
            'Authorization': 'Bearer ' + localStorage.getItem('access_token'),
            'id_token': 'Bearer ' + localStorage.getItem('id_token')
        })
        return this.http.post<any>(this.apiEndpoint + url, data, { headers: hdrs })
            .pipe(
                map(response => response),
                catchError(error => {
                    return throwError(error);
                })
            );
    };

    put(url: string, data?: any, id?: any): Observable<any> {
        if (typeof data === 'object' && data.id) {
            delete data.id;
        }

        if (id) {
            return this.http.put<any>(this.apiEndpoint + url + '/' + id, data, { headers: this.getAdminHeaders() })
                .pipe(
                    map(response => response),
                    catchError(error => {
                        return throwError(error);
                    })
                );
        } else {
            return this.http.put<any>(this.apiEndpoint + url, data, { headers: this.getAdminHeaders() })
                .pipe(
                    map(response => response),
                    catchError(error => {
                        return throwError(error);
                    })
                );
        }
    };

    putWithFormData(url: string, data?: FormData): Observable<any> {
        let userHeaders = _.clone(this._adminHeaders);
        userHeaders.delete('Content-Type')
        return this.http.put<any>(this.apiEndpoint + url, data, { headers: userHeaders })
            .pipe(
                map(response => response),
                catchError(error => {
                    return throwError(error);
                })
            );
    };

    delete(url: string): Observable<object> {
        return this.http.delete<any>(this.apiEndpoint + url, { headers: this.getAdminHeaders() })
            .pipe(
                map(response => response),
                catchError(error => {
                    return throwError(error);
                })
            );
    };

}