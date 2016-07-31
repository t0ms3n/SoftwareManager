# SoftwareManager
Sample N-tier project using WebApi2 + OData + EF 6 + Aurelia with Visual Studio

This project is still work in progress. The goal is to create a sample which per example goes furhter than just creating a basic OData WebApi which do not use DbContext directly.
Feel free to report issues and suggest improvements.

#Overview
The sample currently provides the following features
- OData API using WebApi 2
 + Entity Framework 6.1.3
 + does communicate with its own models (using AutoMapper, Projection and extraction of navigation properties from the query options)
 + Batchsupport
 + Modelvalidation using FluentValidation
- Single Page Application
 + built with Aurelia 1.0.0-rc.1.0.1
 + Typescript
 + Bootstrap

#Setup the database
1. To create the initial database you can use the SoftwareManager.DbMigrations
2. You have to create a database somewhere (configured is (localdb)\MSSQLLocalDB with Inital Catalog "SoftwareManager" in the App.config) 
3. Build the project
4. Call migrate_up.bat in the output folder

#Running the app
To run the app locally, follow these steps:
  1. Ensure that NodeJS is installed.
  2. Ensure ASP.NET Core is installed.
  3. Restore required Nuget packages
  4. In the SoftwareManager.SPA project folder, exeucte the following commands:
   
    ```shell
    npm install
    ```
  5. Ensure that jspm is installed. If you need to install it, use the following command:
  
    ```shell
    npm install -g jspm
    ```
  6. Install the SoftwareManager.SPA dependencies with jspm:
    
    ```shell
    jspm init
    jspm install
    ```
  7. If you have changed the database connection you have to adjust this within the ApplicationSettingService class (Common Project)
  8. Build the solution
  9. Start the configured IIS Express instance. This will boot up the WebApi as well.
  10. Browse to http://localhost:54812 to see the app.
  
