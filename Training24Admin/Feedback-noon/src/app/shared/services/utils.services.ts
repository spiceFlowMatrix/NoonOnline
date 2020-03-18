import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { ToastrService } from "ngx-toastr";
import { JwtHelperService } from "@auth0/angular-jwt";
import * as moment from "moment";
import * as _ from "lodash";
import { LanguageList } from "./../models/menu-lists";
// import { AuthenticationService } from './authentiction.services';
import { DomSanitizer } from "@angular/platform-browser";

@Injectable()
export class UtilService {
    private jwtHelper: JwtHelperService = new JwtHelperService();
    private userRoles: any = [];
    private grades: any = [];
    private namespace: string = "https://noon-online/";

    constructor(
        public router: Router,
        private toastr: ToastrService,
		private domSanitizer: DomSanitizer ,
		// private authenticationService: AuthenticationService
    ) {}

    rltAndLtr(langLocale) {
        let dom: any = document.querySelector("body");
        let rtlFound = dom.classList.toString().indexOf("rtl") > -1;
        if (
            (["fa", "ps"].indexOf(langLocale) > -1 && !rtlFound) ||
            (["en"].indexOf(langLocale) > -1 && rtlFound)
        ) {
            dom.classList.toggle("rtl");
        }
    }

    setUserRoles(res) {
        this.userRoles = res;
    }

    getRoles(): any[] {
        return this.userRoles;
    }

    setGrades(res) {
        this.grades = res;
    }

    getGrades(): any[] {
        return this.grades;
    }

    showErrorToast(
        title: string = "Something went wrong.",
        message: string = "Please, try again later."
    ) {
        this.toastr.error(message, title);
    }

    showErrorWarning(
        title: string = "Something went wrong.",
        message: string = "Please, try again later."
    ) {
        this.toastr.warning(message, title);
    }

    showErrorSuccess(
        title: string = "Something went wrong.",
        message: string = "Please, try again later."
    ) {
        this.toastr.success(message, title);
    }

    showErrorInfo(
        title: string = "Something went wrong.",
        message: string = "Please, try again later."
    ) {
        this.toastr.info(message, title);
    }

    public showErrorCall(err: any, show: number = 0) {
        try {
            if (err.error) {
                try {
                    let errBody: any = err.error;
                    let errStr = errBody.message;
                    // Object.keys(errBody).map(val => errStr += errBody[val]);
                    if (err.status === 404) {
                        return;
                        // if (show == 0)
                        // 	this.showErrorToast(errStr);
                    } else if (err.status == 406 && errBody.message) {
                        if (
                            errBody.message == "Sorry you have to login first"
                        ) {
                            this.logout();
                        }
                        if (show == 0) this.showErrorToast(errBody.message);
                    } else {
                        if (show == 0) this.showErrorToast(errStr);
                    }
                    if (
                        err.status === 401 &&
                        localStorage.getItem("isLoggedin")
                    ) {
                        this.logout();
                    }
                } catch (error) {
                    this.showErrorToast();
                }
            } else if (
                err.status === 401 &&
                localStorage.getItem("isLoggedin")
            ) {
                this.showErrorToast(
                    "Unauthorized access",
                    "Please login again."
                );
                this.router.navigateByUrl('/feed', { replaceUrl: true });
                localStorage.removeItem('access_token');
                localStorage.removeItem('token');
                localStorage.removeItem('isLoggedin');
                localStorage.removeItem('expires_at');
                localStorage.removeItem('id_token');
                localStorage.removeItem('state');
            } else {
                if (err.status === 404) return;
                this.showErrorToast();
            }
        } catch (error) {
            this.showErrorToast();
        }
    }

    getAccountDetails() {
        try {
            // console.log(this.parseJwt(localStorage.getItem("id_token")));
            // return this.jwtHelper.decodeToken(localStorage.getItem('token'));
            return this.jwtHelper.decodeToken(localStorage.getItem("id_token"));
        } catch (error) {
            this.logout();
        }
        return {};
    }

    checkRefreshToken() {
        let user: any = this.getAccountDetails();
        if (user.exp && typeof user.exp == "number") {
            if (
                this.jwtHelper.isTokenExpired(localStorage.getItem("id_token"))
            ) {
                this.logout();
                return false;
            }
            let diff = moment(user.exp * 1000, "x").diff(moment(), "days");
            if (diff < 2) {
                return true;
            }
        }
        return false;
    }

    getRole() {
        try {
            return this.getAccountDetails()[this.namespace + "roles"];
        } catch (error) {
            this.logout();
        }
    }

    public openFileSelecter(id) {
        let element = document.getElementById(id);
        if (element) element.click();
    }

    public allowOnlyNumber(querySelector, isFloatingLabel?: boolean) {
        let element: HTMLInputElement = document.querySelector(
            "#" + querySelector
        );
        if (element) {
            element.addEventListener("keypress", e => {
                var charCode = e.which ? e.which : e.keyCode;
                if (
                    !isFloatingLabel &&
                    charCode != 8 &&
                    charCode != 0 &&
                    e.key != "+" &&
                    charCode != 32 &&
                    (charCode < 48 || charCode > 57)
                ) {
                    e.preventDefault();
                } else if (
                    isFloatingLabel &&
                    charCode != 8 &&
                    charCode != 0 &&
                    e.key != "+" &&
                    charCode != 32 &&
                    (charCode < 48 || charCode > 57)
                ) {
                    if (e.key == "." && element.value.indexOf(".") > -1)
                        e.preventDefault();
                }
                return;
            });
        }
    }

    focusInput(id) {
        setTimeout(() => {
            let el: any = document.getElementById(id);
            if (el) el.focus();
        }, 500);
    }

    getTagName(data) {
        let gradeName = data.grade ? data.grade.name : "";
        let courseeName = data.course ? data.course.name : "";

        let otherLabel = "";
        if (data.category && data.category.id == 6) {
            otherLabel = data.chapter.name;
        } else {
            otherLabel = data.lesson ? data.lesson.name: '';
        }

        return gradeName + " " + courseeName + " " + otherLabel;
    }

    sanitize(url: string) {
        return this.domSanitizer.bypassSecurityTrustUrl(url);
    }

    parseJwt(token) {
        try {
            // Get Token Header
            const base64HeaderUrl = token.split(".")[0];
            const base64Header = base64HeaderUrl
                .replace("-", "+")
                .replace("_", "/");
            const headerData = JSON.parse(window.atob(base64Header));

            // Get Token payload and date's
            const base64Url = token.split(".")[1];
            const base64 = base64Url.replace("-", "+").replace("_", "/");
            const dataJWT = JSON.parse(window.atob(base64));
            dataJWT.header = headerData;

            // TODO: add expiration at check ...

            return dataJWT;
        } catch (err) {
            return false;
        }
    }

    public logout() {
        // this.authenticationService.logout();
    }
}
