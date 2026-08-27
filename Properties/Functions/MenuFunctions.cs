using System.Net;
using System.Text.Json;
using CoffeeNChill.Functions.Properties.DTOs;
using CoffeeNChill.Functions.Properties.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace CoffeeNChill.Functions.Properties.Functions
{
    public class MenuFunctions
    {
        private readonly ITableStorageService _service;
        private readonly ILogger<MenuFunctions> _logger;

        public MenuFunctions(ITableStorageService service, ILogger<MenuFunctions> logger)
        {
            _service = service;
            _logger = logger;
        }

        [Function("CreateMenuItem")]
        public async Task<HttpResponseData> CreateMenuItem(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "menu")] HttpRequestData req)
        {
            var body = await req.ReadAsStringAsync();
            var request = JsonSerializer.Deserialize<CreateMenuItemRequest>(body ?? "",
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (request is null)
                return await BadRequest(req, "Invalid request body.");

            var validationError = ValidateCreate(request);
            if (validationError is not null)
                return await BadRequest(req, validationError);

            var created = await _service.CreateMenuItemAsync(request);

            var response = req.CreateResponse(HttpStatusCode.Created);
            await response.WriteAsJsonAsync(created);
            return response;
        }

        [Function("GetAllMenuItems")]
        public async Task<HttpResponseData> GetAllMenuItems(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "menu")] HttpRequestData req)
        {
            var items = await _service.GetAllMenuItemsAsync();
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(items);
            return response;
        }

        [Function("GetMenuItemsByCategory")]
        public async Task<HttpResponseData> GetMenuItemsByCategory(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "menu/category/{category}")] HttpRequestData req,
            string category)
        {
            var items = await _service.GetMenuItemsByCategoryAsync(category);
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(items);
            return response;
        }

        [Function("UpdateMenuItem")]
        public async Task<HttpResponseData> UpdateMenuItem(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "menu/{category}/{id}")] HttpRequestData req,
            string category, string id)
        {
            var body = await req.ReadAsStringAsync();
            var request = JsonSerializer.Deserialize<UpdateMenuItemRequest>(body ?? "",
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (request is null)
                return await BadRequest(req, "Invalid request body.");

            var validationError = ValidateUpdate(request);
            if (validationError is not null)
                return await BadRequest(req, validationError);

            var existing = await _service.GetMenuItemByIdAsync(category, id);
            if (existing is null)
            {
                var notFound = req.CreateResponse(HttpStatusCode.NotFound);
                await notFound.WriteStringAsync($"Menu item {category}/{id} not found.");
                return notFound;
            }

            var updated = await _service.UpdateMenuItemAsync(category, id, request);
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(updated);
            return response;
        }

        [Function("DeleteMenuItem")]
        public async Task<HttpResponseData> DeleteMenuItem(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "menu/{category}/{id}")] HttpRequestData req,
            string category, string id)
        {
            var deleted = await _service.DeleteMenuItemAsync(category, id);

            var response = req.CreateResponse(deleted ? HttpStatusCode.NoContent : HttpStatusCode.NotFound);
            return response;
        }

        private static string? ValidateCreate(CreateMenuItemRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name)) return "Name cannot be empty.";
            if (string.IsNullOrWhiteSpace(request.Category)) return "Category cannot be empty.";
            if (string.IsNullOrWhiteSpace(request.SKU)) return "SKU cannot be empty.";
            if (request.Price < 0) return "Price cannot be negative.";
            return null;
        }

        private static string? ValidateUpdate(UpdateMenuItemRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name)) return "Name cannot be empty.";
            if (request.Price < 0) return "Price cannot be negative.";
            return null;
        }

        private static async Task<HttpResponseData> BadRequest(HttpRequestData req, string message)
        {
            var response = req.CreateResponse(HttpStatusCode.BadRequest);
            await response.WriteStringAsync(message);
            return response;
        }
    }
}


