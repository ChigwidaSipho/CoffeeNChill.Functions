using System.Net;
using CoffeeNChill.Functions.Properties.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace CoffeeNChill.Functions.Properties.Functions
{
    public class GetAllMenuItemsFunction
    {
        private readonly ITableStorageService _service;
        private readonly ILogger<GetAllMenuItemsFunction> _logger;

        public GetAllMenuItemsFunction(ITableStorageService service, ILogger<GetAllMenuItemsFunction> logger)
        {
            _service = service;
            _logger = logger;
        }

        [Function("GetAllMenuItems")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "menu")] HttpRequestData req)
        {
            var items = await _service.GetAllMenuItemsAsync();
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(items);
            return response;
        }
    }
}