# Azure Functions Project

This project contains Azure Functions for integrating with Azure Tables, Blob Storage, Queues, and Azure Files. The functions are implemented using the .NET isolated worker model.

## Project Structure

```
azure-functions-project/
│
│
├── .gitignore                 # Git ignore file
├── host.json                  # Azure Functions host configuration
├── local.settings.json        # Local settings for Azure Functions (local development)
├── Program.cs                 # Entry point for the Azure Functions project
├── azure-functions-project.csproj  # Project file
│
├── Functions/                 # Directory for Azure Functions
│   ├── SaveCustomer.cs
│   ├── SaveProductImage.cs
│   ├── SaveOrder.cs
│   └── SaveDocument.cs
│
├── Models/                    # Directory for data models
│   └── MyEntity.cs
│
├── Properties/                # Directory for project properties
│   └── launchSettings.json
│
└── README.md                  # Project documentation
```

## Functions

### SaveCustomer

Stores customer information in Azure Tables.

**Endpoint**: `/api/SaveCustomer`

**Request Type**: `POST`

**Request Body**:
```json
{
  "PartitionKey": "customer1",
  "RowKey": "001",
  "Data": "Customer data"
}
```

**Example `curl` Command**:
```sh
curl -X POST http://localhost:7071/api/SaveCustomer \
    -H "Content-Type: application/json" \
    -d '{
          "PartitionKey": "customer1",
          "RowKey": "001",
          "Data": "Customer data"
        }'
```

### SaveProductImage

Writes product images to Blob Storage.

**Endpoint**: `/api/SaveProductImage`

**Request Type**: `POST`

**Query Parameter**: `blobName` (optional)

**Request Body**: Plain text content of the image.

**Example `curl` Command**:
```sh
curl -X POST http://localhost:7071/api/SaveProductImage?blobName=testimage.jpg \
    -H "Content-Type: text/plain" \
    -d 'This is the content of the product image.'
```

### SaveOrder

Queues orders in Azure Queues.

**Endpoint**: `/api/SaveOrder`

**Request Type**: `POST`

**Request Body**:
```json
{
  "OrderId": "12345",
  "Product": "Product name",
  "Quantity": 1
}
```

**Example `curl` Command**:
```sh
curl -X POST http://localhost:7071/api/SaveOrder \
    -H "Content-Type: application/json" \
    -d '{
          "OrderId": "12345",
          "Product": "Product name",
          "Quantity": 1
        }'
```

### SaveDocument

Writes documents to Azure Files.

**Endpoint**: `/api/SaveDocument`

**Request Type**: `POST`

**Query Parameter**: `fileName` (optional)

**Request Body**: Plain text content of the document.

**Example `curl` Command**:
```sh
curl -X POST http://localhost:7071/api/SaveDocument?fileName=testdocument.txt \
    -H "Content-Type: text/plain" \
    -d 'This is the content of the document.'
```

## Setup

### Prerequisites

- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- [Azure Functions Core Tools](https://docs.microsoft.com/en-us/azure/azure-functions/functions-run-local)

### Configuration

1. **Clone the repository**:
   ```sh
   git clone <repository-url>
   cd azure-functions-project
   ```

2. **Configure local settings**:
   Update the `local.settings.json` file with your Azure Storage connection string.
   ```json
   {
     "IsEncrypted": false,
     "Values": {
       "AzureWebJobsStorage": "DefaultEndpointsProtocol=https;AccountName=your_account_name;AccountKey=your_account_key;EndpointSuffix=core.windows.net",
       "FUNCTIONS_WORKER_RUNTIME": "dotnet"
     }
   }
   ```

3. **Install dependencies**:
   ```sh
   dotnet restore
   ```

### Running the Functions Locally

1. **Start the Azure Functions runtime**:
   ```sh
   func start
   ```

2. **Test the functions** using the provided `curl` commands.

### Deployment

Deploy the functions to Azure using the Azure Functions extension in Visual Studio Code or Visual Studio.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.
