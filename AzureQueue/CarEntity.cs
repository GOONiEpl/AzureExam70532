using Microsoft.WindowsAzure.Storage.Table;

namespace AzureTables
{
    public class CarEntity : TableEntity
    {
        public CarEntity(int ID, int year, string make, string model, string color)
        {
            this.UniqueID = ID;
            this.Year = year;
            this.Make = make;
            this.Model = model;
            this.Color = color;
            this.PartitionKey = "car";
            this.RowKey = ID.ToString();
        }
        public CarEntity() { }
        public int UniqueID { get; set; }
        public int Year { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
    }
}