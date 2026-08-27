using Azure;
using Azure.Data.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeNChill.Functions.Properties.Models
{
    public class MenuItem : ITableEntity
    {
        // Define properties for the menu item
        public string PartitionKey{ get; set; } = string.Empty;

        // RowKey is typically a unique identifier for the entity within a partition
        public string RowKey{ get; set; } = string.Empty;

        // Timestamp is automatically managed by Azure Table Storage, so you don't need to set it manually
        public string Name{ get; set; } = string.Empty;

        // Description of the menu item
        public string Description { get; set; } = string.Empty;

        // Price of the menu item
        public double Price { get; set; }

        //  Availability status of the menu item
        public bool IsAvailable { get; set; }

        // Timestamp is automatically managed by Azure Table Storage, so you don't need to set it manually
        public DateTimeOffset? Timestamp { get; set; }

        // ETag is used for optimistic concurrency in Azure Table Storage
        public ETag ETag { get; set; }

    }
}
