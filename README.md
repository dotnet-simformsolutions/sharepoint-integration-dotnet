# SharePoint Integration Console App

This is a .NET Core console application that integrates with SharePoint, allowing you to perform operations such as fetching site data, retrieving list items, and uploading files. The application uses dependency injection for services and reads configuration values from an `appsettings.json` file.

## Features

- **OAuth Authentication**: Securely authenticate with SharePoint using OAuth.
- **Fetch Site Data**: Retrieve metadata and other information from a SharePoint site.
- **List Items Retrieval**: Fetch items from a specific folder within a SharePoint list.
- **File Upload**: Upload files to a specific folder in a SharePoint document library.

## Prerequisites

- [.NET Core SDK](https://dotnet.microsoft.com/download) (version 8.0)
- A valid SharePoint Online tenant with the necessary API permissions.
- The following NuGet packages installed:
  - `Microsoft.Extensions.Configuration`
  - `Microsoft.Extensions.Configuration.Json`
  - `Newtonsoft.Json`
  - `Microsoft.Identity.Client`
  - `System.Net.Http`

## Getting Started

### 1. Set Up Configuration

Create an appsettings.json file in the root directory of the project. This file will store your SharePoint credentials and other configuration settings.

Example `appsettings.json`
````
{
  "SharePointSettings": {
    "ClientId": "<clientID>",
    "TenantId": "<tenantid>",
    "ClientSecret": "<clientsecret>",
    "SharePointUrl": "<MySharepointURL>",
    "FolderName": "My Folder",
    "FileName": "myfile.png"
  }
}
````
Replace `<clientID>`, `<tenantid>`, `<clientsecret>`, `<MySharepointURL>`, and other placeholders with your actual SharePoint details.

### 2. Build and Run the Application

Restore the NuGet packages and run the application:

```
dotnet restore
dotnet run
```

### 3. Application Workflow

- The application first authenticates with SharePoint using the provided client credentials.
- It retrieves data from the specified SharePoint site and prints it to the console.
- It fetches items from the specified folder in the SharePoint list.
- Finally, it uploads a specified file to the folder in the SharePoint document library.