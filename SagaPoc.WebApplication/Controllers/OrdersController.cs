using MassTransit;
using Microsoft.AspNetCore.Mvc;
using SagaPoc.Messages;

namespace SagaPoc.WebApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IRequestClient<SubmitOrder> _client;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(ILogger<OrdersController> logger, IRequestClient<SubmitOrder> client)
        {
            _logger = logger;
            _client = client;
        }

        [HttpGet()]
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation("Accept Order");
            
            var response = await _client.GetResponse<OrderAccepted>(new
            {
                OrderDate = DateTime.UtcNow,
            });

            return Ok(new
            {
                response.Message.CorrelationId,
                response.Message.Timestamp
            });
        }
    }
}