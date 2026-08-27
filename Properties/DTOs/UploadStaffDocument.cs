using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeNChill.Functions.Properties.DTOs
{
    public class UploadStaffDocument
    {
        public string Name { get; set; } = string.Empty;

        //  Description of the menu item
        public string Description { get; set; } = string.Empty;

        //  Price of the menu item
        public double Price { get; set; }

        //  Availability status of the menu item
        public bool IsAvailable { get; set; } 

    }
}
 