using BigDataApı.Repositories.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace BigDataApı.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderAnalyticsController : ControllerBase
    {
        private readonly IOrderAnalyticsService _orderAnalyticsService;

        public OrderAnalyticsController(IOrderAnalyticsService orderAnalyticsService)
        {
            _orderAnalyticsService = orderAnalyticsService;
        }

        [HttpGet("CategoryOrderCountsByYear")]
        public async Task<IActionResult> GetCategoryOrderCountsByYear()
        {
            var result = await _orderAnalyticsService.GetCategoryOrderCountsByYear();
            return Ok(result);
        }
        [HttpGet("OrdersPerCity")]
        public async Task<IActionResult> GetOrdersPerCity()
        {
            var result = await _orderAnalyticsService.GetOrdersPerCity();
            return Ok(result);
        }
    }
}