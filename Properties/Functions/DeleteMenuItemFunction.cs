using System.Net;
using CoffeeNChill.Functions.Properties.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace CoffeeNChill.Functions.Properties.Functions
{
    public class DeleteMenuItemFunction
    {
        private readonly ITableStorageService _service;
        private readonly ILogger<DeleteMenuItemFunction> _logger;
         
        public DeleteMenuItemFunction(ITableStorageService service, ILogger<DeleteMenuItemFunction> logger)
        {
            _service = service;
            _logger = logger;
        }

        [Function("DeleteMenuItem")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "menu/{category}/{id}")] HttpRequestData req,
            string category, string id)
        {
            var deleted = await _service.DeleteMenuItemAsync(category, id);
            var response = req.CreateResponse(deleted ? HttpStatusCode.NoContent : HttpStatusCode.NotFound);
            return response;
        }
    }
}