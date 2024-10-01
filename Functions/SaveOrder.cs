using System.IO;
using System.Net;
using System.Threading.Tasks;
using Azure.Storage.Queues;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace ST10267411_CLDV6212_Function
{
    public class SaveOrder
    {
        private readonly ILogger _logger;
        private readonly QueueClient _queueClient;

        public SaveOrder(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<SaveOrder>();
            string connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
            _queueClient = new QueueClient(connectionString, "orders");
            _queueClient.CreateIfNotExists();
        }

        [Function("SaveOrder")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var message = await new StreamReader(req.Body).ReadToEndAsync();

            await _queueClient.SendMessageAsync(message);

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json; charset=utf-8");
            await response.WriteStringAsync("Order queued.");

            return response;
        }
    }
}
