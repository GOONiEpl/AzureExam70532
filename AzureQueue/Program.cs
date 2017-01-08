using System;
using System.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Queue;


namespace AzureQueue
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

            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            CloudQueue myQueue = queueClient.GetQueueReference("thisisaqueue");


            Console.WriteLine("\nFinished.");
            Console.ReadKey();

        }
    }
}
