using BigDataApı.ML.NET.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace BigDataApı.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerAnalyticsController : ControllerBase
    {
        private readonly ICustomerAnalyticsService _customerAnalyticsService;

        public CustomerAnalyticsController(ICustomerAnalyticsService customerAnalyticsService)
        {
            _customerAnalyticsService = customerAnalyticsService;
        }

        [HttpGet("AverageOrdersCountPerCustomer")]
        public async Task<IActionResult> GetAverageOrdersCountPerCustomer()
        {
            var result = await _customerAnalyticsService.GetAveragelOrdersCountPerCustomer();
            return Ok(result);
        }

        [HttpGet("TotalActiveCustomerIn3Month")]
        public async Task<IActionResult> GetTotalActiveCustomerIn3Month()
        {
            var result = await _customerAnalyticsService.GetTotalActiveCustomerIn3Month();
            return Ok(result);
        }

        [HttpGet("TotalCustomerCount")]
        public async Task<IActionResult> GetTotalCustomerCount()
        {
            var result = await _customerAnalyticsService.GetTotalCustomerCount();
            return Ok(result);
        }

        [HttpGet("TotalDeactiveCustomerIn6Month")]
        public async Task<IActionResult> GetTotalDeactiveCustomerIn6Month()
        {
            var result = await _customerAnalyticsService.GetTotalDeactiveCustomerIn6Month();
            return Ok(result);
        }
    }
}