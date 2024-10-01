using System.IO;
using System.Net;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace ST10267411_CLDV6212_Function
{
    public class SaveProductImage
    {
        private readonly ILogger _logger;
        private readonly BlobContainerClient _blobContainerClient;

        public SaveProductImage(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<SaveProductImage>();
            string connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
            _blobContainerClient = new BlobContainerClient(connectionString, "productimages");
            _blobContainerClient.CreateIfNotExists();
        }

        [Function("SaveProductImage")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            string blobName = req.Url.Query.Contains("blobName") ? req.Url.Query.Split("blobName=")[1] : "defaultBlobName";
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            var blobClient = _blobContainerClient.GetBlobClient(blobName);

            using (var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(requestBody)))
            {
                await blobClient.UploadAsync(stream, overwrite: true);
            }

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");
            await response.WriteStringAsync("Product image stored in blob.");

            return response;
        }
    }
}
