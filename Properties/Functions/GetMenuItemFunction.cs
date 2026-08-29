using System.Net;
using CoffeeNChill.Functions.Properties.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace CoffeeNChill.Functions.Properties.Functions
{
    public class GetMenuItemFunction
    {
        private readonly ITableStorageService _service;
        private readonly ILogger<GetMenuItemFunction> _logger;

        public GetMenuItemFunction(ITableStorageService service, ILogger<GetMenuItemFunction> logger)
        {
            _service = service;
            _logger = logger;
        } 

        [Function("GetMenuItem")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "menu/{category}/{id}")] HttpRequestData req,
            string category, string id)
        {
            var item = await _service.GetMenuItemByIdAsync(category, id);

            if (item is null)
            {
                var notFound = req.CreateResponse(HttpStatusCode.NotFound);
                await notFound.WriteStringAsync($"Menu item {category}/{id} not found.");
                return notFound;
            }

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(item);
            return response;
        }

        [Function("GetMenuItemsByCategory")]
        public async Task<HttpResponseData> RunByCategory(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "menu/category/{category}")] HttpRequestData req,
            string category)
        {
            var items = await _service.GetMenuItemsByCategoryAsync(category);
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(items);
            return response;
        }
    }
}