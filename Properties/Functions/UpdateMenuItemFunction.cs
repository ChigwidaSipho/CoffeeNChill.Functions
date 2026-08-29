using System.Net;
using System.Text.Json;
using CoffeeNChill.Functions.Properties.DTOs;
using CoffeeNChill.Functions.Properties.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace CoffeeNChill.Functions.Properties.Functions
{
    public class UpdateMenuItemFunction
    {
        private readonly ITableStorageService _service;
        private readonly ILogger<UpdateMenuItemFunction> _logger;

        public UpdateMenuItemFunction(ITableStorageService service, ILogger<UpdateMenuItemFunction> logger)
        {
            _service = service;
            _logger = logger;
        }

        [Function("UpdateMenuItem")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "menu/{category}/{id}")] HttpRequestData req,
            string category, string id)
        {
            var body = await req.ReadAsStringAsync();
            var request = JsonSerializer.Deserialize<UpdateMenuItemRequest>(body ?? "",
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (request is null)
                return await BadRequest(req, "Invalid request body.");

            var validationError = Validate(request);
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

        private static string? Validate(UpdateMenuItemRequest request)
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