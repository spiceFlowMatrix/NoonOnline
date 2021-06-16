// https://medium.com/@natchiketa/angular-cli-and-os-environment-variables-4cfa3b849659

import { writeFile } from 'fs';
import { argv } from 'yargs';

// This is good for local dev environments, when it's better to
// store a projects environment variables in a .gitignore'd file
require('dotenv').config();

// Would be passed to script like this:
// `ts-node set-env.ts --environment=dev`
// we get it from yargs's argv object
const environment = argv.environment ? argv.environment : '';
const isProd = environment === 'prod';
var API_URL = '';
var CLIENT_ID = process.env.CLIENT_ID ? process.env.CLIENT_ID :'';
var AUTH_DOMAIN =  process.env.AUTH_DOMAIN ? process.env.AUTH_DOMAIN :'';
var AUDIENCE =  process.env.AUDIENCE ? process.env.AUDIENCE :'';
var CALLBACK_URL =  process.env.CALLBACK_URL ? process.env.CALLBACK_URL :'';
var LOGOUT_CALLBACK_URL =  process.env.LOGOUT_CALLBACK_URL ? process.env.LOGOUT_CALLBACK_URL :'';

if (environment) {
  if (environment == 'prod') {
    API_URL = process.env.PROD_API_URL;
  } else if (environment == 'staging') {
    API_URL = process.env.STAGE_API_URL;
  } else {
    API_URL = 'https://coreapistaging.noon-online.com/api/'
  }
}

// const targetPath = `./src/environments/environment.${environment}.ts`;
const targetPath = `./src/environments/environment.prod.ts`;
const envConfigFile = `
export const environment = {
  production: true,
  apiEndpoint: "${API_URL}",  
  clientID: "${CLIENT_ID}",  
  authDomain: "${AUTH_DOMAIN}",  
  audience: "${AUDIENCE}",  
  callbackURL: "${CALLBACK_URL}",  
  logoutcallbackURL: "${LOGOUT_CALLBACK_URL}",  
};
`
// export const environment = {
//   production: ${isProd},
//   superSecretKey: "${process.env.SUPER_SECRET_CRED1}",
//   superDoubleSecret: "${process.env.SUPER_SECRET_CRED2}" 
// };
writeFile(targetPath, envConfigFile, function (err) {
  if (err) {
    console.log(err);
  }

  console.log(`Output generated at ${targetPath}`);
});