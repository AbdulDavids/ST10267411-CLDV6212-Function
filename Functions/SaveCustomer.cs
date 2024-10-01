using System.IO;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Data.Tables;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using ST10267411_CLDV6212_Function.Models;

namespace ST10267411_CLDV6212_Function
{
    public class SaveCustomer
    {
        private readonly ILogger _logger;
        private readonly TableClient _tableClient;

        public SaveCustomer(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<SaveCustomer>();
            string connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
            _tableClient = new TableClient(connectionString, "customers");
            _tableClient.CreateIfNotExists();
        }

        [Function("SaveCustomer")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonSerializer.Deserialize<MyEntity>(requestBody);

            await _tableClient.AddEntityAsync(data);

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");
            response.WriteString("Customer information stored in table.");

            return response;
        }
    }
}