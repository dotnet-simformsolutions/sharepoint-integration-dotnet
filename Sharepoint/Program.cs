using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Sharepoint.Services;


var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

var sharePointSettings = configuration.GetSection("SharePointSettings");
string clientId = sharePointSettings["ClientId"];
string tenantId = sharePointSettings["TenantId"];
string clientSecret = sharePointSettings["ClientSecret"];
string sharePointUrl = sharePointSettings["SharePointUrl"];
string folderName = sharePointSettings["FolderName"];
string fileName = sharePointSettings["FileName"];


SharePointService sharePointService = new SharePointService(clientId, tenantId, clientSecret);
var accessToken = await sharePointService.GetAccessTokenAsync();
var data = await sharePointService.GetSharePointSiteDataAsync(sharePointUrl);
Console.WriteLine(JsonConvert.SerializeObject(data));
var data1 = await sharePointService.GetListItemsAsync(sharePointUrl, folderName);
await sharePointService.UploadFileAsync(sharePointUrl, folderName, fileName);
Console.Read();