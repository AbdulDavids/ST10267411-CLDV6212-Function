using System.IO;
using System.Net;
using System.Threading.Tasks;
using Azure.Storage.Files.Shares;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace ST10267411_CLDV6212_Function
{
    public class SaveDocument
    {
        private readonly ILogger _logger;
        private readonly ShareClient _shareClient;

        public SaveDocument(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<SaveDocument>();
            string connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
            _shareClient = new ShareClient(connectionString, "documents");
            _shareClient.CreateIfNotExists();
        }

        [Function("SaveDocument")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            string fileName = req.Url.Query.Contains("fileName") ? req.Url.Query.Split("fileName=")[1] : "defaultFileName";
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            var directoryClient = _shareClient.GetRootDirectoryClient();
            var fileClient = directoryClient.GetFileClient(fileName);

            using (var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(requestBody)))
            {
                await fileClient.CreateAsync(stream.Length);
                await fileClient.UploadAsync(stream);
            }

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");
            await response.WriteStringAsync("Document stored in file share.");

            return response;
        }
    }
}
