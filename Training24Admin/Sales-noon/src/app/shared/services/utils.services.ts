import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { JwtHelperService } from '@auth0/angular-jwt';
import * as moment from 'moment';
import { AuthenticationService } from './authentiction.services';

@Injectable()
export class UtilService {
	private jwtHelper: JwtHelperService = new JwtHelperService();
	private userRoles: any = [];
	public discountPackages: any = [];
	private namespace: string = 'https://noon-online/';

	constructor(
		public router: Router,
		private toastr: ToastrService,
		private authenticationService: AuthenticationService
	) {

	}
	setUserRoles(res) {
		this.userRoles = res;
	}

	getRoles(): any[] {
		return this.userRoles;
	}

	showErrorToast(title: string = 'Something went wrong.', message: string = "Please, try again later.") {
		this.toastr.error(message, title);
	}

	showPasswordToast() {
		this.toastr.error("Password Is Required", "Error", { disableTimeOut: true });
	}

	showErrorWarning(title: string = 'Something went wrong.', message: string = "Please, try again later.") {
		this.toastr.warning(message, title);
	}

	showErrorSuccess(title: string = 'Something went wrong.', message: string = "Please, try again later.") {
		this.toastr.success(message, title);
	}

	public showErrorCall(err: any, show: number = 0) {
		try {
			if (err.error) {
				try {
					let errBody: any = err.error;
					let errStr = errBody.message;
					// Object.keys(errBody).map(val => errStr += errBody[val]);
					if (err.status === 404) {
						return
						// if (show == 0)
						// 	this.showErrorToast(errStr);
					} else if (err.status == 406 && errBody.message) {
						if (errBody.message == 'Sorry you have to login first') {
							this.logout()
						}
						if (show == 0)
							this.showErrorToast(errBody.message);
					} else {
						if (show == 0)
							this.showErrorToast(errStr);
					}
					if (err.status === 401 && localStorage.getItem('isLoggedin')) {
						this.logout();
					}
				} catch (error) {
					this.showErrorToast();
				}
			} else {
				if (err.status === 404)
					return
				this.showErrorToast();
			}
		} catch (error) {
			this.showErrorToast();
		}
	}

	getAccountDetails() {
		try {
			return this.jwtHelper.decodeToken(localStorage.getItem('id_token'));
		} catch (error) {
			this.logout();
		}
		return {};
	}

	checkRefreshToken() {
		let user: any = this.getAccountDetails();
		if (user.exp && typeof user.exp == 'number') {
			if (this.jwtHelper.isTokenExpired(localStorage.getItem('access_token'))) {
				this.logout();
				return false
			}
			let diff = moment(user.exp * 1000, 'x').diff(moment(), 'days');
			if (diff < 2) {
				return true;
			}
		}
		return false;
	}

	getRole() {
		try {
			return this.getAccountDetails()[this.namespace + 'roles'];
		} catch (error) {
			this.logout();
		}
	}

	public openFileSelecter(id) {
		let element = document.getElementById(id);
		if (element)
			element.click();
	}

	public allowOnlyNumber(querySelector, isFloatingLabel?: boolean) {
		let element: HTMLInputElement = document.querySelector('#' + querySelector);
		if (element) {
			element.addEventListener('keypress', (e) => {
				var charCode = (e.which) ? e.which : e.keyCode;
				if (!isFloatingLabel && charCode != 8 && charCode != 0 && e.key != '+' && charCode != 32 && (charCode < 48 || charCode > 57)) {
					e.preventDefault();
				} else if (isFloatingLabel && charCode != 8 && charCode != 0 && e.key != '+' && charCode != 32 && (charCode < 48 || charCode > 57)) {
					if (e.key == '.' && element.value.indexOf('.') > -1)
						e.preventDefault();
				}
				return;
			});
		}
	}

	public getRandomCode(unit) {
		return unit ? unit + "-" + (new Date().getTime()) : (new Date().getTime());
	}

	getLocation() {
		return window.location;
	}

	downloadFile(documenturl) {
		if (documenturl) {
			var element = document.createElement('a');
			element.setAttribute('href', documenturl);
			element.setAttribute('download', "filename");
			element.setAttribute('target', "_blank");
			element.style.display = 'none';
			document.body.appendChild(element);
			element.click();
			document.body.removeChild(element);
		}
	}

	public logout() {
		this.authenticationService.logout();
	}
}