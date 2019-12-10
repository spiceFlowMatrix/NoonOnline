import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { URLSearchParams } from '@angular/http';
import { Observable, of, throwError } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import * as _ from 'lodash';
import { environment } from '../../../environments/environment';

@Injectable()
export class CommonAPIService {

    private _adminHeaders = new HttpHeaders({
        'Accept': 'application/json',
        'Content-Type': 'application/json'
    })

    constructor(private http: HttpClient) {

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
        return this.http.get<any>(environment.apiEndpoint + url + (urlSearchParams.toString() ? "?" + urlSearchParams.toString() : ''), { headers: this.getAdminHeaders() })
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

        return this.http.get<any>('http://192.168.22.24:8001/api/getJoinTeamData', { headers: this.getAdminHeaders() })
            .pipe(
                map(response => response),
                catchError(error => {
                    return throwError(error);
                })
            );
    };

    getById(url: string, id: any): Observable<any> {
        return this.http.get<any>(environment.apiEndpoint + url + id, { headers: this.getAdminHeaders() })
            .pipe(
                map(response => response),
                catchError(error => {
                    return throwError(error);
                })
            );
    };

    post(url: string, data: any): Observable<any> {
        return this.http.post<any>(environment.apiEndpoint + url, data, { headers: this.getAdminHeaders() })
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
        return this.http.post<any>(environment.apiEndpoint + url, data, { headers: hdrs })
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
            return this.http.put<any>(environment.apiEndpoint + url + '/' + id, data, { headers: this.getAdminHeaders() })
                .pipe(
                    map(response => response),
                    catchError(error => {
                        return throwError(error);
                    })
                );
        } else {
            return this.http.put<any>(environment.apiEndpoint + url, data, { headers: this.getAdminHeaders() })
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
        return this.http.put<any>(environment.apiEndpoint + url, data, { headers: userHeaders })
            .pipe(
                map(response => response),
                catchError(error => {
                    return throwError(error);
                })
            );
    };

    delete(url: string): Observable<object> {
        return this.http.delete<any>(environment.apiEndpoint + url, { headers: this.getAdminHeaders() })
            .pipe(
                map(response => response),
                catchError(error => {
                    return throwError(error);
                })
            );
    };

    getImage(imageUrl: string) {
        // imageUrl = imageUrl.replace(/^https:\/\//i, 'http://');
        return this.http.get(imageUrl, { observe: 'response', responseType: 'blob' })
        .pipe(
            map(response => response),
        ); 
  
    }
    getAudio(audioUrl : string){
        return this.http.get(audioUrl, {observe: 'response' , responseType: 'blob'}).pipe(
            map(response => response),
            catchError(error => {
                return throwError(error);
            }
            )
        );
    }

}