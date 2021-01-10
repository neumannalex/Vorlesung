using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Azure.Documents.Client;
using Vorlesung.Shared.Downloads;
using System.Linq;
using Microsoft.Azure.Documents.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Web;
using System.Diagnostics;

namespace Vorlesung.Api.DownloadFunctions
{
    public static class DownloadFunctions
    {
        private const string downloadDbId = "downloads";
        private const string downloadDbContainerId = "container2";
        private const string downloadBlobStorageContainerId = "downloads";

        [FunctionName("DownloadFile")]
        public static async Task<IActionResult> DownloadFile(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "files/{id}")] HttpRequest req,
            [Blob(downloadBlobStorageContainerId, Connection = "StorageConnection")] CloudBlobContainer blobContainer,
            [CosmosDB(
                databaseName: downloadDbId,
                collectionName: downloadDbContainerId,
                ConnectionStringSetting = "CosmosDBConnection")] DocumentClient documentClient,
            string id,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            if (!await blobContainer.ExistsAsync())
                return new NotFoundObjectResult("Container does not exist.");

            // Get info from db
            var documentUri = UriFactory.CreateDocumentUri(downloadDbId, downloadDbContainerId, id);
            try
            {
                var response = await documentClient.ReadDocumentAsync<Download>(documentUri,
                    new RequestOptions
                    {
                        PartitionKey = new Microsoft.Azure.Documents.PartitionKey(Download.GetHash(id))
                    });
                var document = response.Document;

                if (document == null)
                    return new BadRequestObjectResult("Download not found");

                // name of the physical file behind the blob
                var filename = document.Filename;
                var contentType = GetContentTypeFromFilename(filename);

                // get blob from storage            
                var blob = blobContainer.GetBlobReference(document.BlobName);

                if (await blob.ExistsAsync())
                {
                    // update Downloadcounter
                    document.DownloadCount++;
                    await documentClient.UpsertDocumentAsync(UriFactory.CreateDocumentCollectionUri(downloadDbId, downloadDbContainerId), document);

                    // return blob
                    var stream = await blob.OpenReadAsync();
                    return new FileStreamResult(stream, contentType) { FileDownloadName = filename };
                }
                else
                {
                    return new NotFoundObjectResult("File not found.");
                }
            }
            catch(Exception ex)
            {
                return new BadRequestObjectResult("Download not found");
            }
        }

        [FunctionName("GetDownloads")]
        public static async Task<IActionResult> GetDownloads(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "downloads")] HttpRequest req,
            [CosmosDB(
                databaseName: downloadDbId,
                collectionName: downloadDbContainerId,
                ConnectionStringSetting = "CosmosDBConnection")] DocumentClient documentClient,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function GetDownloads processed a request.");

            // Get info from db
            var feedOptions = new FeedOptions
            {
                EnableCrossPartitionQuery = true,
                MaxDegreeOfParallelism = 256
            };

            var collectionUri = UriFactory.CreateDocumentCollectionUri(downloadDbId, downloadDbContainerId);

            var query = documentClient.CreateDocumentQuery<Download>(collectionUri, feedOptions)
                                .OrderByDescending(x => x.CreatedOn)
                                .AsDocumentQuery();

            try
            {
                var timer = Stopwatch.StartNew();
                var data = new List<Download>();

                while (query.HasMoreResults)
                {
                    foreach (Download item in await query.ExecuteNextAsync())
                    {
                        data.Add(item);
                    }
                }

                timer.Stop();

                //log.LogInformation($"Query duration: {timer.Elapsed}, RU: {query.} ");

                return new OkObjectResult(data);
            }
            catch(Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        [FunctionName("GetDownload")]
        public static async Task<IActionResult> GetDownload(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "downloads/{id}")] HttpRequest req,
            [CosmosDB(
                databaseName: downloadDbId,
                collectionName: downloadDbContainerId,
                ConnectionStringSetting = "CosmosDBConnection")] DocumentClient documentClient,
            string id,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function GetDownloads processed a request.");

            // Get info from db
            var documentUri = UriFactory.CreateDocumentUri(downloadDbId, downloadDbContainerId, id);
            try
            {
                var response = await documentClient.ReadDocumentAsync<Download>(documentUri,
                new RequestOptions
                {
                    PartitionKey = new Microsoft.Azure.Documents.PartitionKey(Download.GetHash(id))
                });
                var document = response.Document;

                if (document == null)
                    return new BadRequestObjectResult("Download not found");
                else
                    return new OkObjectResult(document);
            }
            catch(Exception ex)
            {
                return new BadRequestObjectResult("Download not found");
            }
        }

        [FunctionName("CreateDownload")]
        public static async Task<IActionResult> CreateDownload(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "downloads")] HttpRequestMessage req,
            [Blob(downloadBlobStorageContainerId, Connection = "StorageConnection")] CloudBlobContainer blobContainer,
            [CosmosDB(
                databaseName: downloadDbId,
                collectionName: downloadDbContainerId,
                ConnectionStringSetting = "CosmosDBConnection")] DocumentClient documentClient,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function GetDownloads processed a request.");

            // get data from http trigger
            var multipartMemoryStreamProvider = new MultipartMemoryStreamProvider();
            await req.Content.ReadAsMultipartAsync(multipartMemoryStreamProvider);

            var file = GetHttpContentFromFromMultipartStreamProviderByKey(multipartMemoryStreamProvider, "file");
            if (file == null)
                return new BadRequestObjectResult("No file found. Please post data as form-data and set the parametername for the file to be uploaded to 'file'.");
            var fileInfo = file.Headers.ContentDisposition;
            log.LogInformation(JsonConvert.SerializeObject(fileInfo, Formatting.Indented));

            // upload blob
            var blobname = Guid.NewGuid().ToString("n");
            var cloudBlockBlob = blobContainer.GetBlockBlobReference(blobname);

            cloudBlockBlob.Properties.ContentType = file.Headers.ContentType.MediaType;

            using (var uploadFileStream = await file.ReadAsStreamAsync())
            {
                await cloudBlockBlob.UploadFromStreamAsync(uploadFileStream);
            }

            var originalFilename = fileInfo.FileName.Replace("\"", "");

            // save info to db
            var download = new Download
            {
                DisplayName = string.Empty,
                Filename = originalFilename,
                BlobName = blobname,
                Url = HttpUtility.UrlDecode(cloudBlockBlob.Uri.ToString()),
                DownloadCount = 0
            };

            var collectionUri = UriFactory.CreateDocumentCollectionUri(downloadDbId, downloadDbContainerId);
            var response = await documentClient.CreateDocumentAsync(collectionUri, download);

            if (response.Resource == null)
                return new BadRequestResult();
            else
                return new OkObjectResult(download);
        }

        [FunctionName("DeleteDownload")]
        public static async Task<IActionResult> DeleteDownload(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "downloads/{id}")] HttpRequest req,
            [Blob(downloadBlobStorageContainerId, Connection = "StorageConnection")] CloudBlobContainer blobContainer,
            [CosmosDB(
                databaseName: downloadDbId,
                collectionName: downloadDbContainerId,
                ConnectionStringSetting = "CosmosDBConnection")] DocumentClient documentClient,
            string id,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function DeleteDownload processed a request.");

            // Get info from db
            Download document = null;
            var documentUri = UriFactory.CreateDocumentUri(downloadDbId, downloadDbContainerId, id);
            try
            {
                var readResponse = await documentClient.ReadDocumentAsync<Download>(documentUri,
                    new RequestOptions
                    {
                        PartitionKey = new Microsoft.Azure.Documents.PartitionKey(Download.GetHash(id))
                    });

                document = readResponse.Document;
            }
            catch
            {

            }

            if (document == null)
                return new BadRequestObjectResult("Download not found");

            // get blob from storage            
            var blob = blobContainer.GetBlobReference(document.BlobName);

            // delete blob            
            await blob.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots, null, null, null);
            if (await blob.ExistsAsync())
                return new BadRequestObjectResult($"Could not delete blob named '{document.BlobName}'.");

            // delete db entry
            try
            {
                await documentClient.DeleteDocumentAsync(documentUri,
                new RequestOptions
                {
                    PartitionKey = new Microsoft.Azure.Documents.PartitionKey(Download.GetHash(id))
                });

                return new OkResult();
            }
            catch(Exception ex)
            {
                return new BadRequestObjectResult($"Could not delete document '{id}' from db: " + ex.Message);
            }
        }


        public static HttpContent GetHttpContentFromFromMultipartStreamProviderByKey(MultipartMemoryStreamProvider provider, string key)
        {
            try
            {
                return provider.Contents.Where(x => x.Headers.ContentDisposition.Name.ToLower() == $"\"{key}\"").FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }

        public static string GetContentTypeFromFilename(string filename)
        {
            var provider = new FileExtensionContentTypeProvider();

            string contentType;
            if(!provider.TryGetContentType(filename, out contentType))
            {
                contentType = "application/octet-stream";
            }

            return contentType;
        }
    }
}
