using Azure;
using Azure.Data.Tables;
using CoffeeNChill.Functions.Properties.DTOs;
using CoffeeNChill.Functions.Properties.Interfaces;
using CoffeeNChill.Functions.Properties.Models;

namespace CoffeeNChill.Functions.Properties.Services
{
    public class TableStorageService : ITableStorageService
    {
        private readonly TableClient _tableClient;

        public TableStorageService(string connectionString, string tableName = "MenuItems")
        {
            _tableClient = new TableClient(connectionString, tableName);
            _tableClient.CreateIfNotExists();
        }

        public async Task<MenuItem> CreateMenuItemAsync(CreateMenuItemRequest request)
        {
            var entity = new MenuItem
            {
                PartitionKey = request.Category,
                RowKey = request.SKU,
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                IsAvailable = request.IsAvailable
            };

            await _tableClient.AddEntityAsync(entity);
            return entity;
        }

        public async Task<List<MenuItem>> GetAllMenuItemsAsync()
        {
            var results = new List<MenuItem>();
            await foreach (var item in _tableClient.QueryAsync<MenuItem>())
            {
                results.Add(item);
            }
            return results;
        }

        public async Task<List<MenuItem>> GetMenuItemsByCategoryAsync(string category)
        {
            var results = new List<MenuItem>();
            await foreach (var item in _tableClient.QueryAsync<MenuItem>(x => x.PartitionKey == category))
            {
                results.Add(item);
            }
            return results;
        }

        public async Task<MenuItem?> GetMenuItemByIdAsync(string category, string sku)
        {
            try
            {
                var response = await _tableClient.GetEntityAsync<MenuItem>(category, sku);
                return response.Value;
            }
            catch (RequestFailedException ex) when (ex.Status == 404)
            {
                return null;
            }
        }

        public async Task<MenuItem> UpdateMenuItemAsync(string category, string sku, UpdateMenuItemRequest updatedMenuItem)
        {
            var entity = new MenuItem
            {
                PartitionKey = category,
                RowKey = sku,
                Name = updatedMenuItem.Name,
                Description = updatedMenuItem.Description,
                Price = updatedMenuItem.Price,
                IsAvailable = updatedMenuItem.IsAvailable
            };

            await _tableClient.UpsertEntityAsync(entity, TableUpdateMode.Replace);
            return entity;
        }

        public async Task<bool> DeleteMenuItemAsync(string category, string sku)
        {
            try
            {
                await _tableClient.DeleteEntityAsync(category, sku);
                return true;
            }
            catch (RequestFailedException ex) when (ex.Status == 404)
            {
                return false;
            }
        }
    } 
}