using CoffeeNChill.Functions.Properties.Models;
using CoffeeNChill.Functions.Properties.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoffeeNChill.Functions.Properties.Interfaces
{
    public interface ITableStorageService
    {
        Task<MenuItem> CreateMenuItemAsync(CreateMenuItemRequest menuItem);

        Task<List<MenuItem>> GetAllMenuItemsAsync();

        Task<List<MenuItem>> GetMenuItemsByCategoryAsync(string category);
        Task<MenuItem?> GetMenuItemByIdAsync(string category, string sku);

        Task<MenuItem> UpdateMenuItemAsync(string category, string sku, UpdateMenuItemRequest updatedMenuItem);

        Task<bool> DeleteMenuItemAsync(string category, string sku);
    }
}