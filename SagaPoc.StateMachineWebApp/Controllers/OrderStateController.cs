using MassTransit;
using Microsoft.AspNetCore.Mvc;
using SagaPoc.Messages;

namespace SagaPoc.StateMachineWebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderStateController : ControllerBase
    {
        private readonly IPublishEndpoint _submitOrderClient;
        private readonly ILogger<OrderStateController> _logger;

        public OrderStateController(IPublishEndpoint submitOrderClient, ILogger<OrderStateController> logger)
        {
            _submitOrderClient = submitOrderClient;
            _logger = logger;
        }

        [HttpGet]
        [Route("submit")]
        public async Task<IActionResult> Submit()
        {
            _logger.LogInformation("Submitting order state");

            await _submitOrderClient.Publish<SubmitOrder>(new 
            {
                CorrelationId = NewId.NextGuid(),
                OrderDate = DateTime.Now
            });

            return Ok();
        }

        [HttpGet]
        [Route("accept")]
        public async Task<IActionResult> Accept()
        {
            _logger.LogInformation("Accepting order state");

            await _submitOrderClient.Publish<OrderAccepted>(new
            {
                OrderDate = DateTime.Now
            });

            return Ok();
        }
    }
}
