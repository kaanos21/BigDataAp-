using BigDataApı.Repositories.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace BigDataApı.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticsRepository _statisticsRepository;

        public StatisticsController(IStatisticsRepository statisticsRepository)
        {
            _statisticsRepository = statisticsRepository;
        }

        [HttpGet("TotalCategoryCount")]
        public async Task<IActionResult> TotalCategoryCount()
        {
            var value = await _statisticsRepository.GetTotalCategoryCountAsync();
            return Ok(value);
        }

        [HttpGet("TotalProductCount")]
        public async Task<IActionResult> TotalProductCount()
        {
            var value = await _statisticsRepository.GetTotalProductCountAsync();
            return Ok(value);
        }

        [HttpGet("TotalCustomerCount")]
        public async Task<IActionResult> TotalCustomerCount()
        {
            var value = await _statisticsRepository.GetTotalCustomerCountAsync();
            return Ok(value);
        }

        [HttpGet("TotalOrderCount")]
        public async Task<IActionResult> TotalOrderCount()
        {
            var value = await _statisticsRepository.GetTotalOrderCountAsync();
            return Ok(value);
        }

        [HttpGet("TotalCustomerCountryCount")]
        public async Task<IActionResult> TotalCustomerCountryCount()
        {
            var value = await _statisticsRepository.GetTotalCustomerCountryCountAsync();
            return Ok(value);
        }

        [HttpGet("TotalCustomerCityCount")]
        public async Task<IActionResult> TotalCustomerCityCount()
        {
            var value = await _statisticsRepository.GetTotalCustomerCityCountAsync();
            return Ok(value);
        }

        [HttpGet("TotalCompletedOrderCount")]
        public async Task<IActionResult> TotalCompletedOrderCount()
        {
            var value = await _statisticsRepository.GetTotalOrderStatusByCompleted();
            return Ok(value);
        }
        [HttpGet("TotalCancelledOrderCount")]
        public async Task<IActionResult> TotalCancelledOrderCount()
        {
            var value = await _statisticsRepository.GetTotalOrderStatusByCancelled();
            return Ok(value);
        }
        [HttpGet("TotalOrdersInOctober2025")]
        public async Task<IActionResult> TotalOrdersInOctober2025()
        {
            var value = await _statisticsRepository.GetTotalOrdersİnOctober2025();
            return Ok(value);
        }
        [HttpGet("AverageProductPrice")]
        public async Task<IActionResult> AverageProductPrice()
        {
            var value = await _statisticsRepository.GetAverageProductPrice();
            return Ok(Math.Round(value, 2));
        }
        [HttpGet("AverageProductQuantity")]
        public async Task<IActionResult> AverageProductQuantity()
        {
            var value = await _statisticsRepository.GetAverageProductQuantity();
            return Ok(Math.Round(value,2));
        }
        [HttpGet("MostExpensiveProductName")]
        public async Task<IActionResult> MostExpensiveProductName()
        {
            var value = await _statisticsRepository.GetMostExpensiveProductNameAsync();
            return Ok(value);
        }
        [HttpGet("LeastExpensiveProductName")]
        public async Task<IActionResult> LeastExpensiveProductName()
        {
            var value = await _statisticsRepository.GetLeastExpensiveProductNameAsync();
            return Ok(value);
        }
        [HttpGet("MostStockProductName")]
        public async Task<IActionResult> MostStockProductName()
        {
            var value = await _statisticsRepository.GetMostStockProductNameAsync();
            return Ok(value);
        }
        [HttpGet("LastAddedCustomerFullName")]
        public async Task<IActionResult> LastAddedCustomerFullName()
        {
            var value = await _statisticsRepository.GetLastAddedCustomerFullNameAsync();
            return Ok(value);
        }
        [HttpGet("MostPaymentMethod")]
        public async Task<IActionResult> MostPaymentMethod()
        {
            var value = await _statisticsRepository.GetMostPaymantMethodAsync();
            return Ok(value);
        }
        [HttpGet("MostSoldProduct")]
        public async Task<IActionResult> MostSoldProduct()
        {
            var value = await _statisticsRepository.GetMostSoldProductAsync();
            return Ok(value);
        }
        [HttpGet("LeastSoldProduct")]
        public async Task<IActionResult> LeastSoldProduct()
        {
            var value = await _statisticsRepository.GetLeastSoldProductAsync();
            return Ok(value);
        }
        [HttpGet("MostSellProductCountryName")]
        public async Task<IActionResult> MostSellProductCountry()
        {
            var value = await _statisticsRepository.GetMostSellProductCountryNameAsync();
            return Ok(value);
        }
        [HttpGet("MostSellCategoryName")]
        public async Task<IActionResult> MostSellCategoryName()
        {
            var value = await _statisticsRepository.GetMostSellCategoryNameAsync();
            return Ok(value);
        }
    }
}