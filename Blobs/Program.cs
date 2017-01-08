// Course: https://www.udemy.com/70532-azure/learn/v4/overview
// Author course: Scott Duffy

using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;

namespace AzureBlobs
{
    class Program
    {
        static void Main(string[] args)
        {
            string storageConnection =
                    System.Environment.GetEnvironmentVariable("AzureStorageConnectionString", EnvironmentVariableTarget.User);
            // or use App.config 
            //ConfigurationManager.AppSettings["StorageConnectionString"];

            if (storageConnection == null)
            {
                Console.WriteLine("No define Environment Variable in system");
                return;
            }

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(storageConnection);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("objective2");
            container.CreateIfNotExists();

            // S08L37
            // UploadBlob(container);

            // S08L38
            // ListAttributes(container);
            // SetMetadata(container);
            // ListMetadata(container);

            // S08L40
            // CopyBlob(container);

            // S08L42
            // UploadBlobSubdirectory(container);

            // S10L57
            // CreateSharedAccessPolicy(container);

            // S10L59
            CreateCrosPolicy(blobClient);
            
            Console.WriteLine("\nFinished.");
            Console.ReadKey();
        }

        private static void CreateCrosPolicy(CloudBlobClient blobClient)
        {
            ServiceProperties sp = new ServiceProperties();
            sp.Cors.CorsRules.Add(new CorsRule()
            {
                AllowedMethods = CorsHttpMethods.Get,
                AllowedOrigins = new List<string>() { "http://localhost:8080/"},
                MaxAgeInSeconds = 3600,
            });
            blobClient.SetServiceProperties(sp);
        }

        private static void CreateSharedAccessPolicy(CloudBlobContainer container)
        {
            // Create a new stored access policy and sefine its containts
            SharedAccessBlobPolicy sharedPolicy = new SharedAccessBlobPolicy()
            {
                SharedAccessExpiryTime = DateTime.UtcNow.AddHours(24),
                Permissions = SharedAccessBlobPermissions.Read |
                              SharedAccessBlobPermissions.Write |
                              SharedAccessBlobPermissions.List
            };

            // Get the container's existing permissions
            BlobContainerPermissions permissions = new BlobContainerPermissions();

            // Add the new policy to the container's permissions
            permissions.SharedAccessPolicies.Clear();
            permissions.SharedAccessPolicies.Add("PolicyName", sharedPolicy);
            container.SetPermissions(permissions);


        }

        private static void UploadBlobSubdirectory(CloudBlobContainer container)
        {
            CloudBlobDirectory directory = container.GetDirectoryReference("parent-directory");
            CloudBlobDirectory childDirectory = directory.GetDirectoryReference("child-directory");
            CloudBlockBlob blockBlob = childDirectory.GetBlockBlobReference("newexamobjectives");

            using (var fileStream = System.IO.File.OpenRead(@"m:\AzureCloudExam\Blobs\files\gcg.pdf"))
            {
                blockBlob.UploadFromStream(fileStream);
            }
        }

        private static void CopyBlob(CloudBlobContainer container)
        {
            CloudBlob blockBlob = container.GetBlobReference("examobjectives");
            CloudBlob copyToBlob = container.GetBlobReference("examobjectives-copy");
            copyToBlob.StartCopyAsync(new Uri(blockBlob.Uri.AbsoluteUri));
        }

        private static void ListMetadata(CloudBlobContainer container)
        {
            container.FetchAttributes();
            Console.WriteLine("\nMetadata:");
            foreach (var item in container.Metadata)
            {
                Console.WriteLine(@"Key: {0}{2}{2}Value: {1}", item.Key, item.Value, '\t');
            }
        }

        private static void SetMetadata(CloudBlobContainer container)
        {
            container.Metadata.Clear();
            container.Metadata.Add("author", "Marek Rzepka");
            container.Metadata["authoredOn"] = "Jan 8, 2017";
            container.SetMetadata();
        }

        private static void UploadBlob(CloudBlobContainer container)
        {
            CloudBlockBlob blockBlob = container.GetBlockBlobReference("examobjectives");

            using (var fileStream = System.IO.File.OpenRead(@"m:\AzureCloudExam\Blobs\files\gcg.pdf"))
            {
                blockBlob.UploadFromStream(fileStream);
            }
        }

        private static void ListAttributes(CloudBlobContainer container)
        {
            container.FetchAttributes();
            Console.WriteLine("Container name: "+ container.StorageUri.PrimaryUri.ToString());
            Console.WriteLine("Last modyfied: " + container.Properties.LastModified.ToString());
        }
    }
}
