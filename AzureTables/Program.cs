using System;
using System.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;


namespace AzureTables
{
    class Program
    {
        static void Main(string[] args)
        {
            string storageConnection =
                    System.Environment.GetEnvironmentVariable("AzureStorageConnectionString", EnvironmentVariableTarget.User);
                    // or use App.config 
                    //ConfigurationManager.AppSettings["StorageConnectionString"];

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(storageConnection);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("FirstTestTable");
            table.CreateIfNotExists();

            // S09L46
            // CreateRecord(table);

            // S09L47
            // RetrieveRecord(table);

            // S09L48
            // CreateRecords(table);
            RetrieveRecords(table);

            Console.WriteLine("\nFinished.");
            Console.ReadKey();
        }

        private static void RetrieveRecords(CloudTable table)
        {
            TableQuery<CarEntity> carquery = new TableQuery<CarEntity>();
            foreach (CarEntity _car in table.ExecuteQuery(carquery))
            {
                Console.WriteLine(_car.UniqueID + ": " +
                    _car.Make + " " +
                    _car.Model + " - " +
                    _car.Year + "; Color: " +
                    _car.Color);
            }
        }

        private static void CreateRecords(CloudTable table)
        {
            CarEntity newCar = new CarEntity(124, 2015, "Porsche", "Panamera GTS", "Carmine Red");
            TableOperation insert = TableOperation.Insert(newCar);
            table.Execute(insert);

            newCar = new CarEntity(125, 2014, "Porsche", "911 Turbo", "Turkish Red");
            insert = TableOperation.Insert(newCar);
            table.Execute(insert);

            newCar = new CarEntity(126, 2016, "Porsche", "911 GTR RS", "Turkish Red");
            insert = TableOperation.Insert(newCar);
            table.Execute(insert);

            newCar = new CarEntity(127, 1980, "Fiat", "126p", "Red");
            insert = TableOperation.Insert(newCar);
            table.Execute(insert);

            newCar = new CarEntity(128, 1979, "Polonez", "Caro", "Gold");
            insert = TableOperation.Insert(newCar);
            table.Execute(insert);
        }

        private static void RetrieveRecord(CloudTable table)
        {
            TableOperation retrieve = TableOperation.Retrieve<CarEntity>("car", "123");
            TableResult result = table.Execute(retrieve);

            if (result.Result == null)
            {
                Console.WriteLine("not found");
            }
            else
            {
                CarEntity _car = new CarEntity();
                _car = ((CarEntity) result.Result);
                Console.WriteLine("Find the car: " + _car.Make + " " + _car.Model);
            }
        }

        private static void CreateRecord(CloudTable table)
        {
            CarEntity newCar = new CarEntity(123, 2017, "Porsche", "Panamera GTS", "Carmine Red");
            TableOperation insert = TableOperation.Insert(newCar);
            table.Execute(insert);
        }
    }
}
