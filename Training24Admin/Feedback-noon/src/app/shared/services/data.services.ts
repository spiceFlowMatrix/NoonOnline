import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable()
export class DataService {
    public coursesList: any = [];
    public users: any = {};
    // Observable string sources
    private feedbackDetailObservable = new Subject<boolean>();
    private managerObservable = new Subject<boolean>();
    // private staffObservable = new Subject<boolean>();
    private sideMenuObservable = new Subject<boolean>();
    private statusObservable = new Subject<boolean>();

    // Observable string streams
    feedbackDetail$ = this.feedbackDetailObservable.asObservable();
    managerUpdate$ = this.managerObservable.asObservable();
    // staffUpdate$ = this.staffObservable.asObservable();
    sideMenu$ = this.sideMenuObservable.asObservable();
    status$ = this.statusObservable.asObservable();

    constructor() {

    }

    updateManagerdetail(obj: any) {
        this.managerObservable.next(obj);
    }

    // updateStaffdetail(obj: any) {
    //     this.staffObservable.next(obj);
    // }

    setdetail(obj: any) {
        this.feedbackDetailObservable.next(obj);
    }

    updateStatusChange(obj: any) {
        this.statusObservable.next(obj);
    }

    setUsers(users) {
        this.users = {
            aafManagers: [],
            coordinators: [],
            filmingManagers: [],
            graphicsManagers: [],
            editingManagers: [],
            qas: [],
            feedbackedgeTeam: [],
            filmingStaffs: [],
            graphicsStaffs: [],
            editingStaffs: [],
        }

        for (let index = 0; index < users.length; index++) {
            if (users[index].roles && users[index].roles.length > 0) {
                if (users[index].roles.indexOf(6) > -1) {
                    this.users.aafManagers.push(users[index]);
                }
                if (users[index].roles.indexOf(7) > -1) {
                    this.users.coordinators.push(users[index]);
                }
                if (users[index].roles.indexOf(8) > -1) {
                    this.users.editingManagers.push(users[index]);
                }
                if (users[index].roles.indexOf(9) > -1) {
                    this.users.filmingManagers.push(users[index]);
                }
                if (users[index].roles.indexOf(10) > -1) {
                    this.users.graphicsManagers.push(users[index]);
                }
                if (users[index].roles.indexOf(11) > -1) {
                    this.users.qas.push(users[index]);
                }
                if (users[index].roles.indexOf(12) > -1) {
                    this.users.feedbackedgeTeam.push(users[index]);
                }
                if (users[index].roles.indexOf(14) > -1) {
                    this.users.filmingStaffs.push(users[index]);
                }
                if (users[index].roles.indexOf(15) > -1) {
                    this.users.editingStaffs.push(users[index]);
                }
                if (users[index].roles.indexOf(16) > -1) {
                    this.users.graphicsStaffs.push(users[index]);
                }
            }
        }
    }

    getUsers() {
        return this.users;
    }



    getCourseList(): any {
        return this.coursesList;
    }

    setSideMenu(isopen: boolean) {
        this.sideMenuObservable.next(isopen);
    }
}