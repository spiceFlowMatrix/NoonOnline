// The file contents for the current environment will overwrite these during build.
// The build system defaults to the dev environment which uses `environment.ts`, but if you do
// `ng build --env=prod` then `environment.prod.ts` will be used instead.
// The list of which env maps to which file can be found in `.angular-cli.json`.

//export const environment = {
//  production: false,
//  // apiEndpoint: 'https://training24-197210.appspot.com/api/'
//  // apiEndpoint: 'http://192.168.22.11:8091/api/'
//  // apiEndpoint: 'http://123.201.6.140:8060/api/'
//  clientID:'ptpzrLrRrMRlRYi0GqOOsb-3YnXbWDoJ',
//  authDomain:'satyamdev.auth0.com',
//  audience:'https://satyamdev.auth0.com/',
//  apiEndpoint: 'http://192.168.22.11:8070/api/',
//  // apiEndpoint: 'http://123.201.6.140:8070/api/',
//  callbackURL:'http://localhost:4200/sales',
//  logoutcallbackURL:'http://localhost:4200/sales?logout=true',
//  receiptTitle:'دوامداره انلاین زده کړه'
//  // apiEndpoint: 'https://coreapistaging.noon-online.com/api/'
//};

export const environment = {
    production: false,
    clientID: 'ptpzrLrRrMRlRYi0GqOOsb-3YnXbWDoJ',
    authDomain: 'satyamdev.auth0.com',
    audience: 'https://satyamdev.auth0.com/',
    apiEndpoint: '/api/',
    callbackURL: '',
    logoutcallbackURL: '',
    receiptTitle: 'دوامداره انلاین زده کړه'
};
