using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeNChill.Functions.Properties.DTOs
{
    public class UpdateMenuItemRequest
    {
        // Define properties for the menu item update request
        public string Category { get; set; } = string.Empty;

        //  RowKey is typically a unique identifier for the entity within a partition
        public string SKU { get; set; } = string.Empty;

        //  Name of the menu item
        public string Name { get; set; } = string.Empty;

        //  Description of the menu item
        public string Description { get; set; } = string.Empty;

        //  Price of the menu item
        public double Price { get; set; }

        //  Availability status of the menu item
        public bool IsAvailable { get; set; }
    }
}
