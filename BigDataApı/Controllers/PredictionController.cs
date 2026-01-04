using BigDataApı.ML.NET.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace BigDataApı.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PredictionController : ControllerBase
    {
        private readonly IPredictionService _predictionService;

        public PredictionController(IPredictionService predictionService)
        {
            _predictionService = predictionService;
        }

        [HttpGet("payment-forecast-2026-q1")]
        public async Task<IActionResult> GetPaymentForecast()
        {
            var result = await _predictionService.GetPaymentMethodForecastWith2025To2026Q1Async();

            if (result == null)
            {
                return NotFound("Tahmin üretmek için yeterli veri bulunamadı.");
            }

            return Ok(result);
        }


        [HttpGet("germany-cities-sales-forecast")]
        public async Task<IActionResult> GetGermanyCitiesSalesForecast()
        {
            var result = await _predictionService.GetGermanyCitiesSalesForecast();
            if (result == null)
            {
                return NotFound("Tahmin üretmek için yeterli veri bulunamadı.");
            }
            return Ok(result);
        }
    }
}