**Tech used**
- C# with ASP.NET Core Web API
- Microsoft Visual Studio 2022 
- Microsoft SQL Server 2022


**Configuration**
1. Database
   - Require to have Microsoft SQL Server 2022
   - Require to have Microsoft SQL Server Management 20 (SSMS)
   - Create a new database with SSMS
   - Open the **Database** folder
     ![image](https://github.com/user-attachments/assets/25eb0676-d2d3-4538-a7ad-a8671a9941f5)
   - Open the CREATE_DB.bat with Notepad, Notepad++ or any editor tools
   - Set your database connection here and save it
     ![image](https://github.com/user-attachments/assets/2a583941-9034-4cb3-b2e7-81c634514552)    
   - Run the batch file by double click it
     ![image](https://github.com/user-attachments/assets/5cb8c8d5-b4b4-462f-a011-b56a8047c3ef)
   - It will help to create the database table by using the sql file in **Table** folder
     ![image](https://github.com/user-attachments/assets/0c785b6b-b402-4afe-8e1f-653c9dff394f)

2. Connection string
   - Open the _CarParkFinder.sln_ with Microsoft Visual Studio 2022
   - Open appsettings.json
   - Modify your database connection strings here
     ![image](https://github.com/user-attachments/assets/08c7f59f-22b8-4292-bf4d-ad419a7130d8)
   - Save the file
     
3. Run the application
