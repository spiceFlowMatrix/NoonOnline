// import { Injectable } from '@angular/core';
// import { AUTH_CONFIG } from './auth0-variables';
// import { Router } from '@angular/router';
// import * as auth0 from 'auth0-js';

// (window as any).global = window;

// @Injectable()
// export class AuthService {

//   auth0 = new auth0.WebAuth({
//     clientID: AUTH_CONFIG.clientID,
//     domain: AUTH_CONFIG.domain,
//     responseType: 'token id_token',
//     redirectUri: AUTH_CONFIG.callbackURL
//   });

//   constructor(public router: Router) {}

//   public login(): void {
//     this.auth0.authorize();
//   }

//   public handleAuthentication(): void {
//     this.auth0.parseHash((err, authResult) => {
//       if (authResult && authResult.accessToken && authResult.idToken) {
//         this.setSession(authResult);
//         this.router.navigate(['dashboard']);
//         // this.router.navigate(['/home']);
//       } else if (err) {
//         debugger
//         this.router.navigate(['/home']);
//         console.log(err);
//         alert(`Error: ${err.error}. Check the console for further details.`);
//       }
//     });
//   }

//   private setSession(authResult): void {
//     // Set the time that the access token will expire at
//     const expiresAt = JSON.stringify((authResult.expiresIn * 1000) + new Date().getTime());
//     localStorage.setItem('access_token', authResult.accessToken);
//     localStorage.setItem('id_token', authResult.idToken);
//     localStorage.setItem('expires_at', expiresAt);
//   }

//   public logout(): void {
//     // Remove tokens and expiry time from localStorage
//     localStorage.removeItem('access_token');
//     localStorage.removeItem('id_token');
//     localStorage.removeItem('expires_at');
//     // Go back to the home route
//     this.router.navigate(['/']);
//   }

//   public isAuthenticated(): boolean {
//     // Check whether the current time is past the
//     // access token's expiry time
//     const expiresAt = JSON.parse(localStorage.getItem('expires_at') || '{}');
//     return new Date().getTime() < expiresAt;
//   }

// }
import { Injectable } from '@angular/core';
import { AUTH_CONFIG } from './auth0-variables';
import { Router } from '@angular/router';
import { Observable, Subject, Observer, of, timer } from 'rxjs';
import { flatMap } from 'rxjs/operators';

// import 'rxjs/add/operator/filter';
import * as auth0 from 'auth0-js';
import { pipe } from '@angular/core/src/render3/pipe';

(window as any).global = window;

@Injectable()
export class AuthService {
  auth0 = new auth0.WebAuth({
    clientID: AUTH_CONFIG.clientID,
    domain: AUTH_CONFIG.domain,
    audience: AUTH_CONFIG.audience + "api/v2/",
    responseType: 'token id_token',
    redirectUri: localStorage.getItem('callback-sales'),
      // redirectUri: AUTH_CONFIG.callbackURL,
    scope: 'openid profile'
  });

  userProfile: any;
  refreshSubscription: any;
  private observer = new Subject<boolean>();
  // ssoAuthComplete$: Observable<boolean> = new Observable(
  //   obs => (this.observer = obs)
  // );

  ssoAuthComplete$ = this.observer.asObservable();

  constructor(public router: Router) { }

  public login(): void {
    this.auth0.authorize();
  }

  public handleAuthentication() {
    return new Promise((resolve, reject) => {
      this.auth0.parseHash((err, authResult) => {
        if (authResult && authResult.accessToken && authResult.idToken) {
          this.setSession(authResult);
          window.location.hash = '';
          // this.router.navigate(['/home']);
          let Obj: any = {
            deviceType: 'browser',
            deviceToken: '',
            version: ''
          };
          try {
            // Obj = JSON.parse(localStorage.getItem('data'));
            Obj.deviceType = this.get_browser().name;
            Obj.version = this.get_browser().version;
            Obj['login_data'] = true;
            Obj['key'] = 1;
            // Obj.authresult = authResult;
            localStorage.removeItem('data');
            resolve(Obj);
          } catch (error) {
            Obj['key'] = 2;
            resolve(Obj)
          }
        } else if (err) {
          reject(err);
          this.router.navigate(['/sales']);
          console.log(err);
          alert(`Error: Something went wrong.`);
        }
        resolve({ key: 0 })
      });
    });
  }

  public getProfile(cb): void {
    const accessToken = localStorage.getItem('access_token');
    if (!accessToken) {
      throw new Error('Access token must exist to fetch profile');
    }

    const self = this;
    this.auth0.client.userInfo(accessToken, (err, profile) => {
      if (profile) {
        self.userProfile = profile;
      }
      cb(err, profile);
    });
  }

  private setSession(authResult): void {
    // Set the time that the access token will expire at
    const expiresAt = JSON.stringify(
      authResult.expiresIn * 1000 + new Date().getTime()
    );
    localStorage.setItem('access_token', authResult.accessToken);
    localStorage.setItem('id_token', authResult.idToken);
    localStorage.setItem('expires_at', expiresAt);
    localStorage.setItem('state', authResult.state);

    this.scheduleRenewal();
  }

  public logout(): void {
    // Remove tokens and expiry time from localStorage
    localStorage.removeItem('access_token');
    localStorage.removeItem('id_token');
    localStorage.removeItem('expires_at');
    this.auth0.logout({
      returnTo: 'http://localhost:4200',
      clientID: AUTH_CONFIG.clientID
    });
    this.unscheduleRenewal();
    // Go back to the home route
    this.router.navigate(['/']);
  }

  public isAuthenticated(): boolean {
    // Check whether the current time is past the
    // access token's expiry time
    const expiresAt = JSON.parse(localStorage.getItem('expires_at') || '{}');
    return new Date().getTime() < expiresAt;
  }

  public renewToken() {
    if (!localStorage.getItem('access_token') || !localStorage.getItem('id_token')) {
      return true;
    }
    this.auth0.checkSession({
      audience: AUTH_CONFIG.audience + 'api/v2/',
      scope: 'openid profile',
    },
      (err, result) => {
        if (err) {
          alert(
            `Could not get a new token (${err.error}: ${err.error_description}).`
          );
          let loginModel: any = {};
          loginModel.deviceType = this.get_browser().name;
          loginModel.version = this.get_browser().version;
          localStorage.setItem("data", JSON.stringify(loginModel));
          this.login();
        } else {
          this.setSession(result);
          this.observer.next(true);
        }
        return false;
      }
    );
  }

  public scheduleRenewal() {
    if (!this.isAuthenticated()) return;
    this.unscheduleRenewal();

    const expiresAt = JSON.parse(window.localStorage.getItem('expires_at'));
    const source = of(expiresAt).pipe(flatMap((expiresAt: any) => {
      const now = Date.now();

      // Use the delay in a timer to
      // run the refresh at the proper time
      return timer(Math.max(1, expiresAt - now));
    }));
    // const source = Observable.of(expiresAt).flatMap(expiresAt => {
    //   const now = Date.now();

    //   // Use the delay in a timer to
    //   // run the refresh at the proper time
    //   return timer(Math.max(1, expiresAt - now));
    // });

    // Once the delay time from above is
    // reached, get a new JWT and schedule
    // additional refreshes
    this.refreshSubscription = source.subscribe(() => {
      this.renewToken();
      this.scheduleRenewal();
    });
  }

  public unscheduleRenewal() {
    if (!this.refreshSubscription) return;
    this.refreshSubscription.unsubscribe();
  }

  get_browser() {
    try {
      var ua = navigator.userAgent, tem, M = ua.match(/(opera|chrome|safari|firefox|msie|trident(?=\/))\/?\s*(\d+)/i) || [];
      if (/trident/i.test(M[1])) {
        tem = /\brv[ :]+(\d+)/g.exec(ua) || [];
        return { name: 'IE', version: (tem[1] || '') };
      }
      if (M[1] === 'Chrome') {
        tem = ua.match(/\bOPR|Edge\/(\d+)/)
        if (tem != null) { return { name: 'Opera', version: tem[1] }; }
      }
      M = M[2] ? [M[1], M[2]] : [navigator.appName, navigator.appVersion, '-?'];
      if ((tem = ua.match(/version\/(\d+)/i)) != null) { M.splice(1, 1, tem[1]); }
      return {
        name: M[0],
        version: M[1]
      };
    } catch (error) {
      return {
        name: navigator.userAgent,
        version: navigator.appVersion
      };
    }
  }
}
