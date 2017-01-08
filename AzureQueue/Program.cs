// Course: https://www.udemy.com/70532-azure/learn/v4/overview
// Author course: Scott Duffy

using System;
using System.Configuration;
using System.Data;
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
            myQueue.CreateIfNotExists();

            // S09L51
            //InsertMessageQueue(myQueue);

            // S09L52
            // PeekMessageQueue(myQueue);
            // GetMessageQueue(myQueue);

            Console.WriteLine("\nFinished.");
            Console.ReadKey();
        }

        private static void GetMessageQueue(CloudQueue myQueue)
        {
            CloudQueueMessage newMessage = new CloudQueueMessage("" +
                DateTime.Now.ToString() + " Record added correctly.");
            myQueue.AddMessage(newMessage);
            Console.WriteLine("New message: " + newMessage.AsString);

            CloudQueueMessage oldmessage = myQueue.GetMessage();
            Console.WriteLine("PeekMessage: " + oldmessage.AsString);
        }

        private static void PeekMessageQueue(CloudQueue myQueue)
        {
            CloudQueueMessage newMessage = new CloudQueueMessage("" + 
                DateTime.Now.ToString() + " Record added correctly.");
            myQueue.AddMessage(newMessage);
            Console.WriteLine("New message: " + newMessage.AsString);

            CloudQueueMessage oldmessage = myQueue.PeekMessage();
            Console.WriteLine("PeekMessage: " + oldmessage.AsString);
        }

        private static void InsertMessageQueue(CloudQueue myQueue)
        {
            CloudQueueMessage newMessage = new CloudQueueMessage("" +
                DateTime.Now.ToString() + " Record added correctly.");
            myQueue.AddMessage(newMessage);
        }
    }
}
