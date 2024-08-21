using Microsoft.Identity.Client;
using System.Net.Http.Headers;

namespace Sharepoint.Services
{
    public class SharePointService
    {
        private readonly string clientId;
        private readonly string tenantId;
        private readonly string clientSecret;
        private readonly string authority;
        private readonly string scope;

        public SharePointService(string clientId, string tenantId, string clientSecret)
        {
            this.clientId = clientId;
            this.tenantId = tenantId;
            this.clientSecret = clientSecret;
            this.authority = $"https://login.microsoftonline.com/{tenantId}";
            this.scope = "https://graph.microsoft.com/.default"; // or SharePoint scope
        }

        public async Task<string> GetAccessTokenAsync()
        {
            var app = ConfidentialClientApplicationBuilder.Create(clientId)
                .WithClientSecret(clientSecret)
                .WithAuthority(new Uri(authority))
                .Build();

            var result = await app.AcquireTokenForClient(new[] { scope }).ExecuteAsync();
            return result.AccessToken;
        }
        public async Task<string> GetSharePointSiteDataAsync(string siteUrl)
        {
            var accessToken = await GetAccessTokenAsync();

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await client.GetAsync($"{siteUrl}/_api/web");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    throw new HttpRequestException("Failed to fetch data from SharePoint");
                }
            }
        }
        public async Task<string> GetListItemsAsync(string siteUrl, string listName)
        {
            var accessToken = await GetAccessTokenAsync();

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var response = await client.GetAsync($"{siteUrl}/_api/web/lists/getbytitle('{listName}')/items");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    throw new HttpRequestException("Failed to fetch list items from SharePoint");
                }
            }
        }
        public async Task UploadFileAsync(string siteUrl, string libraryName, string filePath)
        {
            var accessToken = await GetAccessTokenAsync();
            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            var fileName = System.IO.Path.GetFileName(filePath);
            var uploadUrl = $"{siteUrl}/_api/web/GetFolderByServerRelativeUrl('{libraryName}')/Files/add(url='{fileName}',overwrite=true)";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var content = new ByteArrayContent(fileBytes);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                var response = await client.PostAsync(uploadUrl, content);

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException("Failed to upload file to SharePoint");
                }
            }
        }
    }
}
