using System;
using System.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

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
            UploadBlobSubdirectory(container);

            Console.WriteLine("\nFinished.");
            Console.ReadKey();
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
