# SoftwareManager
Sample project using WebApi + OData + EF 6 + Aurelia with Visual Studio

This is still work in progress and does not follow best practices at some points (especially Aurelia I guess). Feel free to report issues and suggest improvements.

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
  
