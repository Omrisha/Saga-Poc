using MassTransit;
using Microsoft.AspNetCore.Mvc;
using SagaPoc.Messages;

namespace SagaPoc.WebApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IRequestClient<OrderInoviced> _client;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(ILogger<OrdersController> logger, IRequestClient<OrderInoviced> client)
        {
            _logger = logger;
            _client = client;
        }

        [HttpGet()]
        public async Task<IActionResult> Submit()
        {
            _logger.LogInformation("Submit Order");
            
            var result = await _client.GetResponse<OrderPaymentResult>(new
            {
                Timestamp = DateTime.UtcNow,
                Amount = 500
            });

            return Ok(result);
        }
    }
}