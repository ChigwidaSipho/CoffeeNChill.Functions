using System.Net;
using System.Text.Json;
using CoffeeNChill.Functions.Properties.DTOs;
using CoffeeNChill.Functions.Properties.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace CoffeeNChill.Functions.Properties.Functions
{
    public class CreateMenuItemFunction
    {
        private readonly ITableStorageService _service;
        private readonly ILogger<CreateMenuItemFunction> _logger;

        public CreateMenuItemFunction(ITableStorageService service, ILogger<CreateMenuItemFunction> logger)
        {
            _service = service;
            _logger = logger;
        }

        [Function("CreateMenuItem")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "menu")] HttpRequestData req)
        {
            var body = await req.ReadAsStringAsync();
            var request = JsonSerializer.Deserialize<CreateMenuItemRequest>(body ?? "",
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (request is null)
                return await BadRequest(req, "Invalid request body.");

            var validationError = Validate(request);
            if (validationError is not null)
                return await BadRequest(req, validationError);

            var created = await _service.CreateMenuItemAsync(request);

            var response = req.CreateResponse(HttpStatusCode.Created);
            await response.WriteAsJsonAsync(created);
            return response;
        }

        private static string? Validate(CreateMenuItemRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name)) return "Name cannot be empty.";
            if (string.IsNullOrWhiteSpace(request.Category)) return "Category cannot be empty.";
            if (string.IsNullOrWhiteSpace(request.SKU)) return "SKU cannot be empty.";
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