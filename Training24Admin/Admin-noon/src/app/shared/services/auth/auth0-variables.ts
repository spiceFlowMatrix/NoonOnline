import { environment } from './../../../../environments/environment';

interface AuthConfig {
  clientID: string;
  domain: string;
  callbackURL: string;
  audience: string;
}
const callback = window.location.href;
export const AUTH_CONFIG: AuthConfig = {
  clientID: environment.clientID,
  // clientID: 'JrqsuJsTpJ7Ez8u86HsoWtamT1xR3LOU',
  domain: environment.authDomain,
  // // callbackURL: 'http://localhost:3000/callback',
  // clientID: 'TWFR8jVnGukdFXw3qaTq5RcilMOjVh4A',
  // domain: 'pradip-ibl.auth0.com',
  callbackURL: callback,
  audience: environment.audience
};
