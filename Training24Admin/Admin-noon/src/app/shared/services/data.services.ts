import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable()
export class DataService {
    coursesList: any = [];
    gradeData: any = {};
    constructor() {

    }

    // Observable string sources
    private sideMenuObservable = new Subject<boolean>();
    private courseDataRefreshObservable = new Subject<any>();

    // Observable string streams
    sideMenu$ = this.sideMenuObservable.asObservable();
    courseDataRefresh$ = this.courseDataRefreshObservable.asObservable();


    refreshCourseData(data: any) {
        this.courseDataRefreshObservable.next(data);
    }

    setSideMenu(isopen: boolean) {
        this.sideMenuObservable.next(isopen);
    }

    getCourseList() {
        return this.coursesList;
    }

    setFilterData(key, value) {
        this.gradeData[key] = value;
    }
}