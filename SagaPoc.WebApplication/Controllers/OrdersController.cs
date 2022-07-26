using MassTransit;
using Microsoft.AspNetCore.Mvc;
using SagaPoc.Messages;

namespace SagaPoc.WebApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IPublishEndpoint _client;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(ILogger<OrdersController> logger, IPublishEndpoint client)
        {
            _logger = logger;
            _client = client;
        }

        [HttpGet()]
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation("Accept Order");
            
            await _client.Publish<SubmitOrder>(new
            {
                OrderDate = DateTime.UtcNow,
            });

            return Ok();
        }
    }
}