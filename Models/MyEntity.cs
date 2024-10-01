using Azure;
using Azure.Data.Tables;

namespace ST10267411_CLDV6212_Function.Models
{
    public class MyEntity : ITableEntity
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public string Data { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

        public MyEntity() { }

        public MyEntity(string partitionKey, string rowKey, string data)
        {
            PartitionKey = partitionKey;
            RowKey = rowKey;
            Data = data;
        }
    }
}