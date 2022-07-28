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
        public async Task<IActionResult> Submit()
        {
            await _submitOrderClient.Publish<SubmitOrder>(new 
            {
                OrderDate = DateTime.Now
            });

            return Ok();
        }
    }
}
