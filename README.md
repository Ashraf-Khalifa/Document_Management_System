Document Management System

Overview:

Developed using ASP.NET Core Web API in Visual Studio, this project aims to enhance file interaction and management.
It efficiently handles various file formats like "image/jpeg," "image/png," and "application/pdf," with a maximum file size limit of 500 KB.

The user-friendly front-end interface features a prominent "Fetch Files" button accessible via "https://localhost:****/index.html." 
This button triggers requests to the back-end system powered by the ContactsController. The back-end offers a range of API functionalities:

- GetFiles: Retrieves URLs of files stored in a designated server folder.
- RemoveFile: Allows users to delete selected files from the server.
- UploadFiles: Simplifies file uploads while storing metadata in a dedicated database.
- DownloadPdfFile: Enables users to download specific files.

Central to the system's architecture is the ContactsAPIDbContext, overseeing efficient database management and essential metadata storage for uploaded files.
Database operations are expertly managed using SQL Server. Notably, the integration of Swagger bolsters user experience through thorough API documentation.

Included Packages:
- Microsoft.EntityFrameworkCore Tools
- Microsoft.EntityFrameworkCore.SqlServer
- Microsoft.EntityFrameworkCore.InMemory

Important Note:
Remember to specify the server name correctly in the "appsettings.json" file.
