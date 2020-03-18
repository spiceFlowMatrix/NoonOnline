//export const environment = {
//  production: true,
//  apiEndpoint: 'http://123.201.6.140:8070/api/',
//  audience:'https://satyamdev.auth0.com/',
//  clientID:'ptpzrLrRrMRlRYi0GqOOsb-3YnXbWDoJ',
//  authDomain:'satyamdev.auth0.com',
//  callbackURL:'http://prod.feedback.noon.shopostreet.in/login',
//  logoutcallbackURL:'http://prod.feedback.noon.shopostreet.in/login?logout=true'
//  // apiEndpoint: 'https://coreapistaging.noon-online.com/api/',
//};

export const environment = {
    production: true,
    clientID: 'ptpzrLrRrMRlRYi0GqOOsb-3YnXbWDoJ',
    authDomain: 'satyamdev.auth0.com',
    audience: 'https://satyamdev.auth0.com/',
    apiEndpoint: '/api/',
    callbackURL: 'http://localhost:6020/feedbacknoon/feed/',
    logoutcallbackURL: 'http://localhost:6020/feedbacknoon/feed?logout=true'
};

