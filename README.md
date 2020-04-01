1. How to run the application in a Windows 10 or Ubuntu 18.04.04 LTS.

1.1 Run angular applications

1.1.1 Run application Windows

> First we required to install Nodejs which you can get from https://nodejs.org/en/download/ 
> After installing nodejs we have to install angular with the command 

> npm install -g @angular/cli

> After installing angular we have to go to appropriate repo in order to run applications.
> To run we need 2 commands
  - npm install
  - ng serve -o (for running it locally)
  - ng build --prod (to make build of the repo to deploy it on server which process create build in dist folder)


1.1.2 Run application on Ubuntu
> First install Nodejs from below commands

> sudo apt-get update
 
> sudo apt-get install nodejs
 
> sudo apt-get install npm

> after that Install Angular

> npm install -g @angular/cli

> After installing angular we have to go to appropriate repo in order to run applications.
> To run we need 2 commands
  - npm install
  - ng serve -o (for running it locally)
  - ng build --prod (to make build of the repo to deploy it on server which process create build in dist folder)

1.2 Run .Net core application

1.2.1 Run .Net core in Windows

> download and install .net core latest sdk from below link:

  - https://dotnet.microsoft.com/download

> download and install .net core runtime from below link:
  
  - https://dotnet.microsoft.com/download
  
> Open project in visual studio

  - Right click On Training24Admin > Build
  
> Now after sucessful build you can run project by click F5 or option provided in toolbar

> To publish code you can right click on Traning24Admin > Publish

1.2.1 Run .Net core in Ubuntu

> The first thing to do is install the necessary repository. To do this, open a terminal window and issue the following commands:

  - wget -q https://packages.microsoft.com/config/ubuntu/18.04/packages-microsoft-prod.deb
  - sudo dpkg -i packages-microsoft-prod.deb
  
> Once the repository has been added, there's a single dependency that must be installed. Do this with the following commands:

  - sudo add-apt-repository universe
  - sudo apt-get install apt-transport-https
  
> Finally, install DotNet Core with these commands:

  - sudo apt-get update
  - sudo apt-get install dotnet-sdk-2.1 <replace with latest version want to use>

> Build all class librarey from solution 
 
  - dotnet build
  
> Locate to main folder and run application

  - dotnet run
  
> To publish code in release mode use below command :

  - dotnet publish -c Release

2)to run hosted applications (web apps) via docker image
Here i got best explanation for running angular webapp via docker image
https://www.whitehorses.nl/blog/running-angular-application-docker-dummies

3)The full list of prerequisite infrastructure for the application and critical configuration guidelines for them.
There’s no prerequisite infrastructure for the  application 
Issue we might face in future is sometimes if there’s change in package.json we might have to add the new command in ci/cd for installing new package due to that we have change in package.json file.
Because sometimes npm  install failed to install newly added package.

And in critical configuration we have auth0 variable which we are transforming from local environment to system environment.

4) The full list of system environment variables required to run the application in its target environment

https://docs.google.com/spreadsheets/d/1AjhI6ftsCt1G5oAxEgmtUolrOkbvWlGSu-28Jemgums/edit?usp=sharing

5) The full list of system environment variables required to successfully perform the CI/CD process for the application.

https://docs.google.com/spreadsheets/d/1AjhI6ftsCt1G5oAxEgmtUolrOkbvWlGSu-28Jemgums/edit?usp=sharing


6) Android Environment variable document listed below :

https://docs.google.com/document/d/1ZaqIHpCQ4N0X8MfXYiqRJTo48UzLRFQ6UuOqTOJ5mC8/edit?usp=sharing


